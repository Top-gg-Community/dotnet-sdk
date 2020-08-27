using System.Collections.Generic;

namespace DiscordBotList
{
    /// <summary>
    /// Represents a collection of parameters for a search request.
    /// </summary>
    public class BotSearchParams
    {
        /// <summary>
        /// Represents the max limit for this <see cref="BotSearchParams"/>.
        /// </summary>
        public const int MaxLimit = 500;

        /// <summary>
        /// Represents the default limit for this <see cref="BotSearchParams"/>.
        /// </summary>
        public const int DefaultLimit = 50;

        /// <summary>
        /// Represents the default offset for this <see cref="BotSearchParams"/>.
        /// </summary>
        public const int DefaultOffset = 0;

        /// <summary>
        /// Gets or sets a 32-bit integer that represents the upper bound of this <see cref="BotSearchParams"/>.
        /// </summary>
		public int Limit { get; set; } = MaxLimit;

        /// <summary>
        /// Gets or sets a 32-bit integer that represents the offset of this <see cref="BotSearchParams"/>.
        /// </summary>
		public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the sort parameter of this <see cref="BotSearchParams"/>.
        /// </summary>
        public FieldSort Sort { get; set; }

        /// <summary>
        /// Gets or sets the collection of search filters for each <see cref="DblField"/>.
        /// </summary>
        public List<FieldSearch> SearchFilters { get; set; } = new List<FieldSearch>();

        /// <summary>
        /// Gets or sets the collection of fields to retrieve on each successful result.
        /// </summary>
        public List<DblField> Fields { get; set; } = new List<DblField>();
	}
}
