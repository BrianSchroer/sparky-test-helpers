using System;

namespace SparkyTestHelpers.AspNetCore
{
    /// <summary>
    /// Exception thrown by <see cref="ControllerActionTester"/>.
    /// </summary>
    public class ControllerActionTestException : Exception
    {
        public ControllerActionTestException() : base()
        {
        }

        public ControllerActionTestException(string message)
            : base(message)
        {
        }

        public ControllerActionTestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ControllerActionTestException(
            System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
