namespace SparkyTestHelpers.Mapping
{
    /// <summary>
    /// This class is for testing that properties were successfully "mapped" from one type to another.
    /// </summary>
    public static class MapTester
    {
        /// <summary>
        /// "Factory" method that creates a new <see cref="MapTester{TSource, TDestination}"/> 
        /// instance and initializes it to automatically test properties that have the 
        /// same name on the "from" and "to"
        /// <typeparamref name="TSource"/> and <typeparamref name="TDestination"/> types.
        /// </summary>
        /// <typeparam name="TSource">The "map from" type.</typeparam>
        /// <typeparam name="TDestination">The "map to" type.</typeparam>
        /// <returns>New <see cref="MapTester{TSource, TDestination}"/> instance.</returns>
        /// <example>
        /// <code><![CDATA[
        ///     MapTester
        ///         .ForMap<Foo, Bar>()
        ///         .AssertMappedValues(foo, bar);
        /// ]]>
        /// </code>
        /// </example>
        public static MapTester<TSource, TDestination> ForMap<TSource, TDestination>()
        {
            return new MapTester<TSource, TDestination>();
        }
    }
}