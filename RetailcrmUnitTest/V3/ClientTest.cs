using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V3;

namespace RetailcrmUnitTest.V3
{
    [TestClass]
    public class ClientTest
    {
        private readonly Client _client;

        public ClientTest()
        {
            _client = new Client(
                System.Environment.GetEnvironmentVariable("RETAILCRM_URL"),
                System.Environment.GetEnvironmentVariable("RETAILCRM_KEY")
            );
        }

        [TestMethod]
        public void InitTest()
        {
            Assert.IsInstanceOfType(_client, typeof(Client));

            string siteCode = "default";

            _client.SetSite(siteCode);

            Assert.AreEqual(_client.GetSite(), siteCode);
        }

        [TestMethod]
        public void StatisticUpdate()
        {
            Response response = _client.StatisticUpdate();

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetStatusCode() == 200);
        }
    }
}
