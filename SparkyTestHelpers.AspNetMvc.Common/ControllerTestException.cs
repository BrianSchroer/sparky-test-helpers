using System;

namespace SparkyTestHelpers.AspNetMvc.Common
{
    /// <summary>
    /// Exception thrown by <see cref="ControllerActionTester"/>.
    /// </summary>
    public class ControllerTestException : Exception
    {
        public ControllerTestException() : base()
        {
        }

        public ControllerTestException(string message)
            : base(message)
        {
        }

        public ControllerTestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ControllerTestException(
            System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
