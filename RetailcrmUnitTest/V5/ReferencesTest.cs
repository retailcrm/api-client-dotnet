using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V5;

namespace RetailcrmUnitTest.V5
{
    [TestClass]
    public class ReferencesTest
    {
        private readonly Client _client;

        public ReferencesTest()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _client = new Client(appSettings["apiUrl"], appSettings["apiKey"]);
        }

        [TestMethod]
        public void CostGroups()
        {
            Response response = _client.CostGroups();
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("costGroups"));
        }

        [TestMethod]
        public void CostItems()
        {
            Response response = _client.CostItems();
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("costItems"));
        }

        [TestMethod]
        public void LegalEntities()
        {
            Response response = _client.LegalEntities();
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("legalEntities"));
        }

        [TestMethod]
        public void CostGroupsEdit()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response response = _client.CostGroupsEdit(
                new Dictionary<string, object>
                {
                    { "code", guid},
                    { "name", $"TestCostGroup-{guid}" },
                    { "color", "#da5c98" }
                }
            );

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200 || response.GetStatusCode() == 201);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }

        [TestMethod]
        public void CostItemsEdit()
        {
            string groupGuid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response groupResponse = _client.CostGroupsEdit(
                new Dictionary<string, object>
                {
                    { "code", groupGuid},
                    { "name", $"TestCostGroup-{groupGuid}" },
                    { "color", "#60b29a" }
                }
            );

            Assert.IsTrue(groupResponse.IsSuccessfull());
            Assert.IsTrue(groupResponse.GetStatusCode() == 200 || groupResponse.GetStatusCode() == 201);
            Assert.IsInstanceOfType(groupResponse, typeof(Response));
            Assert.IsTrue(groupResponse.GetResponse().ContainsKey("success"));

            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response response = _client.CostItemsEdit(
                new Dictionary<string, object>
                {
                    { "code", guid},
                    { "group", groupGuid },
                    { "name", $"TestCostItem-{guid}" },
                    { "type", "const" }
                }
            );

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200 || response.GetStatusCode() == 201);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }

        [TestMethod]
        public void LegalEntitiesEdit()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response response = _client.LegalEntitiesEdit(
                new Dictionary<string, object>
                {
                    { "code", guid},
                    { "legalName", $"Test LegalEntity-{guid}" },
                    { "contragentType", "legal-entity"},
                    { "countryIso", "RU"}
                }
            );

            Debug.WriteLine(response.GetRawResponse());

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200 || response.GetStatusCode() == 201);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }

        [TestMethod]
        public void Couriers()
        {
            Response response = _client.Couriers();
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("couriers"));
        }

        [TestMethod]
        public void CouriersCreate()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 3);

            Response response = _client.CouriersCreate(
                new Dictionary<string, object>
                {
                    { "firstName", $"CourierFirstName-{guid}"},
                    { "lastName", $"CourierLastName-{guid}" },
                    { "active", false},
                    { "email", $"{guid}@example.com"}
                }
            );

            Debug.WriteLine(response.GetRawResponse());

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 201);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }

        [TestMethod]
        public void CouriersEdit()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 3);

            Response response = _client.CouriersCreate(
                new Dictionary<string, object>
                {
                    { "firstName", $"CourierFirstName-{guid}"},
                    { "lastName", $"CourierLastName-{guid}" },
                    { "active", false},
                    { "email", $"{guid}@example.com"}
                }
            );

            Debug.WriteLine(response.GetRawResponse());

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 201);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));

            string id = response.GetResponse()["id"].ToString();

            Response responseEdit = _client.CouriersEdit(
                new Dictionary<string, object>
                {
                    { "id", id},
                    { "firstName", "CourierFirstName"},
                    { "lastName", "CourierLastName" },
                    { "active", true}
                }
            );

            Debug.WriteLine(responseEdit.GetRawResponse());

            Assert.IsTrue(responseEdit.IsSuccessfull());
            Assert.IsTrue(responseEdit.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }
    }
}
