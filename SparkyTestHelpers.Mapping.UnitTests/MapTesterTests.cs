using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Mapping.UnitTests.TestClasses;
using SparkyTestHelpers.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SparkyTestHelpers.Mapping.UnitTests
{
    [TestClass]
    public class MapTesterTests
    {
        private MapTester<Source, Dest> _mapTester;

        private Source _source;
        private Dest _dest;

        [TestInitialize]
        public void TestInitialize()
        {
            _source = new Source
            {
                Name = "Test Name",
                Id = 123,
                FirstName = "Brian",
                LastName = "Schroer",
                FullName = "Brian Schroer",
                Children = new [] {"Child1", "Child2" }
            };

            _dest = new Dest
            {
                Name = "Test Name",
                Id = 123,
                Children = new[] { "Child1", "Child2" }
            };

            _mapTester = MapTester.ForMap<Source, Dest>()
                //.WithLogging()
                .IgnoringMember(dest => dest.DestOnly)
                .IgnoringMember(dest => dest.Children);
        }

        [TestMethod]
        public void WithLogging_should_work_with_default_logger()
        {
            var loggedMessages = new List<string>();

            AssertExceptionNotThrown.WhenExecuting(() =>
                _mapTester.WithLogging()
                .AssertMappedValues(_source, _dest));
        }

        [TestMethod]
        public void WithLogging_should_work_with_custom_logger()
        {
            var loggedMessages = new List<string>();

            AssertExceptionNotThrown.WhenExecuting(() =>
                _mapTester.WithLogging(message => loggedMessages.Add(message))
                .AssertMappedValues(_source, _dest));

            Assert.IsTrue(loggedMessages.Count > 0);
            Console.Write(string.Join('\n', loggedMessages));
        }

        [TestMethod]
        public void AssertMappedValues_should_throw_exception_for_unmapped_dest_property()
        {
            AssertExceptionThrown
                .OfType<ScenarioTestFailureException>()
                .WithMessageContaining("Property \"DestOnly\" was not tested")
                .WhenExecuting(() => MapTester.ForMap<Source, Dest>().AssertMappedValues(_source, _dest));
        }

        [TestMethod]
        public void AssertMappedValues_should_throw_exception_for_property_with_different_source_and_dest_values()
        {
            _dest.Name = "Different";

            AssertExceptionMessageContaining(
                "Mapping test failed for property \"Name\". Expected<Test Name>. Actual: <Different>");
        }

        [TestMethod]
        public void WhereMember_should_throw_expected_exception_for_bad_property_name_expression()
        {
            AssertExceptionThrown
                .OfType<MapTesterException>().WithMessage("Invalid property expression: \"dest => dest.Name.ToString()\".")
                .WhenExecuting(() => _mapTester.WhereMember(dest => dest.Name.ToString()));
        }

        [TestMethod]
        public void ShouldEqual_should_use_specified_callback_function()
        {
            string expected = $"{_source.FirstName} {_source.LastName}";

            _mapTester.WhereMember(dest => dest.Name).ShouldEqual(src => expected);

            AssertExceptionMessageContaining(
                $"Mapping test failed for property \"Name\". Expected<{expected}>. Actual: <{_dest.Name}>");

            _dest.Name = expected;

            AssertMappedValues();
        }

        [TestMethod]
        public void ShouldEqualValue_should_use_specified_value()
        {
            string expected = "Dumbledore";
            _dest.Name = expected;

            AssertExceptionMessageContaining(
                $"Mapping test failed for property \"Name\". Expected<{_source.Name}>. Actual: <{_dest.Name}>");

            _mapTester.WhereMember(dest => dest.Name).ShouldEqualValue(expected);
            AssertMappedValues();
        }

        [TestMethod]
        public void IsTestedBy_should_use_custom_assertion()
        {
            string expected = $"{_source.FirstName} {_source.LastName}";
            _mapTester.WhereMember(dest => dest.Name).IsTestedBy((src, dest) => Assert.AreEqual(expected, dest.Name));

            AssertExceptionMessageContaining("Expected:<Brian Schroer>. Actual:<Test Name>");

            _dest.Name = expected;
            AssertMappedValues();
        }

        [TestMethod]
        public void IgnoringMember_should_exclude_dest_member_check()
        {
            var input = new RestaurantEditModel { Cuisine = CuisineType.Italian, Name = "Test name" };
            var output = new Restaurant { Cuisine = CuisineType.Italian, Name = "Test name" };

            MapTester.ForMap<RestaurantEditModel, Restaurant>()
                .WhereMember(dest => dest.Id).ShouldEqualValue(0) 
                .AssertMappedValues(input, output);
        }

        [TestMethod]
        public void IgnoringMemberNamesStartingWith_should_work_as_expected()
        {
            var loggedMessages = new List<string>();

            MapTester.ForMap<Source, Dest>()
                .WithLogging(loggedMessages.Add)
                .IgnoringMemberNamesStartingWith("Dest")
                .IgnoringMember(dest => dest.Children)
                .AssertMappedValues(_source, _dest);

            Assert.IsNotNull(loggedMessages.SingleOrDefault(x => x == "Property \"DestOnly\" was not tested."));
        }

        [TestMethod]
        public void IgnoringAllOtherMembers_should_work_as_expected()
        {
            var loggedMessages = new List<string>();

            MapTester.ForMap<Source, Dest>()
                .WithLogging(loggedMessages.Add)
                .IgnoringMember(dest => dest.Children)
                .IgnoringAllOtherMembers()
                .AssertMappedValues(_source, _dest);

            Assert.IsNotNull(loggedMessages.SingleOrDefault(x => x == "Property \"DestOnly\" was not tested."));
        }

        [TestMethod]
        public void HiddenProperty_should_resolve_to_top_most_member()
        {
            var restaurantEditModel = new RestaurantEditModel {Cuisine = CuisineType.Italian, Name = "Test name"};
            var restaurantModel = new Restaurant {Cuisine = CuisineType.Italian, Name = "Test name"};

            var restaurantList = new RestaurantList {Restaurants = new[] {restaurantModel}};
            var editRestaurantList = new RestaurantEditList {Restaurants = new[] {restaurantEditModel}};

            MapTester.ForMap<RestaurantList, RestaurantEditList>()
                // ignored since the internal constructor of MapTester is what's being tested
                .IgnoringMember(dest => dest.Restaurants)
                .AssertMappedValues(restaurantList, editRestaurantList);
        }

        private void AssertMappedValues()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _mapTester.AssertMappedValues(_source, _dest));
        }

        private void AssertExceptionMessageContaining(string subString, Action action = null)
        {
            action = action ?? (() => _mapTester.AssertMappedValues(_source, _dest));

            AssertExceptionThrown
                .OfType<ScenarioTestFailureException>()
                .WithMessageContaining(subString)
                .WhenExecuting(action);
        }
    }
}
