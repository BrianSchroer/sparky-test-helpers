using System;

namespace SparkyTestHelpers.Scenarios
{
    /// <summary>
    /// 
    /// </summary>
    public class ScenarioTestsInconclusiveException : Exception
    {
        public ScenarioTestsInconclusiveException(string msg) : base(msg)
        {
        }
    }
}