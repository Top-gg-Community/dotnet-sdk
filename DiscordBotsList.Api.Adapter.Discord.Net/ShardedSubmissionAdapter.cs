using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Adapter.Discord.Net
{
    internal class ShardedSubmissionAdapter : SubmissionAdapter, IAdapter
	{
		public ShardedSubmissionAdapter(DblClient api, DiscordShardedClient client, TimeSpan updateTime)
			: base(api, client, updateTime)
		{ }

		public override async Task RunAsync()
		{
			if (DateTime.Now > lastTimeUpdated + updateTime)
			{
				await api.UpdateStatsAsync(
					0, 
					(client as DiscordShardedClient).Shards.Count, 
					(client as DiscordShardedClient).Shards.Select(x => x.Guilds.Count).ToArray()
				);

				lastTimeUpdated = DateTime.Now;
				SendLog("Submitted stats to DiscordBotsList.org");
			}
		}
	}
}