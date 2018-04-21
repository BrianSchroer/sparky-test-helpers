namespace SparkyTestHelpers.Mapping.UnitTests.TestClasses
{
    public class RestaurantEditList : RestaurantList
    {
        // required to create multiple property types for the PropertyInfoResolverTests
        public new RestaurantEditModel[] Restaurants { get; set; }
    }
}