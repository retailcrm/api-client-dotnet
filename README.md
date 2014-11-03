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

string url, key;
Dictionary<string, object> orderTypes = new Dictionary<string, object>();

RestApi api = new RestApi(url, key);
try {
    orderTypes = api.orderTypesList();
} catch (ApiException e) {
    Console.WriteLine(ex.Message);
} catch (CurlException e) {
    Console.WriteLine(ex.Message);
}

```