# DiscordBotList.Net
[![NuGet](https://img.shields.io/nuget/vpre/DiscordBotsList.Api.svg?maxAge=2592000?style=plastic)](https://www.nuget.org/packages/DiscordBotsList.Api)

This is an API wrapper for [top.gg](https://top.gg/) (previously Discord Bot List), written in C# for .NET.

## Unauthorized Usage
If you don't have an API token, you can only initialize an unauthenticated API client.

### Setting up the API client
Initializing a new API client that is unauthorized is simple.
```cs
BaseDblClient dblClient = new BaseDblClient();
```

### Searching
Using the API client, you can now search for a specific bot by specifying its Discord ID.
```cs
IDblBot bot = await dblClient.GetBotAsync(160105994217586689);
```

Likewise, you can also search for users by specifying their Discord ID.
```cs
IDblUser bot = await dblClient.GetUserAsync(121919449996460033);
```

## Authorized Usage
If you have an API key from top.gg, you may initialize an authenticated API client instead.

### Setting up the API client
```cs
DblClient dblClient = new DblClient(BOT_DISCORD_ID, API_TOKEN);
```

### Updating stats
You can get the instance of your bot for the API as so:
```cs
IDblSelfBot self = await dblClient.GetSelfAsync();
```

If you are currently running a bot that does not utilize shards, you can simply specify the guild count.
```cs
await self.UpdateStatsAsync(2133);
```

Otherwise, if you are running a bot that utilizes shards, the way you update your stats is approached a little bit differently.
When updating stats, you would have to specify the index at which you are beginning to update your shards, the current shard count your bot has, and an array containing the individual guild counts for each shard starting at the index.
```cs
await self.UpdateStatsAsync(24, 50, new[] { 12, 421, 62, 241, 524, 534 });
```

In this example, this bot has 50 shards. I want to update the shards starting at shard 24, and proceed from there. In this case, shard 24 now has 12 guilds, shard 25 has 421 guilds, shard 26 has 62 guilds, and so forth.


### Generating widgets
Widgets can also be generated using this API. By simply specifying the configuration for the widget, you can easily generate widgets by building the properties with a specified ID.
```cs
string widgetUrl = new SmallWidgetOptions()
	.SetType(WidgetType.OWNER)
	.SetLeftColor(255, 255, 255);
	.Build(160105994217586689);
```

In this example, the following code snippet will generate a widget URL that looks like this:

![Widget Example](https://top.gg/api/widget/status/160105994217586689.svg?leftcolor=FFFFFF)

This also supports large widgets.
```cs
string largeWidgetUrl = new LargeWidgetOptions()
	.SetTopColor(255, 255, 255)
	.SetMiddleColor(0, 0, 0)
	.SetUsernameColor(0, 0, 0)
	.Build(160105994217586689);
```

The following code snippet will generate a large widget URL that looks like this:

![Large Widget Example](https://top.gg/api/widget/160105994217586689.svg?topcolor=FFFFFF&middlecolor=000000&usernamecolor=000000)

### Installing

#### Visual Studio
You can install this library for a solution or project by right-clicking the main solution, and selecting `Manage NuGet packages...`. In here, you can now search for `DiscordBotsList.Api`, and press the first available entry to install.

#### NuGet
You can install this package from NuGet [here](https://www.nuget.org/packages/DiscordBotsList.Api)! Likewise, if you wish to install this by console, you can simply type:
```
> Install-Package DiscordBotsList.Api
```
