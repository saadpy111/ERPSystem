# ERPMala — Architecture Document

> A Multi-Tenant SaaS ERP System  
> Target: .NET 9 | SQL Server | Clean Architecture | Modular Monolith | CQRS

---

# 1. High Level Overview

## What Problem Does This Project Solve?

Small and medium enterprises (SMEs) in the Egyptian market lack affordable, integrated ERP software that covers inventory, accounting, HR, procurement, point-of-sale, and e-commerce website management under one roof. Existing solutions are either expensive, not localized (Arabic support, EGP currency, Paymob payments), or require complex on-premise infrastructure.

**ERPMala** is a cloud-native, multi-tenant SaaS ERP backend that provides a full suite of business management modules accessible via a single API. It enables business owners to register, select a subscription plan, activate modules, and immediately start managing their operations — from inventory and procurement to HR, accounting, and online storefront.

## Overall Idea

A single ASP.NET Core API hosts all business modules in a modular monolith. Each module is a self-contained vertical slice with its own Domain, Application, Persistence, Infrastructure, and API layers. Modules communicate only through the SharedKernel — via interfaces, events, or contracts — never by referencing each other's internals. Multi-tenancy is enforced at the database level through global query filters. Authentication uses JWT with a hybrid permission model (token claims + database fallback), and authorization intersects with subscription module enablement.

## Main Business Domains

| Module | Purpose |
|--------|---------|
| **Identity & Access** | User registration, JWT authentication, role/permission management, tenant lifecycle |
| **Subscription & Billing** | SaaS plans, multi-currency pricing, Paymob payments, module activation |
| **Inventory** | Products, categories, warehouses, locations, stock quantities, moves, adjustments |
| **HR** | Employees, departments, jobs, attendance, leave, payroll, loans, recruitment |
| **Accounting** | Chart of accounts, journal entries, vouchers, posting engine, multi-currency, budgets, financial reports |
| **Procurement** | Vendors, purchase orders, purchase requisitions, goods receipts, purchase invoices |
| **Website** | E-commerce storefront, cart/checkout, orders, coupons, wallet, analytics, themes, CMS |
| **POS** | (Scaffolded — not yet implemented) |
| **Report** | Dynamic report engine with SQL-based definitions, AI-powered natural language query (Groq) |

---

# 2. Architecture Style

## Architecture Pattern

**Modular Monolith with Clean Architecture (Domain-Driven Design)**

Each business module follows Clean Architecture layers:
```
API → Application → Domain
                  → Persistence
                  → Infrastructure
```

Cross-cutting concerns live in `SharedKernel`.

## Why This Architecture Was Chosen

1. **Startup speed** — A monolith can be developed, tested, and deployed much faster than a distributed system. The project went from zero to a functional multi-module ERP in a single solution.

2. **Module isolation** — Each module is a vertical slice with its own DbContext, own MediatR pipeline, own controllers. Modules cannot accidentally reference each other's internals. This enforces discipline and makes future extraction to microservices viable.

3. **Shared Kernel** — Contracts, event types, middleware, authorization infrastructure are shared without coupling modules to each other. Modules depend on `SharedKernel`, never on another module directly.

4. **Future-proofing** — If a module grows too large, it can be extracted into a separate microservice because the boundaries are already cleanly defined. The modular monolith is explicitly designed as a stepping stone to microservices, not a permanent state.

## Advantages

- Single deployment unit — simple DevOps, one Docker container
- No network overhead between modules (in-process MediatR)
- Shared transactional boundaries across modules (same SQL Server)
- Easy debugging, logging, and monitoring
- Uniform code conventions enforced by solution structure
- Database migrations managed centrally per module

## Tradeoffs

- Cannot scale modules independently — the entire monolith scales as one unit
- All modules share the same process — a memory leak in one affects all
- Build and deployment times increase with codebase size
- Requires strong discipline to prevent module boundary violations
- Limited technology diversity — all modules must use .NET

---

# 3. Solution Structure

## Projects

### Host Project

| Project | Responsibility |
|---------|---------------|
| **ERP.Api** | Composition root. Registers all module DI, configures middleware pipeline, sets up Swagger (8 docs), seeds data on startup. |

### Shared Kernel

| Project | Responsibility |
|---------|---------------|
| **SharedKernel.Core** | Cross-cutting concerns: authorization infrastructure (`HasPermissionAttribute`, `PermissionPolicyProvider`, `PermissionAuthorizationHandler`), multi-tenancy (`ITenantProvider`, `TenantProvider`), middleware (`TenantResolutionMiddleware`, `TenantRequiredMiddleware`), shared contracts (`IInventoryReadService`, `IUserLookupService`, `ITenantReadService`), constants (`Permissions.cs`, `Roles.cs`), enums (`BillingInterval`), and subscription intersection interfaces (`ISubscriptionModuleChecker`, `IPermissionModuleMapper`, `ISubscriptionService`, `ITenantRoleProvisioningService`). |
| **Events** | Shared MediatR domain event types organized by module (e.g., `InventoryPriceChangedEvent`, `OrderCreatedEvent`, `ClientRegisteredEvent`). Allows cross-module event-driven communication without direct references. |

### Business Modules (each has 5 projects)

```
Modules/
├── IdentityModule/       Auth, users, roles, permissions, tenants
│   ├── Identity.Domain          Pure entities, enums
│   ├── Identity.Application     CQRS handlers, DTOs, services, contracts
│   ├── Identity.Persistence     EF Core DbContext, repositories, migrations
│   ├── Identity.Infrastructure  (placeholder)
│   └── Identity.Api             Controllers, DI registration, JWT service
│
├── InventoryModule/      Products, stock, warehouses
├── HrModule/             Employees, payroll, attendance
├── AccountingModule/     GL, posting engine, reports
├── ProcurementModule/    Vendors, POs, goods receipt
├── SubscriptionModule/   Plans, payments, module provisioning
├── WebsiteModule/        Storefront, cart, orders, CMS
├── ReportModule/         Dynamic reporting engine
└── POSModule/            (scaffold only)
```

### Dependency Direction

