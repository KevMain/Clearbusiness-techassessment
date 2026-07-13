# Technical Assessment - Clearbusiness

A full-stack .NET application built with Clean Architecture, demonstrating modern development practices including Domain-Driven Design and Test-Driven Development.

## 🚀 Quick Start

### Prerequisites
- .NET 10 SDK
- Node.js 18+

### Run the Application

**Easiest: PowerShell Script**
```powershell
.\run.ps1
```

**Or Manually:**

Backend:
```bash
cd TechnicalAssessment/TechnicalAssessment
dotnet run
```
→ API runs at `https://localhost:5001`

Frontend:
```bash
cd ClientApp
npm install && npm run dev
```
→ UI runs at `http://localhost:5173`

**Visual Studio:**
1. Open `TechnicalAssessment.slnx`
2. Press F5 (backend only - run frontend separately)

### Run Tests
```bash
dotnet test
```

## 📁 Project Structure

```
TechnicalAssessment/          # ASP.NET Core Web API
TechnicalAssessment.Domain/   # Core business entities & logic
TechnicalAssessment.Infrastructure/  # Data access & CSV parsing
tests/                        # Unit & integration tests
ClientApp/                    # Vue 3 frontend
```

## 📋 API Endpoints

**Authentication:**
```
POST /api/auth/login    # Get JWT token
```

**Customers:**
```
GET /api/customers      # List customers with order totals
```

**Orders:**
```
GET /api/orders/customer/{id}  # Customer's orders
GET /api/orders/{id}/items     # Order details
```

**Discounts:**
```
GET /api/discount/rules        # Available discount rules
```

All endpoints (except login) require JWT: `Authorization: Bearer <token>`

## 🎯 Key Features

**Architecture:**
- ✅ Clean Architecture - clear layer separation
- ✅ Domain-Driven Design - rich domain models
- ✅ Repository Pattern - testable data access
- ✅ Domain Events - decoupled business logic
- ✅ Dependency Injection throughout

**Technical:**
- ✅ TDD with comprehensive test coverage
- ✅ Custom CSV parser with flexible date formats
- ✅ JWT authentication
- ✅ In-memory database (easy to swap)
- ✅ RESTful API design
- ✅ Vue 3 + TypeScript frontend

## 🔧 Technical Highlights

### CSV Import
Custom lightweight parsers supporting multiple date formats (dd/MM/yyyy, yyyy-MM-dd), header detection, and graceful error handling. Built using TDD to demonstrate testing approach.

### Domain Events

When a discount is applied, we raise a `CustomerDiscountApplied` domain event.

**Why model as an event?**
- Discount application is a significant business occurrence that other systems may need to react to
- Decouples the core discount logic from side effects (analytics, notifications, audit logs)
- Keeps domain logic focused and testable

**Evolution with message brokers:**
Currently uses in-memory event dispatcher. Production evolution:
1. Publish `CustomerDiscountApplied` to message broker (RabbitMQ, Azure Service Bus, Kafka)
2. Multiple independent services subscribe to the event
3. Each subscriber processes asynchronously with its own retry/failure handling
4. Horizontal scaling of subscribers without affecting core discount logic

**Real-world subscribers:**
- **Analytics Service**: Update customer lifetime value metrics
- **Notification Service**: Email/SMS customer about discount received
- **Audit Service**: Record discount event for compliance and reporting
- **Billing Service**: Adjust invoice calculations and payment schedules

### Testing
- Unit tests for domain logic and parsers
- Integration tests for API endpoints
- Test-first development approach

### Frontend Integration
Vue 3 SPA with Vite dev proxy for seamless API communication during development.

---

**Author:** Kevin Main  
**Purpose:** Technical Assessment for Clearbusiness
