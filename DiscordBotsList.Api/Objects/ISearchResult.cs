using System.Collections.Generic;

namespace DiscordBotsList.Api.Objects
{
    public interface ISearchResult<T>
    {
		/// <summary>
		/// Items returned from search
		/// </summary>
		IEnumerable<T> Items { get; }

		/// <summary>
		/// The current page you've navigated
		/// </summary>
		int CurrentPage { get; }

		/// <summary>
		/// Set items per page
		/// </summary>
		int ItemsPerPage { get; }

		/// <summary>
		/// Total amount of items found in the search
		/// </summary>
		int ItemCount { get; }

		/// <summary>
		/// Total amount of pages found in the search
		/// </summary>
		int PageCount { get; }
	}
}
