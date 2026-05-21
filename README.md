  # .NET 8 Clean Architecture Boilerplate - Enterprise Product Catalog API

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-8.0-purple.svg)](https://docs.microsoft.com/en-us/ef/core/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-brightgreen.svg)]()
[![Logging](https://img.shields.io/badge/Logging-Serilog-orange.svg)](https://serilog.net/)

An enterprise-grade, production-ready RESTful API boilerplate built with **ASP.NET Core 8**, following **Clean Architecture** principles. This project showcases an upgrade from a traditional legacy 3-Tier (BAL/DAL) architecture to a highly scalable, decoupled, and robust software architecture designed for modern enterprise standards.

---

## 🏛️ Architectural Overview

This boilerplate strictly adheres to **Clean Architecture / Domain-Driven Design (DDD)** principles, ensuring that the business logic remains independent of databases, frameworks, and external UIs.
  
              ┌──────────────────────────────┐
              │         Presentation         │
              │      (ProductCatalog.WebAPI) │
              └──────────────┬───────────────┘
                             │
              ┌──────────────▼───────────────┐
              │         Application          │
              │  (ProductCatalog.Application)│
              └──────────────┬───────────────┘
                             │
     ┌───────────────────────┴───────────────────────┐
     │                                               │
┌────────▼──────────────────┐                   ┌────────▼──────────────────┐
│      Infrastructure       │                   │          Domain           │
│(ProductCatalog.Infras...) │                   │ (ProductCatalog.Domain)   │
└───────────────────────────┘                   └───────────────────────────┘


1. **Domain Layer**: Contains enterprise-wide business models, entities, and core logic (completely independent of external libraries).
2. **Application Layer**: Contains DTOs, service interfaces, validation rules (FluentValidation), and core application workflow logic.
3. **Infrastructure Layer**: Implements external concerns such as database persistence (Entity Framework Core), repository pattern concrete implementations, and data context.
4. **Presentation (WebAPI) Layer**: The entry point of the application handling HTTP Requests/Responses, Routing, API Versioning, Middlewares, and Swagger documentation.

---

## 🚀 Key Enterprise Features

### 1. Robust API Versioning
To protect clients from breaking changes, the API implements strict URL-segment versioning control out of the box using `Asp.Versioning.Mvc`.
* **Example Endpoints**: `GET /api/v1/Products`, `POST /api/v1/Products`

### 2. Centralized Global Exception Handling
Equipped with an advanced custom Middleware pipeline that intercepts any unhandled runtime exceptions. It guarantees the API **never crashes** into a raw HTML stack trace, forcing all internal server errors (500) into a highly elegant, unified JSON format.

### 3. Unified API Response Wrapper (`BaseResponse<T>`)
Every response follows a predictable enterprise JSON signature, enhancing frontend integration predictability:
    ```json
    {
      "success": false,
      "message": "Validation failed.",
      "data": null,
      "errors": [
        "Product name is required.",
        "Price must be greater than zero."
      ]
    }


### 4. Decoupled FluentValidation Pipeline
Business validation rules are fully isolated from the models into high-performance Validator classes instead of polluting entities with standard [Required] data annotations.

### 5. High-Performance Pagination & Filtering
Prevents database performance degradation via cursor/paging control. The GetPagedProductsAsync endpoint leverages efficient .Skip() and .Take() logic paired with conditional keyword lookups, returning proper paging metadata (totalCount, totalPages).

### 6. Structured Industrial Logging via Serilog
Replaces default console loggers with Serilog. Features dual-sink capability:

Real-time formatted console output during local development.

Rolling daily file-based physical text logging (logs/log-.txt) to trace application footprints in production environments safely.

📂 Project Structure
Plaintext
ProductCatalog/
│
├── src/
│   ├── ProductCatalog.Domain/          # Core Business Entities & Domain Logic
│   │   └── Entities/
│   │       └── Product.cs
│   │
│   ├── ProductCatalog.Application/     # Service Interfaces, DTOs, & Validators
│   │   ├── Common/Models/BaseResponse.cs
│   │   ├── DTOs/Products/
│   │   ├── Interfaces/
│   │   └── Validators/Products/
│   │
│   ├── ProductCatalog.Infrastructure/  # DbContext, Migrations, & Repositories
│   │   ├── Data/ApplicationDbContext.cs
│   │   ├── Migrations/
│   │   └── Repositories/
│   │
│   └── ProductCatalog.WebAPI/          # Controllers, Middlewares, & Configurations
│       ├── Controllers/v1/
│       ├── Middlewares/ExceptionHandlingMiddleware.cs
│       └── Program.cs
│
└── ProductCatalog.sln
🛠️ Getting Started & Installation
Prerequisites
.NET 8.0 SDK or later

Visual Studio 2022 (v17.8+) or VS Code

SQL Server / LocalDB or chosen Relational DB

1. Database Configuration
Update the connection string in src/ProductCatalog.WebAPI/appsettings.json:

JSON
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductCatalogDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
2. Apply EF Core Database Migrations
Open the Package Manager Console (PMC) inside Visual Studio, ensure the Default Project is set to ProductCatalog.Infrastructure, then run:

PowerShell
Add-Migration InitialCreate
Update-Database
3. Run the Application
Press F5 or execute the following CLI command inside the ProductCatalog.WebAPI project folder:

Bash
dotnet run
Once initialized, navigate to https://localhost:xxxx/swagger to explore the interactive Swagger UI Documentation.