using System.Collections.Generic;
using System.Xml.Linq;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.XmlConfig
{
    /// <summary>
    /// Helper class for testing .config file XML values.
    /// </summary>
    public class ConfigXmlTester : XmlTester
    {
        /// <summary>
        /// Creates a new <see cref="ConfigXmlTester"/> instance.
        /// </summary>
        /// <param name="xml">XML string to be parsed to an <see cref="XDocument"/> for testing.</param>
        /// <param name="exceptionPrefix">(Optional) prefix for exception messages.</param>
        public ConfigXmlTester(string xml, string exceptionPrefix = null) : base(xml, exceptionPrefix)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ConfigXmlTester"/> instance.
        /// </summary>
        /// <param name="xDocument">The <see cref="XDocument"/> to be tested.</param>
        /// <param name="exceptionPrefix">(Optional) prefix for exception messages.</param>
        public ConfigXmlTester(XDocument xDocument, string exceptionPrefix = null) : base(xDocument, exceptionPrefix)
        {
        }

        /// <summary>
        /// Build XPath string for AppSettings key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>XPath string.</returns>
        public static string AppSettingsKeyXPath(string key)
        {
            return $"configuration/appSettings/add[@key='{key}']";
        }

        /// <summary>
        /// Assert that specified configuration/appSettings key doesn't exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="XmlTesterException" if key is found. />
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
   }
}
