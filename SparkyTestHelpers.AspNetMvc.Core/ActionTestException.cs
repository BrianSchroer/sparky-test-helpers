using System;

namespace SparkyTestHelpers.AspNetMvc.Core
{
    /// <summary>
    /// ASP.NET MVC Core action test exception. 
    /// </summary>
    public class ActionTestException : Exception
    {
        public ActionTestException() : base()
        {
        }

        public ActionTestException(string message)
            : base(message)
        {
        }

        public ActionTestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ActionTestException(
            System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
