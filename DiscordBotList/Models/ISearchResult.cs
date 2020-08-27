using System.Collections.Generic;

namespace DiscordBotList.Models
{
	/// <summary>
	/// Represents a generic search result.
	/// </summary>
	/// <typeparam name="T">The type that this <see cref="ISearchResult{T}"/> will return.</typeparam>
    public interface ISearchResult<out T>
    {
		/// <summary>
		/// Represents a collection of all of the retrieved values from this <see cref="ISearchResult{T}"/>.
		/// </summary>
		IReadOnlyList<T> Values { get; }

		/// <summary>
		/// Gets a 32-bit integer that defines the current page that this <see cref="ISearchResult{T}"/> is on.
		/// </summary>
		int PageIndex { get; }

		/// <summary>
		/// Gets a 32-bit integer that defines the total number of values that can appear on a page in this <see cref="ISearchResult{T}"/>.
		/// </summary>
		int PageSize { get; }

        /// <summary>
        /// Gets a 32-bit integer that defines the total number of pages in this <see cref="ISearchResult{T}"/>.
        /// </summary>
        int PageCount { get; }

		/// <summary>
		/// Gets a 32-bit integer that defines the total number of values found in this <see cref="ISearchResult{T}"/>.
		/// </summary>
		int ResultCount { get; }
    }
}
