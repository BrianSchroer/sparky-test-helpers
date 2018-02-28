using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Xml.Config;

namespace SparkyTestHelpers.Xml.UnitTests
{
    /// <summary>
    /// <see cref="ConfigXPath"/> tests.
    /// </summary>
    [TestClass]
    public class ConfigXPathTests
    {
        [TestMethod]
        public void AppSettingForKey_should_return_expected_value()
        {
            Assert.AreEqual(
                "configuration/appSettings/add[@key='specifiedKey']",
                ConfigXPath.AppSettingForKey("specifiedKey"));
        }

        [TestMethod]
        public void ClientEndpointForName_should_return_expected_value()
        {
            Assert.AreEqual(
                "configuration/system.serviceModel/client/endpoint[@name='testEndpoint']",
                ConfigXPath.ClientEndpointForName("testEndpoint"));
        }
    }
}
