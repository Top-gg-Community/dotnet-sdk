using System;

namespace DiscordBotList.Models
{
	/// <summary>
	/// Defines a collection of unique statuses for an <see cref="IDblUser"/>.
	/// </summary>
	[Flags]
    public enum DblStatus
	{
		/// <summary>
		/// Marks this <see cref="DblStatus"/> with a supporter flag.
		/// </summary>
		Supporter = 1,

        /// <summary>
        /// Marks this <see cref="DblStatus"/> with a certified flag.
        /// </summary>
		Certified = 2,

        /// <summary>
        /// Marks this <see cref="DblStatus"/> with a moderator flag.
        /// </summary>
		Moderator = 4,

        /// <summary>
        /// Marks this <see cref="DblStatus"/> with a web moderator flag.
        /// </summary>
		WebModerator = 8,

        /// <summary>
        /// Marks this <see cref="DblStatus"/> with an administrator flag.
        /// </summary>
		Admin = 16
	}
}
