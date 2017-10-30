using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V4;

namespace RetailcrmUnitTest.V4
{
    [TestClass]
    public class StoresTest
    {
        private readonly Client _client;
        private readonly NameValueCollection _appSettings;

        public StoresTest()
        {
            _appSettings = ConfigurationManager.AppSettings;
            _client = new Client(_appSettings["apiUrl"], _appSettings["apiKey"], _appSettings["site"]);
        }

        [TestMethod]
        public void StoreProducts()
        {
            Response response = _client.StoreProducts();

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("products"));
        }

        
    }
}
