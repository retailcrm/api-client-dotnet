RetailCRM REST API client for .NET
==================================

Java Client for [RetailCRM REST API](http://www.retailcrm.ru/docs/rest-api/index.html).
version: 3.0.0

Requirements
------------
* [Newtonsoft.Json](http://james.newtonking.com/json)

Usage
------------

### Create API client class

``` csharp
using RetailCrm;
...
RestApi api = new RestApi(
    "https://demo.intarocrm.ru",
    "T9DMPvuNt7FQJMszHUdG8Fkt6xHsqngH"
);
```
Constructor arguments are:

1. Your RetailCRM acount URL-address
2. Your site API Token

### Example: get order types list

``` csharp

ApiClient api;
string url, key;
try
{
    api = new ApiClient(url, key);
}
catch (WebException e)
{
    System.Console.WriteLine(e.ToString());
}

Dictionary<string, object> tmpOrder = new Dictionary<string, object>(){
                {"number", "example"},
                {"externalId", "example"},
                {"createdAt", DateTime.Now.ToString("Y-m-d H:i:s")},
                {"discount", 50},
                {"phone", "89263832233"},
                {"email", "vshirokov@gmail.com"},
                {"customerComment", "example"},
                {"customFields", new Dictionary<string, object>(){
                                     {"reciever_phone", "example"},
                                     {"reciever_name", "example"},
                                     {"ext_number", "example"}
                                 }
                },
                {"contragentType", "individual"},
                {"orderType", "eshop-individual"},
                {"orderMethod", "app"},
                {"customerId", "555"},
                {"managerId", 8},
                {"items", new Dictionary<string, object>(){
                              {"0", new Dictionary<string, object>(){
                                        {"initialPrice", 100},
                                        {"quantity", 1},
                                        {"productId", 55},
                                        {"productName", "example"}
                                    }
                              }
                          }
                },
                {"delivery", new Dictionary<string, object>(){
                                 {"code", "courier"},
                                 {"date", DateTime.Now.ToString("Y-m-d")},
                                 {"address", new Dictionary<string, object>(){
                                                 {"text", "exampleing"}
                                             }
                                 }
                             }
                }
            };

ApiResponse response = null;
try
{
    response = api.ordersEdit(order);
}
catch (WebException e)
{
    System.Console.WriteLine(e.ToString());
}

if (response.isSuccessful() && 201 == response["statusCosde"]) {
	System.Console.WriteLine("Заказ успешно создан. ID заказа в retailCRM = " + response["id"]);
} else {
	System.Console.WriteLine("Ошибка создания заказа: [Статус HTTP-ответа " + response["statusCosde"] + "] " + response["errorMsg"]);
}

```