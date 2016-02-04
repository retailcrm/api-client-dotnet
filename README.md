.NET-клиент для retailCRM API
=============================

.NET-клиент для работы с [RetailCRM API](http://www.retailcrm.ru/docs/rest-api/index.html).

version: 3.0.4

Обязательные требования
-----------------------
* [Newtonsoft.Json](http://james.newtonking.com/json)

Установка через NuGet
---------------------

Для начала требуется скачать и установить сам [NuGet](http://docs.nuget.org/consume/installing-nuget).

После этого для установки клиента требуется запустить комманду в [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console)
``` bash
PM> Install-Package RetailCRM.ApiClient
```

Примеры использования
---------------------

### Получение информации о заказе

``` csharp
using RetailCrm;
using RetailCrm.Response;
...
ApiClient api;
try
{
    api = new ApiClient(
    	"https://demo.retailcrm.ru",
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
		"Ошибка получения информации о заказа: [Статус HTTP-ответа " +
		response["statusCosde"] + "] " +
		response["errorMsg"]
	);
}

```
### Создание заказа

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
	System.Console.WriteLine(
		"Заказ успешно создан. ID заказа в retailCRM = " + response["id"]
	);
} else {
	System.Console.WriteLine(
		"Ошибка создания заказа: [Статус HTTP-ответа " +
		response["statusCosde"] + "] " +
		response["errorMsg"]
	);
}

```
