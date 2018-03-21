using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V4;

namespace RetailcrmUnitTest.V4
{
    [TestClass]
    public class ReferencesTest
    {
        private readonly Client _client;

        public ReferencesTest()
        {
            _client = new Client(
                Environment.GetEnvironmentVariable("RETAILCRM_URL"),
                Environment.GetEnvironmentVariable("RETAILCRM_KEY")
            );
        }

        [TestMethod]
        public void PriceTypes()
        {
            Response response = _client.PriceTypes();

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("priceTypes"));
        }

        [TestMethod]
        public void PriceTypesEdit()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response response = _client.PriceTypesEdit(
                new Dictionary<string, object>
                {
                    { "code", guid},
                    { "name", $"TestPriceType-{guid}" }
                }
            );

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200 || response.GetStatusCode() == 201);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }
    }
}
