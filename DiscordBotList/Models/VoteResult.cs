using Newtonsoft.Json;

namespace DiscordBotList.Models
{
    /// <summary>
    /// Represents the result of a vote request.
    /// </summary>
    public readonly struct VoteResult
    {
        internal VoteResult(int voted)
        {
            Vote = voted;
        }

        [JsonProperty("voted")]
        private int Vote { get; }

        /// <summary>
        /// Represents the status of this <see cref="VoteResult"/>.
        /// </summary>
        public bool Voted => Vote == 1;
    }
}
