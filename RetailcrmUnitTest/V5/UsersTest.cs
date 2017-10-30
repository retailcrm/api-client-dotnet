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
    public class UsersTest
    {
        private readonly Client _client;

        public UsersTest()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _client = new Client(appSettings["apiUrl"], appSettings["apiKey"]);
        }

        [TestMethod]
        public void UsersStatus()
        {
            Response users = _client.Users();
            Assert.IsTrue(users.IsSuccessfull());
            Assert.IsTrue(users.GetStatusCode() == 200);
            Assert.IsInstanceOfType(users, typeof(Response));
            Assert.IsTrue(users.GetResponse().ContainsKey("success"));

            object[] list = (object[])users.GetResponse()["users"];
            var user = list[0] as Dictionary<string, object>;
            Debug.Assert(user != null, nameof(user) + " != null");
            int uid = int.Parse(user["id"].ToString());
            
            Response status = _client.UsersStatus(uid, "break");
            Assert.IsTrue(status.IsSuccessfull());
            Assert.IsTrue(status.GetStatusCode() == 200);
            Assert.IsInstanceOfType(status, typeof(Response));
            Assert.IsTrue(status.GetResponse().ContainsKey("success"));
        }
    }
}
