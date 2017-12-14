using System;
using System.Text.RegularExpressions;

namespace DotNetTestHelpers.Core.Assertion
{
    /// <summary>
    /// Assert that an exception is not thrown when an action is executed. This class/method doesn't do much,
    /// but it clarifies the intent of Unit Tests that which to show that an action works correctly.
    /// </summary>
    public static class AssertExceptionNotThrown
    {
        /// <summary>
        /// Asserts that an exception was not thrown when executing an Action.
        /// </summary>
        /// <param name="action">The action which should not throw an exception.</param>
        public static void WhenExecuting(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}