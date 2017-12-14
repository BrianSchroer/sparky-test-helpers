using System;

namespace DotNetTestHelpers.Core.Scenarios
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