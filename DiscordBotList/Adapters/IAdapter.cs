using System;
using System.Threading.Tasks;

namespace DiscordBotList.Adapters
{
	/// <summary>
	/// Represents a generic adapter.
	/// </summary>
    public interface IAdapter : IDisposable
    {
        /// <summary>
        /// Fired when this <see cref="IAdapter"/> successfully updates.
        /// </summary>
		event Action<string> AdapterUpdated;

        /// <summary>
        /// Represents the <see cref="DateTime"/> at which this <see cref="IAdapter"/> was last updated (in UTC).
        /// </summary>
        DateTime LastUpdated { get; }

		/// <summary>
		/// Represents the <see cref="Task"/> used when an <see cref="IAdapter"/> is told to update.
		/// </summary>
		Task UpdateAsync();

        /// <summary>
        /// Starts or resumes this <see cref="IAdapter"/>.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this <see cref="IAdapter"/>.
        /// </summary>
		void Stop();
	}
}
