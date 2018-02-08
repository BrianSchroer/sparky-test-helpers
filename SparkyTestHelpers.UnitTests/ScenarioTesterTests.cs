using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Core.Scenarios;
using System.Linq;

namespace SparkyTestHelpers.UnitTests
{
    /// <summary>
    /// <see cref="ScenarioTester{TScenario}" /> unit tests.
    /// </summary>
    [TestClass]
    public class ScenarioTesterTests
    {
        [TestMethod]
        public void Scenarios_property_should_return_scenarios_passed_to_constructor()
        {
            var scenarios = new[] { "a", "b", "c" };

            var tester = new ScenarioTester<string>(scenarios);

            Assert.AreEqual(scenarios.Length, tester.Scenarios.Count());
            for (int i = 0; i < scenarios.Length; i++)
            {
                Assert.AreSame(scenarios[i], tester.Scenarios.ElementAt(i));
            }
        }

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

                string expected = "Scenario[1] (2 of 3) - Assert.Fail failed. Test b failed!" 
                    + "\n\nScenario data - System.String: \"b\"\n";

                Assert.AreEqual(expected, ex.Message);
            }

            Assert.AreEqual(scenarios.Length, callbackCount);
        }

        [TestMethod]
        public void TestEach_should_throw_ScenarioTestFailureException_with_message_describing_each_failure()
        {
            var scenarios = new[] { "a", "b", "c" };

            try
            {
                new ScenarioTester<string>(scenarios).TestEach(scenario =>
                {
 
                    if (scenario != "b")
                    {
                        Assert.Fail($"Test {scenario} failed!");
                    }
                });
            }
            catch (ScenarioTestFailureException ex)
            {
                Console.WriteLine(ex.Message);

                string expected = 
                    "Scenario[0] (1 of 3) - Assert.Fail failed. Test a failed!"
                        + "\n\nScenario data - System.String: \"a\"\n"
                        + "\nScenario[2] (3 of 3) - Assert.Fail failed. Test c failed!"
                        + "\n\nScenario data - System.String: \"c\"\n";

                Assert.AreEqual(expected, ex.Message);
            }
        }
    }
}
