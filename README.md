## LibraryAPI

LibraryAPI is a simple RESTful Web API for managing books, authors, and publishers. It follows a modular monolith architecture using ASP.NET Core, and includes structured logging, Swagger documentation, and async support.

### Features

* Create, read, update, and delete books
* Manage authors and publishers
* Structured logging with Serilog
* Interactive API documentation with Swagger

### Technologies

* .NET 9.0 / C# 13
* ASP.NET Core Web API
* Entity Framework Core
* Serilog
* Swagger / OpenAPI

### Setup

**Prerequisites**:

* .NET 9 SDK
* SQL Server or SQLite

**Steps**:

1. Clone the repo:

```bash
git clone https://github.com/DreKurdeK/LibraryAPI.git
cd LibraryAPI
```

2. Configure `appsettings.json` with your DB connection string
3. Run the application:

```bash
dotnet run
```

4. Access Swagger UI at:

```
https://localhost:5001
```
