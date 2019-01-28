using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V3;

namespace RetailcrmUnitTest.V3
{
    [TestClass]
    public class OrdersTest
    {
        private readonly Client _client;

        public OrdersTest()
        {
            _client = new Client(
                Environment.GetEnvironmentVariable("RETAILCRM_URL"),
                Environment.GetEnvironmentVariable("RETAILCRM_KEY")
            );
        }

        [TestMethod]
        public void OrdersCreateReadUpdate()
        {
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            long unixTime = ((DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond);

            Dictionary<string, object> order = new Dictionary<string, object>
            {
                {"number", unixTime},
                {"createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                {"lastName", "Doe"},
                {"firstName", "John"},
                {"email", "john@example.com"},
                {"phone", "+79999999999"}
            };


            Response createResponse = _client.OrdersCreate(order);
            Assert.IsTrue(createResponse.IsSuccessfull());
            Assert.IsInstanceOfType(createResponse, typeof(Response));
            Assert.IsTrue(createResponse.GetResponse().ContainsKey("id"));

            string id = createResponse.GetResponse()["id"].ToString();

            Response getResponse = _client.OrdersGet(id, "id");
            Assert.IsTrue(getResponse.IsSuccessfull());
            Assert.IsInstanceOfType(getResponse, typeof(Response));
            Assert.IsTrue(getResponse.GetResponse().ContainsKey("order"));

            Dictionary<string, object> update = new Dictionary<string, object>
            {
                {"id", id},
                {"status", "cancel-other"}
            };

            Response updateResponse = _client.OrdersUpdate(update, "id");
            Assert.IsTrue(updateResponse.IsSuccessfull());
            Assert.IsInstanceOfType(updateResponse, typeof(Response));
            Assert.IsTrue(updateResponse.GetStatusCode() == 200);
        }

        [TestMethod]
        public void OrdersFixExternalId()
        {
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            long unixTime = ((DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond);

            Dictionary<string, object> order = new Dictionary<string, object>
            {
                {"number", unixTime},
                {"createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                {"lastName", "Doe"},
                {"firstName", "John"},
                {"email", "john@example.com"},
                {"phone", "+79999999999"}
            };

            Response createResponse = _client.OrdersCreate(order);
            string id = createResponse.GetResponse()["id"].ToString();
            string externalId = $"{unixTime}ID";

            Dictionary<string, object>[] fix =
            {
                new Dictionary<string, object>
                {
                    { "id", id },
                    { "externalId", externalId }
                }
            };

            Assert.IsTrue(createResponse.IsSuccessfull());
            Assert.IsInstanceOfType(createResponse, typeof(Response));
            Assert.IsTrue(createResponse.GetResponse().ContainsKey("id"));

            Response fixResponse = _client.OrdersFixExternalIds(fix);
            Assert.IsTrue(fixResponse.IsSuccessfull());
            Assert.IsInstanceOfType(fixResponse, typeof(Response));
        }

        [TestMethod]
        public void OrdersList()
        {
            Response response = _client.OrdersList();
            
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("orders"));

            Dictionary<string, object> filter = new Dictionary<string, object>
            {
                { "extendedStatus", "new" },
                { "createdAtTo", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
            };

            Response responseFiltered = _client.OrdersList(filter, 2, 100);

            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("orders"));
        }

        [TestMethod]
        public void OrdersHistory()
        {
            Response response = _client.OrdersHistory();

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("orders"));


            DateTime datetime = DateTime.Now;
            DateTime from = datetime.AddHours(-24);
            DateTime to = datetime.AddHours(-1);

            Response responseFiltered = _client.OrdersHistory(from, to, 50, 1, false);

            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("orders"));
        }

        [TestMethod]
        public void OrdersStatuses()
        {
            List<string> ids = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                string[] statuses = new string[5];

                statuses[0] = "new";
                statuses[1] = "cancel-other";
                statuses[2] = "complete";
                statuses[3] = "assembling-complete";
                statuses[4] = "client-confirmed";

                Dictionary<string, object> order = new Dictionary<string, object>
                    {
                        { "number", $"order-{i}" },
                        { "createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                        { "lastName", $"Doe{i}" },
                        { "firstName", $"John{i}" },
                        { "email", $"john{i}@example.com" },
                        { "phone", $"+7999999999{i}" },
                        { "status", statuses[i] }
                    };

                Response createResponse = _client.OrdersCreate(order);
                ids.Add(createResponse.GetResponse()["id"].ToString());
            }

            Response response = _client.OrdersStatuses(ids);

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("orders"));
        }

        [TestMethod]
        public void OrdersUpload()
        {
            List<object> orders = new List<object>();

            for (int i = 0; i < 5; i++)
            {
                string[] statuses = new string[5];

                statuses[0] = "new";
                statuses[1] = "cancel-other";
                statuses[2] = "complete";
                statuses[3] = "assembling-complete";
                statuses[4] = "client-confirmed";

                Dictionary<string, object> order = new Dictionary<string, object>
                {
                    { "number", $"order-{i}" },
                    { "createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "lastName", $"Doe{i}" },
                    { "firstName", $"John{i}" },
                    { "email", $"john{i}@example.com" },
                    { "phone", $"+7999999999{i}" },
                    { "status", statuses[i] }
                };
                
                orders.Add(order);
            }

            Response response = _client.OrdersUpload(orders);

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 201);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("uploadedOrders"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `order` must contains a data")]
        public void OrdersCreateArgumentExeption()
        {
            Dictionary<string, object> order = new Dictionary<string, object>();
            _client.OrdersCreate(order);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `order` must contains a data")]
        public void OrdersUpdateEmptyOrderArgumentExeption()
        {
            Dictionary<string, object> order = new Dictionary<string, object>();
            _client.OrdersUpdate(order);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `order` must contains an id or externalId")]
        public void OrdersUpdateIdArgumentExeption()
        {
            Dictionary<string, object> order = new Dictionary<string, object> {{"status", "cancel-other"}};
            _client.OrdersUpdate(order);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "You must set the array of `ids` or `externalIds`.")]
        public void OrdersStatusesArgumentExeption()
        {
            _client.OrdersStatuses(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Too many ids or externalIds. Maximum number of elements is 500")]
        public void OrdersStatusesLimitArgumentExeption()
        {
            List<string> ids = new List<string>();

            for (int i = 0; i <= 501; i++)
            {
                ids.Add(i.ToString());
            }

            _client.OrdersStatuses(ids);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `orders` must contains a data")]
        public void OrdersUploadEmptyOrdersArgumentExeption()
        {
            List<object> orders = new List<object>();
            _client.OrdersUpload(orders);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `orders` must contain 50 or less records")]
        public void OrdersUploadLimitArgumentExeption()
        {
            List<object> orders = new List<object>();

            for (int i = 0; i < 51; i++)
            {
                Dictionary<string, object> order = new Dictionary<string, object>
                {
                    { "number", $"order-{i}" },
                    { "createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "lastName", $"Doe{i}" },
                    { "firstName", $"John{i}" },
                    { "email", $"john{i}@example.com" },
                    { "phone", $"+7999999999{i}" }
                };

                orders.Add(order);
            }

            _client.OrdersUpload(orders);
        }
    }
}
