using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Topgg.Sdk.Webhooks.Payloads;
using Topgg.Sdk.Webhooks.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;
using System.Threading;

namespace Topgg.Sdk.Webhooks;

/// <summary>A Top.gg webhook event listener.</summary>
public abstract class WebhookEventListener
{
    private byte[] Secret;
    private readonly TimeSpan Timeout;
    private readonly TimeSpan TimestampWindow;
    private ILogger Logger;
    private readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new ULongToStringConverter(), new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    /// <summary>Creates a new webhook event listener.</summary>
    /// <param name="secret">The secret to use to authorize external requests.</param>
    /// <param name="timeout">The timeout for reading payloads. Defaults to five seconds.</param>
    /// <param name="timestampWindow">The accepted time window for timestamps before they get rejected to help mitigate replay attacks. Defaults to 30 seconds.</param>
    public WebhookEventListener(string secret, TimeSpan timeout, TimeSpan timestampWindow)
    {
        SetSecret(secret);
        Timeout = timeout;
        TimestampWindow = timestampWindow;

        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        Logger = factory.CreateLogger("Top.gg WebhookEventListener");
    }

    /// <summary>Creates a new webhook event listener.</summary>
    /// <param name="secret">The secret to use to authorize external requests.</param>
    /// <param name="timeout">The timeout for reading payloads. Defaults to five seconds.</param>
    public WebhookEventListener(string secret, TimeSpan timeout) : this(secret, timeout, TimeSpan.FromSeconds(30)) { }

    /// <summary>Creates a new webhook event listener.</summary>
    /// <param name="secret">The secret to use to authorize external requests.</param>
    public WebhookEventListener(string secret) : this(secret, TimeSpan.FromSeconds(5)) { }

    /// <summary>Sets the secret to use to authorize external requests.</summary>
    /// <param name="newSecret">The new secret to use to authorize external requests.</param>
    public void SetSecret(string newSecret) => Secret = Encoding.UTF8.GetBytes(newSecret);

