using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Adapter.Discord.Net
{
	public static class DiscordNetDblUtils
	{
		public static DiscordNetDblApi CreateDblApi(this DiscordSocketClient client, string dblToken)
			=> new DiscordNetDblApi(client, dblToken);

		public static ShardedDiscordNetDblApi CreateDblApi(this DiscordShardedClient client, string dblToken)
			=> new ShardedDiscordNetDblApi(client, dblToken);
	}

    public class DiscordNetDblApi : DblClient
    {
		protected IDiscordClient client;

		public DiscordNetDblApi(IDiscordClient client, string dblToken) : base(client.CurrentUser.Id, dblToken)
		{
			this.client = client;
		}

		public async Task<IDblBot> GetBotAsync(IUser user)
			=> await GetBotAsync(user.Id);

		public async Task<IDblUser> GetUserAsync(IUser user)
			=> await GetUserAsync(user.Id);

		/// <summary>
		/// Creates an IAdapter that updates your servercount on RunAsync().
		/// </summary>
		/// <param name="client">Your already connected client</param>
		/// <param name="updateTime">Timespan for when you want to submit guildcount, leave null if you want it every JoinedGuild event</param>
		/// <returns>an IAdapter that updates your servercount on RunAsync(), does not automatically do it yet.</returns>
		/// <seealso cref="ListenAsync()"/>
		public virtual IAdapter CreateListener(TimeSpan? updateTime = null)
			=> new SubmissionAdapter(this, client, updateTime ?? TimeSpan.Zero);
	}

	public class ShardedDiscordNetDblApi : DiscordNetDblApi
	{
		public ShardedDiscordNetDblApi(DiscordShardedClient client, string dblToken) : base(client, dblToken)
		{ }

		/// <summary>
		/// Creates an IAdapter that updates your servercount on RunAsync().
		/// </summary>
		/// <param name="client">Your already connected client</param>
		/// <param name="updateTime">Timespan for when you want to submit guildcount, leave null if you want it every JoinedGuild event</param>
		/// <returns>an IAdapter that updates your servercount on RunAsync(), does not automatically do it yet.</returns>
		/// <seealso cref="ListenAsync()"/>
		public override IAdapter CreateListener(TimeSpan? updateTime = null)
			=> new ShardedSubmissionAdapter(this, client as DiscordShardedClient, updateTime ?? TimeSpan.Zero);
	}
}
