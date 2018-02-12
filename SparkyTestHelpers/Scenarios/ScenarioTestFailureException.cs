using System;

namespace SparkyTestHelpers.Scenarios
{
    /// <summary>
    /// Exception thrown for a scenario test failure.
    /// </summary>
    public class ScenarioTestFailureException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="ScenarioTestFailureException"/> instance.
        /// </summary>
        /// <param name="msg">The exception message.</param>
        public ScenarioTestFailureException(string msg) : base(msg)
        {
        }
    }
}