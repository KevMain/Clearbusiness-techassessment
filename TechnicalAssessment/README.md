# TechnicalAssessment - ASP.NET Core API

The backend API layer built with ASP.NET Core 10.0, exposing RESTful endpoints for the application.

## 🚀 Quick Start

### Command Line
```bash
cd TechnicalAssessment/TechnicalAssessment
dotnet restore
dotnet run
```

The API runs at `https://localhost:5001` (or the port shown in console).

### Visual Studio
1. Open `TechnicalAssessment.slnx`
2. Set `TechnicalAssessment` as startup project
3. Press F5 to run/debug

## 📋 API Endpoints

### Hello World Example
```
GET /api/hello
```

Returns a simple greeting message to verify the API is running.

### Additional Endpoints
*(Document your actual endpoints here as you build them)*

Example:
```
GET /api/customers           # List all customers
GET /api/customers/{id}      # Get customer by ID
POST /api/customers          # Create new customer
PUT /api/customers/{id}      # Update customer
DELETE /api/customers/{id}   # Delete customer

GET /api/orders              # List all orders
POST /api/orders/import      # Import orders from CSV
```

## 🏗️ Project Structure

The API follows Clean Architecture:

- **Controllers**: HTTP request handling (`/Controllers`)
- **Dependency Injection**: Service registration (`Program.cs`)
- **Configuration**: App settings (`appsettings.json`)

Business logic lives in the Application and Domain layers (separate projects).

## ⚙️ Configuration

### Database
- **Development**: In-memory database (no setup required)
- **Production**: Configured via connection string in `appsettings.json`

### CORS
Enabled for local development to allow frontend communication from `http://localhost:5173`.

### Environment Settings
- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development overrides

## 🧪 Testing

Run all tests from solution root:
```bash
dotnet test
```

This includes:
- Unit tests (Domain and Application logic)
- Integration tests (API endpoints with in-memory database)

## 📦 Dependencies

- **ASP.NET Core 10.0**: Web framework
- **Entity Framework Core**: ORM and database access
- **Microsoft.AspNetCore.Cors**: Cross-origin support

---

For full project documentation, see the [main README](../README.md).
