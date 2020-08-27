using Newtonsoft.Json;

namespace DiscordBotList.Models
{
    /// <summary>
    /// Represents the result of a weekend request.
    /// </summary>
    public struct WeekendResult
    {
        internal WeekendResult(bool isWeekend)
        {
            IsWeekend = isWeekend;
        }

        /// <summary>
        /// Represents the status of this <see cref="WeekendResult"/>.
        /// </summary>
        [JsonProperty("is_weekend")]
		public bool IsWeekend { get; internal set; }
    }
}
