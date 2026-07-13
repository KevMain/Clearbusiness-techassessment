# Technical Assessment - Crib Sheet

## Quick Overview
Full-stack .NET 10 application with Vue 3 frontend. Demonstrates Clean Architecture, DDD, and TDD. Features customer/order management with CSV import and discount system.

---

## Architecture Deep Dive

### Clean Architecture - 3 Layers

**1. Domain Layer** (`TechnicalAssessment.Domain`)
- **What**: Core business entities and logic
- **No dependencies** on other layers or frameworks
- Contains: `Customer`, `Order`, `OrderItem` entities
- **Why**: Keeps business rules isolated and testable

**2. Infrastructure Layer** (`TechnicalAssessment.Infrastructure`)
- **What**: Technical implementations (database, CSV parsing)
- **Depends on**: Domain layer only
- Contains: CSV parsers, in-memory data store, repositories
- **Why**: Can swap implementations without touching business logic

**3. API Layer** (`TechnicalAssessment/TechnicalAssessment`)
- **What**: HTTP endpoints, authentication, DI setup
- **Depends on**: Domain + Infrastructure
- Contains: Controllers, Program.cs, JWT config
- **Why**: Entry point, orchestrates everything

### Data Flow Example
```
HTTP Request → Controller → Repository (Infrastructure) → Domain Entity → Response
```

---

## Key Components Explained

### CSV Parsing
**Location**: `TechnicalAssessment.Infrastructure`

**How it works:**
1. `CustomerCsvParser` and `OrderCsvParser` implement `IRecordParser<T>`
2. Parse one line at a time: `string.Split(',')` 
3. Trim whitespace, validate required fields
4. Return domain object or `null` if invalid

**Date parsing supports:**
- `dd/MM/yyyy` (25/12/2023)
- `d/M/yyyy` (5/3/2023)
- `yyyy-MM-dd` (2023-12-25)
- Falls back to `DateTime.TryParse`

**Design decision:**
- Custom parser vs CsvHelper library
- **Why custom**: Demonstrate TDD from scratch, full control, minimal dependencies
- **Trade-off**: Doesn't handle quoted commas - documented for production upgrade

### Domain Events
**Location**: `TechnicalAssessment.Domain/Events`

**Example**: `CustomerDiscountApplied`
```csharp
public class CustomerDiscountApplied : IDomainEvent
{
	public string CustomerId { get; }
	public decimal DiscountAmount { get; }
}
```

**How it works:**
1. Discount logic applies to customer
2. Raises `CustomerDiscountApplied` event
3. In-memory dispatcher notifies subscribers
4. Decouples discount logic from side effects

**Why use events:**
- Other systems can react (analytics, notifications)
- Core logic stays focused
- Production path: RabbitMQ, Azure Service Bus

### Authentication
**Type**: JWT (JSON Web Tokens)

**Flow:**
1. POST `/api/auth/login` with credentials
2. Backend generates JWT signed with secret key
3. Client stores token
4. Client sends `Authorization: Bearer <token>` header
5. Backend validates signature and expiration

**Configuration**: `appsettings.json`
```json
"Jwt": {
  "Key": "secret-key",
  "Issuer": "TechnicalAssessment",
  "Audience": "TechnicalAssessment"
}
```

### In-Memory Database
**Why in-memory:**
- No setup required for reviewers
- Fast for demos
- Easy to swap for SQL Server (change one line in Program.cs)

**Trade-off**: Data lost on restart (acceptable for assessment)

---

## Design Patterns Used

### 1. Repository Pattern
**Interface**: `ICustomerRepository`, `IOrderRepository`
**Implementation**: In-memory (could be EF Core, Dapper, etc.)

**Why:**
- Abstracts data access
- Testable (mock repositories in tests)
- Swappable persistence

### 2. Dependency Injection
**Everything** injected via interfaces in `Program.cs`:
```csharp
builder.Services.AddSingleton<IDataStore, InMemoryDataStore>();
```

**Why:**
- Loose coupling
- Testability
- Easy to change implementations

### 3. Domain Events
(See above)

### 4. SOLID Principles

**S - Single Responsibility**
- Each class has one job (e.g., `CustomerCsvParser` only parses)

**O - Open/Closed**
- `IRecordParser<T>` open for extension (new parsers), closed for modification

**L - Liskov Substitution**
- Any `IRecordParser<Customer>` implementation can replace another

**I - Interface Segregation**
- Small, focused interfaces (`IRecordParser`, `ICustomerRepository`)

**D - Dependency Inversion**
- High-level modules depend on abstractions (interfaces), not concrete classes

---

## Testing Approach

### Test-Driven Development (TDD)
**Process:**
1. Write failing test first
2. Write minimal code to pass
3. Refactor

**Example**: CSV parser tests written before parser implementation

### Test Coverage

**Unit Tests** (`TechnicalAssessment.Domain.Tests`)
- CSV parsing: valid/invalid rows, date formats, headers
- Domain logic: discount calculations, validation

**Integration Tests** (`TechnicalAssessment.Integration.Tests`)
- API endpoints with in-memory database
- Full request/response cycle

**Run tests**: `dotnet test`

---

## Technical Decisions & Justifications

### Why .NET 10?
- Latest version (2026)
- Performance improvements
- Modern C# features

