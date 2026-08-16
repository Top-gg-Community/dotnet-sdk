using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DiscordBotsList.Api.Internal;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     Represents a structured error body returned by the Top.gg v1 API.
    ///     Errors follow <a href="https://www.rfc-editor.org/rfc/rfc7807">RFC 7807</a>
    ///     and are delivered with the <c>application/problem+json</c> content type.
    /// </summary>
    public class ProblemDetails
    {
        /// <summary>A URI identifying the error type.</summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>A short, human-readable summary of the error.</summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>The HTTP status code of the response.</summary>
        [JsonPropertyName("status")]
        public int? Status { get; set; }

        /// <summary>A human-readable explanation of this specific occurrence.</summary>
        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        /// <summary>A URI identifying the specific occurrence of the problem.</summary>
        [JsonPropertyName("instance")]
        public string Instance { get; set; }
    }

    /// <summary>
    ///     Thrown whenever a Top.gg v1 API request is not successful. Carries the
    ///     HTTP status code, the parsed <see cref="ProblemDetails" /> body, and, for
    ///     <c>429 Too Many Requests</c> responses, the suggested retry delay.
    /// </summary>
    public class TopggApiException : Exception
    {
        public TopggApiException(string message, HttpStatusCode statusCode, ProblemDetails problem = null, TimeSpan? retryAfter = null)
            : base(message)
        {
            StatusCode = statusCode;
            Problem = problem;
            RetryAfter = retryAfter;
        }

        /// <summary>The HTTP status code returned by the API.</summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>The parsed RFC 7807 problem details, if the body could be read.</summary>
        public ProblemDetails Problem { get; }

        /// <summary>
        ///     The suggested delay before retrying, read from the <c>Retry-After</c>
        ///     header. Only populated for <c>429 Too Many Requests</c> responses.
        /// </summary>
        public TimeSpan? RetryAfter { get; }

        internal static async Task<TopggApiException> CreateAsync(HttpResponseMessage response)
        {
            ProblemDetails problem = null;
            try
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrEmpty(content))
                {
                    problem = JsonSerializer.Deserialize<ProblemDetails>(content, JsonDefaults.Options);
                }
            }
            catch
            {
                // The body was not RFC 7807 JSON; fall through with a generic message.
            }

            TimeSpan? retryAfter = null;
            if (response.StatusCode == HttpStatusCode.TooManyRequests && response.Headers.RetryAfter is { } retryCondition)
            {
                retryAfter = retryCondition.Delta ?? retryCondition.Date - DateTimeOffset.UtcNow;
            }

            var message = problem?.Detail
                          ?? problem?.Title
                          ?? $"The Top.gg API request failed with status code {(int)response.StatusCode}.";

            return new TopggApiException(message, response.StatusCode, problem, retryAfter);
        }
    }
}
