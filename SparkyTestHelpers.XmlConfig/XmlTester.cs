using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SparkyTestHelpers.XmlConfig
{
    /// <summary>
    /// Helper class for testing <see cref="System.Xml.Linq.XDocument"/> values.
    /// </summary>
    public class XmlTester
    {
        private string _exceptionPrefix;

        /// <summary>
        /// The <see cref="XDocument"/> being tested.
        /// </summary>
        public XDocument XDocument { get; private set; }

        /// <summary>
        /// Creates a new <see cref="XmlTester"/> instance.
        /// </summary>
        /// <param name="xml">XML string to be parsed to an <see cref="System.Xml.Linq.XDocument"/> for testing.</param>
        /// <param name="exceptionPrefix">(Optional) prefix for exception messages.</param>
        public XmlTester(string xml, string exceptionPrefix = null) : this(XDocument.Parse(xml), exceptionPrefix) 
        {
        }

        /// <summary>
        /// Creates a new <see cref="XmlTester"/> instance.
        /// </summary>
        /// <param name="xDocument">The <see cref="System.Xml.Linq.XDocument"/> to be tested.</param>
        /// <param name="exceptionPrefix">(Optional) prefix for exception messages.</param>
        public XmlTester(XDocument xDocument, string exceptionPrefix = null)
        {
            XDocument = xDocument;
            _exceptionPrefix = exceptionPrefix;
        }

        /// <summary>
        /// Assert the element attribute has expected value.
        /// </summary>
        /// <param name="elementExpression">Element XPath expression.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="expectedValue">Expected attribute value.</param>
        /// <exception cref="XmlTesterException" if element not found or unexpected attribute value. />
        public void AssertAttributeValue(string elementExpression, string attributeName, string expectedValue)
        {
            XElement elem = AssertElementExists(elementExpression);
            AssertElementAttributeValue(elem, elementExpression, attributeName, expectedValue);
        }

        /// <summary>
        /// Assert element does not exist for specified XPath expresion.
        /// </summary>
        /// <param name="expression">XPath expression.</param>
        /// <exception cref="XmlTesterException" if element is found. />
        public void AssertElementDoesNotExist(string expression)
        {
            XElement elem = GetElement(expression);
           
            if (elem != null)
            {
                AssertFail($"Element should not exist: {FormatFailureMessage(expression)}");
            }
        }

        /// <summary>
        /// Assert element exists for specified XPath expression.
        /// </summary>
        /// <param name="expression">XPath expression.</param>
        /// <returns>The found <see cref="XElement"/> instance.</returns>
        /// <exception cref="XmlTesterException" if element not found. />
        public XElement AssertElementExists(string expression)
        {
            XElement elem = GetElement(expression);

            if (elem == null)
            {
                AssertFail($"Element not found: {FormatFailureMessage(expression)}");
            }

            return elem;
        }

        /// <summary>
        /// Get XEmelent attribute value.
        /// </summary>
        /// <param name="elem">The <see cref="XElement"/>.</param>
        /// <param name="attributeName">The attribute name.</param>
        /// <returns>The attribute value (null if not found).</returns>
        public string GetAttributeValue(XElement elem, string attributeName)
        {
            XAttribute[] attributes = (elem.Attributes(attributeName) ?? Enumerable.Empty<XAttribute>()).ToArray();

            switch (attributes.Length)
            {
                case 1:
                    return attributes[0].Value;
                case 0:
                    return null;
                default:
                    AssertFail($"{elem.Name} [{attributeName}] attribute count = {attributes.Length}.");
                    return null;
            }
        }

        /// <summary>
        /// Get first element matching XPath expression.
        /// </summary>
        /// <param name="expression">XPath expression.</param>
        /// <returns><see cref="XElement"/> (null if not found).</returns>
        public XElement GetElement(string expression)
        {
            return XDocument.XPathSelectElement(expression);
        }

        /// <summary>
        /// Get elements matching XPath expression.
        /// </summary>
        /// <param name="expression">XPath expression.</param>
        /// <returns><see cref="XElement"/>s.</returns>
        public IEnumerable<XElement> GetElements(string expression)
        {
            return XDocument.XPathSelectElements(expression);
        }

        protected void AssertElementAttributeValue(
            XElement elem, string elementExpression, string attributeName, string expectedValue)
        {
            string actual = GetAttributeValue(elem, attributeName);

            if (actual == null)
            {
                AssertFail(
                    $"Element found, but attribute does not exist: {FormatFailureMessage(elementExpression, attributeName)}");
            }
            else
            {
                if (!actual.Equals(expectedValue, StringComparison.CurrentCulture))
                {
                    AssertFail(
                        $"{FormatFailureMessage(elementExpression, attributeName)}: Expected: <{expectedValue}> actual: <{actual}>");
                }
            }
        }

        /// <summary>
        /// Assert failure by throwing <see cref="XmlTesterException"/>.
        /// </summary>
        /// <param name="message">The failure message.</param>
        protected void AssertFail(string message)
        {
            throw new XmlTesterException($"{_exceptionPrefix}{message}");
        }

        protected static string FormatFailureMessage(string elementExpression, string attributeName = null)
        {
            string formattedExpression = $"Element: <{elementExpression.Replace("/", ">/<")}>";

            return (attributeName == null)
                ? formattedExpression
                : $"{formattedExpression} Attribute: \"{attributeName}\"";
        }
    }
}
