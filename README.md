# DiscordBotsList.Api

C# wrapper for the [top.gg](https://top.gg) API.

Supports the current **v1 API** (projects, metrics, announcements, slash commands, votes, webhooks) and keeps the legacy **v0 API** for existing integrations.

## Install

```powershell
Install-Package DiscordBotsList.Api
Install-Package DiscordBotsList.Api.Adapter.Discord.Net   # optional Discord.Net integration
```

## v1 API (recommended)

The v1 API is token-based. Use a **project token** from the top.gg dashboard. The client talks to `https://top.gg/api/v1` and authenticates with `Authorization: Bearer <token>`.

```cs
using DiscordBotsList.Api.V1;

var api = new TopggV1Client(Environment.GetEnvironmentVariable("TOPGG_TOKEN"));
```

### Project

```cs
Project project = await api.GetProjectAsync();
Console.WriteLine($"{project.Name}: {project.Votes} votes, {project.ReviewScore?.ToString("0.0")} stars");

await api.UpdateProjectAsync(new ProjectUpdate(
    headline: new() { ["en"] = "The best bot", ["de"] = "Der beste Bot" },
    pageContent: new() { ["en"] = "Long description…" }
));
```

### Metrics

```cs
// Discord bots: server + shard count
await api.UpdateMetricsAsync(ProjectMetrics.ForDiscordBot(serverCount: 420, shardCount: 67));

// Discord servers: member + online count
await api.UpdateMetricsAsync(ProjectMetrics.ForDiscordServer(memberCount: 10_000, onlineCount: 1_200));

// Roblox games / Minecraft servers
await api.UpdateMetricsAsync(ProjectMetrics.ForRobloxGame(playerCount: 500));

// Historical batch (10-minute intervals)
await api.UpdateMetricsBatchAsync(new[]
{
    new MetricsBatchEntry { Timestamp = DateTimeOffset.UtcNow.AddMinutes(-10), Metrics = ProjectMetrics.ForDiscordBot(serverCount: 419) },
    new MetricsBatchEntry { Timestamp = DateTimeOffset.UtcNow, Metrics = ProjectMetrics.ForDiscordBot(serverCount: 420) },
});
```

### Announcements

```cs
Announcement announcement = await api.CreateAnnouncementAsync(
    new AnnouncementRequest { Title = "Big update", Content = "Details…", Category = "new-feature" }
);
```

### Slash commands

```cs
await api.UpdateCommandsAsync(new[]
{
    new ApplicationCommand("ping", "Replies with pong"),
    new ApplicationCommand("repeat", "Repeats a message")
    {
        Options = new List<ApplicationCommandOption>
        {
            new ApplicationCommandOption
            {
                Type = 3, // STRING (Discord option type)
                Name = "text",
                Description = "The text to repeat",
                Required = true,
            },
        },
    },
});
```

You can also pass a raw JSON array string if you prefer to keep the payload in config.

### Votes

```cs
// Cursor-based pagination
VotePage page = await api.GetVotesAsync(cursor: null);
while (!page.IsEmpty)
{
    foreach (Vote vote in page.Data)
        Console.WriteLine($"User {vote.UserId} voted with weight {vote.Weight} at {vote.CreatedAt}");

    page = await api.GetVotesAsync(page.Cursor);
}

// Or by start date (not older than one year)
VotePage since = await api.GetVotesAsync(DateTimeOffset.UtcNow.AddDays(-7));

// Stream all votes since a date
await foreach (Vote vote in api.GetAllVotesAsync(DateTimeOffset.UtcNow.AddDays(-7)))
    Console.WriteLine(vote.UserId);

// Did a user vote?
bool voted = await api.HasVotedAsync(123456789012345678UL);
```

`GetVoteStatusAsync`/`HasVotedAsync` return `null`/`false` when the user has no active vote (`GET /projects/@me/votes/:userId` returns `404`).

### Errors

