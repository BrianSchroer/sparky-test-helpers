using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace SparkyTestHelpers.XmlConfig
{
    /// <summary>
    /// This helper is used to test .config AppSettings values and restore the original
    /// values when done.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    ///     AppSettingsHelper
    ///         .WithAppSetting("testKey", "test value")
    ///         .AndAppSetting("testKey2", "test value 2")
    ///         .Test(() =>
    ///         {
    ///             // test code that uses ConfigurationManager.AppSettings "testKey" & "testKey2" values
    ///         });
    /// ]]></code>
    /// </example>
    public sealed class AppSettingsHelper
    {
        private static readonly object _lockObject = new object();

        private Dictionary<string, string> _overrides = new Dictionary<string, string>();

        /// <summary>
        /// Creates new <see cref="AppSettingsHelper"/> instance.
        /// </summary>
        /// <remarks>
        /// Private constructor. Class can only be created via <see cref="AppSettingsHelper.WithAppSetting(string, object)"/>.
        /// </remarks>
        private AppSettingsHelper()
        {
        }

        /// <summary>
        /// Specify AppSettings override.
        /// </summary>
        /// <param name="key">The key to be overridden.</param>
        /// <param name="value">The override value.</param>
        /// <returns>"This" <see cref="AppSettingsHelper"/>.</returns>
        public AppSettingsHelper AndAppSetting(string key, object value)
        {
            _overrides.Add(key, value.ToString());
            return this;
        }

        /// <summary>
        /// Test using an <see cref="AppSettingsHelper"/>.
        /// </summary>
        /// <param name="action">"Callback" test action.</param>
        public void Test(Action action)
        {
            lock (_lockObject)
            {
                Dictionary<string, string> savedAppSettings = BackUpAppSettings();

                try
                {
                    ApplyOverrides(_overrides);
                    action();
                }
                finally
                {
                    RestoreAppSettings(savedAppSettings);
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="AppSettingsHelper"/> instance with an overridden AppSettings value.
        /// </summary>
        /// <param name="key">The AppSettings key to override.</param>
        /// <param name="value">The override value.</param>
        /// <returns>New <see cref="AppSettingsHelper"/> instance.</returns>
        public static AppSettingsHelper WithAppSetting(string key, object value)
        {
            return new AppSettingsHelper().AndAppSetting(key, value);
        }

        private static void ApplyOverrides(Dictionary<string, string> overrides)
        {
            foreach (string key in overrides.Keys)
            {
                ConfigurationManager.AppSettings[key] = overrides[key];
            }
        }
        
        private static Dictionary<string, string> BackUpAppSettings()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            var savedAppSettings = new Dictionary<string, string>();

            foreach (string key in appSettings.AllKeys)
            {
                savedAppSettings.Add(key, appSettings[key]);
            }

            return savedAppSettings;
        }

        private static void RestoreAppSettings(Dictionary<string, string> savedAppSettings)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            foreach (string key in appSettings.AllKeys)
            {
                appSettings[key] = (savedAppSettings.ContainsKey(key)) ? savedAppSettings[key] : null;
            }
        }
    }
}