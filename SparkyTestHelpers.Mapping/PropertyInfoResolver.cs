using System;
using System.Linq;
using System.Reflection;

namespace SparkyTestHelpers.Mapping
{
    /// <summary>
    /// This class is used to get the <see cref="PropertyInfo"/> for a <see cref="TypeInfo"/> / property name combination.
    /// </summary>
    public class PropertyInfoResolver
    {
        /// <summary>
        /// "Singleton" <see cref="PropertyInfoResolver"/> instance.
        /// </summary>
        public static PropertyInfoResolver Instance => LazyResolver.Value;

        private static readonly Lazy<PropertyInfoResolver> LazyResolver 
            = new Lazy<PropertyInfoResolver>(() => new PropertyInfoResolver());

        /// <summary>
        /// Creates a new <see cref="PropertyInfoResolver"/> instance.
        /// </summary>
        /// <remarks>
        /// This constructor is private to implement the "Singleton" pattern for <see cref="Instance"/>.
        /// </remarks>
        private PropertyInfoResolver()
        {
        }

        /// <summary>
        /// Gets <see cref="PropertyInfo"/> from <see cref="TypeInfo"/>
        /// for the specified <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="typeInfo"><see cref="TypeInfo"/> instance.</param>
        /// <param name="propertyName">The </param>
        /// <returns>Resolved <see cref="PropertyInfo"/>.</returns>
        public PropertyInfo ResolveProperty(TypeInfo typeInfo, string propertyName)
        {
            try
            {
                return typeInfo.GetProperty(propertyName);
            }
            catch (AmbiguousMatchException ex)
            {
                var ambiguousProperty = typeInfo.GetProperties().First(property => property.Name == propertyName);
                Console.WriteLine($"{ex.GetType().FullName}: A property was hidden by a derived class." 
                    + " Selecting: '{typeInfo.FullName}: {ambiguousProperty.PropertyType.FullName}' for '{propertyName}'.");
                return ambiguousProperty;
            }
        }

        /// <summary>
        /// Gets <see cref="PropertyInfo"/> from <see cref="TypeInfo"/>
        /// for the specified <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="typeInfo"><see cref="TypeInfo"/> instance.</param>
        /// <param name="propertyName">The </param>
        /// <returns>Resolved <see cref="PropertyInfo"/>.</returns>
        public static PropertyInfo Resolve(TypeInfo typeInfo, string propertyName)
        {
            return Instance.ResolveProperty(typeInfo, propertyName);
        }
    }
}