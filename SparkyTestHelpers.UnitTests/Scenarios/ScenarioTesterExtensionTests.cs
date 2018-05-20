using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.UnitTests.Scenarios
{
    /// <summary>
    /// <see cref="ScenarioTester{TScenario}" /> extension method unit tests.
    /// </summary>
    [TestClass]
    public class ScenarioTesterExtensionTests
    {
        [TestMethod]
        public void Enumerable_TestEach_extension_method_should_call_test_callback_action_for_each_scenario()
        {
            int callbackCount = 0;

            var scenarios = new[] { "a", "b", "c" };

            scenarios.TestEach(scenario =>
            {
                Assert.AreEqual(scenarios[callbackCount], scenario);
                callbackCount++;
            });

            Assert.AreEqual(scenarios.Length, callbackCount);
        }

        [TestMethod]
        public void Enumerable_AfterEachTest_extension_method_should_return_ScenarioTester()
        {
            var scenarios = new[] { "a", "b", "c" };

            Assert.IsInstanceOfType(
                scenarios.AfterEachTest((scenario, ex) => true), 
                typeof(ScenarioTester<string>));
        }

        [TestMethod]
        public void Enumerable_BeforeEachTest_extension_method_should_return_ScenarioTester()
        {
            var scenarios = new[] { "a", "b", "c" };

            Assert.IsInstanceOfType(
                scenarios.BeforeEachTest(scenario => { }), 
                typeof(ScenarioTester<string>));
        }

        [TestMethod]
        public void Enumerable_TestEach_extenion_method_should_throw_ScenarioTestFailureException_with_proper_message_for_anonymous_scenario()
        {
            AssertExceptionThrown
                .OfType<ScenarioTestFailureException>()
                .WithMessageContaining("Scenario data - anonymousType: {\"Input\":3,\"Expected\":\"a\"}")
                .WhenExecuting(() => new[] { new { Input = 3, Expected = "a" } }.TestEach(_ => Assert.Fail()));
        }

        [TestMethod]
        public void Enumerable_TestEach_extenion_method_should_throw_ScenarioTestFailureException_with_proper_message_for_tuple_scenario()
        {
            AssertExceptionThrown
                .OfType<ScenarioTestFailureException>()
                .WithMessageContaining("Scenario data - System.ValueTuple[[System.Int32],[System.String]]: {\"Item1\":3,\"Item2\":\"a\"}")
                .WhenExecuting(() => new[] { (Input: 3, Expected: "a") }.TestEach(_ => Assert.Fail()));
        }
    }
}
