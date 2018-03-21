using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm.Versions.V5;

namespace RetailcrmUnitTest.V5
{
    [TestClass]
    public class TelephonyTest
    {
        private readonly Client _client;

        public TelephonyTest()
        {
            _client = new Client(
               Environment.GetEnvironmentVariable("RETAILCRM_URL"),
               Environment.GetEnvironmentVariable("RETAILCRM_KEY")
           );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "This method is unavailable in API V5")]
        public void TelephonySettingsGetArgumentExeption()
        {
            _client.TelephonySettingsGet("anycode");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "This method is unavailable in API V5")]
        public void TelephonySettingsGetTelephonySettingsEditArgumentExeption()
        {
            _client.TelephonySettingsEdit(new Dictionary<string, object>());
        }
    }
}
