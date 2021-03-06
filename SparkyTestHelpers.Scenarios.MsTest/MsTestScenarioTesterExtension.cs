using System;
using System.Collections.Generic;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.Scenarios.MsTest
{
    /// <summary>
    /// <see cref="ScenarioTester{TScenario}" /> extension methods.
    /// </summary>
    public static class MsTestScenarioTesterExtension
    {
        /// <summary>
        /// Calls the specified <paramref name="test" /> action for each scenario in enumerable.
        /// </summary>
        /// <param name="enumerable">The test scenario enumerable.</param>
        /// <param name="test">"Callback" test action.</param>
        /// <returns><see cref="ScenarioTester{TScenario}" /> instance.</returns>
        /// <example>
        ///     <code><![CDATA[
        ///  new []
        /// {
        ///     new { DateString = "1/31/2023", ShouldBeValid = true },  
        ///     new { DateString = "2/31/2023", ShouldBeValid = false },  
        ///     new { DateString = "3/31/2023", ShouldBeValid = true },  
        ///     new { DateString = "4/31/2023", ShouldBeValid = false },  
        ///     new { DateString = "5/31/2023", ShouldBeValid = true },  
        ///     new { DateString = "6/31/2023", ShouldBeValid = false } 
        /// }
        /// .TestEach(scenario =>
        /// {
        ///     DateTime dt;
        ///     Assert.AreEqual(scenario.ShouldBeValid, DateTime.TryParse(scenario.DateString, out dt));  
        /// });  
        ///     ]]></code>
        /// </example>
        public static ScenarioTester<TScenario> TestEach<TScenario>(
            this IEnumerable<TScenario> enumerable, Action<TScenario> test)
        {
            var tester = (ScenarioTester<TScenario>)new MsTestScenarioTester<TScenario>(enumerable);
            return tester.TestEach(test);
        }
    }
}