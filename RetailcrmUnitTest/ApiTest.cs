using System.Collections.Specialized;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;

namespace RetailcrmUnitTest
{
    [TestClass]
    public class ApiTest
    {
        private readonly Connection _connection;

        public ApiTest()
        {
            _connection = new Connection(
                System.Environment.GetEnvironmentVariable("RETAILCRM_URL"),
                System.Environment.GetEnvironmentVariable("RETAILCRM_KEY")
            );
        }

        [TestMethod]
        public void Versions()
        {
            Response response = _connection.Versions();

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("versions"));
        }

        [TestMethod]
        public void Credentials()
        {
            Response response = _connection.Credentials();

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("credentials"));
        }
    }
}
