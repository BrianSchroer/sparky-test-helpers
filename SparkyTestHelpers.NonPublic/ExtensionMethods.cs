namespace SparkyTestHelpers.NonPublic
{
    /// <summary>
    /// Extension methods for invoking private, protected and internal class members from unit tests.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Creates <see cref="NonPublic"/> instance for <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">A class instance.</param>
        /// <returns>New <see cref="NonPublic"/> instance.</returns>
        public static NonPublic<T> NonPublic<T>(this T instance) => new NonPublic<T>(instance);
    }
} 