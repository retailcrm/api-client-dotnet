using System.Collections.Generic;

namespace Retailcrm.Versions.V4
{
    using ParentClass = V3.Client;

    /// <summary>
    /// V4 Client
    /// </summary>
    public partial class Client : ParentClass
    {
        /// <summary>
        /// V4 Client Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="site"></param>
        public Client(string url, string key, string site = "") : base(url, key, site)
        {
            if ("/" != url.Substring(url.Length - 1, 1))
            {
                url += "/";
            }

            url += "api/v4";

            Request = new Request(url, new Dictionary<string, object> { { "apiKey", key } });
            SiteCode = site;
        }
    }
}
