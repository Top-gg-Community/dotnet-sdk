using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordBotsList.Api.Adapter.Discord.Net
{
    internal class ShardedSubmissionAdapter : SubmissionAdapter
	{
        public ShardedSubmissionAdapter(DblClient api, DiscordShardedClient client, TimeSpan interval)
            : base(api, client, interval)
        {

        }

		public override async Task RunAsync()
		{
			if (!(DiscordClient is DiscordShardedClient shardClient))
				throw new Exception("Expected a sharded client");

			if (DateTime.Now > LastUpdated + Interval)
			{
				await Client.UpdateStatsAsync(
					0,
                    shardClient.Shards.Count,
                    shardClient.Shards.Select(x => x.Guilds.Count).ToArray()
				);

				LastUpdated = DateTime.Now;
				SendLog("Submitted stats to DiscordBotsList.org");
			}
		}
	}
}