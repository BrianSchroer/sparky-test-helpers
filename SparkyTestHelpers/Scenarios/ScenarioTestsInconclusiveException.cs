using System;

namespace SparkyTestHelpers.Scenarios
{
    /// <summary>
    /// Exception thrown when a scenario test is inconclusive.
    /// </summary>
    public class ScenarioTestsInconclusiveException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="ScenarioTestsInconclusiveException"/> instance.
        /// </summary>
        /// <param name="msg">The exception message.</param>
        public ScenarioTestsInconclusiveException(string msg) : base(msg)
        {
        }
    }
}