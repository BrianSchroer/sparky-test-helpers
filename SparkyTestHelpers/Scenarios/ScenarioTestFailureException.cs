using System;

namespace SparkyTestHelpers.Scenarios
{
    public class ScenarioTestFailureException : Exception
    {
        public ScenarioTestFailureException(string msg) : base(msg)
        {
        }
    }
}