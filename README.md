[![NuGet](https://img.shields.io/nuget/v/Retailcrm.SDK.svg)](https://www.nuget.org/packages/Retailcrm.SDK/)

# retailCRM API C# client

This is C# retailCRM API client. This library allows to use all available API versions.

## Install

``` bash
PM> Install-Package Retailcrm.SDK
```

## Usage

### Get order

``` csharp
using System.Diagnostics;
using Retailcrm;
using Retailcrm.Versions.V5;
...

Client api = new Client("https://demo.retailcrm.ru", "T9DMPvuNt7FQJMszHUdG8Fkt6xHsqngH");
Response response = api.OrdersGet("12345", "externalId");

if (response.IsSuccessful()) {
    Debug.WriteLine(response.GetRawResponse());
} else {
    Debug.WriteLine($"Ошибка: [Статус HTTP-ответа {response.GetStatusCode().ToString()}]");
}

```
### Create order

``` csharp
using System.Diagnostics;
using Retailcrm;
using Retailcrm.Versions.V4;
...

Client api = new Client("https://demo.retailcrm.ru", "T9DMPvuNt7FQJMszHUdG8Fkt6xHsqngH");
Response response = api.OrdersCreate(new Dictionary<string, object>
{
    {"externalId", "12345"},
    {"createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
    {"lastName", "Doe"},
    {"firstName", "John"},
    {"email", "john@example.com"},
    {"phone", "+79999999999"},
    {"items", new List<object> {
        new Dictionary<string, object> {
            {"initialPrice", 100},
            {"quantity", 1},
            {"productId", 55},
            {"productName", "example"}
        },
        new Dictionary<string, object> {
            {"initialPrice", 200},
            {"quantity", 2},
            {"productId", 14},
            {"productName", "example too"}
        }
    }}
});

if (response.IsSuccessful()) {
    Debug.WriteLine(response.GetResponse()["externalId"].ToString());
} else {
    Debug.WriteLine($"Ошибка: [Статус HTTP-ответа {response.GetStatusCode().ToString()}]");
}

```
### Documentation

* [English](https://help.retailcrm.pro/Developers/Index)
* [Russian](https://help.retailcrm.ru/Developers/Index)
