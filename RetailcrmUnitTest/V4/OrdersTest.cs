using System;
using System.Collections.Specialized;
using System.Configuration;
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
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _client = new Client(appSettings["apiUrl"], appSettings["apiKey"]);
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
    }
}
