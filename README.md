.NET-клиент RetailCRM API
=========================

.NET-клиент для работы с [RetailCRM API](http://www.retailcrm.ru/docs/Developers/Index). Клиент поддерживает все доступные на текущий момент версии API (v3-v5).

Установка через NuGet
---------------------

``` bash
PM> Install-Package Retailcrm.SDK
```

Примеры использования
---------------------

### Получение информации о заказе

``` csharp
using System.Diagnostics;
using Retailcrm;
using Retailcrm.Versions.V5;
...

Client api = new Client("https://demo.retailcrm.ru", "T9DMPvuNt7FQJMszHUdG8Fkt6xHsqngH");
Response response = api.OrdersGet("12345", "externalId");

if (response.isSuccessful()) {
    Debug.WriteLine(Response.GetRawResponse());
} else {
    Debug.WriteLine($"Ошибка: [Статус HTTP-ответа {response.GetStatusCode().ToString()}]");
}

```
### Создание заказа

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

if (response.isSuccessful()) {
    Debug.WriteLine(Response.GetResponse()["externalId"].ToString());
} else {
    Debug.WriteLine($"Ошибка: [Статус HTTP-ответа {response.GetStatusCode().ToString()}]");
}

```
