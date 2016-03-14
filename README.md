.NET client for retailCRM API
=============================

.NET client for [RetailCRM API](http://www.retailcrm.pro/docs/Developers/ApiVersion3).

Requirements
-----------------------
* [Newtonsoft.Json](http://james.newtonking.com/json)

Install with NuGet
---------------------

Install [NuGet](http://docs.nuget.org/consume/installing-nuget).

Run this command into [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console)
``` bash
PM> Install-Package RetailCRM.ApiClient
```

Usage
---------------------

### Get order

``` csharp
using RetailCrm;
using RetailCrm.Response;
...
ApiClient api;
try
{
    api = new ApiClient(
    	"https://demo.retailcrm.pro",
    	"T9DMPvuNt7FQJMszHUdG8Fkt6xHsqngH"
    );
}
catch (WebException e)
{
    System.Console.WriteLine(e.ToString());
}

ApiResponse response = null;
try
{
    response = api.ordersGet("M-2342");
}
catch (WebException e)
{
    System.Console.WriteLine(e.ToString());
}

if (response.isSuccessful()) {
	System.Console.WriteLine(response["totalSumm"]);
} else {
	System.Console.WriteLine(
		"Error: [HTTP-code  " +
		response["statusCosde"] + "] " +
		response["errorMsg"]
	);
}

```
### Create order

``` csharp
using RetailCrm;
using RetailCrm.Response;
...
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
                {"createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                {"discount", 50},
                {"phone", "89263832233"},
                {"email", "example@gmail.com"},
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
                                                 {"text", "example"}
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
	System.Console.WriteLine(
		"Order created. Order ID is " + response["id"]
	);
} else {
	System.Console.WriteLine(
		"Error: [HTTP-code " +
		response["statusCode"] + "] " +
		response["errorMsg"]
	);
}

```
