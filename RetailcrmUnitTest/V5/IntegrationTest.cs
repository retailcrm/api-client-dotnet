using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V5;

namespace RetailcrmUnitTest.V5
{
    [TestClass]
    public class IntegrationTest
    {
        private readonly Client _client;

        public IntegrationTest()
        {
            var appSettings = ConfigurationManager.AppSettings;
            _client = new Client(appSettings["apiUrl"], appSettings["apiKey"]);
        }

        [TestMethod]
        public void IntegrationsSettingsEditSettingsGet()
        {
            string uid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response response = _client.IntegrationsSettingsEdit(
                new Dictionary<string, object>
                {
                    { "code", uid},
                    { "name", $"TestIntegration-{uid}"},
                    { "active", true},
                    { "accountUrl", $"http://{uid}.example.com"},
                    { "logo", "https://www.ibm.com/cloud-computing/images/cloud/products/cloud-integration/api-economy-icon.svg"},
                }
            );

            Debug.WriteLine(response.GetRawResponse());
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));

            Response responseGet = _client.IntegrationsSettingGet(uid);

            Debug.WriteLine(responseGet.GetRawResponse());
            Assert.IsTrue(responseGet.IsSuccessfull());
            Assert.IsInstanceOfType(responseGet, typeof(Response));
            Assert.IsTrue(responseGet.GetResponse().ContainsKey("success"));
        }
    }
}
