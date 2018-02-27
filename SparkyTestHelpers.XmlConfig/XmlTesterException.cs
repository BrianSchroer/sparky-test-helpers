using System;

namespace SparkyTestHelpers.XmlConfig
{
    /// <summary>
    /// Exception thrown when XML does not contain expected values. 
    /// </summary>
    public class XmlTesterException : Exception
    {
        /// <summary>
        /// Creates new <see cref="XmlTesterException"/> instance.
        /// </summary>
        /// <param name="msg">The exception message.</param>
        public XmlTesterException(string msg) : base(msg)
        {
        }
    }
}