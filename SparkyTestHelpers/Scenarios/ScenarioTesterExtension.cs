using System;
using System.Collections.Generic;

namespace SparkyTestHelpers.Scenarios
{
    /// <summary>
    /// <see cref="ScenarioTester{TScenario}" /> extension methods.
    /// </summary>
    public static class ScenarioTesterExtension
    {
        /// <summary>
        /// Defines <see cref="Action"/> to be called before each
        /// <see cref="TestEach{TScenario}(IEnumerable{TScenario}, Action{TScenario})"/> call.
        /// </summary>
        /// <param name="enumerable">The test scenario enumerable.</param>
        /// <param name="action">The action.</param>
        /// <returns><see cref="ScenarioTester{TScenario}" /> instance.</returns>
        public static ScenarioTester<TScenario> BeforeEachTest<TScenario>(
            this IEnumerable<TScenario> enumerable, Action<TScenario> action)
        {
            return new ScenarioTester<TScenario>(enumerable).BeforeEachTest(action);
        }

        /// <summary>
        /// Defines <see cref="Func{TScenario, Exception, Boolean}"/> to be called after each
        /// <see cref="TestEach{TScenario}(IEnumerable{TScenario}, Action{TScenario})"/> call.
        /// The function receives the scenario and the exception (if any) caught by the test. 
        /// If the function returns true, the scenario test is "passed". If false, exception is thrown to fail the test.
        /// </summary>
        /// <param name="func">The "callback" function.</param>
        /// <param name="action">The action.</param>
        /// <returns><see cref="ScenarioTester{TScenario}" /> instance.</returns>
        public static ScenarioTester<TScenario> AfterEachTest<TScenario>(
            this IEnumerable<TScenario> enumerable, Func<TScenario, Exception, bool> func)
        {
            return new ScenarioTester<TScenario>(enumerable).AfterEachTest(func);
        }

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
            return new ScenarioTester<TScenario>(enumerable).TestEach(test);
        }
    }
}