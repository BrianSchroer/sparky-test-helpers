using Moq;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.Moq
{
    /// <summary>
    /// <see cref="ScenarioTester{TScenario}"/> extension methods.
    /// </summary>
    public static class ScenarioTesterExtensions
    {
        /// <summary>
        /// Call "ResetCalls" for each specified <see cref="Mock"/> before testing each scenario.
        /// </summary>
        /// <typeparam name="TScenario">The scenario type.</typeparam>
        /// <param name="tester">The <see cref="ScenarioTester{TScenario}"/>.</param>
        /// <param name="mocksToReset">The <see cref="Mock"/>(s) to reset.</param>
        /// <returns>The <see cref="ScenarioTester{TScenario}"/>.</returns>
        public static ScenarioTester<TScenario> WithMoqResetsFor<TScenario>(
            this ScenarioTester<TScenario> tester, params Mock[] mocksToReset)
        {
            tester.BeforeEachTest(scenario =>
            {
                foreach (Mock mock in mocksToReset)
                {
                    mock.ResetCalls();
                }
            });

            return tester;
        }
    }
}
