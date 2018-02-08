using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SparkyTestHelpers.Core.Scenarios;

namespace SparkyTestHelpers.MsTest.Scenarios
{
    /// <summary>
    /// <see cref="ScenarioTester{TScenario}" subclass for the MSTest framework.
    /// </summary>
    public class MsTestScenarioTester<TScenario> : ScenarioTester<TScenario>
    {
        /// <summary>
        /// Instantiates a new <cref="MsTestScenarioTester{TScenario}" /> object.
        /// </summary>
        /// <remarks>
        /// This class is rarely used directly. It is more often used via the
        /// <see cref="MsTestScenarioTesterExtension.TestEach{TScenario}(IEnumerable{TScenario}, Action{TScenario})"/>
        /// extension method.
        /// </remarks>
        /// <param name="scenarios">The test scenarios.</param>
        /// <seealso cref="MsTestScenarioTesterExtension.TestEach{TScenario}(IEnumerable{TScenario}, Action{TScenario})"/>
        public MsTestScenarioTester(IEnumerable<TScenario> scenarios) : base(scenarios)
        {
            SetInconclusiveExceptionTypes(typeof(AssertInconclusiveException));
        }

        protected override void ThrowInconclusiveException(string message)
        {
            throw new AssertInconclusiveException(message);
        }
    }
}