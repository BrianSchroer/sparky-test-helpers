using System;

namespace SparkyTestHelpers.AspNetMvc.Routing
{
    public class RouteTesterException : Exception
    {
        public RouteTesterException() : base()
        {
        }

        public RouteTesterException(string message) : base(message)
        {
        }

        public RouteTesterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RouteTesterException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
