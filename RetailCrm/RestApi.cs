using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;

namespace RetailCrm
{
    public class RestApi
    {
        protected string apiUrl;
        protected string apiKey;
        protected string apiVersion = "3";
        protected DateTime generatedAt;
        protected Dictionary<string, string> parameters;

        private string userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private string contentType = "application/x-www-form-urlencoded";

        /// <param name="crmUrl">адрес CRM</param>
        /// <param name="crmKey">ключ для работы с api</param>
        public RestApi(string crmUrl, string crmKey)
        {
            apiUrl = crmUrl + "/api/v" + apiVersion + "/";
            apiKey = crmKey;
            parameters = new Dictionary<string, string>();
            parameters.Add("apiKey", apiKey);
        }

        /// <summary>
        /// Получение заказа по id
        /// </summary>
        /// <param name="id">идентификатор заказа</param>
        /// <param name="by">поиск заказа по id или externalId</param>
        /// <returns>информация о заказе</returns>
        public Dictionary<string, object> orderGet(int id, string by)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "orders/" + id.ToString();
            if (by.Equals("externalId"))
            {
                parameters.Add("by", by);
            }
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Создание заказа
        /// </summary>
        /// <param name="order">информация о заказе</param>
        /// <returns></returns>
        public Dictionary<string, object> orderCreate(Dictionary<string, object> order)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "orders/create";
            string dataJson = JsonConvert.SerializeObject(order);
            parameters.Add("order", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Изменение заказа
        /// </summary>
        /// <param name="order">информация о заказе</param>
        /// <returns></returns>
        public Dictionary<string, object> orderEdit(Dictionary<string, object> order)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "orders/" + order["externalId"].ToString() + "/edit";
            string dataJson = JsonConvert.SerializeObject(order);
            parameters.Add("order", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Пакетная загрузка заказов
        /// </summary>
        /// <param name="orders">массив заказов</param>
        /// <returns></returns>
        public Dictionary<string, object> orderUpload(Dictionary<string, object> orders)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "orders/upload";
            string dataJson = JsonConvert.SerializeObject(orders);
            parameters.Add("orders", dataJson);
            result = request(url, "POST");

            if (result.ContainsKey("uploadedOrders") && result != null)
            {
                return getDictionary(result["uploadedOrders"]);
            }

            return result;
        }

        /// <summary>
        /// Обновление externalId у заказов с переданными id
        /// </summary>
        /// <param name="orders">массив, содержащий id и externalId заказа</param>
        /// <returns></returns>
        public Dictionary<string, object> orderFixExternalIds(Dictionary<string, object> orders)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "orders/fix-external-ids";
            string dataJson = JsonConvert.SerializeObject(orders);
            parameters.Add("orders", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Получение последних измененных заказов
        /// </summary>
        /// <param name="startDate">начальная дата выборки</param>
        /// <param name="endDate">конечная дата</param>
        /// <param name="limit">ограничение на размер выборки</param>
        /// <param name="offset">сдвиг</param>
        /// <returns>массив заказов</returns>
        public Dictionary<string, object> orderHistory(DateTime startDate, DateTime endDate, int limit, int offset)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "orders/history";
            parameters.Add("startDate", startDate.ToString());
            parameters.Add("startDate", endDate.ToString());
            parameters.Add("limit", limit.ToString());
            parameters.Add("offset", offset.ToString());
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Получение клиента по id
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="by">поиск заказа по id или externalId</param>
        /// <returns>информация о клиенте</returns>
        public Dictionary<string, object> customerGet(string id, string by)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "customers/" + id;
            if (by.Equals("externalId"))
            {
                parameters.Add("by", by);
            }
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Получение списка клиентов в соответсвии с запросом
        /// </summary>
        /// <param name="phone">телефон</param>
        /// <param name="email">почтовый адрес</param>
        /// <param name="fio">фио пользователя</param>
        /// <param name="limit">ограничение на размер выборки</param>
        /// <param name="offset">сдвиг</param>
        /// <returns>массив клиентов</returns>
        public Dictionary<string, object> customers(string phone, string email, string fio, int limit, int offset)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "customers";
            parameters.Add("phone", phone);
            parameters.Add("email", email);
            parameters.Add("fio", fio);
            parameters.Add("limit", limit.ToString());
            parameters.Add("offset", offset.ToString());
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Создание клиента
        /// </summary>
        /// <param name="customer">информация о клиенте</param>
        /// <returns></returns>
        public Dictionary<string, object> customerCreate(Dictionary<string, object> customer)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "customers/create";
            string dataJson = JsonConvert.SerializeObject(customer);
            parameters.Add("customer", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Редактирование клиента
        /// </summary>
        /// <param name="customer">информация о клиенте</param>
        /// <returns></returns>
        public Dictionary<string, object> customerEdit(Dictionary<string, object> customer)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "customers/" + customer["externalId"].ToString() + "/edit";
            string dataJson = JsonConvert.SerializeObject(customer);
            parameters.Add("customer", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Пакетная загрузка клиентов
        /// </summary>
        /// <param name="customers">массив клиентов</param>
        /// <returns></returns>
        public Dictionary<string, object> customerUpload(Dictionary<string, object> customers)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "customers/upload";
            string dataJson = JsonConvert.SerializeObject(customers);
            parameters.Add("customers", dataJson);
            result = request(url, "POST");

            if (result.ContainsKey("uploaded") && result != null)
            {
                return getDictionary(result["uploaded"]);
            }

            return result;
        }

        /// <summary>
        /// Обновление externalId у клиентов с переданными id
        /// </summary>
        /// <param name="customers">массив, содержащий id и externalId заказа</param>
        /// <returns></returns>
        public Dictionary<string, object> customerFixExternalIds(Dictionary<string, object> customers)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "customers/fix-external-ids";
            string dataJson = JsonConvert.SerializeObject(customers);
            parameters.Add("customers", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Получение списка заказов клиента
        /// </summary>
        /// <param name="id">идентификатор клиента</param>
        /// <param name="startDate">начальная дата выборки</param>
        /// <param name="endDate">конечная дата</param>
        /// <param name="limit">ограничение на размер выборки</param>
        /// <param name="offset">сдвиг</param>
        /// <param name="by">поиск заказа по id или externalId</param>
        /// <returns>массив заказов</returns>
        public Dictionary<string, object> customerOrdersList(string id, DateTime startDate, DateTime endDate, int limit, int offset, string by)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "customers/" + id + "/orders";
            if (by.Equals("externalId"))
            {
                parameters.Add("by", by);
            }
            parameters.Add("startDate", startDate.ToString());
            parameters.Add("endDate", endDate.ToString());
            parameters.Add("limit", limit.ToString());
            parameters.Add("offset", offset.ToString());
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Получение списка типов доставки
        /// </summary>
        /// <returns>массив типов доставки</returns>
        public Dictionary<string, object> deliveryTypesList()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/delivery-types";
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Редактирование типа доставки
        /// </summary>
        /// <param name="deliveryType">информация о типе доставки</param>
        /// <returns></returns>
        public Dictionary<string, object> deliveryTypeEdit(Dictionary<string, object> deliveryType)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/delivery-types/" + deliveryType["code"].ToString() + "/edit";
            string dataJson = JsonConvert.SerializeObject(deliveryType);
            parameters.Add("deliveryType", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Получение списка служб доставки
        /// </summary>
        /// <returns>массив служб доставки</returns>
        public Dictionary<string, object> deliveryServicesList()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/delivery-services";
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Редактирование службы доставки
        /// </summary>
        /// <param name="deliveryService">информация о службе доставки</param>
        /// <returns></returns>
        public Dictionary<string, object> deliveryServiceEdit(Dictionary<string, object> deliveryService)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/delivery-services/" + deliveryService["code"].ToString() + "/edit";
            string dataJson = JsonConvert.SerializeObject(deliveryService);
            parameters.Add("deliveryService", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Получение списка типов оплаты
        /// </summary>
        /// <returns>массив типов оплаты</returns>
        public Dictionary<string, object> paymentTypesList()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/payment-types";
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Редактирование типа оплаты
        /// </summary>
        /// <param name="paymentType">информация о типе оплаты</param>
        /// <returns></returns>
        public Dictionary<string, object> paymentTypesEdit(Dictionary<string, object> paymentType)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/payment-types/" + paymentType["code"].ToString() + "/edit";
            string dataJson = JsonConvert.SerializeObject(paymentType);
            parameters.Add("paymentType", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Получение списка статусов оплаты
        /// </summary>
        /// <returns>массив статусов оплаты</returns>
        public Dictionary<string, object> paymentStatusesList()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/payment-statuses";
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Редактирование статуса оплаты
        /// </summary>
        /// <param name="paymentStatus">информация о статусе оплаты</param>
        /// <returns></returns>
        public Dictionary<string, object> paymentStatusesEdit(Dictionary<string, object> paymentStatus)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/payment-statuses/" + paymentStatus["code"].ToString() + "/edit";
            string dataJson = JsonConvert.SerializeObject(paymentStatus);
            parameters.Add("paymentStatus", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Получение списка типов заказа
        /// </summary>
        /// <returns>массив типов заказа</returns>
        public Dictionary<string, object> orderTypesList()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/order-types";
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Редактирование типа заказа
        /// </summary>
        /// <param name="orderType">информация о типе заказа</param>
        /// <returns></returns>
        public Dictionary<string, object> orderTypesEdit(Dictionary<string, object> orderType)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/order-types/" + orderType["code"].ToString() + "/edit";
            string dataJson = JsonConvert.SerializeObject(orderType);
            parameters.Add("orderType", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Получение списка способов оформления заказа
        /// </summary>
        /// <returns>массив способов оформления заказа</returns>
        public Dictionary<string, object> orderMethodsList()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/order-methods";
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Редактирование способа оформления заказа
        /// </summary>
        /// <param name="orderMethod">информация о способе оформления заказа</param>
        /// <returns></returns>
        public Dictionary<string, object> orderMethodsEdit(Dictionary<string, object> orderMethod)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/order-methods/" + orderMethod["code"].ToString() + "/edit";
            string dataJson = JsonConvert.SerializeObject(orderMethod);
            parameters.Add("orderMethod", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Получение списка статусов заказа
        /// </summary>
        /// <returns>массив статусов заказа</returns>
        public Dictionary<string, object> orderStatusesList()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/statuses";
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Редактирование статуса заказа
        /// </summary>
        /// <param name="status">информация о статусе заказа</param>
        /// <returns></returns>
        public Dictionary<string, object> orderStatusEdit(Dictionary<string, object> status)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/statuses/" + status["code"].ToString() + "/edit";
            string dataJson = JsonConvert.SerializeObject(status);
            parameters.Add("status", dataJson);
            result = request(url, "POST");

            return result;
        }

        /// <summary>
        /// Получение списка групп статусов заказа
        /// </summary>
        /// <returns>массив групп статусов заказа</returns>
        public Dictionary<string, object> orderStatusGroupsList()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "reference/status-groups";
            result = request(url, "GET");

            return result;
        }

        /// <summary>
        /// Обновление статистики
        /// </summary>
        /// <returns>статус выполненного обновления</returns>
        public Dictionary<string, object> statisticUpdate()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string url = apiUrl + "statistic/update";
            result = request(url, "GET");

            return result;
        }

        /// <returns>дата генерации</returns>
        public DateTime getGeneratedAt()
        {
            return generatedAt;
        }

        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        protected Dictionary<string, object> request(string url, string method)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            string urlParameters = httpBuildQuery(parameters);

            if (method.Equals("GET") && urlParameters.Length > 0)
            {
                url += "?" + urlParameters;
            }

            Exception exception = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = method;

            if (method.Equals("POST"))
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] postBytes = encoding.GetBytes(urlParameters);

                request.ContentType = contentType;
                request.ContentLength = postBytes.Length;
                request.UserAgent = userAgent;

                Stream postStream = request.GetRequestStream();
                postStream.Write(postBytes, 0, postBytes.Length);
                postStream.Flush();
                postStream.Close();
            }

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                exception = ex;
            }

            if (request == null || response == null)
            {
                throw new CurlException(exception.ToString(), exception);
            }

            int statusCode = (int)response.StatusCode;

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);
            string serverResponse = reader.ReadToEnd();

            parameters.Clear();
            parameters.Add("apiKey", apiKey);

            data = jsonDecode(serverResponse);

            if (data.ContainsKey("generatedAt"))
            {
                generatedAt = DateTime.ParseExact(data["generatedAt"].ToString(), "Y-m-d H:i:s",
                                       System.Globalization.CultureInfo.InvariantCulture);
                data.Remove("generatedAt");
            }

            if (statusCode >= 400 || (data.ContainsKey("success") && !(bool)data["success"]))
            {
                throw new ApiException(getErrorMessage(data));
            }

            data.Remove("success");

            if (data.Count == 0)
            {
                return null;
            }

            return data;
        }

        /// <param name="data"></param>
        /// <returns></returns>
        protected string getErrorMessage(Dictionary<string, object> data)
        {
            string error = "";

            if (data.ContainsKey("message"))
            {
                error = data["message"].ToString();
            }
            else if (data.ContainsKey("0"))
            {
                Dictionary<string, object> sub = getDictionary(data["0"]);
                if (sub.ContainsKey("message"))
                {
                    error = sub["message"].ToString();
                }

            }
            else if (data.ContainsKey("errorMsg"))
            {
                error = data["errorMsg"].ToString();
            }
            else if (data.ContainsKey("error"))
            {
                Dictionary<string, object> sub = getDictionary(data["error"]);
                if (sub.ContainsKey("message"))
                {
                    error = sub["message"].ToString();
                }
            }

            if (data.ContainsKey("errors"))
            {
                Dictionary<string, object> sub = getDictionary(data["errors"]);
                foreach (KeyValuePair<string, object> kvp in data)
                {
                    error += ". " + kvp.Value.ToString();
                }
            }

            return error;
        }

        /// <param name="data"></param>
        /// <returns></returns>
        public Dictionary<string, object> getDictionary(object data)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            IDictionary idict = (IDictionary)data;

            foreach (object key in idict.Keys)
            {
                result.Add(key.ToString(), idict[key]);
            }
            return result;
        }

        /// <param name="json"></param>
        /// <returns></returns>
        protected Dictionary<string, object> jsonDecode(string json)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            data = jsonToDictionary(data);

            return data;
        }

        /// <param name="data"></param>
        /// <returns></returns>
        protected static Dictionary<string, object> jsonToDictionary(Dictionary<string, object> data)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> kvp in data)
            {
                string key = kvp.Key;
                object value = kvp.Value;

                if (value.GetType() == typeof(JObject))
                {
                    Dictionary<string, object> valueJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(value.ToString());
                    value = jsonToDictionary(valueJson);
                }
                result.Add(key, value);
            }
            return result;
        }

        /// <param name="data"></param>
        /// <returns></returns>
        protected string httpBuildQuery(Dictionary<string, string> data)
        {
            string queryString = null;
            foreach (KeyValuePair<string, string> kvp in data)
            {
                queryString += kvp.Key + "=" + kvp.Value + "&";
            }

            if (queryString.Length > 0)
            {
                queryString = queryString.Substring(0, queryString.Length - 1);
            }

            return queryString;
        }
    }
}
