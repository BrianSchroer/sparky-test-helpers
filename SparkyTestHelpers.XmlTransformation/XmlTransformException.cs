using System;

namespace SparkyTestHelpers.XmlTransformation
{
    /// <summary>
    /// Exception thrown when an XML transformation does not contain expected values. 
    /// </summary>
    public class XmlTransformException : Exception
    {
        /// <summary>
        /// Creates new <see cref="XmlTransformException"/> instance.
        /// </summary>
        /// <param name="msg">The exception message.</param>
        public XmlTransformException(string msg) : base(msg)
        {
        }
    }
}