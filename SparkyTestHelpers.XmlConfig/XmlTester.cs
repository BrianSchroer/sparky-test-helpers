using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.XmlConfig
{
    /// <summary>
    /// Helper class for testing <see cref="System.Xml.Linq.XDocument"/> values.
    /// </summary>
    public class XmlTester
    {
        private readonly string _exceptionPrefix;

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
        /// <exception cref="XmlTesterException" if element not found, attribute not found, or value does not match. />
        public void AssertAttributeValue(string elementExpression, string attributeName, string expectedValue)
        {
            XElement elem = AssertElementExists(elementExpression);

            AssertElementAttributeValue(elem, elementExpression, attributeName,
                actual => actual.Equals(expectedValue, StringComparison.CurrentCulture)
                    ? null
                    : $"Expected: <{expectedValue}> actual: <{actual}>");
        }

        /// <summary>
        /// Assert that element attributes have expected values.
        /// </summary>
        /// <param name="elementExpression">Element XPath expresion.</param>
        /// <param name="attributeNamesAndExpectedValues">
        /// <see cref="IDictionary{String, String}"/> or other
        /// <see cref="IEnumerable{KeyValuePair{String, String}"/> of attribute names / expected values.
        /// </param>
        public void AssertAttributeValues(string elementExpression, 
            IEnumerable<KeyValuePair<string, string>> attributeNamesAndExpectedValues)
        {
            XElement elem = AssertElementExists(elementExpression);

            attributeNamesAndExpectedValues.TestEach(kvp =>
                AssertElementAttributeValue(elem, elementExpression, kvp.Key,
                    actual => actual.Equals(kvp.Value, StringComparison.CurrentCulture)
                        ? null
                        : $"Expected: <{kvp.Value}> actual: <{actual}>"));
        }

        /// <summary>
        /// Assert the element attribute value matches regular expression pattern.
        /// </summary>
        /// <param name="elementExpression">Element XPath expression.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="pattern">Regex pattern for expected attribute value.</param>
        /// <exception cref="XmlTesterException" if element not found, attribute not found, or value does not match. />
        public void AssertAttributeValueMatch(string elementExpression, string attributeName, string pattern)
        {
            XElement elem = AssertElementExists(elementExpression);

            AssertElementAttributeValue(elem, elementExpression, attributeName,
                actual => Regex.Match(actual, pattern).Success
                ? null
                : $"Value \"{actual}\" doesn't match regex pattern \"{pattern}\".");
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
        /// Assert that element exists, but does not have the specified attribute.
        /// </summary>
        /// <param name="elementExpression">Element XPath expression.</param>
        /// <param name="attributeName">Attribute name.</param>
        public void AssertElementDoesNotHaveAttribute(string elementExpression, string attributeName)
        {
            XElement elem = AssertElementExists(elementExpression);

            XAttribute[] attributes = GetAttributeArray(elem, attributeName);

            if (attributes.Length > 0)
            {
                AssertFail($"{FormatFailureMessage(elementExpression, attributeName)}: Attribute should not exist.");
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
        /// Assert element/attribute value is a well-formed URI string.
        /// </summary>
        /// <param name="elementExpression">XPath expression.</param>
        /// <param name="attributeName">Attribute name.</param>
        public void AssertAttributeValueIsWellFormedUrl(string elementExpression, string attributeName)
        {
            XElement elem = AssertElementExists(elementExpression);

            AssertElementAttributeValue(elem, elementExpression, attributeName,
                actual => IsWellFormedUriString(actual)
                ? null
                : $"Value \"{actual}\" is not a well-formed URI string.");
        }

        /// <summary>
        /// Get XEmelent attribute value.
        /// </summary>
        /// <param name="elem">The <see cref="XElement"/>.</param>
        /// <param name="attributeName">The attribute name.</param>
        /// <returns>The attribute value (null if not found).</returns>
        public string GetAttributeValue(XElement elem, string attributeName)
        {
            XAttribute[] attributes = GetAttributeArray(elem, attributeName);

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

        private void AssertElementAttributeValue(
            XElement elem, string elementExpression, string attributeName, Func<string, string> valueChecker)
        {
            string actual = GetAttributeValue(elem, attributeName);

            if (actual == null)
            {
                AssertFail(
                    $"Element found, but attribute does not exist: {FormatFailureMessage(elementExpression, attributeName)}");
            }
            else
            {
                string errorMessage = valueChecker(actual);
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    AssertFail(
                       $"{FormatFailureMessage(elementExpression, attributeName)}: {errorMessage}");
                }
            }
        }

        private bool IsWellFormedUriString(string uriString)
        {
            return Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Assert failure by throwing <see cref="XmlTesterException"/>.
        /// </summary>
        /// <param name="message">The failure message.</param>
        private void AssertFail(string message)
        {
            throw new XmlTesterException($"{_exceptionPrefix}{message}");
        }

        private static string FormatFailureMessage(string elementExpression, string attributeName = null)
        {
            string attributeString = (attributeName == null) ? null : $"@{attributeName}";
            return $"{elementExpression}{attributeString}";
        }

        private static XAttribute[] GetAttributeArray(XElement elem, string attributeName)
        {
            return (elem.Attributes(attributeName) ?? Enumerable.Empty<XAttribute>()).ToArray();
        }
    }
}
