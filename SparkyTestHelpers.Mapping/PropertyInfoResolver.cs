using System;
using System.Linq;
using System.Reflection;

namespace SparkyTestHelpers.Mapping
{
    public class PropertyInfoResolver
    {
        public static PropertyInfoResolver Instance => LazyResolver.Value;

        private static readonly Lazy<PropertyInfoResolver> LazyResolver = new Lazy<PropertyInfoResolver>(() => new PropertyInfoResolver());
        
        private PropertyInfoResolver()
        {
            // singleton pattern, external code shouldn't call the constructor
        }
        
        public PropertyInfo ResolveProperty(TypeInfo typeInfo, string propertyName)
        {
            try
            {
                return typeInfo.GetProperty(propertyName);
            }
            catch (AmbiguousMatchException e)
            {
                var ambiguousProperty = typeInfo.GetProperties().First(property => property.Name == propertyName);
                //can't use _log here since it hasn't been configured yet.
                Console.WriteLine($"{e.GetType().FullName}: A property was hidden by a derived class.  Selecting: '{typeInfo.FullName}: {ambiguousProperty.PropertyType.FullName}' for '{propertyName}'.");
                return ambiguousProperty;
            }
        }
        
    }
}