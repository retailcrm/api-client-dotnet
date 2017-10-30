using System;
using System.Collections.Generic;

namespace Retailcrm.Versions.V5
{
    public partial class Client
    {
        /// <summary>
        /// Get external store settings
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public new Response StoreSettingGet(string code)
        {
            throw new ArgumentException("This method is unavailable in API V5", code);
        }

        /// <summary>
        /// Edit external store settings
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public new Response StoreSettingsEdit(Dictionary<string, object> configuration)
        {
            throw new ArgumentException("This method is unavailable in API V5");
        }

        /// <summary>
        /// Get products groups
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Response StoreProductsGroups(Dictionary<string, object> filter = null, int page = 1, int limit = 20)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (filter != null && filter.Count > 0)
            {
                parameters.Add("filter", filter);
            }

            if (page > 0)
            {
                parameters.Add("page", page);
            }

            if (limit > 0)
            {
                parameters.Add("limit", limit);
            }

            return Request.MakeRequest("/store/products-groups", Request.MethodGet, parameters);
        }

        /// <summary>
        /// Get products properties
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Response StoreProductsProperties(Dictionary<string, object> filter = null, int page = 1, int limit = 20)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (filter != null && filter.Count > 0)
            {
                parameters.Add("filter", filter);
            }

            if (page > 0)
            {
                parameters.Add("page", page);
            }

            if (limit > 0)
            {
                parameters.Add("limit", limit);
            }

            return Request.MakeRequest("/store/products/properties", Request.MethodGet, parameters);
        }
    }
}
