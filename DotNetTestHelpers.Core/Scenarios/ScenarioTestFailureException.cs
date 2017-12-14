using System;

namespace DotNetTestHelpers.Core.Scenarios
{
    public class ScenarioTestFailureException : Exception
    {
        public ScenarioTestFailureException(string msg) : base(msg)
        {
        }
    }
}