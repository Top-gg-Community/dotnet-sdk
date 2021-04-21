using System;
using System.Threading.Tasks;

namespace DiscordBotsList.Api.Objects
{
    public interface IAdapter
    {
        event Action<string> Log;

        Task RunAsync();

        void Start();

        void Stop();
    }
}