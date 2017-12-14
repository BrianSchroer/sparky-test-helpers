using System;

namespace DotNetTestHelpers.Core.Exceptions
{
    public class ExpectedExceptionNotThrownException : Exception
    {
        public ExpectedExceptionNotThrownException(string msg) : base(msg)
        {
        }
    }
}