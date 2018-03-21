using System;
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
    public class NotesTest
    {
        private readonly Client _client;

        public NotesTest()
        {
            _client = new Client(
               Environment.GetEnvironmentVariable("RETAILCRM_URL"),
               Environment.GetEnvironmentVariable("RETAILCRM_KEY")
           );
        }

        [TestMethod]
        public void NotesCreateDelete()
        {
            Response responseFiltered = _client.NotesCreate(
                new Dictionary<string, object>
                {
                    { "text", "test task" },
                    { "customer", new Dictionary<string, object> { { "id", "4717" } }},
                    { "managerId", Environment.GetEnvironmentVariable("RETAILCRM_USER")}
                }
            );

            Debug.WriteLine(responseFiltered.GetRawResponse());
            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 201);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("success"));

            Response response = _client.NotesDelete(responseFiltered.GetResponse()["id"].ToString());

            Debug.WriteLine(response.GetRawResponse());
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }

        [TestMethod]
        public void NotesList()
        {
            DateTime datetime = DateTime.Now;

            Response responseFiltered = _client.NotesList(
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
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("notes"));
        }
    }
}
