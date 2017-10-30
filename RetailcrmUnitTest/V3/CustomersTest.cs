using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retailcrm;
using Retailcrm.Versions.V3;

namespace RetailcrmUnitTest.V3
{
    [TestClass]
    public class CustomersTest
    {
        private readonly Client _client;
        private readonly NameValueCollection _appSettings;

        public CustomersTest()
        {
            _appSettings = ConfigurationManager.AppSettings;
            _client = new Client(_appSettings["apiUrl"], _appSettings["apiKey"]);
        }

        [TestMethod]
        public void CustomersCreateReadUpdate()
        {
            Dictionary<string, object> customer = new Dictionary<string, object>
            {
                {"externalId", Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12)},
                {"lastName", "Baggins"},
                {"firstName", "Frodo"},
                {"email", "frodo@example.com"},
                {"phone", "+78888888888"}
            };

            Response createResponse = _client.CustomersCreate(customer);

            Assert.IsTrue(createResponse.IsSuccessfull());
            Assert.IsInstanceOfType(createResponse, typeof(Response));
            Assert.IsTrue(createResponse.GetStatusCode() == 201);
            Assert.IsTrue(createResponse.GetResponse().ContainsKey("id"));

            string id = createResponse.GetResponse()["id"].ToString();

            Response getResponse = _client.CustomersGet(id, "id");
            Assert.IsTrue(getResponse.IsSuccessfull());
            Assert.IsInstanceOfType(getResponse, typeof(Response));
            Assert.IsTrue(createResponse.GetStatusCode() == 201);
            Assert.IsTrue(getResponse.GetResponse().ContainsKey("customer"));

            Dictionary<string, object> update = new Dictionary<string, object>
            {
                {"id", id},
                {"email", "frodobaggins@example.com"}
            };

            Response updateResponse = _client.CustomersUpdate(update, "id");
            Assert.IsTrue(updateResponse.IsSuccessfull());
            Assert.IsInstanceOfType(updateResponse, typeof(Response));
            Assert.IsTrue(updateResponse.GetStatusCode() == 200);
        }

        [TestMethod]
        public void CustomersFixExternalId()
        {
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            long unixTime = ((DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond);

            Dictionary<string, object> customer = new Dictionary<string, object>
            {
                {"externalId", Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12)},
                {"createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                {"lastName", "Bull"},
                {"firstName", "John"},
                {"email", "bull@example.com"},
                {"phone", "+77777777777"}
            };

            Response createResponse = _client.CustomersCreate(customer);
            string id = createResponse.GetResponse()["id"].ToString();
            string externalId = $"{unixTime}ID";

            Dictionary<string, object>[] fix =
            {
                new Dictionary<string, object>
                {
                    { "id", id },
                    { "externalId", externalId }
                }
            };

            Assert.IsTrue(createResponse.IsSuccessfull());
            Assert.IsInstanceOfType(createResponse, typeof(Response));
            Assert.IsTrue(createResponse.GetStatusCode() == 201);
            Assert.IsTrue(createResponse.GetResponse().ContainsKey("id"));

            Response fixResponse = _client.CustomersFixExternalIds(fix);
            Assert.IsTrue(fixResponse.IsSuccessfull());
            Assert.IsInstanceOfType(fixResponse, typeof(Response));
            Assert.IsTrue(fixResponse.GetStatusCode() == 200);
        }

        [TestMethod]
        public void CustomersList()
        {
            _client.SetSite(_appSettings["site"]);

            Response response = _client.CustomersList();

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 200);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("customers"));


            Dictionary<string, object> filter = new Dictionary<string, object>
            {
                { "dateTo", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
            };

            Response responseFiltered = _client.CustomersList(filter, 2, 100);

            Assert.IsTrue(responseFiltered.IsSuccessfull());
            Assert.IsTrue(responseFiltered.GetStatusCode() == 200);
            Assert.IsInstanceOfType(responseFiltered, typeof(Response));
            Assert.IsTrue(responseFiltered.GetResponse().ContainsKey("customers"));
        }

        [TestMethod]
        public void CustomersUpload()
        {
            List<object> customers = new List<object>();

            for (int i = 0; i < 5; i++)
            {
                Dictionary<string, object> customer = new Dictionary<string, object>
                {
                    { "createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "lastName", $"Doe{i}" },
                    { "firstName", $"John{i}" },
                    { "email", $"john{i}@example.com" },
                    { "phone", $"+7999999999{i}" }
                };

                customers.Add(customer);
            }

            Response response = _client.CustomersUpload(customers);

            Assert.IsTrue(response.IsSuccessfull());
            Assert.IsTrue(response.GetStatusCode() == 201);
            Assert.IsInstanceOfType(response, typeof(Response));
            Assert.IsTrue(response.GetResponse().ContainsKey("uploadedCustomers"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `customer` must contains a data")]
        public void CustomersCreateArgumentExeption()
        {
            Dictionary<string, object> customer = new Dictionary<string, object>();
            _client.CustomersCreate(customer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `customer` must contains a data")]
        public void CustomersUpdateEmptyCustomerArgumentExeption()
        {
            Dictionary<string, object> customer = new Dictionary<string, object>();
            _client.CustomersUpdate(customer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `customer` must contains an id or externalId")]
        public void CustomersUpdateIdArgumentExeption()
        {
            Dictionary<string, object> customer = new Dictionary<string, object> { { "lastName", "Doe" } };
            _client.CustomersUpdate(customer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `customers` must contains a data")]
        public void CustomersUploadEmptyCustomersArgumentExeption()
        {
            List<object> customers = new List<object>();
            _client.CustomersUpload(customers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Parameter `customers` must contain 50 or less records")]
        public void CustomersUploadLimitArgumentExeption()
        {
            List<object> customers = new List<object>();

            for (int i = 0; i < 51; i++)
            {
                Dictionary<string, object> customer = new Dictionary<string, object>
                {
                    { "createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "lastName", $"Doe{i}" },
                    { "firstName", $"John{i}" },
                    { "email", $"john{i}@example.com" },
                    { "phone", $"+7999999999{i}" }
                };

                customers.Add(customer);
            }

            _client.CustomersUpload(customers);
        }
    }
}
