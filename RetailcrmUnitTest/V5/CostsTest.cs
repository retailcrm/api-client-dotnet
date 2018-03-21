using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V5;

namespace RetailcrmUnitTest.V5
{
    [TestClass]
    public class CostsTest
    {
        private readonly Client _client;

        public CostsTest()
        {
            _client = new Client(
               Environment.GetEnvironmentVariable("RETAILCRM_URL"),
               Environment.GetEnvironmentVariable("RETAILCRM_KEY")
           );
        }

        [TestMethod]
        public void CostsCreateUpdateReadDelete()
        {
            DateTime datetime = DateTime.Now;

            string groupGuid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response groupResponse = _client.CostGroupsEdit(
                new Dictionary<string, object>
                {
                    { "code", groupGuid},
                    { "name", $"TestCostGroup-{groupGuid}" },
                    { "color", "#60b29a" }
                }
            );

            Debug.WriteLine(groupResponse.GetRawResponse());
            Assert.IsTrue(groupResponse.IsSuccessfull());
            Assert.IsTrue(groupResponse.GetStatusCode() == 200 || groupResponse.GetStatusCode() == 201);
            Assert.IsInstanceOfType(groupResponse, typeof(Response));
            Assert.IsTrue(groupResponse.GetResponse().ContainsKey("success"));

            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response itemCostResponse = _client.CostItemsEdit(
                new Dictionary<string, object>
                {
                    { "code", guid},
                    { "group", groupGuid },
                    { "name", $"TestCostItem-{guid}" },
                    { "type", "const" }
                }
            );

            Debug.WriteLine(itemCostResponse.GetRawResponse());
            Assert.IsTrue(itemCostResponse.IsSuccessfull());
            Assert.IsTrue(itemCostResponse.GetStatusCode() == 200 || itemCostResponse.GetStatusCode() == 201);
            Assert.IsInstanceOfType(itemCostResponse, typeof(Response));
            Assert.IsTrue(itemCostResponse.GetResponse().ContainsKey("success"));

            Response costsCreateResponse = _client.CostsCreate(
                new Dictionary<string, object>
                {
                    { "summ", 20000 },
                    { "comment", "test cost" },
                    { "costItem", guid },
                    { "dateFrom", datetime.AddDays(-3).ToString("yyyy-MM-dd")},
                    { "dateTo", datetime.AddDays(+3).ToString("yyyy-MM-dd")},
                }
            );

            Debug.WriteLine(costsCreateResponse.GetRawResponse());
            Assert.IsTrue(costsCreateResponse.IsSuccessfull());
            Assert.IsTrue(costsCreateResponse.GetStatusCode() == 201);
            Assert.IsInstanceOfType(costsCreateResponse, typeof(Response));
            Assert.IsTrue(costsCreateResponse.GetResponse().ContainsKey("success"));

            Response costsUpdateResponse = _client.CostsUpdate(
                new Dictionary<string, object>
                {
                    { "id", costsCreateResponse.GetResponse()["id"].ToString()},
                    { "summ", 30000 },
                    { "comment", "test cost update" },
                    { "costItem", guid },
                    { "dateFrom", datetime.AddDays(-3).ToString("yyyy-MM-dd")},
                    { "dateTo", datetime.AddDays(+3).ToString("yyyy-MM-dd")},
                }
            );

            Debug.WriteLine(costsUpdateResponse.GetRawResponse());
            Assert.IsTrue(costsUpdateResponse.IsSuccessfull());
            Assert.IsTrue(costsUpdateResponse.GetStatusCode() == 200 || costsUpdateResponse.GetStatusCode() == 201);
            Assert.IsInstanceOfType(costsUpdateResponse, typeof(Response));
            Assert.IsTrue(costsUpdateResponse.GetResponse().ContainsKey("success"));

            Response responseGet = _client.CostsGet(int.Parse(costsCreateResponse.GetResponse()["id"].ToString()));

            Debug.WriteLine(responseGet.GetRawResponse());
            Assert.IsTrue(responseGet.IsSuccessfull());
            Assert.IsTrue(responseGet.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseGet, typeof(Response));
            Assert.IsTrue(responseGet.GetResponse().ContainsKey("success"));

            Response response = _client.CostsDelete(costsCreateResponse.GetResponse()["id"].ToString());

            Debug.WriteLine(response.GetRawResponse());
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }

