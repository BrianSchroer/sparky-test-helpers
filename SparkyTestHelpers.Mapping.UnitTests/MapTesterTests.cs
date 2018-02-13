using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Mapping.UnitTests.TestClasses;
using SparkyTestHelpers.Scenarios;
using System;
using System.Collections.Generic;

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
        public void ShouldEqual_should_use_specified_source_property()
        {
            _mapTester.WhereMember(dest => dest.Name).ShouldEqual(src => $"{src.FirstName} {src.LastName}");

            string expected = $"{_source.FirstName} {_source.LastName}";

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
            _mapTester.IgnoringMember(x => x.DestOnly);

            AssertMappedValues();
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
