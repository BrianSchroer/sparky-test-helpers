using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SparkyTestHelpers.Mapping
{
    /// <summary>
    /// Get public instance properties with getters and setters for specified type.
    /// </summary>
    internal static class PropertyEnumerator
    {
        /// <summary>
        /// Get public instance properties with getters and setters for specified type.
        /// </summary>
        /// <param name="typeInfo">The type info.</param>
        /// <returns>Array of properties.</returns>
        public static PropertyInfo[] GetPublicInstanceReadWriteProperties(TypeInfo typeInfo)
        {
            return GetProperties(typeInfo, BindingFlags.Public | BindingFlags.Instance)
                .Where(propertyInfo => propertyInfo.CanRead && propertyInfo.CanWrite)
                .ToArray();
        }

        /// <summary>
        /// Get public instance properties with getters for specified type.
        /// </summary>
        /// <param name="typeInfo">The type info.</param>
        /// <returns>Array of properties.</returns>
        public static PropertyInfo[] GetPublicInstanceReadProperties(TypeInfo typeInfo)
        {
            return GetProperties(typeInfo, BindingFlags.Public | BindingFlags.Instance)
                .Where(propertyInfo => propertyInfo.CanRead)
                .ToArray();
        }

        /// <summary>
        /// Get properties for specified type.
        /// </summary>
        /// <param name="typeInfo">The type info.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/>.</param>
        /// <returns>Array of properties.</returns>
        public static IEnumerable<PropertyInfo> GetProperties(TypeInfo typeInfo, BindingFlags bindingFlags)
        {
            return typeInfo.GetProperties(bindingFlags);
        }
    }
}
