namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// Simple query string parameter key/value struct.
    /// </summary>
    public struct QueryStringParameter
    {
        /// <summary>
        /// Query string parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Query string parameter value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Creates a new <see cref="QueryStringParameter" instance.
        /// </summary>
        /// <param name="name"><see cref="Name" /> value.</param>
        /// <param name="value"><see cref="Value" /> value.</param>
        public QueryStringParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Creates a new <see cref="QueryStringParameter" instance.
        /// </summary>
        /// <param name="name"><see cref="Name" /> value.</param>
        /// <param name="value"><see cref="Value" /> value.</param>
        public QueryStringParameter(string name, object value)
        {
            Name = name;
            Value = (value is null) ? null : value.ToString();
        }
    }
}
