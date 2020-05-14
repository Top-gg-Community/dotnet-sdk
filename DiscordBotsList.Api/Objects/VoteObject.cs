using Newtonsoft.Json;

namespace DiscordBotsList.Api.Objects
{
    public struct VoteObject
    {
        internal VoteObject(int voted)
        {
            _voted = voted;
        }

        [JsonProperty("voted")]
        private int _voted { get; }

        public bool Voted => _voted == 1;
    }
}
