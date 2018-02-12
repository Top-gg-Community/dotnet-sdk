using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotsList.Api
{
    public interface ISearchResult<T>
    {
		/// <summary>
		/// Items returned from search
		/// </summary>
		List<T> Items { get; }

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
		int TotalItems { get; }

		/// <summary>
		/// Total amount of pages found in the search
		/// </summary>
		int TotalPages { get; }
	}
}
