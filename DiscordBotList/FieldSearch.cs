namespace DiscordBotList
{
	/// <summary>
	/// Represents a <see cref="DblField"/> search filter.
	/// </summary>
    public class FieldSearch
	{
		/// <summary>
		/// Initializes an empty <see cref="FieldSearch"/>.
		/// </summary>
		public FieldSearch() { }

		/// <summary>
		/// Initializes a new <see cref="FieldSearch"/>.
		/// </summary>
		/// <param name="field">The <see cref="DblField"/> to filter.</param>
		/// <param name="value">The value that this <see cref="DblField"/> should contain.</param>
        public FieldSearch(DblField field, string value)
        {
            Field = field;
            Value = value;
        }

		/// <summary>
		/// Gets or sets the <see cref="DblField"/> to filter.
		/// </summary>
		public DblField Field { get; set; }

		/// <summary>
		/// Gets or sets the value that this <see cref="DblField"/> should contain.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Returns a string that represents the query parameter of this <see cref="FieldSearch"/>.
		/// </summary>
		public override string ToString()
		    => string.IsNullOrWhiteSpace(Value)
                ? ""
                : $"{Field.ToString().ToLower()}: {Value}";
	}
}
