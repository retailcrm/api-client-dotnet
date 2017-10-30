using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Retailcrm.Versions.V3
{
    public partial class Client
    {
        /// <summary>
        /// Get packs list
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Response PacksList(Dictionary<string, object> filter = null, int page = 1, int limit = 20)
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

            return Request.MakeRequest("/orders/packs", Request.MethodGet, parameters);
        }

        /// <summary>
        /// Create pack
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public Response PacksCreate(Dictionary<string, object> pack)
        {
            if (pack.Count < 1)
            {
                throw new ArgumentException("Parameter `pack` must contains a data");
            }

            return Request.MakeRequest(
                "/orders/packs/create",
                Request.MethodPost,
                new Dictionary<string, object>
                {
                    { "pack", new JavaScriptSerializer().Serialize(pack) }
                }
            );
        }

        /// <summary>
        /// Update pack data
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public Response PacksUpdate(Dictionary<string, object> pack)
        {
            if (pack.Count < 1)
            {
                throw new ArgumentException("Parameter `pack` must contains a data");
            }

            if (!pack.ContainsKey("id"))
            {
                throw new ArgumentException("Parameter `pack` must contains an id");
            }

            return Request.MakeRequest(
                $"/orders/packs/{pack["id"].ToString()}/edit",
                Request.MethodPost,
                new Dictionary<string, object>
                {
                    { "pack", new JavaScriptSerializer().Serialize(pack) }
                }
            );
        }

        /// <summary>
        /// Delete pack
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Response PacksDelete(string id)
        {
            return Request.MakeRequest(
                $"/orders/packs/{id}/delete",
                Request.MethodPost
            );
        }

        /// <summary>
        /// Get pack by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Response PacksGet(string id)
        {
            return Request.MakeRequest(
                $"/orders/packs/{id}",
                Request.MethodGet
            );
        }

        /// <summary>
        /// Get packs history
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Response PacksHistory(Dictionary<string, object> filter = null, int page = 1, int limit = 20)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (filter != null && filter.Count > 0)
            {
                parameters.Add("filter", filter);
            }

            if (page > 1)
            {
                parameters.Add("page", page);
            }

            if (limit > 0)
            {
                parameters.Add("limit", limit);
            }

            return Request.MakeRequest("/orders/packs/history", Request.MethodGet, parameters);
        }
    }
}
