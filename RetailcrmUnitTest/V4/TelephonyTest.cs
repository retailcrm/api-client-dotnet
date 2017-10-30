using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V4;

namespace RetailcrmUnitTest.V4
{
    [TestClass]
    public class TelephonyTest
    {
        private readonly Client _client;
        private readonly NameValueCollection _appSettings;
        private readonly string _phoneCode = "100";
        private readonly string _logoUrl = "http://www.onsitemaintenance.com/img/voip.svg";
        private readonly string _telephonyCode = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

        public TelephonyTest()
        {
            _appSettings = ConfigurationManager.AppSettings;
            _client = new Client(_appSettings["apiUrl"], _appSettings["apiKey"]);
        }

        [TestMethod]
        public void TelephonySettingsEdit()
        {
            Response response = _client.TelephonySettingsEdit(
                new Dictionary<string, object>
                {

                    { "code", _telephonyCode},
                    { "clientId", _appSettings["customer"] },
                    { "makeCallUrl", $"http://{_telephonyCode}.example.com/call"},
                    { "name", $"TestTelephony-{_telephonyCode}"},
                    { "image",  _logoUrl},
                    { "inputEventSupported", true},
                    { "outputEventSupported", true},
                    { "hangupEventSupported", true},
                    {
                        "additionalCodes",
                        new List<object>
                        {
                            new Dictionary<string, object>
                            {
                                { "userId", _appSettings["manager"] },
                                { "code", _phoneCode }
                            }
                        }
                    }
                }
            );

            Debug.WriteLine(response.GetRawResponse());
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200 || response.GetStatusCode() == 201);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }

        [TestMethod]
        public void TelephonyCallEvent()
        {
            Response response = _client.TelephonyCallEvent(
                new Dictionary<string, object>
                {
                    { "phone",  _appSettings["phone"] },
                    { "type", "in" },
                    { "hangupStatus", "failed"},
                    { "codes", new List<string> { _phoneCode }},
                    { "userIds", new List<int> { int.Parse(_appSettings["customer"]) }}
                }
                
            );
            Debug.WriteLine(response.GetRawResponse());
            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("success"));
        }   
    }
}