```
ERP.Api (composition root)
  └── Modules/*.Api
        └── Modules/*.Application
              ├── Modules/*.Domain
              └── SharedKernel.Core
        └── Modules/*.Persistence
              └── Modules/*.Application
        └── Modules/*.Infrastructure
              └── Modules/*.Application
```

**Critical rule**: No project references another module's project. The only cross-module communication is through SharedKernel interfaces and domain events.

### What Should and Should Not Reference a Module

- **ERP.Api** references every module's `.Api` project.
- **ModuleX.Api** references ModuleX's lower layers only.
- **ModuleX.Application** references ModuleX.Domain and SharedKernel.
- **ModuleX.Persistence** references ModuleX.Application and SharedKernel.
- **ModuleX.Infrastructure** references ModuleX.Application.
- **No project** should ever reference another module's `.Domain`, `.Application`, `.Persistence`, or `.Infrastructure`.
- Cross-module integration is via:
  - **Shared interfaces** in SharedKernel (implemented by one module, consumed by another)
  - **Domain events** in the Events project (published by one module, handled by another)

---

# 4. Layers

## API Layer

**Responsibility**: HTTP concerns — routing, model binding, response formatting, Swagger documentation, authentication/authorization attributes.

**Dependency direction**: Depends on Application, Persistence, Infrastructure (through DI composition only).

**What lives here**: Controllers, DI extension methods, API-specific DTOs (request models), middleware registration.

Each module's API project registers its own services via an extension method (e.g., `AddInventoryApiDependencyInjection`) called by `ERP.Api.DependencyInjection.ApiDependencyInjection`.

Controllers follow a consistent pattern:
- `[ApiController]` + `[Route("api/[controller]")]`
- `[ApiExplorerSettings(GroupName = "ModuleName")]` for Swagger doc routing
- `[HasPermission(Permissions.X)]` for authorization
- Inject `IMediator`, delegate to `_mediator.Send()`

## Application Layer

**Responsibility**: Use case orchestration — CQRS commands/queries, handlers, DTO mapping, validation, domain event publishing.

**Dependency direction**: Depends on Domain and SharedKernel. Never depends on Persistence, Infrastructure, or API.

**What lives here**: `IRequest<TResponse>` commands/queries, `IRequestHandler<TRequest, TResponse>` handlers, AutoMapper profiles, FluentValidation validators, service interfaces and their implementations, contracts for repositories.

Handlers:
1. Validate input
2. Call domain services or repositories (through interfaces)
3. Apply business rules
4. Persist changes through unit of work
5. Publish domain events via `IMediator.Publish()`

## Domain Layer

**Responsibility**: The core business logic — entities, value objects, enums, domain services, domain events.

**Dependency direction**: Zero dependencies (pure .NET). This is the innermost layer.

**What lives here**: Entity classes with business behavior, enums, aggregate roots, domain event definitions, value objects.

Every entity either:
- Inherits from `BaseEntity` (provides `Id`, `CreatedAt`, `UpdatedAt`, `TenantId`)
- Or implements `ITenantEntity` (just `TenantId`)

## Persistence Layer

**Responsibility**: Data access — EF Core DbContext, entity configurations, repositories, migrations, seeders.

**Dependency direction**: Depends on Application (for repository interfaces) and SharedKernel (for `ITenantProvider`). Never depends on Domain directly (configurations know about entities).

**What lives here**: `DbContext` subclasses, `IEntityTypeConfiguration<T>` implementations, repository implementations, `UnitOfWork`, migrations, seeders.

Key features:
- Global query filters for multi-tenancy on every entity
- `SaveChangesAsync` override to auto-set `TenantId` on insert
- Repository pattern wrapping EF Core with `IQueryable` exposure
- Unit of Work wrapping `SaveChangesAsync` and `BeginTransactionAsync`

## Infrastructure Layer

**Responsibility**: External concerns — file storage, HTTP clients, background services, third-party integrations.

**Dependency direction**: Depends on Application (for service interfaces). Never depends on Persistence or API.

**What lives here**: `LocalFileService`, `PaymobPaymentService`, `HmacService`, `AIQueryBuilderService`, background services (`OverdueCheckService`).

---

# 5. Modules

## Identity Module

**Purpose**: Authentication, authorization, user/tenant lifecycle management.

**Main entities**: `ApplicationUser` (extends `IdentityUser`), `ApplicationRole`, `ApplicationUserRole`, `Permission`, `RolePermission`, `UserPermission`, `Tenant`, `TenantInvitation`.

**Main services**: `JwtTokenService` (JWT generation with claims), `PermissionService` (hybrid token+DB checking), `PermissionSynchronizationService`, `TenantRoleProvisioningService`, `ModuleRoleMappingService`, `UserLookupService`, `TenantReadService`.

**Main features**:
- User registration (ERP + Website client)
- JWT login with tenant context
- Role CRUD with tenant-scoped naming (`{RoleName}_{TenantId}`)
- Permission catalog (auto-seeded from `SharedKernel.Constants.Permissions`)
- Role-Permission and User-Permission assignments
- Tenant creation flow (create tenant → assign owner → provision subscription → provision roles)
- Tenant invitations with 7-day expiry tokens
- Dual user types: `System` (ERP users) and `Client` (storefront customers)

**Communication with other modules**:
- Implements `ISubscriptionService` (calls Subscription module to create subscription on tenant creation)
- Implements `ISubscriptionModuleChecker`, `IPermissionModuleMapper` (authorization handler checks subscription enablement)
- Publishes `ClientRegisteredEvent` for Website or other consumers
- Exposes `IUserLookupService` for other modules to read user data

## Subscription Module

**Purpose**: SaaS plan management, subscription lifecycle, Paymob payment processing, module provisioning.

**Main entities**: `SubscriptionPlan`, `TenantSubscription`, `PlanModule`, `PlanPrice`, `Module`, `ModulePrice`, `TenantModuleSubscription`, `Payment`, `PaymentTransaction`, `SubscriptionHistory`, `UsageHistory`.

**Main services**: `SubscriptionService` (creates subscriptions), `PaymentInitiationService`, `ModulePurchaseService`, `EffectiveModuleService`, `SubscriptionModuleChecker` (checks module enablement), `PermissionModuleMapper`, `PaymobPaymentService`, `HmacService`.

