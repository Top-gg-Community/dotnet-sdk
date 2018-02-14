using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBotsList.Api
{
    public interface IAdapter
    {
		event Action<string> Log;

		Task RunAsync();

		void Start();
		void Stop();
	}
}
