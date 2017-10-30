using System.Collections.Generic;

namespace Retailcrm.Versions.V5
{
    using ParentClass = V4.Client;

    /// <summary>
    /// V5 Client
    /// </summary>
    public partial class Client : ParentClass
    {
        /// <summary>
        /// V5 Client Constructor
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

            url += "api/v5";

            Request = new Request(url, new Dictionary<string, object> { { "apiKey", key } });
            SiteCode = site;
        }
    }
}
