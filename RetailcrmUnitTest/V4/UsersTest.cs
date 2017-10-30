using System.Collections.Specialized;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V4;

namespace RetailcrmUnitTest.V4
{
    [TestClass]
    public class UsersTest
    {
        private readonly Client _client;
        private readonly NameValueCollection _appSettings;

        public UsersTest()
        {
            _appSettings = ConfigurationManager.AppSettings;
            _client = new Client(_appSettings["apiUrl"], _appSettings["apiKey"]);
        }

        [TestMethod]
        public void UsersGroups()
        {
            Response usersGroups = _client.UsersGroups();
            Assert.IsTrue(usersGroups.IsSuccessfull());
            Assert.IsTrue(usersGroups.GetStatusCode() == 200);
            Assert.IsInstanceOfType(usersGroups, typeof(Response));
            Assert.IsTrue(usersGroups.GetResponse().ContainsKey("success"));
        }

        [TestMethod]
        public void User()
        {
            Response usersGroups = _client.User(int.Parse(_appSettings["manager"]));
            Assert.IsTrue(usersGroups.IsSuccessfull());
            Assert.IsTrue(usersGroups.GetStatusCode() == 200);
            Assert.IsInstanceOfType(usersGroups, typeof(Response));
            Assert.IsTrue(usersGroups.GetResponse().ContainsKey("success"));
        }
    }
}
