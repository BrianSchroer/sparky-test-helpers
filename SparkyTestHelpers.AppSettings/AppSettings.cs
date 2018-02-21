using System;
using System.Collections.Generic;
using SparkyTools.DependencyProvider;

namespace SparkyTestHelpers.AppSettings
{
    /// <summary>
    /// AppSettings test helper methods.
    /// </summary>
    public static class AppSettings
    {
        public static DependencyProvider<Func<string, string>> TestDependencyProvider(Dictionary<string, string> dictionary)
        {
            return new DependencyProvider<Func<string, string>>(
                key => (dictionary.ContainsKey(key)) ? dictionary[key] : null
                ).Static();
        }
    }
}