**Main features**:
- Plan seeding (5 plans: BASIC, STARTER, GROWTH, BUSINESS, ENTERPRISE)
- Multi-currency pricing (EGP + USD, Monthly + Yearly)
- Multi-currency + multi-interval pricing per plan
- Paymob payment intention creation and webhook verification
- Strategy pattern for payment completion (CreateCompany, ModulePurchase, SubscriptionRenewal, etc.)
- Module activation/deactivation with role provisioning
- Real-time quota tracking with counters
- Subscription history audit trail

**Communication with other modules**:
- Implements `SharedKernel.Subscription.*` interfaces consumed by Identity
- Calls `ITenantRoleProvisioningService` to provision roles after module purchase
- Calls `IWebsiteOrderService` on website order payment completion

## Inventory Module

**Purpose**: Product and stock management.

**Main entities**: `Product`, `ProductCategory`, `ProductAttribute`, `ProductAttributeValue`, `ProductImage`, `ProductBarcode`, `ProductCostHistory`, `Warehouse`, `Location`, `StockQuant`, `StockMove`, `StockAdjustment`, `SerialOrBatchNumber`, `InventoryQuarantine`.

**Main services**: `StockMoveHandlerFactory` (strategy pattern per move type), `InventoryReadService` (cross-module), `LocalFileService` (product images).

**Main features**:
- Product CRUD with images, attributes, barcodes
- Category hierarchy (parent-child self-reference)
- Warehouse and location management
- Stock quantity tracking (product + location)
- Stock moves with strategy: Purchase, Sale, Transfer, Adjustment, Return, UnderReview
- Stock adjustments
- Quarantine management
- Product cost history tracking

**Communication with other modules**:
- Implements `IInventoryReadService` for Website module (product catalog data)
- Listens to `OrderCreatedEvent` → reserves stock
- Listens to `GoodsReceivedEvent` → creates stock moves
- Publishes `InventoryPriceChangedEvent`, `InventoryStockChangedEvent` → Website module updates cached products

## HR Module

**Purpose**: Human resources management.

**Main entities**: `Employee`, `Department`, `Job`, `EmployeeContract`, `AttendanceRecord`, `LeaveRequest`, `LeaveType`, `Loan`, `LoanInstallment`, `PayrollRecord`, `PayrollComponent`, `SalaryStructure`, `SalaryStructureComponent`, `Applicant`, `ApplicantEducation`, `ApplicantExperience`, `RecruitmentStage`, `HrAttachment`.

**Main features**:
- Employee lifecycle (hire, activate, terminate, promote)
- Department hierarchy tree
- Job posting and management
- Attendance tracking (check-in/out, delay minutes)
- Leave management with leave types (paid/unpaid)
- Loan management with auto-generated installment schedules
- Payroll processing (base salary + allowances + deductions → net salary)
- Salary structure templates
- Recruitment pipeline (applicants → stages → hire/reject)
- Background service: hourly overdue loan installment check

## Accounting Module

**Purpose**: Financial management.

**Main entities**: `Account`, `JournalEntry`, `JournalEntryLine`, `Voucher`, `VoucherLine`, `FiscalPeriod`, `FiscalYear`, `CostCenter`, `Budget`, `BudgetLine`, `Currency`, `CurrencyRate`, `CashAccount`, `CashTransaction`, `Partner`, `Receivable`, `ReceivablePayment`, `Payable`, `PayablePayment`, `Tax`, `Sequence`, `AccountingMapping`.

**Main features**:
- Chart of accounts (hierarchical, multi-level)
- Journal entries with double-entry posting
- Voucher system (approve → post workflow)
- Immutable ledger (posted entries never modified; reversals create new entries)
- Posting engine with strategy pattern (10 strategies by source type)
- Multi-currency with exchange rates locked at entry time
- Budget control (warning vs hard failure)
- Fiscal year/period management (open/close periods)
- Cost centers
- Cash management (deposit/withdrawal/transfer)
- Receivables and payables tracking
- Financial reports: Trial Balance, General Ledger, Income Statement, Balance Sheet, Account Statement, Budget vs Actual, Expense Analysis, Profitability
- Report export: PDF (QuestPDF), Excel (ClosedXML), CSV (CsvHelper)
- Accounting mappings bridge external modules to GL accounts

## Procurement Module

**Purpose**: Purchasing and vendor management.

**Main entities**: `Vendor`, `PurchaseOrder`, `PurchaseOrderItem`, `PurchaseRequisition`, `PurchaseInvoice`, `GoodsReceipt`, `GoodsReceiptItem`, `ProcurementAttachment`.

**Main features**:
- Vendor management with tax/commercial registration details
- Purchase order lifecycle with line items
- Purchase requisitions
- Goods receipt with confirmation workflow
- Purchase invoice tracking with payment status

**Communication with other modules**:
- References Inventory `ProductId` (foreign key, no navigation — loose coupling)
- Publishes `GoodsReceivedEvent` → Inventory module creates stock moves

## Website Module

**Purpose**: E-commerce storefront, CMS, and tenant website management.

**Main entities**: `TenantWebsite`, `Theme`, `WebsiteProduct`, `WebsiteCategory`, `ProductCollection`, `ProductCollectionItem`, `Offer`, `OfferProduct`, `OfferCategory`, `Cart`, `CartItem`, `Order`, `OrderItem`, `Coupon`, `CouponUsage`, `Brand`, `Testimonial`, `NewsletterSubscriber`, `CustomerProfile`, `CustomerAnalytics`, `FavoriteProduct`, `Wallet`, `WalletTransaction`, `WithdrawalRequest`, `WebsiteVisitorSession`, `WebsiteAnalyticsDaily`, `WebsiteProductImage`.

**Main features**:
- 6 pre-seeded themes (Pharmacy, Food, Furniture, Cars, Electronics, Fashion) with full Arabic content
- Website initialization and domain verification
- Storefront: public product/category/collection/offer browsing
- Cart management (add/update/remove/clear)
- Order checkout with discount application (coupons + offers)
- Coupon system with usage limits and minimum order
- Offer/discount engine (percentage/fixed, scoped to products/categories)
- Wallet system with withdrawal requests
- Customer analytics (order history, spending, favorites)
- Admin dashboard with KPIs, weekly overview, revenue analytics
- Newsletter subscription
- Visitor session tracking with cookie-based identification
- Theme management and application

