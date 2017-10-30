using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V5;

namespace RetailcrmUnitTest.V5
{
    [TestClass]
    public class CustomFieldsTest
    {
        private readonly Client _client;

        public CustomFieldsTest()
        {
            var appSettings = ConfigurationManager.AppSettings;
            _client = new Client(appSettings["apiUrl"], appSettings["apiKey"]);
        }

        [TestMethod]
        public void CustomFieldsList()
        {
            Response responseFiltered = _client.CustomFieldsList(new Dictionary<string, object> { { "entity", "order" } }, 2, 50);

            Debug.WriteLine(responseFiltered.GetRawResponse());
            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("customFields"));
        }

        [TestMethod]
        public void CustomFieldsCreateUpdateRead()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response customFieldsCreateResponse = _client.CustomFieldsCreate(
                new Dictionary<string, object>
                {
                    { "code", $"customfield{guid}" },
                    { "name", $"CustomField-{guid}"},
                    { "type", "string"},
                    { "entity", "order"}
                }
            );

            Debug.WriteLine(customFieldsCreateResponse.GetRawResponse());
            Assert.IsTrue(customFieldsCreateResponse.IsSuccessfull());
            Assert.IsTrue(customFieldsCreateResponse.GetStatusCode() == 201);
            Assert.IsInstanceOfType(customFieldsCreateResponse, typeof(Response));
            Assert.IsTrue(customFieldsCreateResponse.GetResponse().ContainsKey("code"));

            Response customFieldGetResponse = _client.CustomFieldsGet($"customfield{guid}", "order");

            Debug.WriteLine(customFieldGetResponse.GetRawResponse());
            Assert.IsTrue(customFieldGetResponse.IsSuccessfull());
            Assert.IsTrue(customFieldGetResponse.GetStatusCode() == 200);
            Assert.IsInstanceOfType(customFieldGetResponse, typeof(Response));
            Assert.IsTrue(customFieldGetResponse.GetResponse().ContainsKey("customField"));

            Response customFieldsUpdateResponse = _client.CustomFieldsUpdate(
                new Dictionary<string, object>
                {
                    { "code", $"customfield{guid}" },
                    { "name", $"CustomField-{guid}"},
                    { "type", "string"},
                    { "entity", "order"},
                    { "inList", false}
                }
            );

            Debug.WriteLine(customFieldsUpdateResponse.GetRawResponse());
            Assert.IsTrue(customFieldsUpdateResponse.IsSuccessfull());
            Assert.IsTrue(customFieldsUpdateResponse.GetStatusCode() == 200);
            Assert.IsInstanceOfType(customFieldsUpdateResponse, typeof(Response));
            Assert.IsTrue(customFieldsUpdateResponse.GetResponse().ContainsKey("code"));
        }

        [TestMethod]
        public void CustomDictionaryList()
        {
            Response responseFiltered = _client.CustomDictionaryList(new Dictionary<string, object>(), 2, 50);

            Debug.WriteLine(responseFiltered.GetRawResponse());
            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("customDictionaries"));
        }

        [TestMethod]
        public void CustomDictionariesCreateUpdateRead()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);
            string fuid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

            Response customDictionaryCreateResponse = _client.CustomDictionaryCreate(
                new Dictionary<string, object>
                {
                    { "code", $"customdict{guid}" },
                    { "name", $"CustomDict-{guid}"},
                    {
                        "elements",
                        new List<object>
                        {
                            new Dictionary<string, object>
                            {
                                { "code", $"customdictelement-{fuid}" },
                                { "name", $"CustomDictElement-{fuid}" }
                            }
                        }
                    }
                }
            );

            Debug.WriteLine(customDictionaryCreateResponse.GetRawResponse());
            Assert.IsTrue(customDictionaryCreateResponse.IsSuccessfull());
            Assert.IsTrue(customDictionaryCreateResponse.GetStatusCode() == 201);
            Assert.IsInstanceOfType(customDictionaryCreateResponse, typeof(Response));
            Assert.IsTrue(customDictionaryCreateResponse.GetResponse().ContainsKey("code"));

            Response customDictionaryGetResponse = _client.CustomDictionaryGet(customDictionaryCreateResponse.GetResponse()["code"].ToString());

            Debug.WriteLine(customDictionaryGetResponse.GetRawResponse());
            Assert.IsTrue(customDictionaryGetResponse.IsSuccessfull());
            Assert.IsTrue(customDictionaryGetResponse.GetStatusCode() == 200);
            Assert.IsInstanceOfType(customDictionaryGetResponse, typeof(Response));
            Assert.IsTrue(customDictionaryGetResponse.GetResponse().ContainsKey("customDictionary"));

            Response customDictionaryEditResponse = _client.CustomDictionaryUpdate(
                new Dictionary<string, object>
                {
                    { "code", $"customdict{guid}" },
                    { "name", $"CustomDict-{guid}Edited"},
                    {
                        "elements",
                        new List<object>
                        {
                            new Dictionary<string, object>
                            {
                                { "code", $"customdictelement-{fuid}" },
                                { "name", $"CustomDictElement-{fuid}" }
                            },
                            new Dictionary<string, object>
                            {
                                { "code", $"customdictelement-{fuid}1" },
                                { "name", $"CustomDictElement-{fuid}1" }
                            }
                        }
                    }
                }
            );

            Debug.WriteLine(customDictionaryEditResponse.GetRawResponse());
            Assert.IsTrue(customDictionaryEditResponse.IsSuccessfull());
            Assert.IsTrue(customDictionaryEditResponse.GetStatusCode() == 200);
            Assert.IsInstanceOfType(customDictionaryEditResponse, typeof(Response));
            Assert.IsTrue(customDictionaryEditResponse.GetResponse().ContainsKey("code"));
        }
    }
}
