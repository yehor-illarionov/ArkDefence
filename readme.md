# Описание проекта сервера ARKDefence

REST API сервер

## Структура проекта

* Core - (deprecated) используется для референса.
* TestClient - тест соединения с Auth0, тесты работы команд SignalR с одноплатником
* Host - монолит приложения

## Структура Host

* Constants - полезные* константы** приложения
* Controllers - REST API
* Data - модели и контексты ef core
    * Repositories - репозитории для работы с данными
* Exceptions - ошибки приложения 
* Hubs - signalr хабы
* Mappers 
* Messaging
    * Commands - CQRS комманды
    * Consumers - потребители событий системы
    * Events - события системы
    * Requests - CQRS запросы
* Migrations - миграции ef core 2
* Services - утилитарные сервисы (a.k.a. синглтоны)
* ViewModels - DTO (транспортные) объекты для REST API
### Регистрация зависимостей
* ProjectServiceCollectionExtensions - зависимости проекта
* CustomServiceCollectionExtensions - импортируемые зависимости и кастомные настройки
## Стек

**netcoreapp3.1**
### Пакеты
* EntityFrameworkCore 6.0.x
* SignalRCore 
* MassTransit 6.0.x
* Quartz.NET 3.0.x
* Swashbuckle 5.0.x
* Finbuckle.MultiTenant 4.0.x
* Boxed.AspNetCore 5.0.x
* Newtonsoft.Json 12.0.x
### Другое
* C# 8
### База данных
* PostgreSQL 11.x + EntityFrameworkCore

## Авторизация

OAUTH2 (Auth0) JwtBearer

Для signalr jwtbearer добавляется в uri через параметр access_token. В пути хаба должен быть фрагмент /hubs/.

## Multitenant

[Finbuckle.MultiTenant 4.0.x](https://www.finbuckle.com/MultiTenant)

[Route strategy](https://www.finbuckle.com/MultiTenant/Docs/Strategies#route-strategy)

[Data Isolation with Entity Framework Core](https://www.finbuckle.com/MultiTenant/Docs/EFCore#top)

Для доп. изоляции используется Microsoft.AspNetCore.DataProtection с отдельным контекстом в EFCore

* EncryptionAlgorithm.AES_256_CBC
* ValidationAlgorithm.HMACSHA256

## Документация API

/redoc

/swagger