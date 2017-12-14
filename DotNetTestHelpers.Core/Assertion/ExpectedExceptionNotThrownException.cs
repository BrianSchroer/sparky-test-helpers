using System;

namespace DotNetTestHelpers.Core.Assertion
{
    public class ExpectedExceptionNotThrownException : Exception
    {
        public ExpectedExceptionNotThrownException(string msg) : base(msg)
        {
        }
    }
}