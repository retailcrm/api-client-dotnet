using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V4;

namespace RetailcrmUnitTest.V4
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
        public void OrdersHistory()
        {
            DateTime datetime = DateTime.Now;

            Dictionary<string, object> filter = new Dictionary<string, object>
            {
                { "startDate", datetime.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss") },
                { "endDate", datetime.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss")}
            };

            Response response = _client.OrdersHistory(filter, 1, 50);

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("history"));
        }

        [TestMethod]
        public void BigOrderCreateUpdate()
        {
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            long unixTime = ((DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond);

            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            Dictionary<string, object> properties = new Dictionary<string, object>();

            for (int j = 0; j < 10; j++)
            {
                properties.Add(
                    $"property_{j}",
                    new Dictionary<string, object> {
                        { "name", $"Property_{j}" },
                        { "code", $"property_{j}" },
                        { "value", $"{Guid.NewGuid().ToString()}" },
                    }
                );
            }

            for (int i = 0; i < 100; i++) {
                Dictionary<string, object> item = new Dictionary<string, object> {
                    { "initialPrice", i + 100 },
                    { "purchasePrice", i + 90 },
                    { "productName", $"Product_{i}" },
                    { "quantity", 2 },
                    {
                        "offer",
                        new Dictionary<string, object> {
                            { "name", $"Product_{i}" },
                            { "xmlId", $"{Guid.NewGuid().ToString()}" }
                        }
                    },
                    { "properties", properties }
                };

                items.Add(item);
            }

            Dictionary<string, object> order = new Dictionary<string, object>
            {
                {"number", unixTime},
                {"createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                {"lastName", "Doe"},
                {"firstName", "John"},
                {"email", "john@example.com"},
                {"phone", "+79999999999"},
                {"items", items},
                {
                    "delivery",
                    new Dictionary<string, object> {
                        { "code", "self-delivery" },
                        { "cost", "300" },
                        {
                            "address", new Dictionary<string, object> {
                                { "city", "Москва" },
                                { "street", "Ярославская" },
                                { "building", "10" },
                                { "flat", "2" },
                            }
                        }
                    }
                }
            };

            Response createResponse = _client.OrdersCreate(order);

            Assert.IsTrue(createResponse.IsSuccessfull());
            Assert.IsInstanceOfType(createResponse, typeof(Response));
            Assert.AreEqual(createResponse.GetStatusCode(), 201);
            Assert.IsTrue(createResponse.GetResponse().ContainsKey("id"));

            List<Dictionary<string, object>> newItems = new List<Dictionary<string, object>>();

            for (int i = 0; i < 120; i++)
            {
                Dictionary<string, object> item = new Dictionary<string, object> {
                    { "initialPrice", i + 100 },
                    { "purchasePrice", i + 90 },
                    { "productName", $"Product_{i}" },
                    { "quantity", 2 },
                    {
                        "offer",
                        new Dictionary<string, object> {
                            { "name", $"Product_{i}" },
                            { "xmlId", $"{Guid.NewGuid().ToString()}" }
                        }
                    },
                    { "properties", properties }
                };

                newItems.Add(item);
            }

            Response updateResponse = _client.OrdersUpdate(
                new Dictionary<string, object> {
                    { "id", createResponse.GetResponse()["id"].ToString()},
                    { "items", newItems }
                },
                "id"
            );

            Assert.IsTrue(updateResponse.IsSuccessfull());
            Assert.IsInstanceOfType(updateResponse, typeof(Response));
            Assert.AreEqual(updateResponse.GetStatusCode(), 200);
        }
    }
}
