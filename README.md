# DBL-dotnet-Library
top.gg botlist wrapper

## Usage
### Unauthorized api usage
#### Setting up
```cs
DiscordBotListApi DblApi = new DiscordBotListApi();
```

#### Getting bots
```cs
//                            discord id
IBot bot = DblApi.GetBotAsync(160105994217586689);
```

#### Getting users
```cs
//                              discord id
IUser bot = DblApi.GetUserAsync(121919449996460033);
```

### Authorized api usage
#### Setting up
```cs
AuthDiscordBotListApi DblApi = new AuthDiscordBotListApi(BOT_DISCORD_ID, YOUR_TOKEN);
```

#### Updating stats
```cs
IDblSelfBot me = await DblApi.GetMeAsync();
// Update stats sharded   indexShard shardCount shards
await me.UpdateStatsAsync(24,        50,        new[] { 12, 421, 62, 241, 524, 534 });

// Update stats           guildCount
await me.UpdateStatsAsync(2133);
```

#### Widgets
```cs
string widgetUrl = new SmallWidgetOptions()
	.SetType(WidgetType.OWNER)
	.SetLeftColor(255, 255, 255);
	.Build(160105994217586689);
```

Generates ![](https://top.gg/api/widget/status/160105994217586689.svg?leftcolor=FFFFFF)

### Download
#### Nuget
If you're using Nuget you can use find it with the ID `DiscordBotsList.Api` or use
> Install-Package DiscordBotsList.Api
