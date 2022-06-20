# PowerShell модуль для работы с Яндекс Облаком

[PowerShell](https://github.com/PowerShell/PowerShell) - это платформа для автоматизации и скриптовый язык в одном флаконе. До некоторого момента он работал исключительно на Windows, однако, в какой-то момент стал доступен на всех платформах. Этот проект представляет собой попытку общими усилиями реализовать модуль для работы с Яндекс Облаком средствами PowerShell. На данный момент проект в `preview`, работа над ним только начата. Разработка происходит на .NET Core, что дает возможность использовать модуль на всех платформах: Windows, Linux, MacOS

## Требования

1. Учетной записи в Яндекс Облаке
2. [.NET Core](https://docs.microsoft.com/ru-ru/dotnet/core/introduction)
3. Powershell модули управления секретами: Microsoft.PowerShell.SecretManagement, Microsoft.PowerShell.SecretStore

```powershell
Install-Module Microsoft.PowerShell.SecretManagement, Microsoft.PowerShell.SecretStore
```

После установки, модули необходимо будет сконфигурировать, задать парольдля доступа к хранилищу секретов. Не забудьте его :)

## Сборка

Собрать проект относительно просто. Скрипт [build](./build.ps1) собирает его, и складывает в `c:\temp\ycps`.

Для успешного исполнения тестов нужно немного подготовить рабочую машину - добавить некоторые значения секретов, используемые в [pester](https://pester.dev/) тестах:

- YandexOAuthToken: ваш [OAuth токен](https://cloud.yandex.ru/docs/iam/concepts/authorization/oauth-token) Яндекс облака
- YandexTestCloudId: идентификатор Облака
- YandexTestFolderId: каталог в Облаке
- YandexTestNetworkId: тестовая VPC в Облаке
- YandexTestSubnetId: тестовая подсеть в Облаке
- YandexTestVMId: тестовая виртуальная машина

Эти секреты создаются и управляются при помощи модулей PowerShell, указанных в предыдущем разделе.

```powershell
# создание секретов
Set-Secret -Name YandexTestSubnetId -Secret "some_string"
```

## Тестирование

На данный момент, тестирование модуля сделано в [pester](https://pester.dev/). Это позволяет тестировать сразу в условиях, в которых все это будет применяться, а так же посмотреть примеры применения сразу на самом PowerShell. Это будет полезно тем, кто не хочет погружаться в C#, хотя на самом деле реализация очень простая. Тесты лежат вот [тут](yc.tests/pester.tests/). Скрипт [invokePester](./invokePester.ps1) предназначен для упрощения их запуска, но он, пока, привязан к конкретным каталогам диске.
