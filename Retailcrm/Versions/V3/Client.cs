using System;
using System.Collections.Generic;
using System.Linq;

namespace Retailcrm.Versions.V3
{
    /// <summary>
    /// V3 Client
    /// </summary>
    public partial class Client
    {
        /// <summary>
        /// Request
        /// </summary>
        protected Request Request;
        /// <summary>
        /// Site code
        /// </summary>
        protected string SiteCode;

        /// <summary>
        /// V3 Client Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="site"></param>
        public Client(string url, string key, string site = "")
        {
            if ("/" != url.Substring(url.Length - 1, 1))
            {
                url += "/";
            }

            url += "api/v3";

            Request = new Request(url, new Dictionary<string, object> { { "apiKey", key } });
            SiteCode = site;
        }

        /// <summary>
        /// Return current site
        /// </summary>
        /// <returns>string</returns>
        public string GetSite()
        {
            return SiteCode;
        }

        /// <summary>
        /// Return current site
        /// </summary>
        public void SetSite(string site)
        {
            SiteCode = site;
        }

        /// <summary>
        /// Check ID parameter
        /// </summary>
        /// <param name="by"></param>
        protected static void CheckIdParameter(string by)
        {
            string[] allowedForBy = { "externalId", "id" };
            if (allowedForBy.Contains(by) == false)
            {
                throw new ArgumentException($"Value {by} for parameter `by` is not valid. Allowed values are {string.Join(", ", allowedForBy)}");
            }
        }

        /// <summary>
        /// Fill params by site value
        /// </summary>
        /// <param name="site"></param>
        /// <param name="param"></param>
        /// <returns>Dictionary</returns>
        protected Dictionary<string, object> FillSite(string site, Dictionary<string, object> param)
        {
            if (site.Length > 1)
            {
                param.Add("site", site);
            }
            else if (SiteCode.Length > 1)
            {
                param.Add("site", SiteCode);
            }

            return param;
        }
    }
}
