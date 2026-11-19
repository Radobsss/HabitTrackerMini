# HabitTrackerMini

Учебно уеб приложение за проследяване на навици с ASP.NET Core MVC, Entity Framework Core и SQL Server. От страницата **Моите навици** можеш да добавиш навик с име, категория, седмична цел от 1 до 7 дни и незадължително описание. Страницата с подробности показва историята и напредъка за текущата седмица от понеделник до неделя.

За всеки активен навик може да се отбележи най-много едно изпълнение за текущия ден. Приложението определя деня според местната дата на сървъра. Проектът е предназначен за локална работа с един потребител.

## Етапи

| Бранч | Резултат |
| --- | --- |
| `stage/01-domain-models` | `Habit` описва навика, а `HabitEntry` пази датата на изпълнение и връзката към навика. |
| `stage/02-code-first` | `HabitDbContext` конфигурира таблиците, ключовете и ограниченията с Fluent API. Оригиналната миграция `20261022201350_Initial` остава в историята. Следващата миграция `AddHabitEntries` добавя връзка от много изпълнения към един навик и уникален индекс върху `(HabitId, Date)`. |
| `stage/03-add-and-read` | Формата записва навик в SQL Server, а Razor изгледът показва списъка. На този междинен етап контролерът използва контекста пряко. |
| `stage/04-service-layer` | Контролерът използва интерфейс на сървис, регистриран с `AddScoped`. Сървисът извлича и записва данни и проверява правилата за изпълнение. |

`HabitTrackerMini.Web` съдържа контролерите, формите и Razor изгледите. Проектът реферира `HabitTrackerMini.Services` за операциите и `HabitTrackerMini.Data` за регистрацията на `HabitDbContext`. `HabitTrackerMini.Services` реферира `HabitTrackerMini.Data`, където са ентититата, контекстът и миграциите.

## Стартиране

Необходими са .NET 8 SDK и локална инстанция на SQL Server. Връзката по подразбиране е към `(localdb)\MSSQLLocalDB` с Windows удостоверяване и база `HabitTrackerMiniDb`. За тази настройка е необходим SQL Server Express LocalDB.

Ако използваш друга SQL Server инстанция, задай връзката в същата PowerShell сесия, в която ще изпълниш командите. Замени `.\SQLEXPRESS` с името на твоята инстанция.

```powershell
$env:ConnectionStrings__DefaultConnection = 'Server=.\SQLEXPRESS;Database=HabitTrackerMiniDb;Trusted_Connection=True;TrustServerCertificate=True'
```

Изпълни командите от папката с `HabitTrackerMini.sln`. Инсталирай `dotnet-ef` само веднъж.

```powershell
dotnet tool install --global dotnet-ef --version 8.0.10
dotnet build HabitTrackerMini.sln
dotnet ef database update --project HabitTrackerMini.Data --startup-project HabitTrackerMini.Web --context HabitDbContext
dotnet run --project HabitTrackerMini.Web --launch-profile http
```

Отвори [Моите навици](http://localhost:5225/Habits).

Във Visual Studio избери `HabitTrackerMini.Web` за **Startup Project** и `HabitTrackerMini.Data` за **Default project** в **Package Manager Console**. Приложи миграциите и стартирай уеб проекта.

```powershell
Update-Database -Project HabitTrackerMini.Data -StartupProject HabitTrackerMini.Web -Context HabitDbContext
```

## Демонстрация

1. Отвори `/Habits` на празна база. Страницата трябва да покаже съобщение и бутон за добавяне.
2. Добави „Четене“ в категория „Учебни“ с цел 3 дни. Запази без описание. Провери, че навикът се появява в списъка.
3. Опитай да запазиш навик с цел 0 или 8. Формата трябва да покаже грешка, а списъкът да остане без нов запис.
4. Отвори подробностите за „Четене“ и отбележи изпълнение за днес. Седмичният напредък трябва да стане 1 от 3, а историята да покаже една дата.
5. Презареди страницата и провери, че изпълнението за днес се брои веднъж. Рестартирай приложението и провери, че навикът и историята са запазени.
