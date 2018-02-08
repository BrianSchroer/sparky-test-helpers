using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using SparkyTestHelpers.Core.Exceptions;
using SparkyTestHelpers.Core.Scenarios;

namespace SparkyTestHelpers.UnitTests
{
    /// <summary>
    /// <see cref="ForTest" /> unit tests.
    /// </summary>
    [TestClass]
    public class ForTestTests
    {
        [TestMethod]
        public void ForTest_Scenarios_TestEach_extension_method_should_call_test_callback_action_for_each_scenario()
        {
            int callbackCount = 0;

            var scenarios = new[] { "a", "b", "c" };

            ForTest.Scenarios(scenarios).TestEach(scenario =>
            {
                Assert.AreEqual(scenarios[callbackCount], scenario);
                callbackCount++;
            });

            Assert.AreEqual(scenarios.Length, callbackCount);
        }

        [TestMethod]
        public void ForTest_EnumValues_should_return_all_enum_values()
        {
            var stringComparisonValues = Enum.GetValues(typeof(StringComparison)).Cast<StringComparison>().ToArray();
            int callbackCount = 0;

            ForTest.EnumValues<StringComparison>().TestEach(scenario =>
            {
                Assert.AreEqual(stringComparisonValues[callbackCount], scenario);
                callbackCount++;
            });

            Assert.AreEqual(stringComparisonValues.Length, callbackCount);
        }

        [TestMethod]
        public void ForTest_EnumValues_ExceptFor_should_ignore_unwanted_enum_values()
        {
            StringComparison[] stringComparisonValues =
                Enum.GetValues(typeof(StringComparison)).Cast<StringComparison>().ToArray();

            StringComparison[] unwantedValues = new[]
            {
                StringComparison.CurrentCultureIgnoreCase,
                StringComparison.InvariantCultureIgnoreCase
            };

            StringComparison[] wantedValues = stringComparisonValues.Except(unwantedValues).ToArray();

            int callbackCount = 0;

            ForTest.EnumValues<StringComparison>()
                .ExceptFor(unwantedValues)
                .TestEach(scenario =>
                {
                    Assert.AreEqual(wantedValues[callbackCount], scenario);
                    callbackCount++;
                });

            Assert.AreEqual(wantedValues.Length, callbackCount);
        }

        [TestMethod]
        public void ForTest_EnumValues_should_throw_InvalidOperationException_when_type_is_not_enum_type()
        {
            AssertExceptionThrown.OfType<InvalidOperationException>()
                .WithMessage("System.DateTime is not an Enum type.")
                .WhenExecuting(() => ForTest.EnumValues<DateTime>());
        }
    }
}
