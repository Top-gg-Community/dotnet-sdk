using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DiscordBotsList.Api.Tests
{
    public class VoteCacheTests
    {
        private const string TopggUserId = "8226924471638491136";
        private const string DiscordUserId = "661200758510977084";

        private const string VoteWebhookJson = @"{
            ""type"": ""vote.create"",
            ""data"": {
                ""id"": ""808499215864008704"",
                ""weight"": 2,
                ""created_at"": ""2026-08-16T10:00:00Z"",
                ""expires_at"": ""2026-08-17T10:00:00Z"",
                ""project"": {
                    ""id"": ""1230954036934033243"",
                    ""type"": ""bot"",
                    ""platform"": ""discord"",
                    ""platform_id"": ""218109768489992192""
                },
                ""query"": {},
                ""user"": {
                    ""id"": """ + TopggUserId + @""",
                    ""platform_id"": """ + DiscordUserId + @""",
                    ""name"": ""Voter"",
                    ""avatar_url"": ""https://example.com/avatar.png""
                }
            }
        }";

        [Fact]
        public void WebhookVote_IsCachedByBothIds()
        {
            var cache = new VoteCache();
            cache.HandleWebhookEvent(JsonSerializer.Deserialize<TopggWebhookEvent>(VoteWebhookJson));

            Assert.Equal(2, cache.Count);
            Assert.True(cache.HasActiveVote(ulong.Parse(DiscordUserId)));
            Assert.True(cache.HasActiveVote(TopggUserId, VoteSource.Topgg));
            Assert.Equal(2, cache.Get(ulong.Parse(DiscordUserId)).Weight);
            Assert.Equal(2, cache.Get(TopggUserId, VoteSource.Topgg).Weight);
        }

        [Fact]
        public void HandleVote_WithMissingUser_IsIgnored()
        {
            var cache = new VoteCache();
            cache.HandleVote(new VoteWebhookData());

            Assert.Equal(0, cache.Count);
        }

        [Fact]
        public void NonVoteEvent_IsIgnored()
        {
            const string json = @"{""type"":""webhook.test"",""data"":{""project"":{},""user"":{}}}";
            var cache = new VoteCache();
            cache.HandleWebhookEvent(JsonSerializer.Deserialize<TopggWebhookEvent>(json));

            Assert.Equal(0, cache.Count);
        }

        [Fact]
        public void ExpiredVote_IsNotActive()
        {
            var cache = new VoteCache();
            cache.Upsert(new CachedVote(1, 2, 1, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddHours(-1)));

            Assert.False(cache.HasActiveVote(2UL, VoteSource.Discord));
        }

        [Fact]
        public void Remove_DeletesEntry()
        {
            var cache = new VoteCache();
            cache.HandleWebhookEvent(JsonSerializer.Deserialize<TopggWebhookEvent>(VoteWebhookJson));

            cache.Remove(ulong.Parse(DiscordUserId), VoteSource.Discord);

            Assert.False(cache.HasActiveVote(ulong.Parse(DiscordUserId)));
            Assert.True(cache.HasActiveVote(TopggUserId, VoteSource.Topgg));
        }

        [Fact]
        public void Prune_RemovesOnlyExpired()
        {
            var cache = new VoteCache();
            cache.Upsert(new CachedVote(1, 2, 1, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddHours(-1)));
            cache.Upsert(new CachedVote(3, 4, 1, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(12)));

            var removed = cache.Prune();

            Assert.Equal(2, removed);
            Assert.Equal(2, cache.Count);
            Assert.True(cache.HasActiveVote(4UL, VoteSource.Discord));
            Assert.False(cache.HasActiveVote(2UL, VoteSource.Discord));
        }

        [Fact]
        public async Task SyncAsync_BackfillsFromApi()
        {
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"cursor\":\"\",\"data\":[{\"userId\":\"" + TopggUserId + "\",\"platformId\":\"" + DiscordUserId +
                    "\",\"weight\":1,\"createdAt\":\"2026-08-16T10:00:00Z\",\"expiresAt\":\"2026-08-17T10:00:00Z\"}]}"),
            });
            var client = new TopggV1Client("my-v1-token", new HttpClient(handler));
            var cache = new VoteCache();

            var count = await cache.SyncAsync(client, DateTimeOffset.UtcNow.AddDays(-7));

            Assert.Equal(1, count);
            Assert.Equal(2, cache.Count);
            Assert.True(cache.HasActiveVote(ulong.Parse(DiscordUserId)));
        }

        [Fact]
        public async Task RefreshUserAsync_RemovesVote_WhenApiReportsNone()
        {
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.NotFound));
            var client = new TopggV1Client("my-v1-token", new HttpClient(handler));
            var cache = new VoteCache();
            cache.Upsert(new CachedVote(123, 456, 1, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(12)));

            var refreshed = await cache.RefreshUserAsync(client, "456", VoteSource.Discord);

            Assert.Null(refreshed);
            Assert.False(cache.HasActiveVote(456UL, VoteSource.Discord));
        }

        [Fact]
        public async Task RefreshUserAsync_UpsertsCurrentStatus()
        {
            var expiresAt = DateTimeOffset.UtcNow.AddHours(12);
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    $"{{\"created_at\":\"{DateTimeOffset.UtcNow:o}\",\"expires_at\":\"{expiresAt:o}\",\"weight\":1}}"),
            });
            var client = new TopggV1Client("my-v1-token", new HttpClient(handler));
            var cache = new VoteCache();

            var refreshed = await cache.RefreshUserAsync(client, DiscordUserId, VoteSource.Discord);

            Assert.NotNull(refreshed);
            Assert.True(cache.HasActiveVote(ulong.Parse(DiscordUserId)));
        }
    }

    public class TopggAutoPosterTests
    {
        private static TopggV1Client CreateClient(FakeHandler handler)
        {
            return new TopggV1Client("my-v1-token", new HttpClient(handler));
        }

        [Fact]
        public async Task PostAsync_PostsProvidedMetrics()
        {
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.NoContent));
            var poster = new TopggAutoPoster(CreateClient(handler), () => Task.FromResult(ProjectMetrics.ForDiscordBot(420, 67)));
            ProjectMetrics posted = null;
            poster.Posted += metrics =>
            {
                posted = metrics;
                return Task.CompletedTask;
            };

            await poster.PostAsync();

            Assert.Equal(System.Net.Http.HttpMethod.Patch, handler.LastRequest.Method);
            Assert.Equal("/api/v1/projects/@me/metrics", handler.LastRequest.RequestUri.AbsolutePath);
            using var doc = JsonDocument.Parse(handler.LastRequestBody);
            Assert.Equal(420, doc.RootElement.GetProperty("server_count").GetInt32());
            Assert.Equal(67, doc.RootElement.GetProperty("shard_count").GetInt32());
            Assert.Equal(420, posted.ServerCount);
            poster.Dispose();
        }

        [Fact]
        public async Task PostAsync_RaisesPostFailed_OnApiError()
        {
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(
                    "{\"type\":\"about:blank\",\"title\":\"Internal Server Error\",\"status\":500,\"detail\":\"boom\"}"),
            });
            var poster = new TopggAutoPoster(CreateClient(handler), () => Task.FromResult(ProjectMetrics.ForDiscordBot(420)));
            Exception failure = null;
            poster.PostFailed += ex =>
            {
                failure = ex;
                return Task.CompletedTask;
            };

            await poster.PostAsync();

            Assert.IsType<TopggApiException>(failure);
            Assert.False(poster.IsRunning);
            poster.Dispose();
        }

        [Fact]
        public async Task Start_PostsImmediately_AndRaisesPosted()
        {
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.NoContent));
            var postedTcs = new TaskCompletionSource<ProjectMetrics>(TaskCreationOptions.RunContinuationsAsynchronously);
            var poster = new TopggAutoPoster(CreateClient(handler), () => Task.FromResult(ProjectMetrics.ForDiscordBot(420)));
            poster.Posted += metrics =>
            {
                postedTcs.TrySetResult(metrics);
                return Task.CompletedTask;
            };

            poster.Start();
            var metrics = await postedTcs.Task.WaitAsync(TimeSpan.FromSeconds(5));

            Assert.True(poster.IsRunning);
            Assert.Equal(420, metrics.ServerCount);
            Assert.Equal("/api/v1/projects/@me/metrics", handler.LastRequest.RequestUri.AbsolutePath);

            poster.Stop();
            Assert.False(poster.IsRunning);
            poster.Dispose();
        }

        [Fact]
        public void ZeroOrNegativeInterval_IsRejected()
        {
            var api = CreateClient(new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.NoContent)));

            Assert.Throws<ArgumentOutOfRangeException>(() => new TopggAutoPoster(api, () => Task.FromResult(ProjectMetrics.ForDiscordBot(1)), TimeSpan.Zero));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TopggAutoPoster(api, () => Task.FromResult(ProjectMetrics.ForDiscordBot(1)), TimeSpan.FromMinutes(-1)));
        }

        [Fact]
        public async Task ConcurrentPosts_DoNotOverlap()
        {
            var release = new TaskCompletionSource<ProjectMetrics>(TaskCreationOptions.RunContinuationsAsynchronously);
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.NoContent));
            var poster = new TopggAutoPoster(CreateClient(handler), () => release.Task);
            var first = poster.PostAsync();
            await Task.Delay(50);
            var second = poster.PostAsync();
            await Task.Delay(50);

            Assert.Equal(0, handler.RequestCount);

            release.SetResult(ProjectMetrics.ForDiscordBot(1));
            await Task.WhenAll(first, second);

            Assert.Equal(1, handler.RequestCount);
            poster.Dispose();
        }
    }

    internal sealed class FakeHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;

        public HttpRequestMessage LastRequest { get; private set; }
        public string LastRequestBody { get; private set; }
        public int RequestCount { get; private set; }

        public FakeHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
        {
            _responder = responder;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestCount++;
            LastRequest = request;
            LastRequestBody = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
            return _responder(request);
        }
    }
}
