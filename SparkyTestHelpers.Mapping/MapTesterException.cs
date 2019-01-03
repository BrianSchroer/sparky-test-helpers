using System;

namespace SparkyTestHelpers.Mapping
{
    /// <summary>
    /// Exception thrown when an mapping test fails.
    /// </summary>
    public class MapTesterException : Exception
    {
        /// <summary>
        /// Creates new <see cref="MapTesterException"/> instance.
        /// </summary>
        /// <param name="msg">The exception message.</param>
        public MapTesterException(string msg) : base(msg)
        {
        }

        /// <summary>
        /// Creates new <see cref="MapTesterException"/> instance.
        /// </summary>
        public MapTesterException() : base()
        {
        }

        /// <summary>
        /// Creates new <see cref="MapTesterException"/> instance.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">Inner <see cref="Exception"/>.</param>
        public MapTesterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}