    private async Task Dispatch<T>(Func<HttpContext, T, string, Task> callback, HttpContext context, Payload payload, StringValues trace)
    {
        var data = payload.Data.Deserialize<T>(SerializerOptions);

        if (data == null)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 400;

                await context.Response.WriteAsync("Bad Request");
            }
        }
        else
        {
            try
            {
                await callback(context, data, trace.First());
            }
            catch
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 500;

                    await context.Response.WriteAsync("Internal Server Error");
                }
            }
        }
    }

    /// <summary>The handler method to be passed to ASP.NET Core.</summary>
    /// <param name="context">The HTTP request context from ASP.NET Core.</param>
    public async void Handler(HttpContext context)
    {
        var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        if (!context.Request.Headers.TryGetValue("x-topgg-signature", out var signatureHeader) || !context.Request.Headers.TryGetValue("x-topgg-trace", out var trace))
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsync("Missing required headers");
            }

            return;
        }

        using var cancellationTokenSource = new CancellationTokenSource(Timeout);
        byte[] body = [];

        try
        {
            var parsedSignature = signatureHeader.First().Split(',').Select(part => part.Split('=')).ToDictionary(part => part[0], part => part[1]);

            if (!long.TryParse(parsedSignature["t"], out var timestamp))
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 422;

                    await context.Response.WriteAsync("Invalid signature format");
                }

                return;
            }
            else if (Math.Abs(currentTimestamp - (timestamp * 1000)) > TimestampWindow.Milliseconds)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 403;

                    await context.Response.WriteAsync("Timestamp outside of accepted time window");
                }

                return;
            }

            var maxBodyFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();

            if (maxBodyFeature != null && !maxBodyFeature.IsReadOnly)
            {
                maxBodyFeature.MaxRequestBodySize = 2 * 1024 * 1024;
            }

            using var bodyStream = new MemoryStream();

            await context.Request.Body.CopyToAsync(bodyStream, cancellationTokenSource.Token);
            body = bodyStream.ToArray();
            var transformBuffer = Encoding.UTF8.GetBytes($"{parsedSignature["t"]}.").Concat(body).ToArray();

            var signature = Convert.FromHexString(parsedSignature["v1"]);
            var hash = HMACSHA256.HashData(Secret, transformBuffer);

            if (!CryptographicOperations.FixedTimeEquals(signature, hash))
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 401;

                    await context.Response.WriteAsync("Unauthorized");
                }

                return;
            }

            var payload = JsonSerializer.Deserialize<Payload>(body, SerializerOptions);

            if (payload != null)
            {
                switch (payload.Type)
                {
                    case "integration.create": await Dispatch<IntegrationCreatePayload>(OnIntegrationCreate, context, payload, trace); break;
                    case "integration.delete": await Dispatch<IntegrationDeletePayload>(OnIntegrationDelete, context, payload, trace); break;
                    case "webhook.test": await Dispatch<TestPayload>(OnTest, context, payload, trace); break;
                    case "vote.create": await Dispatch<VoteCreatePayload>(OnVoteCreate, context, payload, trace); break;
                }

                return;
            }
        }
        catch (OperationCanceledException)
        {
            if (cancellationTokenSource.IsCancellationRequested && !context.Response.HasStarted)
            {
                context.Response.StatusCode = 408;

                await context.Response.WriteAsync("Request timed out");
            }

            return;
        }
        catch (JsonException err)
        {
            Logger.LogWarning("Unable to parse Top.gg webhook payload. Please report this bug to the SDK maintainers.\nCause: {Cause}\n--- BEGIN BODY DUMP ---\n{Body}\n--- END BODY DUMP ---", err.Message, Encoding.UTF8.GetString(body));

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 204;
            }

            return;
        }
        catch (Exception) { }

        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = 400;

            await context.Response.WriteAsync("Bad Request");
        }
    }

    /// <summary>Registers a listener that fires when a user has connected to your webhook integration.</summary>
    /// <param name="context">The HTTP request context from ASP.NET Core.</param>
    /// <param name="payload">The webhook payload.</param>
    /// <param name="trace">The payload's x-topgg-trace header for debugging and correlating requests with Top.gg support.</param>
    /// <returns>The response for this request.</returns>
    public virtual Task OnIntegrationCreate(HttpContext context, IntegrationCreatePayload payload, string trace) => DefaultResponse(context);

    /// <summary>Registers a listener that fires when a user has disconnected from your webhook integration.</summary>
    /// <param name="context">The HTTP request context from ASP.NET Core.</param>
    /// <param name="payload">The webhook payload.</param>
    /// <param name="trace">The payload's x-topgg-trace header for debugging and correlating requests with Top.gg support.</param>
    /// <returns>The response for this request.</returns>
    public virtual Task OnIntegrationDelete(HttpContext context, IntegrationDeletePayload payload, string trace) => DefaultResponse(context);

    /// <summary>Registers a listener that fires when a test webhook was sent from the dashboard.</summary>
    /// <param name="context">The HTTP request context from ASP.NET Core.</param>
    /// <param name="payload">The webhook payload.</param>
    /// <param name="trace">The payload's x-topgg-trace header for debugging and correlating requests with Top.gg support.</param>
    /// <returns>The response for this request.</returns>
    public virtual Task OnTest(HttpContext context, TestPayload payload, string trace) => DefaultResponse(context);

    /// <summary>Registers a listener that fires when a user votes for your project.</summary>
    /// <param name="context">The HTTP request context from ASP.NET Core.</param>
    /// <param name="payload">The webhook payload.</param>
    /// <param name="trace">The payload's x-topgg-trace header for debugging and correlating requests with Top.gg support.</param>
    /// <returns>The response for this request.</returns>
    public virtual Task OnVoteCreate(HttpContext context, VoteCreatePayload payload, string trace) => DefaultResponse(context);

    private static Task DefaultResponse(HttpContext context)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = 204;
        }

        return Task.CompletedTask;
    }
}