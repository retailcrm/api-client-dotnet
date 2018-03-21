using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V5;

namespace RetailcrmUnitTest.V5
{
    [TestClass]
    public class SegmentsTest
    {
        private readonly Client _client;

        public SegmentsTest()
        {
            _client = new Client(
               Environment.GetEnvironmentVariable("RETAILCRM_URL"),
               Environment.GetEnvironmentVariable("RETAILCRM_KEY")
           );
        }

        [TestMethod]
        public void Segments()
        {
            Response responseFiltered = _client.Segments(
                new Dictionary<string, object>
                {
                    { "active", "1" }
                },
                2,
                50
            );

            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("segments"));
        }
    }
}
