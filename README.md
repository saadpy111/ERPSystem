# 🏢 ERP System

A modular and scalable **ERP (Enterprise Resource Planning)** system built with **.NET 8** following a **Modular Monolithic Architecture**.

This project provides a unified ERP backend that can handle multiple business domains such as **Inventory**, **Accounting**, **CRM**, and more — all isolated as independent modules for clean separation of concerns and maintainability.

---

## 🚀 Key Features

✅ Modular Monolithic structure — each domain is a separate module (Inventory, Accounting, CRM, etc.)  
✅ Clean Architecture principles  
✅ CQRS + Mediator pattern  
✅ Repository & Unit of Work pattern  
✅ Entity Framework Core for data access  
✅ Swagger documentation per module  
✅ Centralized Dependency Injection setup  
✅ Layered architecture (Domain, Application, Infrastructure, Persistence, Api)

---


## 🏗️ Architecture Overview

Each **Module** (e.g., Inventory, Accounting, CRM) contains its own:
- **Domain layer:** Business entities and logic  
- **Application layer:** Use cases, CQRS commands/queries  
- **Infrastructure layer:** External services, file handling, etc.  
- **Persistence layer:** Data access and configurations  
- **Api layer:** REST controllers and request models  

All modules are registered into the main API through dependency injection:
```csharp
builder.Services.AddInventoryApiDependencyInjection(configuration);
builder.Services.AddAccountingApiDependencyInjection(configuration);
builder.Services.AddCrmApiDependencyInjection(configuration);

🧠 Technologies Used

.NET 8 / ASP.NET Core

Entity Framework Core

FluentValidation

MediatR

Swagger / Swashbuckle

AutoMapper

SQL Server

Dependency Injection




---

## 🧩 Project Structure

ERP-System/
│
├── Api/
│ └── ERP.Api/ # Main API entry point (composition root)
│
├── Modules/
│ ├── InventoryModule/
│ │ ├── Inventory.Api/ # API controllers for Inventory
│ │ ├── Inventory.Application/ # Business logic, DTOs, CQRS handlers
│ │ ├── Inventory.Domain/ # Entities, Aggregates, Enums
│ │ ├── Inventory.Persistence/ # EF configurations, Repositories
│ │ └── Inventory.Infrastructure/# File services, external integrations
│ │
│ ├── AccountingModule/
│ │ ├── Accounting.Api/
│ │ ├── Accounting.Application/
│ │ ├── Accounting.Domain/
│ │ ├── Accounting.Persistence/
│ │ └── Accounting.Infrastructure/
│ │
│ ├── CrmModule/
│ │ ├── Crm.Api/
│ │ ├── Crm.Application/
│ │ ├── Crm.Domain/
│ │ ├── Crm.Persistence/
│ │ └── Crm.Infrastructure/
│ │
│ └── ... other modules ...
│
├── SharedKernel/ # Cross-cutting concerns (BaseEntity, Result, Exceptions, etc.)
│
└── README.md


---

🧱 Modular Swagger Configuration
Each module can have its own Swagger UI page:

options.SwaggerDoc("inventory", new() { Title = "Inventory API", Version = "v1" });
options.SwaggerDoc("accounting", new() { Title = "Accounting API", Version = "v1" });
options.SwaggerDoc("crm", new() { Title = "CRM API", Version = "v1" });
Then in the UI:

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/inventory/swagger.json", "Inventory API");
    c.SwaggerEndpoint("/swagger/accounting/swagger.json", "Accounting API");
    c.SwaggerEndpoint("/swagger/crm/swagger.json", "CRM API");
});
⚙️ Getting Started
1️⃣ Clone the repository
git clone https://github.com/yourusername/ERP-System.git
cd ERP-System


2️⃣ Update connection string
In appsettings.json:

json
Copy code
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ERP_DB;Trusted_Connection=True;TrustServerCertificate=True;"
}
3️⃣ Apply migrations
bash
Copy code
dotnet ef database update
4️⃣ Run the project
bash
Copy code
dotnet run --project Api/ERP.Api
5️⃣ Open in browser
👉 https://localhost:5001/swagger

🧩 Example Modules
Module	Description
Inventory	Manages products, stock, warehouses, and movements
Accounting	Handles financial records, invoices, and ledgers
CRM	Manages customers, interactions, and opportunities
HR	(Planned) Employee management, attendance, payroll
Sales	(Planned) Orders, quotations, and sales analytics

🛠️ Future Enhancements
Add Identity & Authentication module

Integrate RabbitMQ or Kafka for async events

Support for Multi-Tenancy

Add GraphQL layer

Docker & CI/CD pipelines

