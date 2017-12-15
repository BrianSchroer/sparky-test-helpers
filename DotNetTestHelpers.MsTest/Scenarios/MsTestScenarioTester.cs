using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DotNetTestHelpers.Core.Scenarios;

namespace DotNetTestHelpers.MsTest.Scenarios
{
    /// <summary>
    /// <see cref="ScenarioTester{TScenario}" subclass for the MSTest framework.
    /// </summary>
    public class MsTestScenarioTester<TScenario> : ScenarioTester<TScenario>
    {
        /// <summary>
        /// Instantiates a new <cref="MsTestScenarioTester{TScenario}" /> object.
        /// </summary>
        /// <param name="scenarios">The test scenarios.</param>
        public MsTestScenarioTester(IEnumerable<TScenario> scenarios) : base(scenarios)
        {
            this.SetInconclusiveExceptionTypes(typeof(AssertInconclusiveException));
        }

        protected override void ThrowInconclusiveException(string message)
        {
            throw new AssertInconclusiveException(message);
        }
    }
}