using System;

namespace DiscordBotList.Adapters
{
    /// <summary>
    /// Represents a collection of properties for an <see cref="IAdapter"/>.
    /// </summary>
    public class AdapterProperties
    {
        /// <summary>
        /// Gets the default value for this <see cref="AdapterProperties"/>.
        /// </summary>
        public static readonly AdapterProperties Default = new AdapterProperties
        {
            RunMode = AdapterRunMode.Sync,
            Message = "Updated stats to top.gg.",
            Interval = TimeSpan.FromMinutes(1)
        };

        /// <summary>
        /// Initializes an empty <see cref="AdapterProperties"/>.
        /// </summary>
        public AdapterProperties() { }

        /// <summary>
        /// Initializes a new <see cref="AdapterProperties"/>.
        /// </summary>
        /// <param name="interval">The interval at which an <see cref="IAdapter"/> should update.</param>
        /// <param name="runMode">The run mode for an <see cref="IAdapter"/>.</param>
        /// <param name="message">The message that is sent whenever an <see cref="IAdapter"/> is updated.</param>
        public AdapterProperties(TimeSpan interval, AdapterRunMode runMode = AdapterRunMode.Sync, string message = null)
        {
            Interval = interval;
            RunMode = runMode;
            Message = message ?? Default.Message;
        }

        /// <summary>
        /// Gets or sets the interval of this <see cref="IAdapter"/>.
        /// </summary>
        /// <remarks>
        /// If the run mode is specified as <see cref="AdapterRunMode.Sync"/>,
        /// the <see cref="IAdapter"/> will attempt to update on each guild update event.
        /// If the previous update time is shorter than the specified interval, the <see cref="IAdapter"/> will not update.
        ///
        /// If the run mode is specified as <see cref="AdapterRunMode.Async"/>,
        /// the <see cref="IAdapter"/> will fire at an automated interval specified, regardless of update events,
        /// using a <see cref="System.Threading.Timer"/> to fire on each interval.
        /// </remarks>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// Gets or sets the message that will be sent through the log upon each <see cref="IAdapter"/> update.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the run mode for this <see cref="IAdapter"/>.
        /// </summary>
        public AdapterRunMode RunMode { get; set; }
    }
}
