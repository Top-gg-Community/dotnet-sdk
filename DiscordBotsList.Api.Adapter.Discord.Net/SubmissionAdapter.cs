using System;
using System.Threading.Tasks;
using Discord;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Adapter.Discord.Net
{
    public enum AdapterRunMode
    {
		/// <summary>
		/// Tells the <see cref="IAdapter"/> to run when an event is fired.
		/// </summary>
		Sync = 1,

		/// <summary>
		/// Tells the <see cref="IAdapter"/> to run on an asynchronous interval.
		/// </summary>
		Async = 2
    }

    public class AdapterProperties
    {
        public static readonly AdapterProperties Default = new AdapterProperties
        {
			RunMode = AdapterRunMode.Sync,
			Message = "Updated stats to top.gg."
        };

		public TimeSpan Interval { get; set; }

		public string Message { get; set; }

		public AdapterRunMode RunMode { get; set; }
    }

	internal class SubmissionAdapter : IAdapter
	{
		protected DblClient Client;
		protected IDiscordClient DiscordClient;
		protected TimeSpan Interval;

		protected DateTime LastUpdated;

		// System.Threading.Tasks.Timer

		public SubmissionAdapter(DblClient client, IDiscordClient discordClient, TimeSpan interval)
        {
            Client = client;
			DiscordClient = discordClient;
			Interval = interval;
		}

		public event Action<string> Log;

		public virtual async Task RunAsync()
		{
			if(DateTime.Now > LastUpdated + Interval)
			{
				await Client.UpdateStatsAsync(
					(await DiscordClient.GetGuildsAsync()).Count
				);

				LastUpdated = DateTime.Now;
				SendLog("Submitted stats to DiscordBotsList.org");
			}
		}

		public virtual void Start()
		{

        }

		public virtual void Stop()
		{
			throw new NotImplementedException();
		}

		protected void SendLog(string msg) 
			=> Log?.Invoke(msg);
	}
}