using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SparkyTestHelpers.XmlTransformation
{
    /// <summary>
    /// Helper class for unit testing XML (.config) file transformations.
    /// </summary>
    public class XmlTransformHelper
    {
        private static readonly Dictionary<string, TransformResults> _transformResults
            = new Dictionary<string, TransformResults>();

        private static readonly object _transformResultsLock = new object();

        private readonly string[] _transformFiles;

        private XDocument _configXDocument;

        /// <summary>
        /// <see cref="XDocument"/> for transformed web.config XML.
        /// </summary>
        public XDocument ConfigXDocument
        {
            get
            {
                if (_configXDocument == null)
                {
                    throw new InvalidOperationException(
                        "\"AssertSuccessfulTransformation\" must be called before any other methods.");
                }

                return _configXDocument;
            }

            private set
            {
                _configXDocument = value;
            }
        }

        /// <summary>
        /// The transformed XML string.
        /// </summary>
        public string TransformedXml { get; private set; }

        /// <summary>
        /// Assert that specified configuration/appSettings key doesn't exist.
        /// </summary>
        /// <param name="key">The key.</param>
        public void AssertAppSettingsKeyDoesNotExist(string key)
        {
            AssertElementDoesNotExist($"configuration/appSettings/add[@key='{key}']");
        }

        /// <summary>
        /// Assert element exists for specified XPath expression.
        /// </summary>
        /// <param name="expression">XPath expression.</param>
        /// <returns>The found <see cref="XElement"/> instance.</returns>
        /// <exception cref="XmlTransformException" if element not found. />
        public XElement AssertElementExists(string expression)
        {
            XElement elem = GetElement(expression);

            if (elem == null)
            {
                AssertFail($"Element not found: {FormatFailureMessage(expression)}");
            }

            return elem;
        }

        public void AssertElementDoesNotExist(string expression)
        {
            XElement elem = GetElement(expression);
           
            if (elem != null)
            {
                AssertFail($"Element should not exist: {FormatFailureMessage(expression)}");
            }
        }

        /// <summary>
        /// Get first element matching XPath expression.
        /// </summary>
        /// <param name="expression">XPath expression.</param>
        /// <returns><see cref="XElement"/>.</returns>
        public XElement GetElement(string expression)
        {
            return ConfigXDocument.XPathSelectElement(expression);
        }

        /// <summary>
        /// Get elements matching XPath expression.
        /// </summary>
        /// <param name="expression">XPath expression.</param>
        /// <returns><see cref="XElement"/>.</returns>
        public IEnumerable<XElement> GetELements(string expression)
        {
            return ConfigXDocument.XPathSelectElements(expression);
        }

        /// <summary>
        /// Assert failure, with message prefix showing the .config transform file names.
        /// </summary>
        /// <param name="message">The failure message.</param>
        private void AssertFail(string message)
        {
            string prefix = $"Testing XML transformation for {string.Join(" / ", _transformFiles ?? new string[0])}\n";
            throw new XmlTransformException($"{prefix}{message}");
        }

        private static string FormatFailureMessage(string elementExpression, string attributeName = null)
        {
            string formattedExpression = $"Element: <{elementExpression.Replace("/", ">/<")}>";

            return (attributeName == null)
                ? formattedExpression
                : $"{formattedExpression} Attribute: \"{attributeName}\".";
        }
    }
}
