using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Mapping.UnitTests.TestClasses;

namespace SparkyTestHelpers.Mapping.UnitTests
{
    /// <summary>
    /// <see cref="PropertyInfoResolver"/> unit tests.
    /// </summary>
    [TestClass]
    public class PropertyInfoResolverTests
    {
        [TestMethod]
        public void OverridenProperty_should_throw_ambiguous_exception()
        {
            Assert.ThrowsException<AmbiguousMatchException>(() =>
                typeof(RestaurantEditList).GetProperty(nameof(RestaurantEditList.Restaurants)));
        }

        [TestMethod]
        public void OverridenProperty_should_resolve_with_GetProperties_First()
        {
            var propertyInfo = typeof(RestaurantEditList).GetProperties()
                .First(property => property.Name == nameof(RestaurantEditList.Restaurants));
            
            Assert.AreEqual(propertyInfo.PropertyType, typeof(RestaurantEditModel[]));
        }

        [TestMethod]
        public void PropertyInfoResolver_can_resolve_hidden_property()
        {
            var propertyInfo = PropertyInfoResolver.Resolve(typeof(RestaurantEditList).GetTypeInfo(),
                nameof(RestaurantEditList.Restaurants));
            Assert.AreEqual(propertyInfo.PropertyType, typeof(RestaurantEditModel[]));
        }
    }
}