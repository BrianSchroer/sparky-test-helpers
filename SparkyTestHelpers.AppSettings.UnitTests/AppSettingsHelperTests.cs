using System;
using System.Linq;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using SparkyTestHelpers.Exceptions;
using System.Collections.Generic;

namespace SparkyTestHelpers.AppSettings.UnitTests
{
    /// <summary>
    /// <see cref="AppSettingsHelper"/> tests.
    /// </summary>
    [TestClass]
    public class AppSettingsHelperTests
    {
        [TestMethod]
        public void Verify_configuration_is_working()
        {
            Assert.AreEqual("value1", ConfigurationManager.AppSettings["test1"]);
        }

        [TestMethod]
        public void Original_AppSettings_values_should_be_restored_after_successful_Test()
        {
            string expected = GetAppSettingsString();

            AppSettingsHelper
                .WithAppSetting("test1", "changed value 1")
                .AndAppSetting("testx", "added value")
                .Test(() =>
                {
                    Assert.AreEqual("changed value 1", ConfigurationManager.AppSettings["test1"]);
                    Assert.AreEqual("added value", ConfigurationManager.AppSettings["testx"]);
                });

            string actual = GetAppSettingsString();
            Assert.AreEqual(expected, GetAppSettingsString());
        }

        [TestMethod]
        public void Original_AppSettings_values_should_be_restored_after_Test_exception()
        {
            string expected = GetAppSettingsString();

            AssertExceptionThrown.OfType<Exception>().WithMessage("boom").WhenExecuting(() =>
                AppSettingsHelper
                    .WithAppSetting("test1", "changed value 1")
                    .AndAppSetting("testx", "added value")
                    .Test(() =>
                    {
                        throw new Exception("boom");
                    })
            );

            string actual = GetAppSettingsString();
            Assert.AreEqual(expected, GetAppSettingsString());
        }

        [TestMethod]
        public void TestDependencyProvider_should_work()
        {
            Func<string, string> getAppSetting = 
                AppSettings.TestDependencyProvider(new Dictionary<string, string>{ { "testKey", "testValue" } }).GetValue();

            Assert.AreEqual("testValue", getAppSetting("testKey"));
            Assert.IsNull(getAppSetting("undefinedKey"));
        }

        private string GetAppSettingsString()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            return string.Join(" | ",
                ConfigurationManager.AppSettings.AllKeys
                    .Where(key => appSettings[key] != null)
                    .OrderBy(key => key)
                    .Select(key => $"{key}=\"{appSettings[key]}\""));
        }
    }
}
