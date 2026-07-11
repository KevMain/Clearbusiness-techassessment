# Clearbusiness Technical Assessment

A full-stack application demonstrating Clean Architecture, TDD, and modern software engineering practices.

## 🚀 Quick Start

### Prerequisites
- .NET 10 SDK
- Node.js 18+ and npm
- Visual Studio 2026 or VS Code (optional)

### Running the Application

**Option 1: One-Command Start (Windows/PowerShell)**
```powershell
.\run.ps1
```
This launches both backend and frontend in separate windows.

**Option 2: Manual Start**

Backend (in terminal 1):
```bash
cd TechnicalAssessment/TechnicalAssessment
dotnet restore
dotnet run
```
→ Backend runs at `https://localhost:5001`

Frontend (in terminal 2):
```bash
cd ClientApp
npm install
npm run dev
```
→ Frontend runs at `http://localhost:5173`

**Option 3: Visual Studio**
1. Open `TechnicalAssessment.slnx`
2. Press F5 to run the backend
3. Run frontend separately (see above)

## 📁 Solution Structure

```
Clearbusiness-techassessment/
├── TechnicalAssessment/          # ASP.NET Core API
├── TechnicalAssessment.Application/  # Use Cases & Business Logic
├── TechnicalAssessment.Domain/       # Core Domain Models
├── TechnicalAssessment.Infrastructure/  # Data Access & External Services
├── tests/                        # Unit & Integration Tests
└── ClientApp/                    # Vue 3 Frontend
```

## 🏗️ Architecture & Design Choices

### Clean Architecture
The solution follows Clean Architecture principles with clear separation of concerns:

- **Domain Layer**: Business entities (Customer, Order) and domain logic with no external dependencies
- **Application Layer**: Use cases, business workflows, and application services
- **Infrastructure Layer**: Data persistence, CSV parsing, external integrations
- **API Layer**: HTTP endpoints, request/response handling

**Why?** Testability, maintainability, and independence from frameworks/databases.

### Key Design Patterns

**Repository Pattern**
- Abstracts data access logic
- Enables easy testing with in-memory implementations
- Swappable persistence layers

**Domain Events**
- Decoupled business event handling (e.g., `CustomerDiscountApplied`)
- In-memory implementation with easy path to message brokers (RabbitMQ, Azure Service Bus)

**CQRS-lite**
- Separated read/write operations where beneficial
- Optimized queries for different use cases

**Dependency Injection**
- All dependencies injected via interfaces
- Highly testable and loosely coupled

### Technology Stack

**Backend**
- **.NET 10**: Latest C# features, performance improvements
- **ASP.NET Core**: RESTful API with minimal boilerplate
- **Entity Framework Core**: Type-safe database access
- **xUnit**: Modern testing framework

**Frontend**
- **Vue 3 + TypeScript**: Type-safe reactive UI
- **Vite**: Fast development and builds

### Testing Strategy

**Test-Driven Development (TDD)**
- Core domain logic written test-first
- Unit tests for business rules, parsers, validators
- Integration tests for API endpoints and database operations

Run all tests:
```bash
dotnet test
```

### CSV Import Implementation
Custom lightweight CSV parsers with:
- Flexible date format support (dd/MM/yyyy, yyyy-MM-dd, etc.)
- Header detection and validation
- Graceful error handling for malformed data
- Culture-invariant parsing

See [Domain README](TechnicalAssessment.Domain/README.md) for detailed parsing behavior.

## 📋 Key Features Demonstrated

✅ Clean Architecture with proper layer separation  
✅ Domain-Driven Design principles  
✅ Test-Driven Development  
✅ SOLID principles throughout  
✅ RESTful API design  
✅ Robust error handling  
✅ Business rule enforcement  
✅ CSV data import with validation  
✅ Domain events for decoupled logic  
✅ Full-stack integration (API + SPA)

## 📖 Additional Documentation

- [ClientApp README](ClientApp/README.md) - Frontend setup and configuration
- [Backend README](TechnicalAssessment/README.md) - API endpoint details
- [Domain README](TechnicalAssessment.Domain/README.md) - CSV parsing and domain events

## 💡 Development Notes

- **Database**: In-memory for easy setup; can be configured for SQL Server
- **CORS**: Enabled for local development
- **Environment**: Settings in `appsettings.Development.json`
- **Proxy**: Vite configured to proxy `/api` requests to backend

---

**Author**: Kevin Main  
**Purpose**: Technical Assessment for Clearbusiness

