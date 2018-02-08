using System;

namespace SparkyTestHelpers.Core.Scenarios
{
    public class ScenarioTestFailureException : Exception
    {
        public ScenarioTestFailureException(string msg) : base(msg)
        {
        }
    }
}