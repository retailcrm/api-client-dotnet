using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm.Versions.V5;

namespace RetailcrmUnitTest.V5
{
    [TestClass]
    public class TelephonyTest
    {
        private readonly Client _client;
        private readonly NameValueCollection _appSettings;

        public TelephonyTest()
        {
            _appSettings = ConfigurationManager.AppSettings;
            _client = new Client(_appSettings["apiUrl"], _appSettings["apiKey"]);
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
