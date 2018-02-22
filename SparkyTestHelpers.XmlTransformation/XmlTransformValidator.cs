using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.XmlTransformation
{
    /// <summary>
    /// Helper class for validating XML (.config) file values.
    /// </summary>
    public class XmlTransformValidator
    {
        private string _exceptionPrefix;

        /// <summary>
        /// <see cref="XDocument"/> for transformed web.config XML.
        /// </summary>
        public XDocument ConfigXDocument { get; set; }

        /// <summary>
        /// Creates a new <see cref="XmlTransformValidator"/> instance.
        /// </summary>
        /// <param name="configXDocument">The .config file <see cref="XDocument"/> to be validated.</param>
        /// <param name="exceptionPrefix">(Optional) prefix for exception messages.</param>
        public XmlTransformValidator(XDocument configXDocument, string exceptionPrefix = null)
        {
            ConfigXDocument = configXDocument;
            _exceptionPrefix = exceptionPrefix;
        }

        /// <summary>
        /// Assert that specified configuration/appSettings key doesn't exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="XmlTransformException" if key is found. />
        public void AssertAppSettingsKeyDoesNotExist(string key)
        {
            AssertElementDoesNotExist(AppSettingsKeyXPath(key));
        }

        /// <summary>
        /// Assert expected value for configuation/appSettings key.
        /// </summary>
        /// <param name="key">The appSettings key.</param>
        /// <param name="expectedValue">The expected value.</param>
        public void AssertAppSettingsValue(string key, string expectedValue)
        {
            AssertAttributeValue(AppSettingsKeyXPath(key), "value", expectedValue);
        }

        /// <summary>
        /// Assert expected values for configuration/appSettings keys.
        /// </summary>
        /// <param name="keysAndValues">
        /// <see cref="IDictionary{TKey, TValue}"/> or other
        /// <see cref="IEnumerable{KeyValuePair{String, String}"/> of keys / expected values.
        /// </param>
        public void AssertAppSettingsValues(IEnumerable<KeyValuePair<string, string>> keysAndValues)
        {
            keysAndValues.TestEach(scanario => AssertAppSettingsValue(scanario.Key, scanario.Value));
        }

        /// <summary>
        /// Assert the element attribute has expected value.
        /// </summary>
        /// <param name="elementExpression">Element XPath expression.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="expectedValue">Expected attribute value.</param>
        /// <exception cref="XmlTransformException" if element not found or unexpected attribute value. />
        public void AssertAttributeValue(string elementExpression, string attributeName, string expectedValue)
        {
            XElement elem = AssertElementExists(elementExpression);
            AssertElementAttributeValue(elem, elementExpression, attributeName, expectedValue);
        }

        /// <summary>
        /// Assert element does not exist for specified XPath expresion.
        /// </summary>
        /// <param name="expression">XPath expression.</param>
        /// <exception cref="XmlTransformException" if element is found. />
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
            return ConfigXDocument.XPathSelectElement(expression);
        }

        /// <summary>
        /// Get elements matching XPath expression.
        /// </summary>
        /// <param name="expression">XPath expression.</param>
        /// <returns><see cref="XElement"/>.</returns>
        public IEnumerable<XElement> GetElements(string expression)
        {
            return ConfigXDocument.XPathSelectElements(expression);
        }

        private void AssertElementAttributeValue(
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

        private static string AppSettingsKeyXPath(string key)
        {
            return $"configuration/appSettings/add[@key='{key}']";
        }

        /// <summary>
        /// Assert failure by throwing <see cref="XmlTransformException"/>.
        /// </summary>
        /// <param name="message">The failure message.</param>
        private void AssertFail(string message)
        {
            throw new XmlTransformException($"{_exceptionPrefix}{message}");
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
