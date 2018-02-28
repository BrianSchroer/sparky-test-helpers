namespace SparkyTestHelpers.XmlConfig
{
    /// <summary>
    /// Config XML XPath string constants.
    /// </summary>
    public static class ConfigXPath
    {
        /// <summary>
        /// XPath string for appSettings elements.
        /// </summary>
        public static string AppSettings => "configuration/appSettings/add";

        /// <summary>
        /// Build XPath string for AppSettings key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>XPath string.</returns>
        public static string AppSettingForKey(string key) => $"configuration/appSettings/add[@key='{key}']";

        /// <summary>
        /// Build XPath string for service client endpoint.
        /// </summary>
        /// <param name="endpointName">The endpoint name.</param>
        /// <returns>XPath string.</returns>
        public static string ClientEndpointForName(string endpointName) => 
            $"configuration/system.serviceModel/client/endpoint[@name='{endpointName}']";

        /// <summary>
        /// Build XPath string for connection string name key.
        /// </summary>
        /// <param name="name">The name key.</param>
        /// <returns>XPath string.</returns>
        public static string ConnectionStringForName(string name) =>
            $"configuration/connectionStrings/add[@name='{name}']";

        /// <summary>
        /// XPath string for connection string elements.
        /// </summary>
        public static string ConnectionStrings => "configuration/connectionStrings/add";

        /// <summary>
        /// XPath string for service client endpoint elements.
        /// </summary>
        public static string ClientEndpoints => "configuration/system.serviceModel/client/endpoint";

        /// <summary>
        /// XPath string for system.web compilation element.
        /// </summary>
        public static string SystemWebCompilation => "configuration/system.web/compilation";
    }
}
