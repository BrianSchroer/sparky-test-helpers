using System.Xml.Linq;

namespace SparkyTestHelpers.Xml.Transformation
{
    /// <summary>
    /// XML transformation results.
    /// </summary>
    public class TransformResults
    {
        /// <summary>
        /// Was the transformation successful?
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// The transformed <see cref="XDocument"/> (if <see cref="Successful"/>)
        /// </summary>
        public XDocument XDocument { get; set; }

        /// <summary>
        /// The transformed XML string
        /// </summary>
        public string TransformedXml { get; set; }

        /// <summary>
        /// Error message (if not <see cref="Successful"/>;
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Details about the files and transformation steps involved.
        /// </summary>
        public string Log { get; set; }
    }
}
