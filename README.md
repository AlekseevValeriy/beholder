# Beholder

Мобильное приложение телегида с централизованным хранением данных о телеканалах в базе данных и передачей их клиентской части через сервер.

## Технологии

![C#](https://img.shields.io/badge/C%23-239120.svg?logo=C-sharp&style=flat)
![MAUI](https://img.shields.io/badge/.NET%20MAUI-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![ASP.NET Minimal API](https://img.shields.io/badge/ASP.NET%20Minimal%20API-6C9A00?style=flat-square&logo=asp-dot-net&logoColor=white)
![MS SQL Server](https://img.shields.io/badge/Microsoft_SQL_Server-CC2927)

## Возможности

- Поиск каналов по названию, номеру, тэгу
- Отображение программы в карточке телеканала
- Система профилей с сохранением избранных каналов

## Установка

```bash
git clone https://github.com/AlekseevValeriy/beholder.git
cd beholder
```

## Использование

```bash
dotnet restore
```

### Клиент

```bash
cd Client
dotnet build -t:Run -f net9.0-android
```

### Сервер

```bash
cd Server
dotnet run
```

## Лицензия
Распространяется по лицензии [MIT](LICENSE).
