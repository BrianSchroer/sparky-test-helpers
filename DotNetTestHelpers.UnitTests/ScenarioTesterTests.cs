using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotNetTestHelpers.Core.Scenarios;

namespace DotNetTestHelpers.UnitTests
{
    /// <summary>
    /// <see cref="ScenarioTester{TScenario}" /> unit tests.
    /// </summary>
    [TestClass]
    public class ScenarioTesterTests
    {
        [TestMethod]
        public void TestEach_should_call_test_callback_action_for_each_scenario()
        {
            int callbackCount = 0;

            var scenarios = new[] { "a", "b", "c" };

            new ScenarioTester<string>(scenarios).TestEach(scenario =>
            {
                Assert.AreEqual(scenarios[callbackCount], scenario);
                callbackCount++;
            });

            Assert.AreEqual(scenarios.Length, callbackCount);
        }

        [TestMethod]
        public void TestEach_should_throw_ScenarioTestFailureException_when_a_scenario_test_fails()
        {
            int callbackCount = 0;

            var scenarios = new[] { "a", "b", "c" };

            try
            {
                new ScenarioTester<string>(scenarios).TestEach(scenario =>
                {
                    int index = callbackCount;
                    callbackCount++;

                    if (scenario == "b")
                    {
                        Assert.Fail("Test b failed!");
                    }
                });
            }
            catch (ScenarioTestFailureException ex)
            {
                Console.WriteLine(ex.Message);
                // Expected failure was thrown
            }

            Assert.AreEqual(scenarios.Length, callbackCount);
        }
    }
}
