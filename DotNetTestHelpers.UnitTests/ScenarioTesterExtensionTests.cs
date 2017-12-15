using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotNetTestHelpers.Core.Scenarios;

namespace DotNetTestHelpers.UnitTests
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
        public void Enumerable_TestEach_extenion_method_should_throw_ScenarioTestFailureException_with_proper_message_for_anonymous_scenario()
        {
            var scenario = new[]
            {
                new { Expected = "a"},
                new { Expected = "b"},
                new { Expected = "c"}
            };
        }
    }
}
