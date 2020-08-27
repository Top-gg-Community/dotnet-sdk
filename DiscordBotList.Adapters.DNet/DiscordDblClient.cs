using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBotList.Models;

namespace DiscordBotList.Adapters.DNet
{
    /// <summary>
    /// Represents an authenticated Discord Bot List API client with an <see cref="IDiscordClient"/> reference.
    /// </summary>
    public class DiscordDblClient : DblClient
    {
        /// <summary>
        /// Initializes a new <see cref="DiscordDblClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="IDiscordClient"/> that this <see cref="DiscordDblClient"/> will reference.</param>
        /// <param name="token">The token used to access the Discord Bot List API.</param>
		public DiscordDblClient(IDiscordClient client, string token)
            : base(client.CurrentUser.Id, token)
		{
            Client = client;
		}

        protected IDiscordClient Client { get; }

        /// <summary>
        /// Returns the bot specified.
        /// </summary>
        /// <param name="user">The user that you wish to retrieve.</param>
        /// <returns>A generic <see cref="IDblBot"/> object.</returns>
		public async Task<IDblBot> GetBotAsync(IUser user)
			=> await GetBotAsync(user.Id);

        /// <summary>
        /// Returns the user specified.
        /// </summary>
        /// <param name="user">The user that you wish to retrieve.</param>
        /// <returns>A generic <see cref="IDblUser"/> object.</returns>
		public async Task<IDblUser> GetUserAsync(IUser user)
			=> await GetUserAsync(user.Id);

        /// <summary>
        /// Creates an <see cref="IAdapter"/> that update your guild count when <see cref="IAdapter.UpdateAsync"/> is invoked.
        /// </summary>
        /// <param name="interval">The interval that this <see cref="IAdapter"/> will update. Leave empty to update on each guild event.</param>
        /// <param name="runMode">The run mode that this <see cref="IAdapter"/> should run as.</param>
        /// <returns>A new <see cref="IAdapter"/> instance that will update your guild count when <see cref="IAdapter.UpdateAsync"/> is invoked.</returns>
        public virtual IAdapter CreateListenAdapter(TimeSpan? interval = null, AdapterRunMode runMode = AdapterRunMode.Sync)
        {
            if (Client is BaseSocketClient socketClient)
                return new SocketSubmissionAdapter(this, socketClient, new AdapterProperties(interval ?? TimeSpan.Zero, runMode));

            return new SubmissionAdapter(this, Client, new AdapterProperties(interval ?? TimeSpan.Zero, runMode));
        }
    }
}
