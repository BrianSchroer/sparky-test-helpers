using System;

namespace SparkyTestHelpers.Core.Exceptions
{
    public class ExpectedExceptionNotThrownException : Exception
    {
        public ExpectedExceptionNotThrownException(string msg) : base(msg)
        {
        }
    }
}