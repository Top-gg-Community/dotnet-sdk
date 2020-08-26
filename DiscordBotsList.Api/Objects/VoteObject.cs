using Newtonsoft.Json;

namespace DiscordBotsList.Api.Objects
{
    /// <summary>
    /// Represents a generic vote object.
    /// </summary>
    public readonly struct VoteObject
    {
        internal VoteObject(int voted)
        {
            Vote = voted;
        }

        [JsonProperty("voted")]
        private int Vote { get; }

        /// <summary>
        /// Represents the status of this <see cref="VoteObject"/>.
        /// </summary>
        public bool Voted => Vote == 1;
    }
}
