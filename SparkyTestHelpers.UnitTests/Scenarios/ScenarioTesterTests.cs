using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Scenarios;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions;

namespace SparkyTestHelpers.UnitTests.Scenarios
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

                StringAssert.Contains(ex.Message, expected);
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
                        + $"\n{new string('_', ConsoleHelper.GetWidth())}"
                        + "\nScenario[2] (3 of 3) - Assert.Fail failed. Test c failed!"
                        + "\n\nScenario data - System.String: \"c\"\n";

                StringAssert.Contains(ex.Message, expected);
            }
        }

        [TestMethod]
        public void TestEach_should_throw_ScenarioTestFailureException_with_message_containing_counts()
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

                string expected = $"3 scenarios tested. 1 passed. 2 failed.:\n\n{new string('_', ConsoleHelper.GetWidth())}";

                StringAssert.StartsWith(ex.Message, expected);
            }
        }

        [TestMethod]
        public void TestEach_should_something_something_InnerException()
        {
            var innerException = new Exception("inner exception message");
            var exception = new Exception("outer exception message", innerException);

            try
            {
                new[] { "test" }.TestEach(scenario => throw exception);
            }
            catch (ScenarioTestFailureException ex)
            {
                ex.Message.Should().Contain("outer exception message\ninner exception message");
            }
        }

        [TestMethod]
        public void BeforeEachTest_should_be_called_before_each_scenario_test()
        {
            var scenarios = new[] { "A", "B", "C", "D" };
            var callbackValues = new List<string>();
            int index = -1;

            new ScenarioTester<string>(scenarios)
                .BeforeEachTest(scenario =>
                {
                    index++;
                    Assert.AreEqual(scenarios[index], scenario);
                    callbackValues.Add(scenario);
                })
                .TestEach(scenario => { });

            StringAssert.Equals(string.Join("", scenarios), string.Join("", callbackValues.ToArray()));
        }

        [TestMethod]
        public void AfterEachTest_should_be_called_after_each_scenario_test()
        {
            var scenarios = new[] { "A", "B", "C", "D" };
            var callbackValues = new List<string>();
            int index = -1;

            new ScenarioTester<string>(scenarios)
                .AfterEachTest((scenario, ex) =>
                {
                    index++;
                    Assert.AreEqual(scenarios[index], scenario);
                    Assert.IsNull(ex);
                    callbackValues.Add(scenario);
                    return (ex == null);
                })
                .TestEach(scenario => { });

            StringAssert.Equals(string.Join("", scenarios), string.Join("", callbackValues.ToArray()));
        }

        [TestMethod]
        public void AfterEachTest_should_ignore_caught_exception_when_true_is_returned()
        {
            var expected = new InvalidOperationException("Something terrible happened!");

            new ScenarioTester<string>(new[] { "x" })
                .AfterEachTest((scenario, ex) =>
                {
                    Assert.AreEqual(expected, ex);
                    return true;
                })
                .TestEach(scenario => throw expected);
        }

        [TestMethod]
        public void AfterEachTest_should_throw_caught_exception_when_false_is_returned()
        {
            var expected = new InvalidOperationException("Something terrible happened!");

            try
            {
                new ScenarioTester<string>(new[] { "x" })
                    .AfterEachTest((scenario, ex) =>
                    {
                        Assert.AreEqual(expected, ex);
                        return false;
                    })
                    .TestEach(scenario => throw expected);
            }
            catch (ScenarioTestFailureException ex)
            {
                StringAssert.Contains(ex.Message, expected.Message);
            }
        }

        [TestMethod]
        public void AfterEachTest_should_throw_new_exception_when_false_is_returned_with_no_exception()
        {
            try
            {
                new ScenarioTester<string>(new[] { "x" })
                    .AfterEachTest((scenario, ex) =>
                    {
                        Assert.IsNull(ex);
                        return false;
                    })
                    .TestEach(scenario => { });
            }
            catch (ScenarioTestFailureException ex)
            {
                StringAssert.Contains(ex.Message, "Scenario failed by \"AfterEachTest\" function.");
            }
        }

        [TestMethod]
        public void TestEach_should_rethrow_exception_thrown_by_AfterEachTest()
        {
            var expected = new InvalidOperationException("AfterEachTest fail!");

            try
            {
                new ScenarioTester<string>(new[] { "x" })
                    .AfterEachTest((scenario, ex) =>
                    {
                        throw expected;
                    })
                    .TestEach(scenario => { });
            }
            catch (ScenarioTestFailureException ex)
            {
                StringAssert.Contains(ex.Message, expected.Message);
            }
        }
    }
}
