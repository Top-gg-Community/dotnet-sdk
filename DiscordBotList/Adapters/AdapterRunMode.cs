namespace DiscordBotList.Adapters
{
    /// <summary>
    /// Defines the run mode for an <see cref="IAdapter"/>.
    /// </summary>
    public enum AdapterRunMode
    {
        /// <summary>
        /// Tells the <see cref="IAdapter"/> to run when an event is fired.
        /// </summary>
        Sync = 1,

        /// <summary>
        /// Tells the <see cref="IAdapter"/> to run on an asynchronous interval.
        /// </summary>
        Async = 2
    }
}