### Why Clean Architecture?
- **Testability**: Domain logic has no dependencies
- **Maintainability**: Clear boundaries
- **Flexibility**: Swap implementations easily

### Why custom CSV parser?
- Demonstrate TDD skill
- Full control over logic
- For production: migrate to CsvHelper (documented)

### Why Vue 3?
- Modern, reactive framework
- TypeScript support
- Fast with Vite

### Why JWT?
- Stateless authentication
- Scales horizontally
- Industry standard

### Why in-memory database?
- Easy reviewer setup
- Fast for assessment
- Production path clear (connection string swap)

---

## Likely Interview Questions & Answers

### "Walk me through the architecture"
"It's Clean Architecture with 3 layers: Domain (business logic), Infrastructure (technical implementations), and API. Domain has zero dependencies - it's pure business rules. Infrastructure implements things like data access and CSV parsing. API layer ties it together with controllers and DI. This keeps business logic testable and decoupled from frameworks."

### "Why Clean Architecture over N-tier?"
"Clean Architecture puts business logic at the center with no dependencies. N-tier often couples business logic to database frameworks. My approach means I can test domain logic without a database, and swap implementations without touching business rules."

### "How does CSV import work?"
"Custom parsers implementing `IRecordParser<T>`. Each parser reads a CSV line, splits on comma, validates fields, and returns a domain object or null. Supports multiple date formats. I chose custom over CsvHelper to demonstrate TDD - tests were written first for each parsing scenario."

### "What would you change for production?"
"1. Real database (SQL Server) with connection strings. 2. CsvHelper for complex CSV scenarios. 3. Domain events to message broker (RabbitMQ/Azure Service Bus). 4. Better error handling with Result<T, Error> instead of null. 5. Logging (Serilog). 6. API versioning. 7. Health checks."

### "Tell me about your testing strategy"
"TDD approach. Unit tests for domain logic and parsers - written first, then implementation. Integration tests for API endpoints with in-memory database. Tests verify business rules, parsing edge cases, and full request cycles. 90%+ coverage on domain layer."

### "How does authentication work?"
"JWT-based. Client POSTs credentials to /api/auth/login. Backend validates and returns signed JWT. Client includes token in Authorization header. Backend middleware validates signature and expiration. Stateless - scales horizontally."

### "What are domain events?"
"They decouple business logic from side effects. Example: when a discount is applied, we raise `CustomerDiscountApplied` event. Other systems can subscribe - analytics, notifications, audit logs. Currently in-memory dispatcher. Production evolution: publish to message broker for true decoupling and scalability."

### "Why dependency injection everywhere?"
"Makes code testable and loosely coupled. Controllers don't `new` up dependencies - they receive interfaces. I can mock repositories in tests, swap implementations without code changes. All wired up in Program.cs."

### "How would you handle concurrent CSV uploads?"
"Current in-memory store isn't thread-safe. Production: 1. Use database with transactions. 2. Queue-based processing (Azure Queue/RabbitMQ). 3. Background service processes uploads. 4. Return job ID to client for status polling. 5. Handle duplicate detection."

### "What's your favorite part of this codebase?"
"The CSV parser tests. They demonstrate TDD clearly - each test describes a scenario (valid dates, missing fields, headers) and the parser evolved to handle them. Clean, readable, and proves the code works."

---

## Project Statistics
- **Lines of code**: ~2000 (excluding tests)
- **Test projects**: 2 (Domain.Tests, Integration.Tests)
- **Test coverage**: High on domain layer
- **Dependencies**: Minimal (ASP.NET Core, no unnecessary packages)

---

## Quick Demo Flow
1. Start backend: `cd TechnicalAssessment/TechnicalAssessment && dotnet run`
2. Start frontend: `cd ClientApp && npm run dev`
3. Open `http://localhost:5173`
4. Show API: `https://localhost:5001/swagger` (if enabled)
5. Run tests: `dotnet test`

---

## If They Ask for Code Walkthrough

**Start here**: `Program.cs`
- DI setup
- JWT configuration
- Middleware pipeline

**Then**: `Controllers/CustomersController.cs`
- How requests flow
- Repository usage

**Then**: `Domain/Customer.cs`
- Rich domain model
- Business rules

**Then**: `Infrastructure/CustomerCsvParser.cs`
- TDD example
- Parsing logic

**Then**: `Domain.Tests/CustomerCsvParserTests.cs`
- Test coverage
- TDD evidence

---

## Red Flags to Avoid
❌ "I followed a tutorial" - emphasize your design decisions
❌ "I didn't have time to test" - you have comprehensive tests
❌ "It's just CRUD" - it's Clean Architecture with DDD and events
❌ Saying "I don't know" - walk through the code if stuck

✅ "I made this design decision because..."
✅ "I chose TDD to demonstrate..."
✅ "For production I would..."
✅ "Let me walk you through the code..."

---

## Key Talking Points
1. **TDD First**: Tests written before implementation
2. **Clean Architecture**: Business logic independent of frameworks
3. **Domain Events**: Production-ready event-driven patterns
4. **SOLID Throughout**: Can explain each principle with code examples
5. **Production Thinking**: Every design has documented evolution path

Good luck! 🚀
