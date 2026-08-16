using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DiscordBotsList.Api.Internal;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     Client for the current Top.gg <c>v1</c> REST API
    ///     (<c>https://top.gg/api/v1</c>). Authenticates every request with a Bearer
    ///     token obtained from your project's Integrations &amp; API settings on Top.gg.
    ///     Errors follow RFC 7807 and are surfaced as <see cref="TopggApiException" />.
    /// </summary>
    /// <remarks>
    ///     Legacy v0 tokens (without the <c>Bearer</c> prefix) do not work on v1
    ///     endpoints. Create a new token from your project's Integrations &amp; API
    ///     settings when migrating from v0.
    /// </remarks>
    public sealed class TopggV1Client : IDisposable
    {
        private const string BaseUrl = "https://top.gg/api/v1";

        private readonly HttpClient _httpClient;
        private readonly bool _ownsClient;
        private readonly string _token;

        /// <param name="token">Your Top.gg v1 API token.</param>
        /// <param name="httpClient">
        ///     An optional <see cref="HttpClient" /> to use. When omitted, a client is
        ///     created and disposed with this instance.
        /// </param>
        public TopggV1Client(string token, HttpClient httpClient = null)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("A Top.gg API token is required.", nameof(token));
            }

            _token = token;
            _ownsClient = httpClient is null;
            _httpClient = httpClient ?? new HttpClient();
        }

        /// <summary>
        ///     Returns the project associated with your current token.
        ///     <c>GET /projects/@me</c>.
        /// </summary>
        public Task<Project> GetProjectAsync(CancellationToken cancellationToken = default)
        {
            return GetAsync<Project>("projects/@me", null, cancellationToken);
        }

        /// <summary>
        ///     Updates the headline and/or page content for your project. Both fields are
        ///     locale-keyed, so you can set content for multiple languages in a single
        ///     request. <c>PATCH /projects/@me</c>.
        /// </summary>
        /// <remarks>
        ///     Changes may take up to 1 hour to appear on the website due to caching.
        /// </remarks>
        public async Task UpdateProjectAsync(ProjectUpdate update, CancellationToken cancellationToken = default)
        {
            if (update is null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            if (update.Headline is null && update.PageContent is null)
            {
                throw new ArgumentException("Provide at least one of Headline or PageContent.", nameof(update));
            }

            using var response = await SendAsync(HttpMethod.Patch, "projects/@me", body: update, cancellationToken: cancellationToken).ConfigureAwait(false);
            await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Creates a new announcement for your project. <c>POST /projects/@me/announcements</c>.
        /// </summary>
        /// <remarks>
        ///     At most one announcement may be created every 4 hours; attempts during the
        ///     cooldown period receive a <c>429</c> with a <c>Retry-After</c> header
        ///     (surfaced via <see cref="TopggApiException.RetryAfter" />).
        /// </remarks>
        public Task<Announcement> CreateAnnouncementAsync(AnnouncementRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return SendAsync<Announcement>(HttpMethod.Post, "projects/@me/announcements", request, cancellationToken);
        }

        /// <summary>
        ///     Creates a new announcement for your project. <c>POST /projects/@me/announcements</c>.
        /// </summary>
        /// <param name="title">The announcement title (3 to 100 characters).</param>
        /// <param name="content">The announcement body text (10 to 2,000 characters).</param>
        /// <param name="category">
        ///     The category to publish under: <c>announcement</c>, <c>event</c>, or
        ///     <c>new_feature</c>. Defaults to <c>announcement</c> when omitted.
        /// </param>
        public Task<Announcement> CreateAnnouncementAsync(string title, string content, string category = null, CancellationToken cancellationToken = default)
        {
            return CreateAnnouncementAsync(new AnnouncementRequest { Title = title, Content = content, Category = category }, cancellationToken);
        }

        /// <summary>
        ///     Submits a single metrics payload for your project. Use this to push fresh
        ///     numbers after an event such as joining or leaving a guild or a player
        ///     connecting. <c>PATCH /projects/@me/metrics</c>.
        /// </summary>
        /// <remarks>
        ///     The payload shape must match the authenticated project's platform; sending
        ///     the wrong shape returns <c>422</c>.
        /// </remarks>
        public async Task UpdateMetricsAsync(ProjectMetrics metrics, CancellationToken cancellationToken = default)
        {
            if (metrics is null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }

            using var response = await SendAsync(HttpMethod.Patch, "projects/@me/metrics", body: metrics, cancellationToken: cancellationToken).ConfigureAwait(false);
            await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Submits up to 100 metrics entries in a single request. Entries without a
        ///     timestamp are applied first in the order they appear; entries with a
        ///     timestamp are applied afterwards in ascending timestamp order, so you can
        ///     backfill gaps or push buffered samples after a network interruption.
        ///     <c>POST /projects/@me/metrics/batch</c>.
        /// </summary>
        public async Task UpdateMetricsBatchAsync(IEnumerable<MetricsBatchEntry> entries, CancellationToken cancellationToken = default)
        {
            if (entries is null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            var batch = new MetricsBatchRequest { Data = entries.ToList() };
            if (batch.Data.Count == 0 || batch.Data.Count > 100)
            {
                throw new ArgumentException("The batch must contain between 1 and 100 entries.", nameof(entries));
            }

            using var response = await SendAsync(HttpMethod.Post, "projects/@me/metrics/batch", body: batch, cancellationToken: cancellationToken).ConfigureAwait(false);
            await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Overwrites the list of slash command definitions for your bot project.
        ///     Pass an empty list to clear all commands.
        ///     <c>PUT /projects/@me/commands</c>.
        /// </summary>
        public async Task UpdateCommandsAsync(IEnumerable<ApplicationCommand> commands, CancellationToken cancellationToken = default)
        {
            using var response = await SendAsync(HttpMethod.Put, "projects/@me/commands", body: commands.ToList(), cancellationToken: cancellationToken).ConfigureAwait(false);
            await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Overwrites the list of slash command definitions for your bot project using
        ///     a raw JSON array of application command objects.
        ///     <c>PUT /projects/@me/commands</c>.
        /// </summary>
        public async Task UpdateCommandsAsync(string commandsJson, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(commandsJson))
            {
                throw new ArgumentException("A JSON array of command objects is required.", nameof(commandsJson));
            }

            var request = new HttpRequestMessage(HttpMethod.Put, BuildUri("projects/@me/commands", null));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            request.Content = new StringContent(commandsJson, Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Returns the next page of votes starting from the given opaque cursor.
        ///     <c>GET /projects/@me/votes?cursor=...</c>.
        /// </summary>
        public Task<VotePage> GetVotesAsync(string cursor, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(cursor))
            {
                throw new ArgumentException("A cursor returned by a previous response is required.", nameof(cursor));
            }

            return GetAsync<VotePage>("projects/@me/votes", $"cursor={Uri.EscapeDataString(cursor)}", cancellationToken);
        }

        /// <summary>
        ///     Returns the first page of votes created at or after <paramref name="startDate" />.
        ///     <c>GET /projects/@me/votes?startDate=...</c>.
        /// </summary>
        /// <remarks>The start date cannot be more than one year in the past.</remarks>
        public Task<VotePage> GetVotesAsync(DateTimeOffset startDate, CancellationToken cancellationToken = default)
        {
            var iso = Uri.EscapeDataString(startDate.UtcDateTime.ToString("o"));
            return GetAsync<VotePage>("projects/@me/votes", $"startDate={iso}", cancellationToken);
        }

        /// <summary>
        ///     Streams every vote from <paramref name="startDate" /> onwards by following
        ///     the cursor returned by each page until the API returns an empty page.
        /// </summary>
        public async IAsyncEnumerable<Vote> GetAllVotesAsync(DateTimeOffset startDate, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            string cursor = null;

            while (!cancellationToken.IsCancellationRequested)
            {
                var page = cursor is null
                    ? await GetVotesAsync(startDate, cancellationToken).ConfigureAwait(false)
                    : await GetVotesAsync(cursor, cancellationToken).ConfigureAwait(false);

                if (page.IsEmpty)
                {
                    yield break;
                }

                foreach (var vote in page.Data)
                {
                    yield return vote;
                }

                cursor = page.Cursor;
                if (string.IsNullOrWhiteSpace(cursor))
                {
                    yield break;
                }
            }
        }

        /// <summary>
        ///     Returns the most recent vote status for a specific user, or <c>null</c>
        ///     when the user has not voted or their vote has expired.
        ///     <c>GET /projects/@me/votes/:user_id</c>.
        /// </summary>
        public Task<VoteStatus> GetVoteStatusAsync(string userId, VoteSource source = VoteSource.Topgg, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("A user ID is required.", nameof(userId));
            }

            return GetVoteStatusCoreAsync(Uri.EscapeDataString(userId), source, cancellationToken);
        }

        /// <summary>
        ///     Returns the most recent vote status for a specific user, or <c>null</c>
        ///     when the user has not voted or their vote has expired.
        ///     <c>GET /projects/@me/votes/:user_id</c>.
        /// </summary>
        public Task<VoteStatus> GetVoteStatusAsync(ulong userId, VoteSource source = VoteSource.Topgg, CancellationToken cancellationToken = default)
        {
            return GetVoteStatusCoreAsync(userId.ToString(), source, cancellationToken);
        }

        /// <summary>
        ///     Returns <c>true</c> if the user has an active vote for your project.
        ///     <c>GET /projects/@me/votes/:user_id</c>.
        /// </summary>
        public async Task<bool> HasVotedAsync(string userId, VoteSource source = VoteSource.Topgg, CancellationToken cancellationToken = default)
        {
            var status = await GetVoteStatusAsync(userId, source, cancellationToken).ConfigureAwait(false);
            return status?.IsActive == true;
        }

        /// <summary>
        ///     Returns <c>true</c> if the user has an active vote for your project.
        ///     <c>GET /projects/@me/votes/:user_id</c>.
        /// </summary>
        public Task<bool> HasVotedAsync(ulong userId, VoteSource source = VoteSource.Topgg, CancellationToken cancellationToken = default)
        {
            return HasVotedAsync(userId.ToString(), source, cancellationToken);
        }

        public void Dispose()
        {
            if (_ownsClient)
            {
                _httpClient.Dispose();
            }
        }

        private async Task<VoteStatus> GetVoteStatusCoreAsync(string encodedUserId, VoteSource source, CancellationToken cancellationToken)
        {
            var sourceParameter = source == VoteSource.Discord ? "discord" : "topgg";
            using var response = await SendAsync(HttpMethod.Get, $"projects/@me/votes/{encodedUserId}", query: $"source={sourceParameter}", cancellationToken: cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return await ReadResponseAsync<VoteStatus>(response, cancellationToken).ConfigureAwait(false);
        }

        private async Task<T> GetAsync<T>(string path, string query, CancellationToken cancellationToken)
        {
            using var response = await SendAsync(HttpMethod.Get, path, query: query, cancellationToken: cancellationToken).ConfigureAwait(false);
            return await ReadResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
        }

        private async Task<T> SendAsync<T>(HttpMethod method, string path, object body, CancellationToken cancellationToken)
        {
            using var response = await SendAsync(method, path, body, null, cancellationToken).ConfigureAwait(false);
            return await ReadResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod method, string path, object body = null, string query = null, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(method, BuildUri(path, query));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            if (body is not null)
            {
                var json = JsonSerializer.Serialize(body, JsonDefaults.Options);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        }

        private static async Task<T> ReadResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                if (string.IsNullOrEmpty(content))
                {
                    return default;
                }

                return JsonSerializer.Deserialize<T>(content, JsonDefaults.Options);
            }

            throw await TopggApiException.CreateAsync(response).ConfigureAwait(false);
        }

        private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw await TopggApiException.CreateAsync(response).ConfigureAwait(false);
        }

        private static Uri BuildUri(string path, string query)
        {
            var builder = new UriBuilder($"{BaseUrl}/{path}");
            if (!string.IsNullOrEmpty(query))
            {
                builder.Query = query;
            }

            return builder.Uri;
        }
    }
}
