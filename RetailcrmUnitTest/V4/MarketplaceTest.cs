using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V4;

namespace RetailcrmUnitTest.V4
{
    [TestClass]
    public class MarketplaceTest
    {
        private readonly Client _client;

        public MarketplaceTest()
        {
            var appSettings = ConfigurationManager.AppSettings;
            _client = new Client(appSettings["apiUrl"], appSettings["apiKey"]);
        }

        [TestMethod]
        public void MarketplaceSettingsEdit()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response response = _client.MarketplaceSettingsEdit(
                new Dictionary<string, object>()
                {
                    { "name", $"MarketplaceApp-{guid}" },
                    { "code", guid},
                    { "configurationUrl", $"http://{guid}.example.com"},
                    { "active", false}
                }
            );

            Debug.WriteLine(response.GetRawResponse());
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }
    }
}
