using Newtonsoft.Json;
using RetailCrm.Http;
using RetailCrm.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCrm
{
    public class ApiClient
    {
        private const string apiVersion = "v3";
        protected Client client;

        /// <summary>
        /// Site code
        /// </summary>
        protected string siteCode;

        /// <summary>
        /// ApiClient creating
        /// </summary>
        /// <param name="url"></param>
        /// <param name="apiKey"></param>
        /// <param name="site"></param>
        public ApiClient(string url, string apiKey, string site = "")
        {
            if ("/" != url.Substring(url.Length - 1, 1))
            {
                url += "/";
            }

            url += "api/" + apiVersion;

            client = new Client(url, new Dictionary<string, object>() { { "apiKey", apiKey } });
            siteCode = site;
        }

        /// <summary>
        /// Create a order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="site"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse ordersCreate(Dictionary<string, object> order, string site = "")
        {
            if (order.Count < 1)
            {
                throw new ArgumentException("Parameter `order` must contains a data");
            }

            return client.makeRequest(
                       "/orders/create",
                       Client.METHOD_POST,
                       this.fillSite(
                           site, 
                           new Dictionary<string, object>() { 
                               { "order", JsonConvert.SerializeObject(order) }
                           }
                       )
                   );
        }

        /// <summary>
        /// Edit a order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="by"></param>
        /// <param name="site"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse ordersEdit(Dictionary<string, object> order, string by = "externalId", string site = "")
        {
            if (order.Count < 1)
            {
                throw new ArgumentException("Parameter `order` must contains a data");
            }

            checkIdParameter(by);

            if (order.ContainsKey(by) == false)
            {
                throw new ArgumentException("Order array must contain the \"" + by + "\" parameter");
            }


            return client.makeRequest(
                       "/orders/" + order[by] + "/edit",
                       Client.METHOD_POST,
                       this.fillSite(
                           site,
                           new Dictionary<string, object>() { 
                               { "order", JsonConvert.SerializeObject(order) },
                               { "by", by }
                           }
                       )
                   );
        }

        /// <summary>
        /// Upload array of the orders
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="site"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse ordersUpload(Dictionary<string, object> orders, string site = "")
        {
            if (orders.Count < 1)
            {
                throw new ArgumentException("Parameter `order` must contains a data");
            }

            return client.makeRequest(
                       "/orders/upload",
                       Client.METHOD_POST,
                       this.fillSite(
                           site,
                           new Dictionary<string, object>() { 
                               { "orders", JsonConvert.SerializeObject(orders) }
                           }
                       )
                   );
        }

        /// <summary>
        /// Get order by id or externalId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="by"></param>
        /// <param name="site"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse ordersGet(string id, string by = "externalId", string site = "")
        {
            checkIdParameter(by);

            return client.makeRequest(
                       "/orders/" + id,
                       Client.METHOD_GET,
                       this.fillSite(
                           site,
                           new Dictionary<string, object>() {
                               { "by", by }
                           }
                       )
                   );
        }

        /// <summary>
        /// Returns a orders history
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="skipMyChanges"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse ordersHistory(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int limit = 100,
            int offset = 0,
            bool skipMyChanges = true
        )
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (startDate != null)
            {
                parameters.Add("startDate", startDate.Value.ToString("Y-m-d H:i:s"));
            }
            if (endDate != null)
            {
                parameters.Add("endDate", endDate.Value.ToString("Y-m-d H:i:s"));
            }
            if (limit > 0)
            {
                parameters.Add("limit", limit);
            }
            if (offset > 0)
            {
                parameters.Add("offset", offset);
            }
            if (skipMyChanges == true)
            {
                parameters.Add("skipMyChanges", skipMyChanges);
            }

            return client.makeRequest("/orders/history", Client.METHOD_GET, parameters);
        }

        /// <summary>
        /// Returns filtered orders list
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse ordersList(Dictionary<string, object> filter = null, int page = 0, int limit = 0)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (filter.Count > 0)
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

            return client.makeRequest("/orders", Client.METHOD_GET, parameters);
        }

        /// <summary>
        /// Returns statuses of the orders
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="externalIds"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse ordersStatuses(Dictionary<string, object> ids = null, Dictionary<string, object> externalIds = null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (ids.Count > 0)
            {
                parameters.Add("ids", ids);
            }
            if (externalIds.Count > 0)
            {
                parameters.Add("externalIds", externalIds);
            }

            return client.makeRequest("/orders/statuses", Client.METHOD_GET, parameters);
        }

        /// <summary>
        /// Save order IDs' (id and externalId) association in the CRM
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse ordersFixExternalId(Dictionary<string, object> ids)
        {
            if (ids.Count < 1)
            {
                throw new ArgumentException("Method parameter must contains at least one IDs pair");
            }

            return client.makeRequest(
                       "/orders/fix-external-ids",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() {
                           { "orders", JsonConvert.SerializeObject(ids) }
                       }
                   );
        }

        /// <summary>
        /// Create a customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="site"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse customersCreate(Dictionary<string, object> customer, string site = "")
        {
            if (customer.Count < 1)
            {
                throw new ArgumentException("Parameter `customer` must contains a data");
            }

            return client.makeRequest(
                       "/customers/create",
                       Client.METHOD_POST,
                       this.fillSite(
                           site,
                           new Dictionary<string, object>() { 
                               { "customer", JsonConvert.SerializeObject(customer) }
                           }
                       )
                   );
        }

        /// <summary>
        /// Edit a customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="by"></param>
        /// <param name="site"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse customersEdit(Dictionary<string, object> customer, string by = "externalId", string site = "")
        {
            if (customer.Count < 1)
            {
                throw new ArgumentException("Parameter `customer` must contains a data");
            }

            checkIdParameter(by);

            if (customer.ContainsKey(by) == false)
            {
                throw new ArgumentException("Customer array must contain the \"" + by + "\" parameter");
            }


            return client.makeRequest(
                       "/customers/" + customer[by] + "/edit",
                       Client.METHOD_POST,
                       this.fillSite(
                           site,
                           new Dictionary<string, object>() { 
                               { "customer", JsonConvert.SerializeObject(customer) },
                               { "by", by }
                           }
                       )
                   );
        }

        /// <summary>
        /// Upload array of the customers
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="site"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse customersUpload(Dictionary<string, object> customers, string site = "")
        {
            if (customers.Count < 1)
            {
                throw new ArgumentException("Parameter `customers` must contains a data");
            }

            return client.makeRequest(
                       "/customers/upload",
                       Client.METHOD_POST,
                       this.fillSite(
                           site,
                           new Dictionary<string, object>() { 
                               { "customers", JsonConvert.SerializeObject(customers) }
                           }
                       )
                   );
        }

        /// <summary>
        /// Get customer by id or externalId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="by"></param>
        /// <param name="site"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse customersGet(string id, string by = "externalId", string site = "")
        {
            checkIdParameter(by);

            return client.makeRequest(
                       "/customers/" + id,
                       Client.METHOD_GET,
                       this.fillSite(
                           site,
                           new Dictionary<string, object>() {
                               { "by", by }
                           }
                       )
                   );
        }

        /// <summary>
        /// Returns filtered customers list
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse customersList(Dictionary<string, object> filter = null, int page = 0, int limit = 0)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (filter.Count > 0)
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

            return client.makeRequest("/customers", Client.METHOD_GET, parameters);
        }

        /// <summary>
        /// Save customer IDs' (id and externalId) association in the CRM
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse customersFixExternalIds(Dictionary<string, object> ids)
        {
            if (ids.Count < 1)
            {
                throw new ArgumentException("Method parameter must contains at least one IDs pair");
            }

            return client.makeRequest(
                       "/customers/fix-external-ids",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() {
                           { "customers", JsonConvert.SerializeObject(ids) }
                       }
                   );
        }

        /// <summary>
        /// Returns deliveryServices list
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse deliveryServicesList()
        {
            return client.makeRequest("/reference/delivery-services", Client.METHOD_GET);
        }

        /// <summary>
        /// Returns deliveryTypes list
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse deliveryTypesList()
        {
            return client.makeRequest("/reference/delivery-types", Client.METHOD_GET);
        }

        /// <summary>
        /// Returns orderMethods list
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse orderMethodsList()
        {
            return client.makeRequest("/reference/order-methods", Client.METHOD_GET);
        }

        /// <summary>
        /// Returns orderTypes list
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse orderTypesList()
        {
            return client.makeRequest("/reference/order-types", Client.METHOD_GET);
        }

        /// <summary>
        /// Returns paymentStatuses list
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse paymentStatusesList()
        {
            return client.makeRequest("/reference/payment-statuses", Client.METHOD_GET);
        }

        /// <summary>
        /// Returns paymentTypes list
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse paymentTypesList()
        {
            return client.makeRequest("/reference/payment-types", Client.METHOD_GET);
        }

        /// <summary>
        /// Returns productStatuses list
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse productStatusesList()
        {
            return client.makeRequest("/reference/product-statuses", Client.METHOD_GET);
        }

        /// <summary>
        /// Returns statusGroups list
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse statusGroupsList()
        {
            return client.makeRequest("/reference/status-groups", Client.METHOD_GET);
        }

        /// <summary>
        /// Returns statuses list
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse statusesList()
        {
            return client.makeRequest("/reference/statuses", Client.METHOD_GET);
        }

        /// <summary>
        /// Returns sites list
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse sitesList()
        {
            return client.makeRequest("/reference/sites", Client.METHOD_GET);
        }

        /// <summary>
        /// Edit deliveryService
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse deliveryServicesEdit(Dictionary<string, object> data)
        {
            if (data.ContainsKey("code") == false)
            {
                throw new ArgumentException("Data must contain \"code\" parameter");
            }

            return client.makeRequest(
                       "/reference/delivery-services/" + data["code"] + "/edit",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() { 
                           { "deliveryService", JsonConvert.SerializeObject(data) }
                       }
                   );
        }

        /// <summary>
        /// Edit deliveryType
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse deliveryTypesEdit(Dictionary<string, object> data)
        {
            if (data.ContainsKey("code") == false)
            {
                throw new ArgumentException("Data must contain \"code\" parameter");
            }

            return client.makeRequest(
                       "/reference/delivery-types/" + data["code"] + "/edit",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() { 
                           { "deliveryType", JsonConvert.SerializeObject(data) }
                       }
                   );
        }

        /// <summary>
        /// Edit orderMethod
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse orderMethodsEdit(Dictionary<string, object> data)
        {
            if (data.ContainsKey("code") == false)
            {
                throw new ArgumentException("Data must contain \"code\" parameter");
            }

            return client.makeRequest(
                       "/reference/order-methods/" + data["code"] + "/edit",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() { 
                           { "orderMethod", JsonConvert.SerializeObject(data) }
                       }
                   );
        }

        /// <summary>
        /// Edit orderType
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse orderTypesEdit(Dictionary<string, object> data)
        {
            if (data.ContainsKey("code") == false)
            {
                throw new ArgumentException("Data must contain \"code\" parameter");
            }

            return client.makeRequest(
                       "/reference/order-types/" + data["code"] + "/edit",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() { 
                           { "orderType", JsonConvert.SerializeObject(data) }
                       }
                   );
        }

        /// <summary>
        /// Edit paymentStatus
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse paymentStatusesEdit(Dictionary<string, object> data)
        {
            if (data.ContainsKey("code") == false)
            {
                throw new ArgumentException("Data must contain \"code\" parameter");
            }

            return client.makeRequest(
                       "/reference/payment-statuses/" + data["code"] + "/edit",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() { 
                           { "paymentStatus", JsonConvert.SerializeObject(data) }
                       }
                   );
        }

        /// <summary>
        /// Edit paymentType
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse paymentTypesEdit(Dictionary<string, object> data)
        {
            if (data.ContainsKey("code") == false)
            {
                throw new ArgumentException("Data must contain \"code\" parameter");
            }

            return client.makeRequest(
                       "/reference/payment-types/" + data["code"] + "/edit",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() { 
                           { "paymentType", JsonConvert.SerializeObject(data) }
                       }
                   );
        }

        /// <summary>
        /// Edit productStatus
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse productStatusesEdit(Dictionary<string, object> data)
        {
            if (data.ContainsKey("code") == false)
            {
                throw new ArgumentException("Data must contain \"code\" parameter");
            }

            return client.makeRequest(
                       "/reference/product-statuses/" + data["code"] + "/edit",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() { 
                           { "productStatus", JsonConvert.SerializeObject(data) }
                       }
                   );
        }

        /// <summary>
        /// Edit order status
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse statusesEdit(Dictionary<string, object> data)
        {
            if (data.ContainsKey("code") == false)
            {
                throw new ArgumentException("Data must contain \"code\" parameter");
            }

            return client.makeRequest(
                       "/reference/statuses/" + data["code"] + "/edit",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() { 
                           { "status", JsonConvert.SerializeObject(data) }
                       }
                   );
        }

        /// <summary>
        /// Edit site
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ApiResponse</returns>
        public ApiResponse sitesEdit(Dictionary<string, object> data)
        {
            if (data.ContainsKey("code") == false)
            {
                throw new ArgumentException("Data must contain \"code\" parameter");
            }

            return client.makeRequest(
                       "/reference/sites/" + data["code"] + "/edit",
                       Client.METHOD_POST,
                       new Dictionary<string, object>() { 
                           { "site", JsonConvert.SerializeObject(data) }
                       }
                   );
        }

        /// <summary>
        /// Update CRM basic statistic
        /// </summary>
        /// <returns>ApiResponse</returns>
        public ApiResponse statisticUpdate()
        {
            return client.makeRequest("/statistic/update", Client.METHOD_GET);
        }

        /// <summary>
        /// Return current site
        /// </summary>
        /// <returns>string</returns>
        public string getSite()
        {
            return this.siteCode;
        }

        /// <summary>
        /// Return current site
        /// </summary>
        public void setSite(string site)
        {
            this.siteCode = site;
        }

        /// <summary>
        /// Check ID parameter
        /// </summary>
        /// <param name="by"></param>
        protected void checkIdParameter(string by)
        {
            string[] allowedForBy = new string[]{"externalId", "id"};
            if (allowedForBy.Contains(by) == false)
            {
                throw new ArgumentException("Value \"" + by + "\" for parameter \"by\" is not valid. Allowed values are " + String.Join(", ", allowedForBy));
            }
        }

        /// <summary>
        /// Fill params by site value
        /// </summary>
        /// <param name="site"></param>
        /// <param name="param"></param>
        /// <returns>Dictionary</returns>
        protected Dictionary<string, object> fillSite(string site, Dictionary<string, object> param)
        {
            if (site.Length > 1)
            {
                param.Add("site", site);
            } 
            else if (siteCode.Length > 1)
            {
                param.Add("site", siteCode);
            }

            return param;
        }
    }
}