On a non-success response the client throws `TopggApiException` with the RFC 7807 problem details and, for `429 Too Many Requests`, the `Retry-After` delay:

```cs
try
{
    await api.UpdateMetricsAsync(metrics);
}
catch (TopggApiException e)
{
    Console.WriteLine($"HTTP {(int)e.StatusCode} {e.StatusCode}: {e.Problem?.Detail}");
    if (e.RetryAfter is TimeSpan retryAfter)
        Console.WriteLine($"Rate limited, retry after {retryAfter}");
}
```

## Webhooks

V1 webhooks deliver `vote.create` and `webhook.test` events as signed JSON POST requests. The **webhook secret** (starts with `whs_`) is shown when you create the webhook and is used to verify the `x-topgg-signature` header, which looks like `t=<unix-ts>,v1=<hmac-sha256>`.

```cs
using DiscordBotsList.Api.Webhooks;

bool valid = TopggWebhook.VerifySignature(
    webhookSecret: "whs_…",
    rawBody: rawBody,
    signatureHeader: headers["x-topgg-signature"],
    maxAge: TimeSpan.FromMinutes(5)   // optional anti-replay window
);
```

Or parse and verify in one step:

```cs
TopggWebhookEvent webhookEvent = TopggWebhook.ParseAndVerify("whs_…", rawBody, signatureHeader);

if (webhookEvent.IsVoteCreate)
{
    VoteWebhookData vote = webhookEvent.GetVoteData();
    Console.WriteLine($"{vote.User.Name} ({vote.User.Id}) voted on {vote.Project.Name}, weight {vote.Weight}");
}
else if (webhookEvent.IsWebhookTest)
{
    Console.WriteLine("Webhook works!");
}
```

The sender only retries if your endpoint does not answer with `2xx` within 5 seconds, so respond quickly.

Example for a Kestrel `HttpListener`-style server is trivial: read the request body, pass the raw body (not a re-encoded string) and the `x-topgg-signature` header to `ParseAndVerify`.

## Discord.Net integration

Install `DiscordBotsList.Api.Adapter.Discord.Net` to auto-post stats on a timer.

### v1 (Discord bots)

```cs
using DiscordBotsList.Api.Adapter.Discord.Net;
using Discord.WebSocket;

DiscordNetTopggV1Api api = client.CreateTopggV1Api(token);
IAdapter listener = api.CreateListener();   // posts server + shard count every 30 minutes
listener.Start();
```

`DiscordNetTopggV1Api` is only for Discord bot projects and posts server + shard count via `PATCH /projects/@me/metrics`. For other metrics and endpoints, use `TopggV1Client` directly.

### Legacy v0 (Discord bots)

```cs
DiscordNetDblApi api = client.CreateDblApi(dblToken);
IAdapter listener = api.CreateListener();
listener.Start();
```

## Legacy v0 API

> The v0 API is deprecated by top.gg. Prefer the v1 API above. The v0 endpoints still work with a plain API token (no `Bearer` prefix).

```cs
using DiscordBotsList.Api;

var dblApi = new AuthDiscordBotListApi(BOT_DISCORD_ID, YOUR_TOKEN);
IDblSelfBot me = await dblApi.GetMeAsync();
await me.UpdateStatsAsync(guildCount: 2133);                    // unsharded
await me.UpdateStatsAsync(shardCount: 8, totalShards: 8, shards: new[] { 12, 421, 62, 241 });
bool hasVoted = await me.HasVotedAsync(userId);
```

Public, unauthenticated lookups still work:

```cs
var api = new DiscordBotListApi();
IDblBot bot = await api.GetBotAsync(160105994217586689);
IDblUser user = await api.GetUserAsync(121919449996460033);
```

## Widgets

```cs
string widgetUrl = new SmallWidgetOptions()
    .SetType(WidgetType.OWNER)
    .SetLeftColor(255, 255, 255)
    .Build(160105994217586689);
```

## License

MIT
