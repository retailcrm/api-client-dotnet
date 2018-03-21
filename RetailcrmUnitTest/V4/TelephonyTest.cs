using System;
using System.Collections.Generic;
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
        private readonly string _phoneCode = "100";
        private readonly string _logoUrl = "http://www.onsitemaintenance.com/img/voip.svg";
        private readonly string _telephonyCode = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

        public TelephonyTest()
        {
            _client = new Client(
                Environment.GetEnvironmentVariable("RETAILCRM_URL"),
                Environment.GetEnvironmentVariable("RETAILCRM_KEY")
            );
        }

        [TestMethod]
        public void TelephonySettingsEdit()
        {
            Response response = _client.TelephonySettingsEdit(
                new Dictionary<string, object>
                {

                    { "code", _telephonyCode},
                    { "clientId", "4717" },
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
                                { "userId", Environment.GetEnvironmentVariable("RETAILCRM_USER") },
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
                    { "phone",  "+79999999999" },
                    { "type", "in" },
                    { "hangupStatus", "failed"},
                    { "codes", new List<string> { _phoneCode }},
                    { "userIds", new List<int> { int.Parse(Environment.GetEnvironmentVariable("RETAILCRM_USER")) }}
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
