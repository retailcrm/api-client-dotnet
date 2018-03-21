using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V3;

namespace RetailcrmUnitTest.V3
{
    [TestClass]
    public class StoresTest
    {
        private readonly Client _client;

        public StoresTest()
        {
            _client = new Client(
                Environment.GetEnvironmentVariable("RETAILCRM_URL"),
                Environment.GetEnvironmentVariable("RETAILCRM_KEY")
            );
        }

        [TestMethod]
        public void InventoriesUpload()
        {
            List<object> offers = new List<object>
            {
                new Dictionary<string, object>
                {
                    { "xmlId", Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12)},
                    { "stores", new List<object>
                        {
                            new Dictionary<string, object>
                            {
                                { "code", Environment.GetEnvironmentVariable("RETAILCRM_STORE") },
                                { "available", 500 },
                                { "purchasePrice", 300}
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "xmlId", Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12)},
                    { "stores", new List<object>
                        {
                            new Dictionary<string, object>
                            {
                                { "code", Environment.GetEnvironmentVariable("RETAILCRM_STORE") },
                                { "available", 600 },
                                { "purchasePrice", 350}
                            }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "xmlId", Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12)},
                    { "stores", new List<object>
                        {
                            new Dictionary<string, object>
                            {
                                { "code", Environment.GetEnvironmentVariable("RETAILCRM_STORE") },
                                { "available", 700 },
                                { "purchasePrice", 400}
                            }
                        }
                    }
                }
            };

            Response response = _client.StoreInventoriesUpload(offers);

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("processedOffersCount"));
        }

        [TestMethod]
        public void Inventories()
        {
            Dictionary<string, object> filter = new Dictionary<string, object>
            {
                { "site", Environment.GetEnvironmentVariable("RETAILCRM_SITE")},
                { "details", 1}
            };

            Response response = _client.StoreInventoriesGet(filter, 1, 50);

            Debug.WriteLine(response.GetRawResponse());

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("offers"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `offers` must contains a data")]
        public void StoreInventoriesUploadArgumentExeption()
        {
            List<object> offers = new List<object>();
            _client.StoreInventoriesUpload(offers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `offers` must contain 250 or less records")]
        public void StoreInventoriesUploadLimitArgumentExeption()
        {
            List<object> offers = new List<object>();

            for (int i = 0; i < 300; i++)
            {
                offers.Add(
                    new Dictionary<string, object>
                    {
                        { "xmlId", Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12)},
                        { "stores", new List<object>
                            {
                                new Dictionary<string, object>
                                {
                                    { "code", Environment.GetEnvironmentVariable("RETAILCRM_STORE") },
                                    { "available", 700 },
                                    { "purchasePrice", 400}
                                }
                            }
                        }
                    }
                );
            }

            _client.StoreInventoriesUpload(offers);
        }
    }
}