**Communication with other modules**:
- Consumes `IInventoryReadService` to sync products from Inventory
- Listens to `InventoryPriceChangedEvent`, `InventoryStockChangedEvent` to keep cached products in sync
- Calls `IOrderPaymentService` from Subscription module for order payment via Paymob
- Publishes `OrderCreatedEvent` for Inventory to reserve stock

## Report Module

**Purpose**: Dynamic report generation with AI-powered natural language queries.

**Main entities**: `Report`, `ReportField`, `ReportParameter`, `ReportFilter`, `ReportGroup`, `ReportSorting`, `ReportDataSource`, `InventoryReport`, `EmployeeReport`.

**Main features**:
- Report definitions stored as metadata (fields, parameters, filters, groupings, sortings)
- SQL-based report execution via `SqlKata` query builder
- Two seeded reports: Inventory Report (16 fields), Employee Report (8 fields)
- AI-powered report generation: natural language → SQL via Groq API
- Dapper-based query execution for read performance
- Domain event consumers for Inventory and HR to sync reporting data

## POS Module

**Status**: Scaffolded only. All four projects exist with proper cross-references but zero implementation. No entities, controllers, DbContext, or handlers.

---

# 6. Request Lifecycle

## Complete Request Flow

```
HTTP Request
  │
  ▼
Kestrel / IIS
  │
  ▼
ASP.NET Core Middleware Pipeline
  │
  ├── app.UseCors("mypolicy")
  ├── app.UseSwagger() + UseSwaggerUI()
  ├── app.UseStaticFiles()
  ├── app.UseHttpsRedirection()
  ├── app.UseAuthentication()       ← JWT Bearer token validation
  ├── app.UseMiddleware<TenantResolutionMiddleware>()
  │       └── Extract tenant from JWT or X-Tenant-Key header
  ├── app.UseMiddleware<VisitorTrackingMiddleware>()
  │       └── Only for storefront/cart/order paths
  └── app.UseAuthorization()
          └── PermissionPolicyProvider → PermissionAuthorizationHandler
                ├── Tenant guard
                ├── Hybrid permission check (token → DB)
                └── Subscription intersection check
  │
  ▼
Routing → Controller Action
  │
  ▼
[HasPermission("Inventory.Products.View")]
  │
  ▼
Controller → _mediator.Send(command/query)
  │
  ▼
MediatR Pipeline (validation behavior)
  │
  ▼
Command/Query Handler
  ├── 1. Validate input
  ├── 2. Load entities via Repository
  ├── 3. Execute domain logic
  ├── 4. Apply changes
  ├── 5. Save via UnitOfWork
  └── 6. Publish domain events
  │
  ▼
Repository → EF Core (global tenant filter)
  │
  ▼
UnitOfWork (transaction management)
  │
  ▼
SQL Server
  │
  ▼
HTTP Response
```

---

# 7. CQRS

## How CQRS Is Implemented

CQRS is implemented at the module level using **MediatR**.

### Commands

Commands represent **mutations** (writes). They follow the naming convention `{Action}{Entity}CommandRequest`:

```csharp
public record CreateProductCommandRequest(CreateProductDto Product) : IRequest<CreateProductResponse>;
```

Commands are sent via `IMediator.Send(command)`. They return response DTOs with a `Success` flag.

### Queries

Queries represent **reads** (no side effects). They follow the naming convention `Get{Entity}{Criteria}QueryRequest`:

```csharp
public record GetPagedProductsQueryRequest(string? Search, int Page, int PageSize) : IRequest<GetPagedProductsResponse>;
```

Queries return DTO projections, never entities.

### Handlers

Each command/query has a dedicated handler class. Handlers:
1. Validate input
2. Call repository interfaces
3. Apply business rules
4. Persist through UnitOfWork
5. Publish domain events
6. Return response

### Validation

FluentValidation validators are registered per command/query. A MediatR pipeline behavior (`ValidationBehavior`) runs validators automatically before the handler executes.

### Benefits

