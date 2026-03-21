#nullable enable

using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Topgg.Sdk.Api.Tests;

internal class Mock : HttpMessageHandler
{
#pragma warning disable SYSLIB1045
    private static readonly (HttpMethod Method, Regex Endpoint, string? Name)[] Routes = {
            (HttpMethod.Get, new Regex(@"^\/projects\/@me$", RegexOptions.Compiled), "GetSelf"),
            (HttpMethod.Get, new Regex(@"^\/projects\/@me\/votes\/\d+$", RegexOptions.Compiled), "GetVote"),
            (HttpMethod.Get, new Regex(@"^\/projects\/@me\/votes$", RegexOptions.Compiled), "GetVotes"),
            (HttpMethod.Post, new Regex(@"^\/projects\/@me\/commands$", RegexOptions.Compiled), null)
        };
#pragma warning restore SYSLIB1045

    private static Stream StreamJson(string name) => typeof(Mock).Assembly.GetManifestResourceStream($"Topgg.Sdk.Api.Tests.Mocks.{name}.json")!;

    internal static string ReadJson(string name)
    {
        using var stream = StreamJson(name);
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri!.Host == "top.gg" && request.RequestUri.AbsolutePath.StartsWith("/api/v1/"))
        {
            var endpoint = request.RequestUri.AbsolutePath[7..];

            foreach (var (Method, Endpoint, Name) in Routes)
            {
                if (request.Method == Method && Endpoint.IsMatch(endpoint))
                {
                    if (Name == null)
                    {
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent));
                    }
                    else
                    {
                        var content = new StreamContent(StreamJson(Name));
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = content,
                        });
                    }
                }
            }
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
    }
}