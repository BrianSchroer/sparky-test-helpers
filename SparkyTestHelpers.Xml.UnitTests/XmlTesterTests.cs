using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using SparkyTestHelpers.Exceptions;
using System.Collections.Generic;
using System.Linq;
using SparkyTestHelpers.Scenarios;
using SparkyTestHelpers.Xml.Config;

namespace SparkyTestHelpers.Xml.UnitTests
{
    /// <summary>
    /// <see cref="XmlTester"/> tests.
    /// </summary>
    [TestClass]
    public class XmlTesterTests
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

            AssertExceptionNotThrown.WhenExecuting(() => new XmlTester(xDocument));
        }

        [TestMethod]
        public void Constructor_using_XML_string_should_work()
        {
            AssertExceptionNotThrown.WhenExecuting(() => new XmlTester(FormattedXml()));
        }

        [TestMethod]
        public void Exceptions_should_use_exceptionPrefix_when_supplied()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"), "EXCEPTION_PREFIX: ");

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("EXCEPTION_PREFIX: Element found, but attribute does not exist: "
                    + "configuration/appSettings/add[@key='testKey']@badAttribute")
                .WhenExecuting(() =>
                    tester.AssertAttributeValue(ConfigXPath.AppSettingForKey("testKey"), "badAttribute", "value"));
        }

        [TestMethod]
        public void GetElement_should_return_element_when_it_exists()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            XElement elem = tester.GetElement(ConfigXPath.AppSettingForKey("testKey"));

            Assert.IsNotNull(elem);
        }

        [TestMethod]
        public void GetElement_should_return_null_when_element_does_not_exist()
        {
            var tester = new XmlTester(FormattedXml());

            XElement elem = tester.GetElement(ConfigXPath.AppSettingForKey("testKey"));

            Assert.IsNull(elem);
        }

        [TestMethod]
        public void GetAttributeValue_should_return_value_when_attribute_exists()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "testValue"));

            XElement elem = tester.GetElement(ConfigXPath.AppSettingForKey("testKey"));

            Assert.AreEqual("testValue", tester.GetAttributeValue(elem, "value"));
        }

        [TestMethod]
        public void GetAttributeValue_should_return_null_when_attribute_does_not_exist()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            XElement elem = tester.GetElement(ConfigXPath.AppSettingForKey("testKey"));

            Assert.IsNull(tester.GetAttributeValue(elem, "badAttributeName"));
        }

        [TestMethod]
        public void AssertAttributeValue_should_not_throw_when_value_matches()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "testValue"));

            AssertExceptionNotThrown
                .WhenExecuting(() =>
                    tester.AssertAttributeValue(ConfigXPath.AppSettingForKey("testKey"), "value", "testValue"));
        }

        [TestMethod]
        public void AssertAttributeValue_should_throw_exception_when_attribute_does_not_exist()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element found, but attribute does not exist: "
                    + "configuration/appSettings/add[@key='testKey']@badAttribute")
                .WhenExecuting(() =>
                    tester.AssertAttributeValue(ConfigXPath.AppSettingForKey("testKey"), "badAttribute", "value"));
        }

        [TestMethod]
        public void AssertAttributeValueMatch_should_throw_exception_when_attribute_does_not_exist()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element found, but attribute does not exist: "
                    + "configuration/appSettings/add[@key='testKey']@badAttribute")
                .WhenExecuting(() =>
                    tester.AssertAttributeValueMatch(ConfigXPath.AppSettingForKey("testKey"), "badAttribute", "value"));
        }

        [TestMethod]
        public void AssertAttributeValue_should_throw_exception_when_value_does_not_match()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessageContaining("configuration/appSettings/add[@key='testKey']@value:"
                    + " Expected: <expectedValue> actual: <actualValue>")
                .WhenExecuting(() =>
                    tester.AssertAttributeValue(ConfigXPath.AppSettingForKey("testKey"), "value", "expectedValue"));
        }

        [TestMethod]
        public void GetElements_should_return_empty_enumerable_when_elements_do_not_exist()
        {
            var tester = new XmlTester(FormattedXml());

            XElement[] elems = tester.GetElements("configuration/appSettings/add").ToArray();

            Assert.AreEqual(0, elems.Length);
        }

        [TestMethod]
        public void GetElements_should_return_elements_when_they_exist()
        {
            string appSettingsXml = string.Join("\n",
                Enumerable.Range(1, 5).Select(i => FormattedAppSettingsElem($"key{i}", $"value{i}")));

            var tester = new XmlTester(FormattedXml(FormattedAppSettingsSection(appSettingsXml)));

            XElement[] elems = tester.GetElements("configuration/appSettings/add").ToArray();

            Assert.AreEqual(5, elems.Length);
        }

        [TestMethod]
        public void AssertAttributeValues_should_not_throw_exception_when_values_match()
        {
            string appSettingsXml = FormattedAppSettingsSection(
                "<add key=\"testKey\" value=\"testValue\" value2=\"testValue2\" value3=\"testValue3\" />");

            var tester = new XmlTester(FormattedXml(appSettingsXml));

            AssertExceptionNotThrown.WhenExecuting(() =>
                tester.AssertAttributeValues(
                    ConfigXPath.AppSettingForKey("testKey"),
                    new Dictionary<string, string>
                    {
                        { "value", "testValue" },
                        { "value2", "testValue2" },
                        { "value3", "testValue3" }
                    }));
        }

        [TestMethod]
        public void AssertAttributeValues_should_throw_exception_when_values_do_not_match()
        {
            string appSettingsXml = FormattedAppSettingsSection(
                "<add key=\"testKey\" value=\"testValue\" value2=\"testValue2X\" value3=\"testValue3\" />");

            var tester = new XmlTester(FormattedXml(appSettingsXml));

            AssertExceptionThrown
                .OfType<ScenarioTestFailureException>()
                .WithMessageContaining("@value2: Expected: <testValue2> actual: <testValue2X>")
                .WhenExecuting(() =>
                    tester.AssertAttributeValues(
                        ConfigXPath.AppSettingForKey("testKey"),
                        new Dictionary<string, string>
                        {
                            { "value", "testValue" },
                            { "value2", "testValue2" },
                            { "value3", "testValue3" }
                        }));
        }

        [TestMethod]
        public void AssertAttributeValueIsWellFormedUrl_should_not_throw_exeption_when_value_is_valid_URL()
        {
            var tester = new XmlTester(FormattedXml(
                "<system.serviceModel><client>"
                + "<endpoint address=\"http://www.somewhere.com/something/service.svc\" name=\"testName\" />"
                + "</client></system.serviceModel>"));

            AssertExceptionNotThrown.WhenExecuting(() =>
                tester.AssertAttributeValueIsWellFormedUrl(
                    ConfigXPath.ClientEndpointForName("testName"), "address"));
        }

        [TestMethod]
        public void AssertAttributeValueIsWellFormedUrl_should_throw_exeption_when_value_is_not_valid_URL()
        {
            var tester = new XmlTester(FormattedXml(
                "<system.serviceModel><client>"
                + "<endpoint address=\"bad URL\" name=\"testName\" />"
                + "</client></system.serviceModel>"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("configuration/system.serviceModel/client/endpoint[@name='testName']@address:"
                    + " Value \"bad URL\" is not a well-formed URI string.")
                .WhenExecuting(() =>
                    tester.AssertAttributeValueIsWellFormedUrl(
                        ConfigXPath.ClientEndpointForName("testName"), "address"));
        }

        [TestMethod]
        public void AssertNoDuplicateElements_should_not_throw_exception_when_there_are_no_duplicates()
        {
            var tester = new XmlTester(FormattedXml(FormattedAppSettingsSection(
                FormattedAppSettingsElem("key1", "value1") 
                + FormattedAppSettingsElem("key2", "value2")
                + FormattedAppSettingsElem("key3", "value3")
                + FormattedAppSettingsElem("key4", "value4")
            )));

            AssertExceptionNotThrown.WhenExecuting(() => 
                tester.AssertNoDuplicateElements(ConfigXPath.AppSettings, "key"));
        }

        [TestMethod]
        public void AssertNoDuplicateElements_should_ignore_specified_keys()
        {
            var tester = new XmlTester(FormattedXml(FormattedAppSettingsSection(
                FormattedAppSettingsElem("TransformApplied", "transform1")
                + FormattedAppSettingsElem("TransformApplied", "transform2")
                + FormattedAppSettingsElem("key1", "value1")
                + FormattedAppSettingsElem("key2", "value2")
                + FormattedAppSettingsElem("key3", "value3")
                + FormattedAppSettingsElem("key4", "value4")
            )));

            AssertExceptionNotThrown.WhenExecuting(() => 
                tester.AssertNoDuplicateElements(ConfigXPath.AppSettings, "key", "TransformApplied"));
        }

        [TestMethod]
        public void AssertNoDuplicateElements_should_not_throw_exception_when_there_are_duplicates()
        {
            var tester = new XmlTester(FormattedXml(FormattedAppSettingsSection(
                FormattedAppSettingsElem("key1", "value1")
                + FormattedAppSettingsElem("key2", "value2")
                + FormattedAppSettingsElem("key2", "value2a")
                + FormattedAppSettingsElem("key3", "value3")
                + FormattedAppSettingsElem("key3", "value3a")
                + FormattedAppSettingsElem("key4", "value4")
            )));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Multiple configuration/appSettings/add@key where key = \"key2\", \"key3\".")
                .WhenExecuting(() => tester.AssertNoDuplicateElements(ConfigXPath.AppSettings, "key"));
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
