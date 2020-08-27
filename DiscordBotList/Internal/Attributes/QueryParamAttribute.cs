using System;

namespace DiscordBotList.Internal
{
    /// <summary>
    /// Marks a property to be included as a query argument for a URL.
    /// </summary>
    internal class QueryParamAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new <see cref="QueryParamAttribute"/>.
        /// </summary>
        /// <param name="name">The query key name to be set.</param>
        /// <param name="format">The format to apply on the value of this query (only if the value inherits <see cref="IFormattable"/>).</param>
        /// <param name="toLower">The casing format to apply on the value of this query.</param>
        internal QueryParamAttribute(string name = null, string format = null, bool toLower = true)
        {
            Name = name;
            Format = format;
            ToLower = toLower;
        }

        /// <summary>
        /// Gets the name of the query to be used.
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// Gets the format to apply on the query value (only if the value inherits <see cref="IFormattable"/>.
        /// </summary>
        internal string Format { get; }

        /// <summary>
        /// Gets the casing format to apply on the query value.
        /// </summary>
        internal bool ToLower { get; }
    }
}
