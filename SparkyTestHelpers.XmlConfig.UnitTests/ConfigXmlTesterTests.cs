using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using SparkyTestHelpers.Exceptions;
using System.Collections.Generic;
using System.Linq;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.XmlConfig.UnitTests
{
    /// <summary>
    /// <see cref="ConfigXmlTester"/> (and base class <see cref="XmlTester"/>) tests.
    /// </summary>
    [TestClass]
    public class ConfigXmlTesterTests
    {
        private string _xmlFormat =
@"<?xml version=""1.0"" encoding=""utf-8""?>
 <configuration>
{0}
 </configuration>";

        private string _appSettingsSectionFormat =
 @"<appSettings>
{0}
</appSettings>";

        [TestMethod]
        public void Constructor_using_XDocument_should_work()
        {
            string xml = FormattedXml();
            var xDocument = XDocument.Parse(xml);

            AssertExceptionNotThrown.WhenExecuting(() => new ConfigXmlTester(xDocument));
        }

        [TestMethod]
        public void Constructor_using_XML_string_should_work()
        {
            AssertExceptionNotThrown.WhenExecuting(() => new ConfigXmlTester(FormattedXml()));
        }

        [TestMethod]
        public void AppSettingsKeyXPath_should_return_expected_value()
        {
            Assert.AreEqual(
                "configuration/appSettings/add[@key='specifiedKey']", 
                ConfigXmlTester.AppSettingsKeyXPath("specifiedKey"));
        }

        [TestMethod]
        public void AssertAppSettingsKeyDoesNotExist_should_not_throw_exception_when_key_does_not_exist()
        {
            var tester = new ConfigXmlTester(FormattedXml());
            AssertExceptionNotThrown.WhenExecuting(() => tester.AssertAppSettingsKeyDoesNotExist("testkey"));
        }

        [TestMethod]
        public void AssertAppSettingsKeyDoesNot_exist_should_throw_exception_when_key_exists()
        {
            var tester = new ConfigXmlTester(FormattedXmlWithAppSetting("testKey", "testValue"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element should not exist: Element: <configuration>/<appSettings>/<add[@key='testKey']>")
                .WhenExecuting(() => tester.AssertAppSettingsKeyDoesNotExist("testKey"));
        }

        [TestMethod]
        public void AssertAppSettingsValue_should_throw_exception_when_key_does_not_exist()
        {
            var tester = new ConfigXmlTester(FormattedXml());

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element not found: Element: <configuration>/<appSettings>/<add[@key='testKey']>")
                .WhenExecuting(() => tester.AssertAppSettingsValue("testKey", "testValue"));
        }

        [TestMethod]
        public void Exceptions_should_use_exceptionPrefix_when_supplied()
        {
            var tester = new ConfigXmlTester(FormattedXml(), "Exception prefix: ");

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Exception prefix: Element not found: Element: <configuration>/<appSettings>/<add[@key='testKey']>")
                .WhenExecuting(() => tester.AssertAppSettingsValue("testKey", "testValue"));
        }

        [TestMethod]
        public void AssertAppSettingsValue_should_throw_exception_when_value_does_not_match()
        {
            var tester = new ConfigXmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element: <configuration>/<appSettings>/<add[@key='testKey']> Attribute: \"value\":" 
                    + " Expected: <expectedValue> actual: <actualValue>")
                .WhenExecuting(() => tester.AssertAppSettingsValue("testKey", "expectedValue"));
        }

        [TestMethod]
        public void GetElement_should_return_element_when_it_exists()
        {
            var tester = new ConfigXmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            XElement elem = tester.GetElement(ConfigXmlTester.AppSettingsKeyXPath("testKey"));

            Assert.IsNotNull(elem);
        }

        [TestMethod]
        public void GetElement_should_return_null_when_element_does_not_exist()
        {
            var tester = new ConfigXmlTester(FormattedXml());

            XElement elem = tester.GetElement(ConfigXmlTester.AppSettingsKeyXPath("testKey"));

            Assert.IsNull(elem);
        }

        [TestMethod]
        public void GetAttributeValue_should_return_value_when_attribute_exists()
        {
            var tester = new ConfigXmlTester(FormattedXmlWithAppSetting("testKey", "testValue"));

            XElement elem = tester.GetElement(ConfigXmlTester.AppSettingsKeyXPath("testKey"));

            Assert.AreEqual("testValue", tester.GetAttributeValue(elem, "value"));
        }

        [TestMethod]
        public void GetAttributeValue_should_return_null_when_attribute_does_not_exist()
        {
            var tester = new ConfigXmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            XElement elem = tester.GetElement(ConfigXmlTester.AppSettingsKeyXPath("testKey"));

            Assert.IsNull(tester.GetAttributeValue(elem, "badAttributeName"));
        }

        [TestMethod]
        public void AssertAttributeValue_should_not_throw_when_value_matches()
        {
            var tester = new ConfigXmlTester(FormattedXmlWithAppSetting("testKey", "testValue"));

            AssertExceptionNotThrown
                .WhenExecuting(() =>
                    tester.AssertAttributeValue(ConfigXmlTester.AppSettingsKeyXPath("testKey"), "value", "testValue"));
        }

        [TestMethod]
        public void AssertAttributeValue_should_throw_exception_when_attribute_does_not_exist()
        {
            var tester = new ConfigXmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element found, but attribute does not exist: " 
                    + "Element: <configuration>/<appSettings>/<add[@key='testKey']> Attribute: \"badAttribute\"")
                .WhenExecuting(() => 
                    tester.AssertAttributeValue(ConfigXmlTester.AppSettingsKeyXPath("testKey"), "badAttribute", "value"));
        }

        [TestMethod]
        public void AssertAttributeValue_should_throw_exception_when_value_does_not_match()
        {
            var tester = new ConfigXmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessageContaining("Element: <configuration>/<appSettings>/<add[@key='testKey']> Attribute: \"value\":"
                    + " Expected: <expectedValue> actual: <actualValue>")
                .WhenExecuting(() =>
                    tester.AssertAttributeValue(ConfigXmlTester.AppSettingsKeyXPath("testKey"), "value", "expectedValue"));
        }

        [TestMethod]
        public void GetElements_should_return_empty_enumerable_when_elements_do_not_exist()
        {
            var tester = new ConfigXmlTester(FormattedXml());

            XElement[] elems = tester.GetElements("configuration/appSettings/add").ToArray();

            Assert.AreEqual(0, elems.Length);
        }

        [TestMethod]
        public void GetElements_should_return_elements_when_they_exist()
        {
            string appSettingsXml = string.Join("\n", 
                Enumerable.Range(1, 5).Select(i => FormattedAppSettingsElem($"key{i}", $"value{i}")));

            var tester = new ConfigXmlTester(FormattedXml(FormattedAppSettingsSection(appSettingsXml)));

            XElement[] elems = tester.GetElements("configuration/appSettings/add").ToArray();

            Assert.AreEqual(5, elems.Length);
        }

        [TestMethod]
        public void AssertAppSettingsValues_should_not_throw_exception_when_values_match()
        {
            KeyValuePair<string, string>[] appSettings = 
                Enumerable.Range(1, 5).Select(i => new KeyValuePair<string, string>($"key{i}", $"value{i}")).ToArray();

            string appSettingsXml = string.Join("\n", appSettings.Select(kv => FormattedAppSettingsElem(kv.Key, kv.Value)));

            var tester = new ConfigXmlTester(FormattedXml(FormattedAppSettingsSection(appSettingsXml)));

            AssertExceptionNotThrown.WhenExecuting(() => tester.AssertAppSettingsValues(appSettings));
        }

        [TestMethod]
        public void AssertAppSettingsValues_should_not_exception_when_values_do_not_match()
        {
            KeyValuePair<string, string>[] appSettings =
                Enumerable.Range(1, 5).Select(i => new KeyValuePair<string, string>($"key{i}", $"value{i}")).ToArray();

            string appSettingsXml = string.Join("\n", appSettings.Select(kv => FormattedAppSettingsElem(kv.Key, kv.Value)));

            appSettingsXml = appSettingsXml.Replace("value3", "value3X");

            var tester = new ConfigXmlTester(FormattedXml(FormattedAppSettingsSection(appSettingsXml)));

            AssertExceptionThrown
                .OfType<ScenarioTestFailureException>()
                .WithMessageContaining("Expected: <value3> actual: <value3X>")
                .WhenExecuting(() => tester.AssertAppSettingsValues(appSettings));
        }

        private string FormattedXml(string xml = null)
        {
            return string.Format(_xmlFormat, xml ?? FormattedAppSettingsSection());
        }

        private string FormattedXmlWithAppSetting(string key, string value)
        {
            return FormattedXml(FormattedAppSettingsSection(FormattedAppSettingsElem(key, value)));
        }

        private string FormattedAppSettingsSection(string appSettingsElementsXml = null)
        {
            return string.Format(_appSettingsSectionFormat, appSettingsElementsXml);
        }

        private string FormattedAppSettingsElem(string key, string value)
        {
            return $"<add key=\"{key}\" value=\"{value}\" />";
        }
    }
}
