# О проектах:

## SocialApp

Проект, который делал в ходе выполнения практического курса на Udemy.\
Простое социальное приложение с токен-аутентификацией, списком пользователей, мессенджинговой системой, системой лайков, загрузкой фото в облако, фильтрами, пэйджингом и т.д.

На фронтенде использовал:
* Angular 8
* Typescript
* Методы и операторы из RxJS
* ng-bootstrap, angular2-jwt, ng2-file-upload, alertify, etc


На бэкенде использовал:
* ASP.NET Core 3.0
* Entity Framework
* CloudinaryDotNet

## TgReminderBotApp

Чат-бот для Telegram, который умеет распознавать в повседневной письменной речи даты и время и напоминать о событии в нужный момент. 

На данный момент реализован следующий функционал:
* Добавление напоминания
* Получение списка всех напоминаний
* Удаление напоминания по номеру в списке

На бэкенде использовал:
* ASP.NET Core 3.0 Web API - использовал webhook для получения данных из Telegram
* Entity Framework
* Telegram.Bot - .NET Client for Telegram Bot API
* Hors - библиотека для распознавания дат и времени в повседневной русскоязычной речи: https://github.com/DenisNP/Hors
* Hangfire - на нём работает бэкграунд сервис, который отправляет сохраненные уведомления
* MediatR - использовал библиотеку как паттерн для создания новых команд и их обработчиков, очень здорово помогло почистить код (сначала все было на некрасивых статичных фабриках)
* ngrok - для простой отладки бота

На фронтенде - Telegram :)


## CodemastersPractice

Здесь привожу только фрагменты кода, который писал во время практики по окончанию очного курса (https://www.itschoolsaransk.ru/developer). На практике занимался решением issues в продакшн-проекте.

Успел поработать с:
* ASP.NET MVC
* Entity Framework
* Dapper
* MSSQL Server
* NUnit
* Angular 1


## SimbirsoftPractice

Писал проект на практике в компании Simbirsoft. Проект - что-то типа новостной ленты в браузере из нескольких пользовательских каналов Telegram. Писал в очень сжатые сроки, и на тот момент не было понимания многих важных вещей (осторожно: плохой код).

Здесь работал с:
* ASP.NET Core MVC 2.0
* Entity Framework
* Telegram API
* Razor Pages
* Docker Container

## CSharpDeveloperCourse

Тут привожу свои работы в ходе прохождения курса https://www.itschoolsaransk.ru/developer. Общий курс по C# и .Net:
* Regex-ы
* Асинхронный код
* ADO.NET
* LINQ и Extension methods
* Основы шифрования и алгоритмов и т.д.


## PhpTestTask

Небольшое тестовое на чистом js и php(никогда до этого не писал на нем). Простой скрипт для обработки формы и отправки email письма и класс для сериализации/десериализации данных в json.
