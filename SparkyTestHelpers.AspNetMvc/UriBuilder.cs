using System;
using System.Collections.Specialized;
using System.Linq;

namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// Helper methods for building <see cref="Uri"/>s.
    /// </summary>
    public static class UriBuilder
    {
        /// <summary>
        /// Builds <see cref="Uri" />.
        /// </summary>
        /// <param name="siteUrlPrefix">site URL prefix, e.g. "http://mySite.com", "http://localhost".</param>
        /// <param name="queryStringParameters">Query string parameter names & values.</param>
        /// <returns><see cref="Uri"/>.</returns>
        public static Uri Build(string siteUrlPrefix, NameValueCollection queryStringParameters)
        {
            string uriString = siteUrlPrefix;

            if (queryStringParameters?.Count > 0)
            {
                string queryString = string.Join("&",
                    queryStringParameters.Keys.Cast<string>().Select(
                        key => $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(queryStringParameters[key])}"
                ));

                uriString = $"{uriString}?{queryString}";
            }

            return new Uri(uriString);
        }

        /// <summary>
        /// Builds <see cref="Uri" /> with site URL prefix "http://localhost".
        /// </summary>
        /// <param name="queryStringParameters">Query string parameter names & values.</param>
        /// <returns><see cref="Uri"/>.</returns>
        public static Uri Build(NameValueCollection queryStringParameters) => Build("http://localhost", queryStringParameters);

        /// <summary>
        /// Builds <see cref="Uri" />.
        /// </summary>
        /// <param name="siteUrlPrefix">site URL prefix, e.g. "http://mySite.com", "http://localhost".</param>
        /// <param name="queryStringParameters">Query string parameter names & values.</param>
        /// <returns><see cref="Uri"/>.</returns>
        public static Uri Build(string siteUrlPrefix, params QueryStringParameter[] queryStringParameters)
        {
            string uriString = siteUrlPrefix;

            if (queryStringParameters.Any())
            {
                string queryString = string.Join("&", queryStringParameters.Select(x => $"{Uri.EscapeDataString(x.Name)}={Uri.EscapeDataString(x.Value)}"));
                uriString = $"{uriString}?{queryString}";
            }

            return new Uri(uriString);
        }

        /// <summary>
        /// Builds <see cref="Uri" /> with site URL prefix "http://localhost".
        /// </summary>
        /// <param name="queryStringParameters">Query string parameter names & values.</param>
        /// <returns><see cref="Uri"/>.</returns>
        public static Uri Build(params QueryStringParameter[] queryStringParameters) => Build("http://localhost", queryStringParameters);
    }
} 