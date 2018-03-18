using System;

namespace SparkyTestHelpers.AspNetCore.Validation
{
    /// <summary>
    /// Exception thrown by <see cref="ValidationForModel{TModel}"/>.
    /// </summary>
    public class ValidationTestException : Exception
    {
        public ValidationTestException() : base()
        {
        }

        public ValidationTestException(string message)
            : base(message)
        {
        }

        public ValidationTestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ValidationTestException(
            System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
