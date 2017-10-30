using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V5;

namespace RetailcrmUnitTest.V5
{
    [TestClass]
    public class OrdersTest
    {
        private readonly Client _client;

        public OrdersTest()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _client = new Client(appSettings["apiUrl"], appSettings["apiKey"]);
        }

        [TestMethod]
        public void OrdersCombine()
        {
            Dictionary<string, object> firstOrder = new Dictionary<string, object>
            {
                {"number", Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12)},
                {"createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                {"lastName", "Doe"},
                {"firstName", "John"},
                {"email", $"{Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12)}@example.com"},
                {"phone", "+79999999999"}
            };


            Response createFirstResponse = _client.OrdersCreate(firstOrder);
            Assert.IsTrue(createFirstResponse.IsSuccessfull());
            Assert.IsInstanceOfType(createFirstResponse, typeof(Response));
            Assert.IsTrue(createFirstResponse.GetResponse().ContainsKey("id"));

            string firstId = createFirstResponse.GetResponse()["id"].ToString();

            Dictionary<string, object> secondOrder = new Dictionary<string, object>
            {
                {"number", Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12)},
                {"createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                {"lastName", "Doe"},
                {"firstName", "John"},
                {"email", $"{Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12)}@example.com"},
                {"phone", "+79999999999"}
            };


            Response createSecondResponse = _client.OrdersCreate(secondOrder);
            Assert.IsTrue(createSecondResponse.IsSuccessfull());
            Assert.IsInstanceOfType(createSecondResponse, typeof(Response));
            Assert.IsTrue(createSecondResponse.GetResponse().ContainsKey("id"));

            string secondId = createSecondResponse.GetResponse()["id"].ToString();

            Dictionary<string, object> firstCombineOrder = new Dictionary<string, object>
            {
                {"id", firstId }
            };

            Dictionary<string, object> secondCombineOrder = new Dictionary<string, object>
            {
                {"id", secondId }
            };
            
            Response combineResponse = _client.OrdersCombine(firstCombineOrder, secondCombineOrder);

            Debug.WriteLine(combineResponse.GetRawResponse());

            Assert.IsTrue(combineResponse.IsSuccessfull());
            Assert.IsInstanceOfType(combineResponse, typeof(Response));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `order` must contains a data")]
        public void OrdersCombineEmptyOrderArgumentExeption()
        {
            Dictionary<string, object> firstOrder = new Dictionary<string, object>();
            Dictionary<string, object> secondOrder = new Dictionary<string, object>
            {
                { "id", "111" }
            };

            _client.OrdersCombine(firstOrder, secondOrder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `resultOrder` must contains a data")]
        public void OrdersCombineEmptyResultOrderArgumentExeption()
        {
            Dictionary<string, object> secondOrder = new Dictionary<string, object>();
            Dictionary<string, object> firstOrder = new Dictionary<string, object>
            {
                { "id", "111" }
            };

            _client.OrdersCombine(firstOrder, secondOrder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `order` must contains `id` key")]
        public void OrdersCombineEmptyIdOrderArgumentExeption()
        {
            Dictionary<string, object> firstOrder = new Dictionary<string, object>
            {
                { "number", Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12) }
            };

            Dictionary<string, object> secondOrder = new Dictionary<string, object>
            {
                { "id", "111" }
            };

            _client.OrdersCombine(firstOrder, secondOrder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `resultOrder` must contains `id` key")]
        public void OrdersCombineEmptyIdResultOrderArgumentExeption()
        {
            Dictionary<string, object> secondOrder = new Dictionary<string, object>
            {
                { "number", Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12) }
            };

            Dictionary<string, object> firstOrder = new Dictionary<string, object>
            {
                { "id", "111" }
            };

            _client.OrdersCombine(firstOrder, secondOrder);
        }
    }
}
