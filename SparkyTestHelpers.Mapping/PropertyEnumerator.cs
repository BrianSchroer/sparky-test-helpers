using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SparkyTestHelpers.Mapping
{
    /// <summary>
    /// Get public instance properties with getters and setters for specified type.
    /// </summary>
    internal class PropertyEnumerator
    {
        /// <summary>
        /// Get public instance properties with getters and setters for specified type.
        /// </summary>
        /// <param name="typ">The type.</param>
        /// <returns>Array of properties.</returns>
        public static PropertyInfo[] GetPublicInstanceReadWriteProperties(Type typ)
        {
            return GetProperties(typ, BindingFlags.Public | BindingFlags.Instance)
                .Where(propertyInfo => propertyInfo.CanRead && propertyInfo.CanWrite)
                .ToArray();
        }

        /// <summary>
        /// Get properties for specified type.
        /// </summary>
        /// <param name="typ">The type.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/>.</param>
        /// <returns>Array of properties.</returns>
        public static IEnumerable<PropertyInfo> GetProperties(Type typ, BindingFlags bindingFlags)
        {
            return typ.GetProperties(bindingFlags);
        }
    }
}
