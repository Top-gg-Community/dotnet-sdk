# Top.gg .NET SDK

The community-maintained .NET SDK for Top.gg.

## Chapters

- [Installation](#installation)
- [Setting up](#setting-up)
- [Usage](#usage)
  - [Getting your project's information](#getting-your-projects-information)
  - [Getting your project's vote information of a user](#getting-your-projects-vote-information-of-a-user)
  - [Getting a cursor-based paginated list of votes for your project](#getting-a-cursor-based-paginated-list-of-votes-for-your-project)
  - [Posting your bot's application commands list](#posting-your-bots-application-commands-list)
  - [Generating widget URLs](#generating-widget-urls)
  - [Webhooks](#webhooks)

## Installation

### Main API wrapper

```console
$ dotnet add package Topgg.Sdk.Api --version 1.0.0
```

### Webhooks only

```console
$ dotnet add package Topgg.Sdk.Webhooks --version 1.0.0
```

## Setting up

```cs
using Topgg.Sdk.Api;

var client = new TopggApi(Environment.GetEnvironmentVariable("TOPGG_TOKEN"));
```

## Usage

### Getting your project's information

```cs
var project = await client.GetSelfAsync();
```

### Getting your project's vote information of a user

#### Discord ID

```cs
using Topgg.Sdk.Api.Data;

var vote = await client.GetVoteAsync(661200758510977084);
```

#### Top.gg ID

```cs
using Topgg.Sdk.Api.Data;

var vote = await client.GetVoteAsync(8226924471638491136, UserSource.Topgg);
```

### Getting a cursor-based paginated list of votes for your project

```cs
var since = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

var firstPage = await client.GetVotesAsync(since);

foreach (var vote in firstPage.Votes)
{
    // ...
}

var secondPage = await firstPage.Next();

foreach (var vote in secondPage.Votes)
{
    // ...
}
```

### Posting your bot's application commands list

#### Discord.Net

```cs
var commands = $"[{string.Join(",", (await bot.GetGlobalApplicationCommandsAsync()).Select(command => command.ToJson()))}]";

await client.PostCommandsAsync(commands);
```

#### Raw

```cs
// Array of application commands that
// can be serialized to Discord API's raw JSON format.
var commands = @"[
  {
    ""options"": [],
    ""name"": ""test"",
    ""name_localizations"": null,
    ""description"": ""command description"",
    ""description_localizations"": null,
    ""contexts"": [],
    ""default_permission"": null,
    ""default_member_permissions"": null,
    ""dm_permission"": false,
    ""integration_types"": [],
    ""nsfw"": false
  }
]";

await client.PostCommandsAsync(commands);
```

### Generating widget URLs

#### Large

```cs
using Topgg.Sdk.Api.Data;
using Topgg.Sdk.Api;

var widgetUrl = Widget.Large(Platform.Discord, ProjectType.Bot, 1026525568344264724);
```

#### Votes

```cs
using Topgg.Sdk.Api.Data;
using Topgg.Sdk.Api;

var widgetUrl = Widget.Votes(Platform.Discord, ProjectType.Bot, 1026525568344264724);
```

#### Owner

```cs
using Topgg.Sdk.Api.Data;
using Topgg.Sdk.Api;

var widgetUrl = Widget.Owner(Platform.Discord, ProjectType.Bot, 1026525568344264724);
```

#### Social

```cs
using Topgg.Sdk.Api.Data;
using Topgg.Sdk.Api;

var widgetUrl = Widget.Social(Platform.Discord, ProjectType.Bot, 1026525568344264724);
```

### Webhooks

With ASP.NET Core:

```cs
using Topgg.Sdk.Webhooks.Payloads;
using Topgg.Sdk.Webhooks;

public class Webhooks() : WebhookEventListener(Environment.GetEnvironmentVariable("TOPGG_WEBHOOK_SECRET"))
{
    // Optional
    public override Task OnIntegrationCreate(HttpContext context, IntegrationCreatePayload payload, string trace) => DefaultResponse(context);

    // Optional
    public override Task OnIntegrationDelete(HttpContext context, IntegrationDeletePayload payload, string trace) => DefaultResponse(context);

    // Optional
    public override Task OnTest(HttpContext context, TestPayload payload, string trace) => DefaultResponse(context);

    // Optional
    public override Task OnVoteCreate(HttpContext context, VoteCreatePayload payload, string trace) => DefaultResponse(context);

    private static async Task DefaultResponse(HttpContext context)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = 204;
        }
    }
}
```

Later, in your server's setup:

```cs
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var webhooks = new CustomWebhooks();

// POST /webhook
app.MapPost("/webhook", webhooks.Handler);

app.Run();
```