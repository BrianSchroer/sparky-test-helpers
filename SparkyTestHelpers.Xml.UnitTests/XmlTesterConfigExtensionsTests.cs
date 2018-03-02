using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using SparkyTestHelpers.Xml.Config;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SparkyTestHelpers.Xml.UnitTests
{
    /// <summary>
    /// <see cref="XmlTesterConfigExtensions"/> tests.
    /// </summary>
    [TestClass]
    public class XmlTesterConfigExtensionsTests
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
        public void AssertClientEndpointAddressIsWellFormedUrl_should_not_throw_exception_for_valid_URL()
        {
            var tester = new XmlTester(FormattedXml(
                "<system.serviceModel><client>"
                + "<endpoint address=\"http://www.somewhere.com/something/service.svc\" name=\"testName\" />"
                + "</client></system.serviceModel>"));

            AssertExceptionNotThrown.WhenExecuting(() =>
                tester.AssertClientEndpointAddressIsWellFormedUrl("testName"));
        }

        [TestMethod]
        public void AssertClientEndpointAddressIsWellFormedUrl_should_throw_exception_for_invalid_URL()
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
                    tester.AssertClientEndpointAddressIsWellFormedUrl("testName"));
        }

        [TestMethod]
        public void AssertAppSettingsKeyDoesNotExist_should_not_throw_exception_when_key_does_not_exist()
        {
            var tester = new XmlTester(FormattedXml());
            AssertExceptionNotThrown.WhenExecuting(() => tester.AssertAppSettingsKeyDoesNotExist("testkey"));
        }

        [TestMethod]
        public void AssertAppSettingsKeyDoesNotExist_should_throw_exception_when_key_exists()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "testValue"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element should not exist: configuration/appSettings/add[@key='testKey']")
                .WhenExecuting(() => tester.AssertAppSettingsKeyDoesNotExist("testKey"));
        }

        [TestMethod]
        public void AssertAppSettingsValue_should_throw_exception_when_key_does_not_exist()
        {
            var tester = new XmlTester(FormattedXml());

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element not found: configuration/appSettings/add[@key='testKey']")
                .WhenExecuting(() => tester.AssertAppSettingsValue("testKey", "testValue"));
        }

        [TestMethod]
        public void AssertAppSettingsValue_should_throw_exception_when_value_does_not_match()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("configuration/appSettings/add[@key='testKey']@value:"
                    + " Expected: <expectedValue> actual: <actualValue>")
                .WhenExecuting(() => tester.AssertAppSettingsValue("testKey", "expectedValue"));
        }

        [TestMethod]
        public void AssertAppSettingsValueMatch_should_throw_exception_when_key_does_not_exist()
        {
            var tester = new XmlTester(FormattedXml());

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element not found: configuration/appSettings/add[@key='testKey']")
                .WhenExecuting(() => tester.AssertAppSettingsValueMatch("testKey", "testValue"));
        }

        [TestMethod]
        public void AssertAppSettingsValueMatch_should_throw_exception_when_value_does_not_match()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("configuration/appSettings/add[@key='testKey']@value:"
                    + " Value \"actualValue\" doesn't match regex pattern \"testPattern\".")
                .WhenExecuting(() => tester.AssertAppSettingsValueMatch("testKey", "testPattern"));
        }

        [TestMethod]
        public void AssertAppSettingsValueMatch_should_not_throw_exception_when_value_matches()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "actualValue"));

            AssertExceptionNotThrown
                .WhenExecuting(() => tester.AssertAppSettingsValueMatch("testKey", ".*Value"));
        }

        [TestMethod]
        public void AssertAppSettingsValues_should_not_throw_exception_when_values_match()
        {
            KeyValuePair<string, string>[] appSettings =
                Enumerable.Range(1, 5).Select(i => new KeyValuePair<string, string>($"key{i}", $"value{i}")).ToArray();

            string appSettingsXml = string.Join("\n", appSettings.Select(kv => FormattedAppSettingsElem(kv.Key, kv.Value)));

            var tester = new XmlTester(FormattedXml(FormattedAppSettingsSection(appSettingsXml)));

            AssertExceptionNotThrown.WhenExecuting(() => tester.AssertAppSettingsValues(appSettings));
        }

        [TestMethod]
        public void AssertAppSettingsValues_should_throw_exception_when_values_do_not_match()
        {
            KeyValuePair<string, string>[] appSettings =
                Enumerable.Range(1, 5).Select(i => new KeyValuePair<string, string>($"key{i}", $"value{i}")).ToArray();

            string appSettingsXml = string.Join("\n", appSettings.Select(kv => FormattedAppSettingsElem(kv.Key, kv.Value)));

            appSettingsXml = appSettingsXml.Replace("value3", "value3X");

            var tester = new XmlTester(FormattedXml(FormattedAppSettingsSection(appSettingsXml)));

            AssertExceptionThrown
                .OfType<ScenarioTestFailureException>()
                .WithMessageContaining("Expected: <value3> actual: <value3X>")
                .WhenExecuting(() => tester.AssertAppSettingsValues(appSettings));
        }

        [TestMethod]
        public void AssertCompilationDebugFalse_should_not_throw_exception_when_element_does_not_exist()
        {
            var tester = new XmlTester(FormattedXml("<system.web></system.web>"));
            AssertExceptionNotThrown.WhenExecuting(() => tester.AssertCompilationDebugFalse());
        }

        [TestMethod]
        public void AssertCompilationDebugFalse_should_not_throw_exception_when_attribute_has_been_removed()
        {
            var tester = new XmlTester(FormattedXml("<system.web><compilation defaultLanguage=\"c#\" /></system.web>"));
            AssertExceptionNotThrown.WhenExecuting(() => tester.AssertCompilationDebugFalse());
        }

        [TestMethod]
        public void AssertCompilationDebugFalse_should_not_throw_exception_when_attribute_is_false()
        {
            var tester = new XmlTester(FormattedXml("<system.web><compilation defaultLanguage=\"c#\" debug=\"false\" /></system.web>"));

            AssertExceptionNotThrown.WhenExecuting(() => tester.AssertCompilationDebugFalse());
        }

        [TestMethod]
        public void AssertCompilationDebugFalse_should_throw_exception_when_attribute_is_true()
        {
            var tester = new XmlTester(FormattedXml("<system.web><compilation defaultLanguage=\"c#\" debug=\"true\" /></system.web>"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("configuration/system.web/compilation@debug: Expected: <false> actual: <true>")
                .WhenExecuting(() => tester.AssertCompilationDebugFalse());
        }

        [TestMethod]
        public void AssertClientEndpointAddressesAreWellFormedUrls_should_not_throw_exeption_when_URLs_are_valid()
        {
            var tester = new XmlTester(FormattedXml(
                "<system.serviceModel><client>"
                + "<endpoint address=\"http://www.somewhere.com/something/service1.svc\" name=\"name1\" />"
                + "<endpoint address=\"http://www.somewhere.com/something/service2.svc\" name=\"name2\" />"
                + "<endpoint address=\"http://www.somewhere.com/something/service3.svc\" name=\"name3\" />"
                + "</client></system.serviceModel>"));

            AssertExceptionNotThrown.WhenExecuting(() => tester.AssertClientEndpointAddressesAreWellFormedUrls());
        }

        [TestMethod]
        public void AssertClientEndpointAddressesAreWellFormedUrls_should_throw_exeption_when_URL_is_invalid()
        {
            var tester = new XmlTester(FormattedXml(
                "<system.serviceModel><client>"
                + "<endpoint address=\"http://www.somewhere.com/something/service1.svc\" name=\"name1\" />"
                + "<endpoint address=\"bad url 2\" name=\"name2\" />"
                + "<endpoint address=\"bad url 3\" name=\"name3\" />"
                + "</client></system.serviceModel>"));

            AssertExceptionThrown
                .OfType<ScenarioTestFailureException>()
                .WithMessageContaining("configuration/system.serviceModel/client/endpoint[@name='name2']@address:"
                    + " Value \"bad url 2\" is not a well-formed URI string.")
                .WhenExecuting(() => tester.AssertClientEndpointAddressesAreWellFormedUrls());
        }

        [TestMethod]
        public void GetAppSettingsElements_should_return_expected_elements()
        {
            var tester = new XmlTester(FormattedXml(
                "<appSettings>"
                    + "<add key=\"Key1\" value=\"Value1\" />"
                    + "<add key=\"Key2\" value=\"Value2\" />"
                    + "<add key=\"Key3\" value=\"Value3\" />"
                + "</appSettings>"));

            XElement[] elems = tester.GetAppSettingsElements().ToArray();

            Assert.AreEqual(3, elems.Length);
            Assert.AreEqual("Key2", tester.GetAttributeValue(elems[1], "key"));
            Assert.AreEqual("Value2", tester.GetAttributeValue(elems[1], "value"));
        }

        [TestMethod]
        public void GetAppSettingsValue_should_return_value_when_key_is_found()
        {
            var tester = new XmlTester(FormattedXmlWithAppSetting("testKey", "testValue"));

            Assert.AreEqual("testValue", tester.GetAppSettingsValue("testKey"));
        }

        [TestMethod]
        public void GetAppSettingsValue_should_throw_exception_when_key_is_not_found()
        {
            var tester = new XmlTester(FormattedXml());

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element not found: configuration/appSettings/add[@key='testKey']")
                .WhenExecuting(() =>
                    tester.GetAppSettingsValue("testKey"));
        }

        [TestMethod]
        public void GetConnectionStringElements_should_return_expected_elements()
        {
            var tester = new XmlTester(FormattedXml(
                "<connectionStrings>"
                    + "<add name=\"Name1\" connectionString=\"String1\" />"
                    + "<add name=\"Name2\" connectionString=\"String2\" />"
                    + "<add name=\"Name3\" connectionString=\"String3\" />"
                + "</connectionStrings>"));

            XElement[] elems = tester.GetConnectionStringElements().ToArray();

            Assert.AreEqual(3, elems.Length);
            Assert.AreEqual("Name2", tester.GetAttributeValue(elems[1], "name"));
            Assert.AreEqual("String2", tester.GetAttributeValue(elems[1], "connectionString"));
        }

        [TestMethod]
        public void GetConnectionString_should_return_expected_value()
        {
            var tester = new XmlTester(FormattedXml(
                "<connectionStrings>"
                    + "<add name=\"Name1\" connectionString=\"String1\" />"
                    + "<add name=\"Name2\" connectionString=\"String2\" />"
                    + "<add name=\"Name3\" connectionString=\"String3\" />"
                + "</connectionStrings>"));

            Assert.AreEqual("String3", tester.GetConnectionString("Name3"));
        }

        [TestMethod]
        public void GetClientEndpointAddress_should_throw_exception_when_endpoint_not_found()
        {
            var tester = new XmlTester(FormattedXml(
                "<system.serviceModel><client>"
                + "<endpoint address=\"testAddress\" name=\"testName\" />"
                + "</client></system.serviceModel>"));

            AssertExceptionThrown
                .OfType<XmlTesterException>()
                .WithMessage("Element not found: configuration/system.serviceModel/client/endpoint[@name='badName']")
                .WhenExecuting(() => tester.GetClientEndpointAddress("badName"));
        }

        [TestMethod]
        public void ConfigXPath_authentication_constants_should_be_correct()
        {
            var tester = new XmlTester(FormattedXml(
                "<system.webServer><security><authentication>"
                + "<anonymousAuthentication enabled=\"true\" />"
                + "<windowsAuthentication enabled=\"false\" />"
                + "</authentication></security></system.webServer>"));

            tester.AssertAttributeValue(ConfigXPath.AnonymousAuthentication, "enabled", "true");
            tester.AssertAttributeValue(ConfigXPath.WindowsAuthentication, "enabled", "false");
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
