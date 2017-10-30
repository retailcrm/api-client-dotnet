using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V4;

namespace RetailcrmUnitTest.V4
{
    [TestClass]
    public class CustomersTest
    {
        private readonly Client _client;

        public CustomersTest()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _client = new Client(appSettings["apiUrl"], appSettings["apiKey"]);
        }

        [TestMethod]
        public void CustomersHistory()
        {
            Response response = _client.CustomersHistory();

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("history"));

            DateTime datetime = DateTime.Now;

            Response responseFiltered = _client.CustomersHistory(
                new Dictionary<string, object>
                {
                    { "startDate", datetime.AddMonths(-2).ToString("yyyy-MM-dd HH:mm:ss") },
                    { "endDate", datetime.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss")}
                }
            );

            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("history"));
        }
    }
}
