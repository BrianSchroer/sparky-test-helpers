using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SparkyTestHelpers.Scenarios.MsTest
{
    /// <summary>
    /// <see cref="ScenarioTester{TScenario}" /> subclass for the MSTest framework.
    /// </summary>
    public class MsTestScenarioTester<TScenario> : ScenarioTester<TScenario>
    {
        /// <summary>
        /// Instantiates a new <see cref="MsTestScenarioTester{TScenario}" /> object.
        /// </summary>
        /// <remarks>
        /// This class is rarely used directly. It is more often used via the
        /// <see cref="MsTestScenarioTesterExtension.TestEach{TScenario}(IEnumerable{TScenario}, System.Action{TScenario})" />
        /// extension method.
        /// </remarks>
        /// <param name="scenarios">The test scenarios.</param>
        /// <see cref="MsTestScenarioTesterExtension.TestEach{TScenario}(IEnumerable{TScenario}, System.Action{TScenario})" /> 
        public MsTestScenarioTester(IEnumerable<TScenario> scenarios) : base(scenarios)
        {
            SetInconclusiveExceptionTypes(typeof(AssertInconclusiveException));
        }

        /// <summary>
        /// Throws an <see cref="AssertInconclusiveException"/>.
        /// </summary>
        /// <param name="message"></param>
        protected override void ThrowInconclusiveException(string message)
        {
            throw new AssertInconclusiveException(message);
        }
    }
}