        [TestMethod]
        public void CostsList()
        {
            DateTime datetime = DateTime.Now;

            Response responseFiltered = _client.CostsList(
                new Dictionary<string, object>
                {
                    { "createdAtFrom", datetime.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss") }
                },
                2,
                50
            );

            Debug.WriteLine(responseFiltered.GetRawResponse());
            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("costs"));
        }

        [TestMethod]
        public void CostsUploadDelete()
        {
            DateTime datetime = DateTime.Now;
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);
            string groupGuid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response groupResponse = _client.CostGroupsEdit(
                new Dictionary<string, object>
                {
                    { "code", groupGuid},
                    { "name", $"TestCostGroup-{groupGuid}" },
                    { "color", "#60b29a" }
                }
            );

            Debug.WriteLine(groupResponse.GetRawResponse());
            Assert.IsTrue(groupResponse.IsSuccessfull());
            Assert.IsTrue(groupResponse.GetStatusCode() == 200 || groupResponse.GetStatusCode() == 201);
            Assert.IsInstanceOfType(groupResponse, typeof(Response));
            Assert.IsTrue(groupResponse.GetResponse().ContainsKey("success"));
            
            Response itemCostResponse = _client.CostItemsEdit(
                new Dictionary<string, object>
                {
                    { "code", guid},
                    { "group", groupGuid },
                    { "name", $"TestCostItem-{guid}" },
                    { "type", "const" }
                }
            );

            Debug.WriteLine(itemCostResponse.GetRawResponse());
            Assert.IsTrue(itemCostResponse.IsSuccessfull());
            Assert.IsTrue(itemCostResponse.GetStatusCode() == 200 || itemCostResponse.GetStatusCode() == 201);
            Assert.IsInstanceOfType(itemCostResponse, typeof(Response));
            Assert.IsTrue(itemCostResponse.GetResponse().ContainsKey("success"));

            Response costsUploadResponse = _client.CostsUpload(
                new List<object>
                {
                    new Dictionary<string, object>
                    {
                        { "summ", 10000 },
                        { "comment", "test cost 1" },
                        { "costItem", guid },
                        { "dateFrom", datetime.AddDays(-3).ToString("yyyy-MM-dd")},
                        { "dateTo", datetime.AddDays(+3).ToString("yyyy-MM-dd")},
                    },
                    new Dictionary<string, object>
                    {
                        { "summ", 20000 },
                        { "comment", "test cost 2" },
                        { "costItem", guid },
                        { "dateFrom", datetime.AddDays(-3).ToString("yyyy-MM-dd")},
                        { "dateTo", datetime.AddDays(+3).ToString("yyyy-MM-dd")},
                    },
                    new Dictionary<string, object>
                    {
                        { "summ", 30000 },
                        { "comment", "test cost 3" },
                        { "costItem", guid },
                        { "dateFrom", datetime.AddDays(-3).ToString("yyyy-MM-dd")},
                        { "dateTo", datetime.AddDays(+3).ToString("yyyy-MM-dd")},
                    },
                }
            );

            Debug.WriteLine(costsUploadResponse.GetRawResponse());
            Assert.IsTrue(costsUploadResponse.IsSuccessfull());
            Assert.IsTrue(costsUploadResponse.GetStatusCode() == 201);
            Assert.IsInstanceOfType(costsUploadResponse, typeof(Response));
            Assert.IsTrue(costsUploadResponse.GetResponse().ContainsKey("success"));

            List<string> uploadedCosts = new List<string>();
            object[] uids = (object[]) costsUploadResponse.GetResponse()["uploadedCosts"];

            foreach (var uid in uids)
            {
                uploadedCosts.Add(uid.ToString());
            }

            Response costsDeleteResponse = _client.CostsDelete(uploadedCosts);

            Debug.WriteLine(costsDeleteResponse.GetRawResponse());
            Assert.IsTrue(costsDeleteResponse.IsSuccessfull());
            Assert.IsTrue(costsDeleteResponse.GetStatusCode() == 200);
            Assert.IsInstanceOfType(costsUploadResponse, typeof(Response));
            Assert.IsTrue(costsUploadResponse.GetResponse().ContainsKey("success"));
        }
    }
}
