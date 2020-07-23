using System;
using System.Threading.Tasks;
using Discord;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Adapter.Discord.Net
{
	internal class SubmissionAdapter : IAdapter
	{
		protected DblClient api;
		protected IDiscordClient client;
		protected TimeSpan updateTime;

		protected DateTime lastTimeUpdated;

		public SubmissionAdapter(DblClient api, IDiscordClient client, TimeSpan updateTime)
		{
			this.client = client;
			this.updateTime = updateTime;
		}

		public event Action<string> Log;

		public virtual async Task RunAsync()
		{
			if(DateTime.Now > lastTimeUpdated + updateTime)
			{
				await api.UpdateStatsAsync(
					(await client.GetGuildsAsync()).Count
				);

				lastTimeUpdated = DateTime.Now;
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