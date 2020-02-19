using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V5;

namespace RetailcrmUnitTest.V5
{
    [TestClass]
    public class TasksTest
    {
        private readonly Client _client;

        public TasksTest()
        {
            _client = new Client(
               Environment.GetEnvironmentVariable("RETAILCRM_URL"),
               Environment.GetEnvironmentVariable("RETAILCRM_KEY")
           );
        }

        [TestMethod]
        [Ignore]
        public void TasksCreateUpdateGet()
        {
            DateTime datetime = DateTime.Now;

            Response responseFiltered = _client.TasksCreate(
                new Dictionary<string, object>
                {
                    { "text", "test task" },
                    { "commentary", "test commentary"},
                    { "datetime", datetime.AddHours(+3).ToString("yyyy-MM-dd HH:mm")},
                    { "performerId", Environment.GetEnvironmentVariable("RETAILCRM_USER")}
                }
            );

            Debug.WriteLine(responseFiltered.GetRawResponse());
            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 201);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("success"));

            Response response = _client.TasksUpdate(
                new Dictionary<string, object>
                {
                    { "id", responseFiltered.GetResponse()["id"].ToString()},
                    { "text", "test task edited" },
                    { "commentary", "test commentary"},
                    { "datetime", datetime.AddHours(+4).ToString("yyyy-MM-dd HH:mm")},
                    { "performerId", Environment.GetEnvironmentVariable("RETAILCRM_USER")}
                }
            );

            Debug.WriteLine(response.GetRawResponse());
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));

            Response responseGet = _client.TasksGet(responseFiltered.GetResponse()["id"].ToString());

            Debug.WriteLine(responseGet.GetRawResponse());
            Assert.IsTrue(responseGet.IsSuccessfull());
            Assert.IsTrue(responseGet.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseGet, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }

        [TestMethod]
        public void TasksList()
        {
            Response responseFiltered = _client.TasksList(
                new Dictionary<string, object>
                {
                    { "performers", new List<string> { Environment.GetEnvironmentVariable("RETAILCRM_USER") } },
                    { "status", "performing" }
                },
                2,
                50
            );

            Debug.WriteLine(responseFiltered.GetRawResponse());
            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("tasks"));
        }
    }
}