- Commands and queries have different models (write vs read)
- Handlers are isolated and unit-testable
- MediatR pipeline behaviors handle cross-cutting concerns (validation, logging, audit)
- Module isolation maintained (each module's CQRS lives entirely within it)
- Future migration to separate read/write databases requires only infrastructure changes

---

# 8. Dependency Injection

## Module-Level Registration

Each module follows a **chain-of-responsibility** DI pattern:

```
Module.Api.DependencyInjection
  ├── Module.Persistence.DependencyInjection (DbContext, repositories)
  ├── Module.Application.DependencyInjection (MediatR, services)
  └── Module.Infrastructure.DependencyInjection (external services)
```

## Composition Root

`ERP.Api.DependencyInjection.ApiDependencyInjection` calls each module's API registration extension method, plus registers:
- **Shared infrastructure**: `ITenantProvider`, `IAuthorizationPolicyProvider`, `IAuthorizationHandler`
- **CORS**: `AllowAnyOrigin`
- **Swagger**: 8 docs with JWT security
- **File services**: Local disk implementations per module
- **Controller discovery**: `AddApplicationPart` per module assembly

## Lifetime Choices

| Lifetime | Used For |
|----------|----------|
| **Singleton** | `PermissionPolicyProvider`, `ModuleRoleMappingService`, `IMapper`, `HttpClient` |
| **Scoped** | DbContext, `ITenantProvider`, repositories, `IUnitOfWork`, MediatR handlers |
| **Transient** | FluentValidation validators, AutoMapper profiles |

## Key Registration Patterns

- **MediatR**: `services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Handler).Assembly))`
- **AutoMapper**: `services.AddAutoMapper(typeof(Profile).Assembly)`
- **FluentValidation**: Pipeline behavior auto-discovery
- **Keyed services** (.NET 9): Payment validators/strategies by `PaymentPurpose` enum
- **Named HttpClient**: `"PaymobClient"` with base URL + timeout; `"GroqClient"` with API key

---

# 9. Authentication & Authorization

## JWT

Generated by `JwtTokenService`. Claims: `sub` (userId), `email`, `fullName`, `jti`, `state` (PendingTenant/TenantOwner/TenantMember), `tenant` (tenant ID), `role`[], `permission`[].

Signed with `HmacSha256`. Token duration: 600 minutes (configurable).

## Identity

Built on **ASP.NET Core Identity** with custom entities:
- `ApplicationUser` extends `IdentityUser` (adds `FullName`, `UserType`, `TenantId`, `State`)
- `ApplicationRole` extends `IdentityRole` (adds `TenantId`, `Scope`)
- `ApplicationUserRole` extends `IdentityUserRole<string>` (adds `TenantId`, `AssignedAt`, `AssignedBy`)

## Roles

System roles: `SuperAdmin`, `InventoryManager`, `HRManager`, `ProcurementManager`, `WebsiteManager`. Tenant-scoped naming: `{RoleName}_{TenantId}`. Roles have a `Scope` (ERP/Website) to separate system users from storefront customers.

## Permissions

Flat catalog organized by module prefix (e.g., `Inventory.Products.View`, `HR.Employees.Create`). Seeded via reflection from `SharedKernel.Constants.Permissions`. Assigned to roles (`RolePermission`) or directly to users (`UserPermission`).

## Policies

Custom `PermissionPolicyProvider` dynamically creates policies from permission strings. The `[HasPermission("Inventory.Products.View")]` attribute sets the policy name, and the policy provider creates the `PermissionRequirement` dynamically.

## Tenant Awareness

The `PermissionAuthorizationHandler` performs a **three-gate check**:
1. **Tenant guard**: JWT must contain a `tenant` claim
2. **Permission check**: Hybrid — JWT claims first (fast), DB fallback (consistent)
3. **Subscription gate**: The module owning the requested permission must be enabled in the tenant's subscription

---

# 10. Multi-Tenancy

## Tenant Resolution

Two sources:
1. **JWT `tenant` claim** — for authenticated requests
2. **`X-Tenant-Key` header** — for public storefront requests (resolved via `ITenantDomainResolver`)

If both are present, they must match. Mismatch → 401 "Cross-tenant access is not allowed."

## Tenant Isolation

Two levels:
1. **Database level**: Global query filters (`HasQueryFilter(e => e.TenantId == currentTenantId)`) on every entity. `SaveChangesAsync` override auto-assigns `TenantId` on new entities.
2. **Application level**: `ITenantProvider` (scoped, backed by `AsyncLocal`) flows tenant context across async boundaries.

## Tenant Provider

```csharp
public class TenantProvider : ITenantProvider
{
    private static readonly AsyncLocal<string?> _tenantId = new();
    public string? GetTenantId() => _tenantId.Value;
    public void SetTenantId(string tenantId) => _tenantId.Value = tenantId;
}
```

## Security Considerations

- Cross-tenant access validated at middleware level
- Tenant ID comes from signed JWT (cannot be tampered)
- Global query filters prevent data leakage even if queries omit tenant filter
- Insert enforcement prevents accidental cross-tenant data creation
- `.IgnoreQueryFilters()` used selectively for cross-tenant lookups (e.g., login by email)

---

# 11. Database Design

## Database

**SQL Server** hosted on `databaseasp.net`. All modules share one database with per-module schemas:

| Schema | Module |
|--------|--------|
| `Identity` | Identity |
| `Inventory` | Inventory |
| `Subscription` | Subscription |
| `Hr` | HR |
| `Website` | Website |
| `Report` | Report |
| `Accounting` | Accounting |
| (default) | Procurement |

## Why SQL Server

ACID transactions across modules, mature EF Core support, relational integrity for complex ERP data (chart of accounts, double-entry, stock tracking).

## Relationships

Intra-module: proper foreign keys with `Restrict`/`Cascade`/`SetNull` delete behavior. Cross-module: loose coupling via ID references only (no FK constraints).

## Migrations

Each module's Persistence project manages its own EF Core migrations independently.

## Repository Pattern

Repository interfaces defined in Application layer, implemented in Persistence. Some modules use generic repository (`IGenericRepository<T>`), others define specific interfaces for complex queries.

## Unit of Work

Wraps `SaveChangesAsync` and provides `BeginTransactionAsync`/`CommitTransactionAsync`/`RollbackTransactionAsync`. Some modules use specific `IUnitOfWork` with named repository properties; others use a dictionary-based generic approach.

---

# 12. Design Patterns

| # | Pattern | Where Used | Why |
|---|---------|-----------|-----|
| 1 | **Clean Architecture** | Entire solution | Separation of concerns, dependency inversion |
| 2 | **Modular Monolith** | Solution structure | Module isolation with simple deployment |
| 3 | **CQRS** (MediatR) | Every module's Application | Separate read/write models |
| 4 | **Repository** | Every module's Persistence | Abstract data access |
| 5 | **Unit of Work** | Every module's Persistence | Coordinated transactions |
| 6 | **Strategy** | Stock moves, payment completion, posting engine | Extensible algorithms by type |
| 7 | **Mediator** (MediatR) | Application layer | Decouple request from handler |
| 8 | **Specification** | Global query filters | Encapsulate tenant scoping |
| 9 | **Value Object** | `Order.ShippingAddress` | Immutable address with no identity |
| 10 | **Domain Event** | SharedKernel/Events | Cross-module communication |
| 11 | **Policy Provider** | Authorization | Dynamic policy creation |
| 12 | **AsyncLocal** | TenantProvider | Flow tenant context across async |
| 13 | **Chain of Responsibility** | Module DI registration | Clean composition root |
| 14 | **Paged Result** | All list endpoints | Standard pagination |
| 15 | **Keyed Services** (.NET 9) | Payment validators/strategies | Dispatch by purpose |

---

# 13. Integrations

## Payment — Paymob

Egyptian payment gateway for EGP transactions.

**Flow**:
1. Frontend requests payment → server creates `Payment` (Pending)
2. Server calls Paymob API `POST v1/intention/` → receives checkout URL
3. Frontend redirects user to Paymob checkout page
4. Paymob sends webhook to `POST /api/payment/verify?hmac=...`
5. Server validates HMAC (SHA512, constant-time compare)
6. Server creates `PaymentTransaction` record (idempotency check)
7. Server executes **strategy** by `PaymentPurpose` (CreateCompany, ModulePurchase, etc.)
8. Paymob redirects browser to `RedirectionUrl`
9. Frontend polls `GET /api/payment/redirect` for status

## File Storage — Local Disk

Files saved to `wwwroot/uploads/{folder}/` with GUID filenames. Served via ASP.NET Core static files.

## AI — Groq API

Report module converts natural language to SQL via Groq API for ad-hoc report queries.

## Background Jobs — Hosted Services

HR `OverdueCheckService` runs hourly to mark past-due loan installments as Overdue.

## Email

Not implemented — no SMTP configuration exists.

---

# 14. Important Workflows

## User Registration → Company Creation → Payment

1. POST `/api/WebAuth/register-client` → creates user (PendingTenant state), returns JWT without tenant
2. User browses plans → GET `/api/subscription/plans`
3. POST `/api/companies/payment` { planCode, currency, interval, companyData }
   → Creates Payment (Purpose.CreateCompany), calls Paymob, returns checkout URL
4. User pays on Paymob
5. Paymob webhook → HMAC validation → CreateCompanyCompletionStrategy
   → Create Tenant → Assign owner → Create Subscription → Provision roles → Sync permissions
6. Frontend polls status → receives new JWT with tenant claims

## Login

ERP: POST `/api/ERPAuth/login` → find user (ignore tenant filter) → verify password → generate JWT (sub, email, tenant, roles, permissions) → return token + UI permissions
Website: POST `/api/WebAuth/login` → similar + tenant domain resolution

## Purchase Order → Goods Receipt → Stock Update

1. POST `/api/PurchaseOrders` → create PO with items
2. POST `/api/GoodsReceipts` { purchaseOrderId, items[] } → create goods receipt
3. POST `/api/GoodsReceipts/{id}/confirm` → confirm → publish `GoodsReceivedEvent`
4. Inventory handler → create `StockMove` (Type=Purchase) → update `StockQuant`

## Payroll Processing

1. Define salary structure with allowance/deduction components
2. Assign structure to employee contract
3. Create payroll record (Draft) → base salary from contract
4. Calculate payroll → sum fixed + percentage components → NetSalary = Base + Allowances - Deductions
5. Approve → mark Paid

---

# 15. Security

| Mechanism | Details |
|-----------|---------|
| **Password hashing** | ASP.NET Core Identity (PBKDF2) |
| **JWT signing** | HMAC-SHA256, configurable key |
| **Token expiration** | 600 minutes (configurable) |
| **Permission-based auth** | `[HasPermission]` attribute on endpoints |
| **Hybrid permission check** | JWT claims (fast) → DB (consistent) |
| **Module gating** | Subscription intersection with auth |
| **Multi-tenancy** | Global query filters + TenantId enforcement |
| **Cross-tenant prevention** | Middleware validates JWT tenant vs header |
| **HMAC validation** | Constant-time comparison for webhooks |
| **Input validation** | FluentValidation + model binding |
| **SQL injection** | EF Core parameterized queries + Dapper parameters |
| **Immutable ledger** | Posted journal entries never modified |
| **Soft delete** | Accounting entities retain data |

---

# 16. Performance

| Optimization | Details |
|-------------|---------|
| **Pagination** | All list endpoints use `PagedResult<T>` with offset-based pagination |
| **AsNoTracking** | Read-only queries use `.AsNoTracking()` |
| **Async I/O** | Full async/await throughout |
| **Indexes** | TenantId, foreign keys, status/date/code columns indexed |
| **Eager loading** | `.Include()` / `.ThenInclude()` prevents N+1 |
| **Dapper + SqlKata** | Report module uses raw SQL for read performance |
| **Compiled queries** | Not implemented |
| **Caching** | Not implemented |
| **Database-level** | Connection pooling via SqlClient |

---

# 17. Scalability

## Current

Single-process monolith, single SQL Server instance.

## Future Growth Paths

- **Vertical scaling**: Docker container on larger VMs
- **Read replicas**: CQRS already separates reads/writes at code level — modify query repos to use read connection
- **Module extraction**: Each module is already a vertical slice — extract Website or Inventory to separate microservices with HTTP/message-queue communication
- **Database separation**: Schemas are already per-module — move high-volume modules to separate database instances
- **Redis caching**: Add between API and database for storefront reads
- **Message queue**: Replace in-process MediatR events with RabbitMQ for cross-module communication

---

# 18. Why This Design

- **Clean Architecture** instead of simple CRUD: ERP business rules are complex and long-lived. Clean Architecture keeps domain logic pure, testable, and framework-independent.

- **Modular Monolith** instead of microservices: Faster development, ACID transactions across modules, single deployment. Modules are already structured as future microservices if needed.

- **CQRS** instead of service layer: Commands need validation, business rules, persistence, and events. Queries need fast projections. CQRS keeps concerns separate; MediatR makes it ergonomic.

- **Custom permissions** instead of just roles: Roles are too coarse for ERP. Fine-grained permissions (`"Inventory.Products.Edit"`) give necessary granularity with hybrid speed/consistency checking.

- **SQL Server** instead of NoSQL: ERP data is inherently relational (double-entry accounting, stock tracking, chart of accounts). ACID compliance is non-negotiable.

- **Paymob** instead of Stripe: The target market is Egypt. Paymob supports EGP and local payment methods.

---

# 19. Challenges

## 1. Cross-Module Authorization with Subscription Gating

**Problem**: Authorization must check both permission and subscription module enablement without circular dependencies.

**Solution**: Three interfaces in SharedKernel (`IPermissionModuleMapper`, `ISubscriptionModuleChecker`, `ITenantRoleProvisioningService`) break the cycle. Identity module consumes them, Subscription module implements them.

## 2. Tenant ID in Global Query Filters

**Problem**: EF Core global query filters are baked into the model at `OnModelCreating` time (once per DbContext lifetime). Tenant ID is per-request from JWT.

**Solution**: Scoped DbContext + `ITenantProvider` set by middleware early in the pipeline. Filter captures tenant ID at the right moment. `SaveChangesAsync` override auto-assigns TenantId.

## 3. Immutable Accounting Ledger

**Problem**: Posted journal entries cannot be modified per accounting regulations, but users make mistakes.

**Solution**: Reversals create new entries with negated amounts linked via `ReversedSourceJournalId`. Original entries remain unchanged.

## 4. Paymob Webhook Idempotency

**Problem**: Paymob sends at-least-once webhook delivery. Duplicate processing would create duplicate subscriptions.

**Solution**: `HasTransactionAsync` checks for duplicate gateway transaction IDs before processing.

## 5. Polymorphic Attachments

**Problem**: Multiple modules need file attachments on any entity without creating separate tables per entity type.

**Solution**: Single `Attachment` entity with `EntityType` (string discriminator) and `EntityId` (GUID). No FK constraint but flexible.

## 6. Multi-Currency Accounting

**Problem**: Exchange rates change over time. Historical entries must maintain their original converted values.

**Solution**: Each `JournalEntryLine` stores `ForeignAmount`, `ExchangeRate` (locked at entry), and `BaseAmount` (pre-computed). Rates never change retroactively.

## 7. Module Role Provisioning on Subscription Change

**Problem**: When tenants upgrade/downgrade subscriptions, roles and permissions must be updated to match the new module set.

**Solution**: `PermissionSynchronizationService` maps enabled modules to roles, creating tenant-scoped roles and assigning appropriate permissions. Runs on company creation, module purchase/cancellation, and manual sync.

---

# 20. Interview Questions

## Q1: Why is this a modular monolith and not microservices?

**A**: The project was built as a startup. Microservices add significant overhead (service discovery, distributed transactions, container orchestration, eventual consistency) that would slow down development before proving product-market fit. The modular monolith gives us clean module boundaries with in-process communication, single-database ACID transactions, and a simple deployment model. Every module is already structured as a vertical slice with its own DbContext and API, so extracting any module to a separate microservice requires only packaging changes — not architectural rewrites.

## Q2: How does multi-tenancy work and what are the isolation levels?

**A**: Shared-database, shared-table multi-tenancy. Every entity has a `TenantId` column. Tenant isolation is enforced at the database level via EF Core global query filters (`HasQueryFilter(e => e.TenantId == currentTenantId)`) and at the application level via the `TenantProvider` (scoped, backed by `AsyncLocal`). The `SaveChangesAsync` override auto-assigns `TenantId` on new entities. Cross-tenant access is prevented by middleware that validates the JWT tenant claim matches the `X-Tenant-Key` header.

## Q3: What is the hybrid permission check and why does it exist?

**A**: The hybrid check tries JWT `permission` claims first (fast path, no database roundtrip) and falls back to querying the `PermissionRepository` (consistent path) if the claim is not found in the token. This exists because: (a) including all permissions in the JWT speeds up the common case, (b) the database is the source of truth, so if a token is stale (permissions changed but token not refreshed), the system still works correctly.

## Q4: How would you scale if a single module (e.g., Website) needs more resources?

**A**: Extract the Website module into a separate microservice. Steps: (1) move Website.Api, Application, Domain, Persistence to a new solution, (2) replace in-process MediatR events with a message queue (RabbitMQ), (3) replace `IInventoryReadService` (in-process interface) with an HTTP/gRPC client, (4) deploy the Website microservice on its own scaled-out instances. The modular monolith design explicitly anticipated this — module boundaries are already clean.

## Q5: How is the Paymob webhook secured against replay attacks?

**A**: Each webhook includes an `hmac` query parameter that is the HMAC-SHA512 of the request body, signed with our shared secret. We validate this by computing the HMAC on our side and doing a constant-time comparison to prevent timing attacks. Additionally, we track processed transaction IDs — if the same transaction arrives again (Paymob's at-least-once delivery), we detect the duplicate and acknowledge without re-processing.

## Q6: How does the Accounting posting engine ensure double-entry integrity?

**A**: The `PostingService` validates the fiscal period is open, runs budget control on debit lines, selects the appropriate `IPostingStrategy` by source type, and generates balanced journal entry lines (total debits = total credits). Each `JournalEntry` has a status (`Draft` → `Posted`), and once posted, it is immutable. Corrections create reversal entries with negated lines linked via `ReversedSourceJournalId`.

## Q7: What happens when a tenant's subscription expires?

**A**: Currently, there is no automated expiration background job. The `PermissionAuthorizationHandler` calls `ISubscriptionModuleChecker.IsModuleEnabledAsync()`, which verifies the tenant has an active subscription. If the subscription expires, authorization would deny all permission-gated requests. `SubscriptionHistory` records all state transitions.

## Q8: Why did you choose AsyncLocal for TenantProvider instead of HttpContext?

**A**: `AsyncLocal` flows across async boundaries automatically in .NET, while `HttpContext` is only available within the ASP.NET Core pipeline. Using `AsyncLocal` means the tenant ID is accessible in middleware, controllers, MediatR handlers, repository constructors, and even background services (where `HttpContext` is null).

## Q9: How are domain events used for cross-module communication?

**A**: Domain events are MediatR `INotification` classes in `SharedKernel/Events`. A handler publishes via `IMediator.Publish()` after saving changes. Other modules register `INotificationHandler<T>` implementations. Example: Procurement publishes `GoodsReceivedEvent`, Inventory's `GoodsReceivedEventHandler` creates stock moves. This is in-process and synchronous.

## Q10: How would you add a new business module (e.g., CRM)?

**A**: Create `Modules/CRMModule/` with 5 projects following the template. Define entities in Domain, create DbContext in Persistence, add CQRS handlers in Application, add controllers in Api. Register DI via `AddCrmApiDependencyInjection()` called from `ERP.Api`. Add Swagger doc. Add permissions in SharedKernel. Seed the module in `ModuleSeeder`. Existing infrastructure (auth, multi-tenancy, MediatR) is reused automatically.

## Q11: How does the system handle currency conversion in accounting?

**A**: Each `JournalEntryLine` stores `ForeignAmount` (transaction currency), `ExchangeRate` (locked at entry creation), and `BaseAmount` (computed as `ForeignAmount * ExchangeRate`). The rate is stored with the entry, so historical reports always show correct converted amounts.

## Q12: What is the purpose of the `PermissionModuleMapper`?

**A**: It maps a permission string like `"Inventory.Products.View"` to its owning module name `"Inventory"`. Used by `PermissionAuthorizationHandler` for the subscription intersection check: after verifying the user has a permission, it checks if the owning module is enabled in the tenant's subscription plan.

## Q13: How does the system ensure data consistency across modules without distributed transactions?

**A**: The system is a modular monolith sharing a single SQL Server database. Cross-module operations can use a single database transaction spanning module DbContexts. The `UnitOfWork` pattern provides `BeginTransactionAsync`, and since all modules share the same database, a single `BEGIN TRANSACTION` / `COMMIT` ensures ACID guarantees across modules.

## Q14: How are the 8 Swagger documents configured?

**A**: Each controller is decorated with `[ApiExplorerSettings(GroupName = "ModuleName")]`. The `DocInclusionPredicate` checks if the controller's group name matches the doc name. All docs share the same JWT security definition.

## Q15: How does the strategy pattern for stock moves work?

**A**: Stock moves have 6 types (Purchase, Sale, Transfer, Adjustment, Return, UnderReview). `IStockMoveHandler` defines `HandleMove(StockMove)`, with 6 implementations. The `StockMoveHandlerFactory` uses a dictionary mapping `StockMoveType` → handler. When a stock move is created, the appropriate handler is resolved and executed.

## Q16: What are the gaps in the current implementation?

**A**: (1) POS module is empty scaffold. (2) No email service. (3) No caching layer. (4) No refresh token mechanism. (5) No background job scheduler (only one BackgroundService). (6) No event bus (events are in-process only). (7) CORS allows any origin. (8) No rate limiting. (9) No comprehensive logging framework.

## Q17: How does tenant role provisioning work when modules change?

**A**: `PermissionSynchronizationService` maps effective module codes to roles via `ModuleRoleMappingService`, creates tenant-scoped role names, collects permissions for those modules, assigns them to the corresponding roles, and grants all permissions to `SuperAdmin`.

## Q18: How does the system handle fiscal year and period management?

**A**: `FiscalYear` and `FiscalPeriod` entities manage accounting periods. Before posting, the engine calls `GetOpenPeriodForDateAsync` to verify the period is open. Closed periods reject new postings.

## Q19: How is the website's storefront data kept in sync with inventory?

**A**: The Website module maintains local copies (`WebsiteProduct`, `WebsiteCategory`). Synchronization happens via domain events: `InventoryPriceChangedEvent` and `InventoryStockChangedEvent`. Publishing products calls `IInventoryReadService` for initial sync.

## Q20: How does the visitor tracking middleware work?

**A**: Intercepts storefront/cart/order paths. Looks for `.VisitorSessionId` cookie. If missing, creates `WebsiteVisitorSession` and sets HttpOnly cookie. Updates `LastSeenAt` and `WebsiteAnalyticsDaily` counters.

## Q21: Why does each module have its own DbContext instead of a unified one?

**A**: Separate DbContexts mean: (a) each module's migrations are independent, (b) modules can be extracted to separate databases, (c) no accidental cross-module query coupling, (d) each module chooses its own EF Core configuration.

## Q22: How are the 5 subscription plans structured and how do they relate to modules?

**A**: BASIC_NOMODULES (0 modules), STARTER_2MOD (HR+Report), GROWTH_3MOD (HR+Inventory+Website), BUSINESS_4MOD (HR+Inventory+Procurement+Report), ENTERPRISE_FULL (all 7). `PlanModule` join entity links plans to modules. Each plan has multi-currency, multi-interval pricing.

## Q23: How does the system prevent access to unsubscribed modules?

**A**: Through `PermissionAuthorizationHandler`: (1) extracts permission name, (2) calls `IPermissionModuleMapper.GetModuleForPermission()` to get owning module, (3) calls `ISubscriptionModuleChecker.IsModuleEnabledAsync()` to verify enablement, (4) fails authorization if not enabled.

## Q24: What is the `BillingInterval` enum and how is it used?

**A**: Values: `OneTime=0`, `Monthly=1`, `Quarterly=3`, `Yearly=12`. Used in `PlanPrice`, `TenantSubscription`, `TenantModuleSubscription`, and payment initiation. The numeric value represents months for period calculation.

## Q25: How does the AI-powered report generation work?

**A**: `AIQueryBuilderService` sends a natural language prompt + schema context to Groq API. The AI returns SQL. The SQL is executed via Dapper. Results return as dynamic dataset. (No validation on generated SQL — security concern.)

## Q26: Why does Identity.Api reference Subscription.Application?

**A**: This is Dependency Inversion — Identity.Application depends on `ISubscriptionService` (an abstraction in SharedKernel). Identity.Api wires the implementation from Subscription.Application. Identity.Application knows only the interface.

## Q27: How would you implement refresh tokens?

**A**: Add `RefreshToken` entity. On login, generate short-lived access token (15 min) + long-lived refresh token (7 days). Store refresh token hash in DB. Add `POST /auth/refresh` endpoint. Background cleanup of expired tokens.

## Q28: How does the system handle concurrent stock reservations?

**A**: Currently not implemented. `OrderCreatedEvent` triggers stock reduction but no locking on `StockQuant`. Potential race condition. Fix: pessimistic locking (`UPDLOCK, ROWLOCK`), check constraint (quantity >= 0), or `SemaphoreSlim` per product.

## Q29: How is TenantId propagated to background services?

**A**: Background services run outside HTTP pipeline. `ITenantProvider` (AsyncLocal) has no tenant set. The `OverdueCheckService` operates cross-tenant by iterating all tenants directly without relying on query filters.

## Q30: How would you migrate from modular monolith to microservices in production?

**A**: Strangler fig pattern. Phase 1: Extract Website module to separate service. Phase 2: Deploy alongside monolith, route `/api/website/**` to new service via reverse proxy. Phase 3: Replace in-process events with message queue. Phase 4: Extract more modules as needed. Phase 5: Retire monolith. Module boundaries and cross-module interfaces already define service contracts — only transport changes.
