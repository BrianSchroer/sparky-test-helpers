using System;
using System.Collections.Generic;
using SparkyTools.DependencyProvider;

namespace SparkyTestHelpers.AppSettings
{
    /// <summary>
    /// AppSettings <see cref="DependencyProvider"/> helper methods.
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Creates <see cref="DependencyProvider"/> for supplying test configuration values from a dictionary.
        /// </summary>
        /// <param name="dictionary">The <see cref="Dictionary{String, StringComparer}"/></param>
        /// <returns>New <see cref="DependencyProvider{Func{String, String}}"/> instance.</returns>
        public static DependencyProvider<Func<string, string>> TestDependencyProvider(Dictionary<string, string> dictionary)
        {
            return new DependencyProvider<Func<string, string>>(
                key => (dictionary.ContainsKey(key)) ? dictionary[key] : null
                ).Static();
        }
    }
}
