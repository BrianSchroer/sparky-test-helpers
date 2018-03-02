using SparkyTestHelpers.Scenarios;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SparkyTestHelpers.Xml.Config
{
    /// <summary>
    /// <see cref="XmlTester"/> extension methods for .config file XML.
    /// </summary>
    public static class XmlTesterConfigExtensions
    {
        /// <summary>
        /// Assert that specified configuration/appSettings key doesn't exist.
        /// </summary>

        /// <param name="xmlTester">The <see cref="XmlTester"/></param>
        /// <param name="key">The key.</param>
        /// <exception cref="XmlTesterException">if key is found.</exception>
        public static void AssertAppSettingsKeyDoesNotExist(this XmlTester xmlTester, string key)
        {
            xmlTester.AssertElementDoesNotExist(ConfigXPath.AppSettingForKey(key));
        }

        /// <summary>
        /// Assert expected value for configuration/appSettings key.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <param name="key">The appSettings key.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <exception cref="XmlTesterException">if element not found, attribute not found, or value does not match.</exception>
        public static void AssertAppSettingsValue(this XmlTester xmlTester, string key, string expectedValue)
        {
            xmlTester.AssertAttributeValue(ConfigXPath.AppSettingForKey(key), "value", expectedValue);
        }

        /// <summary>
        /// Assert that value for configuration/appSettings key matches RegEx pattern.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <param name="key">The appSettings key.</param>
        /// <param name="regExPattern">The RegEx pattern.</param>
        /// <exception cref="XmlTesterException">if element not found, attribute not found, or value does not match.</exception>
        public static void AssertAppSettingsValueMatch(this XmlTester xmlTester, string key, string regExPattern)
        {
            xmlTester.AssertAttributeValueMatch(ConfigXPath.AppSettingForKey(key), "value", regExPattern);
        }

        /// <summary>
        /// Assert expected values for configuration/appSettings keys.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <param name="keysAndValues">
        /// <see cref="IDictionary{String, String}"/> or other
        /// <see cref="IEnumerable{KeyValuePair}"/> of keys / expected values.
        /// </param>
        /// <exception cref="XmlTesterException">if element not found or value does not match.</exception>
        public static void AssertAppSettingsValues(
            this XmlTester xmlTester, IEnumerable<KeyValuePair<string, string>> keysAndValues)
        {
            keysAndValues.TestEach(kvp => 
                xmlTester.AssertAppSettingsValue(kvp.Key, kvp.Value));
        }

        /// <summary>
        /// Assert that ServiceModel client endpoint address is a well-formed URLs.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <param name="endpointName">The endpoint name.</param>
        /// <exception cref="XmlTesterException">if endpoint not found or address is not a well-formed URL.</exception>
        public static void AssertClientEndpointAddressIsWellFormedUrl(this XmlTester xmlTester, string endpointName)
        {
            xmlTester.AssertAttributeValueIsWellFormedUrl(ConfigXPath.ClientEndpointForName(endpointName), "address");
        }

        /// <summary>
        /// Assert that all ServiceModel client endpoint addresses are well-formed URLs.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <exception cref="XmlTesterException">if any endpoint address is not a well-formed URL.</exception>
        public static void AssertClientEndpointAddressesAreWellFormedUrls(this XmlTester xmlTester)
        {
            xmlTester.GetClientEndpointElements().TestEach(endpoint =>
                xmlTester.AssertClientEndpointAddressIsWellFormedUrl(xmlTester.GetAttributeValue(endpoint, "name")));
        }

        /// <summary>
        /// Assert that confirmation/system.web/compilation "debug" attribute has been removed or set to false.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <exception cref="XmlTesterException">if element has debug value of true.</exception>
        public static void AssertCompilationDebugFalse(this XmlTester xmlTester)
        {
            var elem = xmlTester.GetElement(ConfigXPath.SystemWebCompilation);

            if (elem != null)
            {
                string value = xmlTester.GetAttributeValue(elem, "debug");
                if (value == "true")
                {
                    xmlTester.AssertAttributeValue(ConfigXPath.SystemWebCompilation, "debug", "false");
                }
            }
        }

        /// <summary>
        /// Get <see cref="XElement"/> for "appSettings" key.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <param name="key">The key.</param>
        /// <returns>The <see cref="XElement"/>.</returns>
        public static XElement GetAppSettingsElement(this XmlTester xmlTester, string key)
        {
            return xmlTester.AssertElementExists(ConfigXPath.AppSettingForKey(key));
        }

        /// <summary>
        /// Get appSettings <see cref="XElement"/>s.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <returns>New <see cref="IEnumerable{XElement}"/>.</returns>
        public static IEnumerable<XElement> GetAppSettingsElements(this XmlTester xmlTester)
        {
            return xmlTester.GetElements(ConfigXPath.AppSettings);
        }

        /// <summary>
        /// Get value for "appSettings" key.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public static string GetAppSettingsValue(this XmlTester xmlTester, string key)
        {
            return xmlTester.GetAttributeValue(xmlTester.GetAppSettingsElement(key), "value");
        }

        /// <summary>
        /// Get "address" value for ServiceModel client endpoint.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <param name="endpointName">The endpoint name.</param>
        /// <returns>The endpoint address.</returns>
        public static string GetClientEndpointAddress(this XmlTester xmlTester, string endpointName)
        {
            return xmlTester.GetAttributeValue(xmlTester.GetClientEndpointElement(endpointName), "address");
        }

        /// <summary>
        /// Get <see cref="XElement"/> for ServiceModel client endpoint.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <param name="endpointName">The endpoint name.</param>
        /// <returns>The <see cref="XElement"/>.</returns>
        public static XElement GetClientEndpointElement(this XmlTester xmlTester, string endpointName)
        {
            return xmlTester.AssertElementExists(ConfigXPath.ClientEndpointForName(endpointName));
        }

        /// <summary>
        /// Get service client endpoint <see cref="XElement"/>s.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <returns>New <see cref="IEnumerable{XElement}"/>.</returns>
        public static IEnumerable<XElement> GetClientEndpointElements(this XmlTester xmlTester)
        {
            return xmlTester.GetElements(ConfigXPath.ClientEndpoints);
        }

        /// <summary>
        /// Get connection string for name key.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <param name="name">The name key</param>
        /// <returns>The connection string.</returns>
        public static string GetConnectionString(this XmlTester xmlTester, string name)
        {
            return xmlTester.GetAttributeValue(xmlTester.GetConnectionStringElement(name), "connectionString");
        }

        /// <summary>
        /// Get connection string <see cref="XElement"/> for name key.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <param name="name">The name key.</param>
        /// <returns>The <see cref="XElement"/>.</returns>
        public static XElement GetConnectionStringElement(this XmlTester xmlTester, string name)
        {
            return xmlTester.AssertElementExists(ConfigXPath.ConnectionStringForName(name));
        }


        /// <summary>
        /// Get connection string <see cref="XElement"/>s.
        /// </summary>
        /// <param name="xmlTester"><see cref="XmlTester"/>.</param>
        /// <returns>New <see cref="IEnumerable{XElement}"/>.</returns>
        public static IEnumerable<XElement> GetConnectionStringElements(this XmlTester xmlTester)
        {
            return xmlTester.GetElements(ConfigXPath.ConnectionStrings);
        }
    }
}
