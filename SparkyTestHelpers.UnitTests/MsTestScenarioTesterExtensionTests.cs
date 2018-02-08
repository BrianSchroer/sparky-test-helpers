using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SparkyTestHelpers.Core.Exceptions;
using SparkyTestHelpers.MsTest.Scenarios;

namespace SparkyTestHelpers.UnitTests
{
    /// <summary>
    /// <see cref="ScenarioTester{TScenario}" /> extension method unit tests.
    /// </summary>
    [TestClass]
    public class MsTestScenarioTesterExtensionTests
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
        public void Enumerable_TestEach_extension_method_should_throw_inconclusive_exception_when_all_exceptions_are_inconclusive()
        {
            var scenarios = new[] { "a", "b", "c" };

            AssertExceptionThrown
                .OfType<AssertInconclusiveException>()
                .WhenExecuting(() =>
                    scenarios.TestEach(scenario => throw new AssertInconclusiveException("fail")));
        }

        [TestMethod]
        public void Enumerable_TestEach_extension_method_should_throw_failure_exception_when_not_all_exceptions_are_inconclusive()
        {
            var scenarios = new[] { "a", "b", "c" };

            AssertExceptionThrown
                .OfType<SparkyTestHelpers.Core.Scenarios.ScenarioTestFailureException>()
                .WhenExecuting(() => scenarios.TestEach(scenario =>
                  {
                      switch (scenario)
                      {
                          case "b":
                              throw new AssertInconclusiveException("inconclusive");
                          default:
                              throw new InvalidOperationException("fail");
                      }
                  }));
        }
    }
}
