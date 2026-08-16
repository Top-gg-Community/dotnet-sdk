using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DiscordBotsList.Api.Internal;
using Xunit;

namespace DiscordBotsList.Api.Tests
{
    public class WebhookSignatureTests
    {
        private const string Secret = "whs_supersecret";
        private const string Body = "{\"type\":\"vote.create\",\"data\":{\"id\":\"808499215864008704\"}}";

        private static string Sign(string secret, string timestamp, string body)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes($"{timestamp}.{body}"));
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        private static string Header(string timestamp, string signature)
        {
            return $"t={timestamp},v1={signature}";
        }

        [Fact]
        public void ValidSignature_IsAccepted()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var signature = Sign(Secret, timestamp, Body);

            Assert.True(TopggWebhook.VerifySignature(Secret, Body, Header(timestamp, signature)));
        }

        [Fact]
        public void TamperedBody_IsRejected()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var signature = Sign(Secret, timestamp, Body);

            Assert.False(TopggWebhook.VerifySignature(Secret, Body + "tampered", Header(timestamp, signature)));
        }

        [Fact]
        public void WrongSecret_IsRejected()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var signature = Sign(Secret, timestamp, Body);

            Assert.False(TopggWebhook.VerifySignature("whs_wrong", Body, Header(timestamp, signature)));
        }

        [Fact]
        public void MalformedHeader_IsRejected()
        {
            Assert.False(TopggWebhook.VerifySignature(Secret, Body, "not-a-signature"));
            Assert.False(TopggWebhook.VerifySignature(Secret, Body, ""));
        }

        [Fact]
        public void StaleTimestamp_IsRejectedWhenMaxAgeIsSet()
        {
            var timestamp = DateTimeOffset.UtcNow.AddHours(-1).ToUnixTimeSeconds().ToString();
            var signature = Sign(Secret, timestamp, Body);

            Assert.True(TopggWebhook.VerifySignature(Secret, Body, Header(timestamp, signature)));
            Assert.False(TopggWebhook.VerifySignature(Secret, Body, Header(timestamp, signature), maxAge: TimeSpan.FromMinutes(5)));
        }

        [Fact]
        public void FutureTimestamp_IsRejectedWhenMaxAgeIsSet()
        {
            var timestamp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds().ToString();
            var signature = Sign(Secret, timestamp, Body);

            Assert.False(TopggWebhook.VerifySignature(Secret, Body, Header(timestamp, signature), maxAge: TimeSpan.FromMinutes(5)));
        }

        [Fact]
        public void HeaderDictionary_IsMatchedCaseInsensitively()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var signature = Sign(Secret, timestamp, Body);

            var headers = new List<KeyValuePair<string, string>>
            {
                new("X-Topgg-Trace", "some-trace-id"),
                new("X-TOPGG-SIGNATURE", Header(timestamp, signature)),
            };

            Assert.True(TopggWebhook.VerifySignature(Secret, Body, headers));
        }

        [Fact]
        public void ComputeSignature_MatchesManualHmac()
        {
            const string timestamp = "1723123456";
            var expected = Sign(Secret, timestamp, Body);
            Assert.Equal(expected, TopggWebhook.ComputeSignature(Secret, timestamp, Body));
        }

        [Fact]
        public void ParseAndVerify_ReturnsNull_WhenSignatureIsInvalid()
        {
            Assert.Null(TopggWebhook.ParseAndVerify(Secret, Body, "t=1,v1=deadbeef"));
        }

        [Fact]
        public void ParseAndVerify_ReturnsEvent_WhenSignatureIsValid()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var signature = Sign(Secret, timestamp, Body);

            var evt = TopggWebhook.ParseAndVerify(Secret, Body, Header(timestamp, signature));
            Assert.NotNull(evt);
            Assert.Equal(TopggWebhookEvent.VoteCreateType, evt.Type);
        }
    }

    public class WebhookEventTests
    {
        [Fact]
        public void VoteCreateEvent_IsDeserialized()
        {
            const string payload = @"{
              ""type"": ""vote.create"",
              ""data"": {
                ""id"": ""808499215864008704"",
                ""weight"": 2,
                ""created_at"": ""2026-02-09T00:47:14.2510149+00:00"",
                ""expires_at"": ""2026-02-09T12:47:14.2510149+00:00"",
                ""project"": {
                  ""id"": ""803190510032756736"",
                  ""type"": ""bot"",
                  ""platform"": ""discord"",
                  ""platform_id"": ""160105994217586689""
                },
                ""query"": {
                  ""key1"": ""value"",
                  ""key2"": ""value2""
                },
                ""user"": {
                  ""id"": ""1234567890"",
                  ""platform_id"": ""160105994217586689"",
                  ""name"": ""username"",
                  ""avatar_url"": ""https://example.com/avatar.png""
                }
              }
            }";

            var evt = TopggWebhook.Deserialize(payload);
            Assert.True(evt.IsVoteCreate);
            Assert.False(evt.IsWebhookTest);

            var data = evt.GetVoteData();
            Assert.Equal(808499215864008704UL, data.Id);
            Assert.Equal(2, data.Weight);
            Assert.Equal("803190510032756736", data.Project.Id.ToString());
            Assert.Equal("discord", data.Project.Platform);
            Assert.Equal(160105994217586689UL, data.Project.PlatformId);
            Assert.Equal("value", data.Query["key1"]);
            Assert.Equal(160105994217586689UL, data.User.PlatformId);
            Assert.Equal("username", data.User.Name);
        }

        [Fact]
        public void WebhookTestEvent_IsDeserialized()
        {
            const string payload = @"{
              ""type"": ""webhook.test"",
              ""data"": {
                ""user"": {
                  ""id"": ""1234567890"",
                  ""platform_id"": ""160105994217586689"",
                  ""name"": ""username"",
                  ""avatar_url"": ""https://example.com/avatar.png""
                },
                ""project"": {
                  ""id"": ""803190510032756736"",
                  ""type"": ""bot"",
                  ""platform"": ""discord"",
                  ""platform_id"": ""160105994217586689""
                }
              }
            }";

            var evt = TopggWebhook.Deserialize(payload);
            Assert.True(evt.IsWebhookTest);

            var data = evt.GetWebhookTestData();
            Assert.Equal(803190510032756736UL, data.Project.Id);
            Assert.Equal(160105994217586689UL, data.User.PlatformId);
        }
    }

    public class SerializationTests
    {
        [Fact]
        public void ProjectResponse_IsDeserialized()
        {
            const string json = @"{
                ""id"": ""218109768489992192"",
                ""name"": ""Miki"",
                ""type"": ""bot"",
                ""platform"": ""discord"",
                ""headline"": ""A great bot with tons of features!"",
                ""tags"": [""anime"", ""economy"", ""fun"", ""leveling""],
                ""votes"": 1120,
                ""votesTotal"": 313389,
                ""reviewScore"": 4.38,
                ""reviewCount"": 62245
            }";

            var project = JsonSerializer.Deserialize<Project>(json, JsonDefaults.Options);

            Assert.Equal(218109768489992192UL, project.Id);
            Assert.Equal("Miki", project.Name);
            Assert.Equal("bot", project.Type);
            Assert.Equal("discord", project.Platform);
            Assert.Equal(1120, project.Votes);
            Assert.Equal(313389, project.VotesTotal);
            Assert.Equal(4.38, project.ReviewScore.Value, 2);
            Assert.Equal(62245, project.ReviewCount);
        }

        [Fact]
        public void VotePage_IsDeserialized()
        {
            const string json = @"{
                ""cursor"": ""<cursor-token>"",
                ""data"": [
                    {
                        ""userId"": ""1234567890"",
                        ""platformId"": ""9876543210"",
                        ""weight"": 1,
                        ""createdAt"": ""2026-01-28T02:19:19.145733Z"",
                        ""expiresAt"": ""2026-01-28T14:19:19.145733Z""
                    }
                ]
            }";

            var page = JsonSerializer.Deserialize<VotePage>(json, JsonDefaults.Options);

            Assert.False(page.IsEmpty);
            Assert.Equal("<cursor-token>", page.Cursor);
            var vote = page.Data.Single();
            Assert.Equal(1234567890UL, vote.UserId);
            Assert.Equal(9876543210UL, vote.PlatformId);
            Assert.Equal(1, vote.Weight);
        }

        [Fact]
        public void VoteStatus_IsDeserialized()
        {
            const string json = @"{
                ""created_at"": ""2023-10-01T12:34:56.789Z"",
                ""expires_at"": ""2023-10-01T18:34:56.789Z"",
                ""weight"": 1
            }";

            var status = JsonSerializer.Deserialize<VoteStatus>(json, JsonDefaults.Options);

            Assert.Equal(new DateTimeOffset(2023, 10, 1, 12, 34, 56, 789, TimeSpan.Zero), status.CreatedAt);
            Assert.Equal(1, status.Weight);
            Assert.False(status.IsActive);
        }

        [Fact]
        public void DiscordBotMetrics_AreSerialized()
        {
            var metrics = ProjectMetrics.ForDiscordBot(serverCount: 420, shardCount: 67);
            var json = JsonSerializer.Serialize(metrics, JsonDefaults.Options);
            using var doc = JsonDocument.Parse(json);

            Assert.Equal(420, doc.RootElement.GetProperty("server_count").GetInt32());
            Assert.Equal(67, doc.RootElement.GetProperty("shard_count").GetInt32());
            Assert.False(doc.RootElement.TryGetProperty("member_count", out _));
        }

        [Fact]
        public void AnnouncementRequest_IsSerialized()
        {
            var request = new AnnouncementRequest { Title = "Version 2.0 Released!", Content = "We just released version 2.0.", Category = "new_feature" };
            var json = JsonSerializer.Serialize(request, JsonDefaults.Options);
            using var doc = JsonDocument.Parse(json);

            Assert.Equal("Version 2.0 Released!", doc.RootElement.GetProperty("title").GetString());
            Assert.Equal("new_feature", doc.RootElement.GetProperty("category").GetString());
        }

        [Fact]
        public void ApplicationCommand_IsSerialized()
        {
            var command = new ApplicationCommand("ping", "Replies with Pong!");
            var json = JsonSerializer.Serialize(command, JsonDefaults.Options);
            using var doc = JsonDocument.Parse(json);

            Assert.Equal("ping", doc.RootElement.GetProperty("name").GetString());
            Assert.Equal("Replies with Pong!", doc.RootElement.GetProperty("description").GetString());
        }

        [Fact]
        public void ProjectUpdate_IsSerialized()
        {
            var update = new ProjectUpdate(
                headline: new Dictionary<string, string> { { "en", "A great bot with tons of features!" } },
                pageContent: new Dictionary<string, string> { { "en", "# Welcome" } });
            var json = JsonSerializer.Serialize(update, JsonDefaults.Options);
            using var doc = JsonDocument.Parse(json);

            Assert.Equal("A great bot with tons of features!", doc.RootElement.GetProperty("headline").GetProperty("en").GetString());
            Assert.Equal("# Welcome", doc.RootElement.GetProperty("pageContent").GetProperty("en").GetString());
        }

        [Fact]
        public void AnnouncementResponse_WithNumericCategory_IsDeserialized()
        {
            const string json = @"{""title"":""T"",""content"":""C"",""category"":1,""createdAt"":""2026-03-14T15:09:26Z""}";
            var announcement = JsonSerializer.Deserialize<Announcement>(json, JsonDefaults.Options);

            Assert.Equal("1", announcement.Category);
            Assert.Equal(new DateTimeOffset(2026, 3, 14, 15, 9, 26, TimeSpan.Zero), announcement.CreatedAt);
        }
    }

    public class TopggApiExceptionTests
    {
        [Fact]
        public async Task CreateAsync_ParsesProblemDetailsAndRetryAfter()
        {
            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
            {
                Content = new StringContent(@"{
                    ""type"": ""about:blank"",
                    ""title"": ""Too Many Requests"",
                    ""status"": 429,
                    ""detail"": ""Only one announcement every 4 hours.""
                }", Encoding.UTF8, "application/problem+json"),
            };
            response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromSeconds(3600));

            var ex = await TopggApiException.CreateAsync(response);

            Assert.Equal(HttpStatusCode.TooManyRequests, ex.StatusCode);
            Assert.Equal("Only one announcement every 4 hours.", ex.Problem.Detail);
            Assert.Equal(TimeSpan.FromSeconds(3600), ex.RetryAfter);
        }

        [Fact]
        public async Task CreateAsync_FallsBackToStatusMessage_WhenBodyIsNotJson()
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new StringContent("Bad Gateway"),
            };

            var ex = await TopggApiException.CreateAsync(response);

            Assert.Equal(HttpStatusCode.BadGateway, ex.StatusCode);
            Assert.Contains("502", ex.Message);
        }
    }

    public class TopggV1ClientTests
    {
        private sealed class FakeHandler : HttpMessageHandler
        {
            private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;

            public HttpRequestMessage LastRequest { get; private set; }
            public string LastRequestBody { get; private set; }

            public FakeHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
            {
                _responder = responder;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequest = request;
                LastRequestBody = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
                return _responder(request);
            }
        }

        private static TopggV1Client CreateClient(FakeHandler handler, out FakeHandler outHandler)
        {
            outHandler = handler;
            return new TopggV1Client("my-v1-token", new HttpClient(handler));
        }

        [Fact]
        public async Task GetProjectAsync_SendsBearerAuth_AndParsesProject()
        {
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"id\":\"218109768489992192\",\"name\":\"Miki\",\"type\":\"bot\",\"platform\":\"discord\",\"votes\":1120,\"votesTotal\":313389}"),
            });
            var client = CreateClient(handler, out _);

            var project = await client.GetProjectAsync();

            Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
            Assert.Equal("/api/v1/projects/@me", handler.LastRequest.RequestUri.AbsolutePath);
            Assert.Equal("Bearer my-v1-token", handler.LastRequest.Headers.Authorization.ToString());
            Assert.Equal(218109768489992192UL, project.Id);
            Assert.Equal(313389, project.VotesTotal);
        }

        [Fact]
        public async Task UpdateMetricsAsync_SendsPatchWithCorrectBody()
        {
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.NoContent));
            var client = CreateClient(handler, out _);

            await client.UpdateMetricsAsync(ProjectMetrics.ForDiscordBot(420, 67));

            Assert.Equal(HttpMethod.Patch, handler.LastRequest.Method);
            Assert.Equal("/api/v1/projects/@me/metrics", handler.LastRequest.RequestUri.AbsolutePath);
            using var doc = JsonDocument.Parse(handler.LastRequestBody);
            Assert.Equal(420, doc.RootElement.GetProperty("server_count").GetInt32());
            Assert.Equal(67, doc.RootElement.GetProperty("shard_count").GetInt32());
        }

        [Fact]
        public async Task UpdateMetricsBatchAsync_SendsBatchPayload()
        {
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.NoContent));
            var client = CreateClient(handler, out _);

            await client.UpdateMetricsBatchAsync(new[]
            {
                new MetricsBatchEntry { Metrics = ProjectMetrics.ForDiscordBot(419) },
                new MetricsBatchEntry { Timestamp = DateTimeOffset.UtcNow, Metrics = ProjectMetrics.ForDiscordBot(420, 67) },
            });

            Assert.Equal("/api/v1/projects/@me/metrics/batch", handler.LastRequest.RequestUri.AbsolutePath);
            using var doc = JsonDocument.Parse(handler.LastRequestBody);
            Assert.Equal(2, doc.RootElement.GetProperty("data").GetArrayLength());
        }

        [Fact]
        public async Task GetVoteStatusAsync_ReturnsNull_OnNotFound()
        {
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.NotFound));
            var client = CreateClient(handler, out _);

            var status = await client.GetVoteStatusAsync("1234567890", VoteSource.Discord);

            Assert.Null(status);
            Assert.Equal("source=discord", handler.LastRequest.RequestUri.Query.TrimStart('?'));
        }

        [Fact]
        public async Task HasVotedAsync_ReflectsExpiry()
        {
            var expiresAt = DateTimeOffset.UtcNow.AddHours(1);
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent($"{{\"created_at\":\"{DateTimeOffset.UtcNow:o}\",\"expires_at\":\"{expiresAt:o}\",\"weight\":1}}"),
            });
            var client = CreateClient(handler, out _);

            Assert.True(await client.HasVotedAsync(1234567890UL));
        }

        [Fact]
        public async Task CreateAnnouncementAsync_ParsesResponse()
        {
            var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent("{\"title\":\"Version 2.0 Released!\",\"content\":\"We just released version 2.0.\",\"createdAt\":\"2026-03-14T15:09:26Z\"}"),
            });
            var client = CreateClient(handler, out _);

            var announcement = await client.CreateAnnouncementAsync("Version 2.0 Released!", "We just released version 2.0.", "new_feature");

            Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
            Assert.Equal("Version 2.0 Released!", announcement.Title);
            Assert.Equal(new DateTimeOffset(2026, 3, 14, 15, 9, 26, TimeSpan.Zero), announcement.CreatedAt);
        }

        [Fact]
        public async Task UpdateProjectAsync_ThrowsWhenBothFieldsAreMissing()
        {
            var client = CreateClient(new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.NoContent)), out _);
            await Assert.ThrowsAsync<ArgumentException>(() => client.UpdateProjectAsync(new ProjectUpdate()));
        }

        [Fact]
        public async Task GetAllVotesAsync_FollowsCursorUntilEmpty()
        {
            var firstPage = true;
            var handler = new FakeHandler(_ =>
            {
                if (firstPage)
                {
                    firstPage = false;
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("{\"cursor\":\"next-page\",\"data\":[{\"userId\":\"1234567890\",\"platformId\":\"9876543210\",\"weight\":1,\"createdAt\":\"2026-01-28T02:19:19Z\",\"expiresAt\":\"2026-01-28T14:19:19Z\"}]}"),
                    };
                }

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"cursor\":\"\",\"data\":[]}"),
                };
            });
            var client = CreateClient(handler, out _);

            var votes = await client.GetAllVotesAsync(DateTimeOffset.UtcNow.AddDays(-1)).ToListAsync();

            Assert.Single(votes);
            Assert.Contains("cursor=next-page", handler.LastRequest.RequestUri.Query);
        }
    }

    internal static class EnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
        {
            var list = new List<T>();
            await foreach (var item in source)
            {
                list.Add(item);
            }

            return list;
        }
    }
}
