using System.Collections.Generic;

namespace Retailcrm
{
    /// <summary>
    /// Unversioned API Client
    /// </summary>
    public class Connection
    {
        private readonly Request _request;

        /// <summary>
        /// Unversioned API Client Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        public Connection(string url, string key)
        {
            if ("/" != url.Substring(url.Length - 1, 1))
            {
                url += "/";
            }

            url += "api/";

            _request = new Request(url, new Dictionary<string, object> { { "apiKey", key } });
        }

        /// <summary>
        /// Get available API versions
        /// </summary>
        /// <returns></returns>
        public Response Versions()
        {
            return _request.MakeRequest(
                "api-versions",
                Request.MethodGet
            );
        }

        /// <summary>
        /// Get available API methods
        /// </summary>
        /// <returns></returns>
        public Response Credentials()
        {
            return _request.MakeRequest(
                "credentials",
                Request.MethodGet
            );
        }
    }
}
