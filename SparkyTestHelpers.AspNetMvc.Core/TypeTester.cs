using System;
using System.Reflection;

namespace SparkyTestHelpers.AspNetMvc.Core
{
    /// <summary>
    /// <see cref="Type"/> tester.
    /// </summary>
    public static class TypeTester
    {
        /// <summary>
        /// Is object of expected type?
        /// </summary>
        /// <param name="obj">The object to be tested.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <returns><c>true</c> if the object is of the expected type; otherwise, <c>false</c>.</returns>
        public static bool IsOfType(object obj, Type expectedType)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            Type actualType = obj.GetType();
            return actualType == expectedType || actualType.GetTypeInfo().IsSubclassOf(expectedType);
        }
    }
}
