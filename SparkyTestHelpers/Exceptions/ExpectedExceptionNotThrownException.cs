using System;

namespace SparkyTestHelpers.Exceptions
{
    /// <summary>
    /// Exception thrown when an expected exception is not thrown.
    /// </summary>
    public class ExpectedExceptionNotThrownException : Exception
    {
        /// <summary>
        /// Creates new <see cref="ExpectedExceptionNotThrownException"/> instance.
        /// </summary>
        /// <param name="msg">The exception message.</param>
        public ExpectedExceptionNotThrownException(string msg) : base(msg)
        {
        }
    }
}