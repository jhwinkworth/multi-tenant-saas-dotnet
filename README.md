# Multi-Tenant SaaS Backend (.NET 8, ASP.NET Core)

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A **work-in-progress** multi-tenant SaaS backend built with ASP.NET Core 8, demonstrating clean architecture, multi-tenancy, and modern .NET patterns.  

---

## Features

- **Multi-Tenant Architecture**  
  - Tenant-aware entities: `Tenant`, `User`, `Project`, `TaskItem`, `Subscription`, `Plan`
  - Global query filters for tenant isolation  

- **Layered Architecture**  
  - **API Layer**: Controllers and endpoints  
  - **Application Layer**: Services, interfaces, DTOs  
  - **Domain Layer**: Entities and business logic  
  - **Infrastructure Layer**: EF Core, repositories, tenant provider  

- **Authentication & Authorization**  
  - JWT-based authentication  
  - Role-based access (Admin / User)  
  - Tenant context derived from JWT claims  

- **REST APIs**  
  - CRUD operations for Projects, TaskItems, Users, Subscriptions, and Plans  
  - Tenant-scoped endpoints  

- **Testing**  
  - Unit tests for services and controllers  
  - DTO-safe testing pattern  
  - xUnit + FluentAssertions  

- **Ready for CI/CD**  
  - Can integrate GitHub Actions for automated test runs  

---

## Technologies

- [.NET 8 / ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- JWT Authentication
- xUnit + FluentAssertions for testing
- PostgreSQL (via Npgsql) or any EF-supported provider

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2026 (or VS Code)
- PostgreSQL database (or another EF Core provider)

### Clone Repository

```bash
git clone https://github.com/jhwinkworth/multi-tenant-saas-dotnet.git
cd multi-tenant-saas-dotnet
