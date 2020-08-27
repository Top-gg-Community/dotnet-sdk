namespace DiscordBotList
{
    /// <summary>
    /// Represents a basic <see cref="DblField"/> sort parameter.
    /// </summary>
    public class FieldSort
	{
        /// <summary>
        /// Initializes an empty <see cref="FieldSort"/>.
        /// </summary>
		public FieldSort() { }

        /// <summary>
        /// Initializes a new <see cref="FieldSort"/>.
        /// </summary>
        /// <param name="field">The <see cref="DblField"/> to sort by.</param>
        /// <param name="reverse">The direction at which this <see cref="FieldSort"/> is applied (default is A to Z).</param>
        public FieldSort(DblField field, bool reverse = false)
        {
            Field = field;
            Reverse = reverse;
        }

        /// <summary>
        /// Gets or sets the <see cref="DblField"/> to sort by.
        /// </summary>
		public DblField Field { get; set; }

        /// <summary>
        /// Gets or sets the direction at which this <see cref="FieldSort"/> is applied.
        /// </summary>
		public bool Reverse { get; set; }
	}
}
