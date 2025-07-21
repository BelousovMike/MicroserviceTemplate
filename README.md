[![.NET 9](https://img.shields.io/badge/.NET-9.0-blueviolet)](https://dotnet.microsoft.com/)
[![Build & Publish NuGet](https://github.com/BelousovMike/MicroserviceTemplate/actions/workflows/nuget-publish.yml/badge.svg)](https://github.com/BelousovMike/MicroserviceTemplate/actions/workflows/nuget-publish.yml)
[![NuGet](https://img.shields.io/nuget/v/BM.MicroserviceTemplate.NET?label=NuGet)](https://www.nuget.org/packages/BM.MicroserviceTemplate.NET/)

# BM.MicroserviceTemplate.NET

<img src="https://raw.githubusercontent.com/BelousovMike/MicroserviceTemplate/main/.github/bm-logo.svg" alt="BM" width="120"/>

**BM.MicroserviceTemplate.NET** — это стартовый шаблон для микросервисов на .NET 9 с PostgreSQL, Docker, гибкой и масштабируемой архитектурой, Aspire и CI/CD. Всё, что нужно для быстрого, поддерживаемого и удобного старта!

---

## 📦 Структура решения

```
src/
  Microservice.Template.Core/           — бизнес-логика, сущности, агрегаты
  Microservice.Template.UseCases/       — сценарии (CQRS, команды, запросы)
  Microservice.Template.Infrastructure/ — работа с БД, репозитории
  Microservice.Template.ServiceDefaults/— DI, OpenTelemetry, HealthChecks
  Microservice.Template.Web/            — Web API (FastEndpoints, MediatR, Serilog)
  Microservice.Template.AspireHost/     — Aspire host для оркестрации

tests/
  Microservice.Template.UnitTests/         — модульные тесты
  Microservice.Template.IntegrationTests/  — интеграционные тесты
  Microservice.Template.FunctionalTests/   — функциональные тесты
  Microservice.Template.Aspire.Tests/      — тесты для Aspire

Docker/
  docker-compose.yml
  Dockerfile
  .env
```

---

## 🚀 Установка и старт работы с шаблоном

1. **Установите шаблон из NuGet:**
   Откройте терминал (можно прямо в Rider) и выполните:
   ```sh
   dotnet new install BM.MicroserviceTemplate.NET
   ```
   > Если нужно установить конкретную версию, используйте:
   > ```sh
   > dotnet new install BM.MicroserviceTemplate.NET::1.0.2
   > ```

2. **Проверьте, что шаблон установлен:**
   ```sh
   dotnet new --list
   ```
   В списке появится `BM.MicroserviceTemplate.NET` и его short name `microservice-template`.

3. **Создайте новый проект по шаблону:**
   Перейдите в нужную папку и выполните:
   ```sh
   dotnet new  microservice-template -n MyService
   ```
   - `microservice-template` — short name шаблона
   - `-n MyService` — имя создаваемого решения/проекта

4. **Откройте проект в Rider:**
   - Через **File → Open...** выберите созданную папку, либо перетащите её в окно Rider.

5. **(Опционально) Удалить шаблон:**
   Если нужно удалить шаблон:
   ```sh
   dotnet new uninstall BM.MicroserviceTemplate.NET
   ```

---

## ⚙️ Параметры шаблона

При создании проекта можно указать дополнительные параметры для кастомизации:

| Параметр         | Тип    | Значение по умолчанию | Описание                        |
|------------------|--------|----------------------|----------------------------------|
| aspire           | bool   | false                | Включить поддержку .NET Aspire   |
| httpPort         | text   | 5001                 | HTTP-порт приложения             |
| httpsPort        | text   | 5000                 | HTTPS-порт приложения            |

**Примеры использования:**

```sh
# Создать проект с поддержкой Aspire и кастомными портами

dotnet new microservice-template -n MyService --aspire true --httpPort 8080 --httpsPort 8443
```

---

## 🐳 Запуск через Docker

1. **Создайте файл Docker/.env** (пример ниже).
2. **Запустите всё одной командой:**
   ```sh
   cd Docker
   docker-compose up --build
   ```

### Пример Docker/.env

```env
POSTGRES_DB=appdb
POSTGRES_USER=appuser
POSTGRES_PASSWORD=yourStrongPassword

PGHOST=postgres
PGPORT=5432
PGDATABASE=appdb
PGUSER=appuser
PGPASSWORD=yourStrongPassword

HTTP_PORT=5000
HTTPS_PORT=5001
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:80;https://+:443
```

---

## 🛠️ Технологии и паттерны

- **.NET 9.0** (C# 13)
- **Гибкая архитектура** (DDD, CQRS, Clean Architecture, best practices .NET)
- **FastEndpoints** — быстрый REST API
- **MediatR** — CQRS, обработка команд и запросов
- **Serilog** — структурированное логгирование
- **Entity Framework Core** — работа с PostgreSQL
- **OpenTelemetry** — трассировка и метрики
- **Aspire** — оркестрация сервисов (если нужно)
- **StyleCop, Roslyn Analyzers** — анализ и стиль кода
- **Docker** — для локальной разработки и CI/CD

---

## 💡 Почему этот шаблон удобен?

- **Старт за 5 минут** — не нужно собирать архитектуру с нуля.
- **Всё на PostgreSQL** — никаких SQLite, только то, что реально используют в продакшене.
- **Aspire-ready** — если захотите оркестрацию, всё уже готово.
- **OpenTelemetry и HealthChecks** — мониторинг и диагностика из коробки.
- **Тесты** — есть примеры модульных, интеграционных и функциональных тестов.
- **Docker** — для локальной разработки и CI/CD.
- **Чистый, читаемый код** — чтобы не стыдно было показывать коллегам.

---

## 🧑‍💻 Советы и лайфхаки

- **Меняйте .env под себя** — не коммитьте реальные пароли, используйте секреты в CI/CD.
- **Swagger** — всегда включён в dev-режиме, удобно для тестирования ручками.
- **HealthChecks** — не открывайте наружу в проде, только для внутреннего мониторинга!
- **Aspire** — если не нужен, просто не подключайте соответствующий проект.
- **CI/CD** — есть готовый workflow для публикации шаблона в NuGet, просто настройте секрет с API-ключом.

---

## 🏗️ Как добавить свою фичу

1. Добавьте сущность/агрегат в Core.
2. Опишите UseCase (команду/запрос) в UseCases.
3. Реализуйте инфраструктурные зависимости в Infrastructure.
4. Зарегистрируйте сервисы в Web/ServiceConfigs.
5. Добавьте endpoint в Web/FastEndpoints.

---

## 🧪 Тестирование

- Все тесты автоматически прогоняются при сборке и запуске CI/CD.
- Для ручного запуска:
  ```sh
  dotnet test
  ```

---

## 📚 Полезные ссылки

- [Clean Architecture (Uncle Bob, статья)](https://8thlight.com/blog/uncle-bob/2012/08/13/the-clean-architecture.html)
- [FastEndpoints](https://fast-endpoints.com/)
- [MediatR](https://github.com/jbogard/MediatR)
- [Serilog](https://serilog.net/)
- [OpenTelemetry](https://opentelemetry.io/)
- [Aspire](https://learn.microsoft.com/ru-ru/dotnet/aspire/)

---

## 📝 Лицензия

MIT — делайте с этим шаблоном что хотите, но не забывайте про хорошую архитектуру :)

---

## 👤 Автор

Михаил Белоусов (BelousovMike)

Если есть вопросы, идеи или баги — пишите issue или делайте pull request. Удачи и чистого кода!

## 🙏 Благодарности

Спасибо авторам и сообществам следующих библиотек и инструментов, которые сделали этот шаблон возможным:

- [Ardalis (Steve Smith)](https://github.com/ardalis) — GuardClauses, Specification, Result, ApiEndpoints и другие полезные пакеты
- [FastEndpoints](https://github.com/FastEndpoints/FastEndpoints) — за быстрый и удобный REST API
- [MediatR (Jimmy Bogard)](https://github.com/jbogard/MediatR) — за CQRS и удобную обработку команд/запросов
- [Serilog](https://serilog.net/) — за мощное структурированное логгирование
- [OpenTelemetry](https://opentelemetry.io/) — за трассировку и метрики
- [Microsoft](https://github.com/dotnet) — за .NET, Entity Framework Core, Aspire и весь стек
- [StyleCop, Roslyn Analyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) — за анализ и стиль кода
- [JetBrains Rider](https://www.jetbrains.com/rider/) — за удобную IDE
- [SonarSource](https://www.sonarsource.com/) — за SonarLint и SonarQube

И всем авторам open source, чьи идеи и инструменты вдохновили этот шаблон!
