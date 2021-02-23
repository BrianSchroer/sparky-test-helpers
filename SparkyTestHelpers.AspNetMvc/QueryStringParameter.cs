using System.Collections.Generic;
using System.Collections.Specialized;

namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// <see cref="QueryStringParameter" /> extension methods.
    /// </summary>
    public static class QueryStringParameterExensionMethods
    {
        /// <summary>
        /// Creates <see cref="NameValueCollection" /> from <see cref="QueryStringParameter"/>s.
        /// </summary>
        /// <param name="queryStringParameters"><see cref="QueryStringParameter"/>s.</param>
        /// <returns><see cref="NameValueCollection"/>.</returns>
        public static NameValueCollection ToNameValueCollection(this IEnumerable<QueryStringParameter> queryStringParameters)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (QueryStringParameter item in queryStringParameters)
            {
                nameValueCollection.Add(item.Name, item.Value);
            }

            return nameValueCollection;
        }
    }
} 