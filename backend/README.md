# Issue Tracker - Backend API

A RESTful API built with .NET 9 and Clean Architecture for managing issues with full CRUD operations, validation, and RFC 9457 compliant error handling.

## Architecture

This project follows **Clean Architecture** principles with the **Repository Pattern** for proper layer separation and dependency inversion:

```
backend/
├── src/
│   ├── IssueTracker.API/          # Presentation Layer (Controllers, Middleware)
│   ├── IssueTracker.Application/  # Business Logic Layer (Services, DTOs, Validation)
│   ├── IssueTracker.Core/         # Domain Layer (Entities, Enums, Interfaces)
│   └── IssueTracker.Infrastructure/ # Data Access Layer (DbContext, Repositories)
```

### Dependency Flow

```
         Core (Abstractions)
              ↑         ↑
              │         │
              │         │
       Application  Infrastructure
       (Services)   (Repositories)
```

- **Application** depends on **Core** interfaces
- **Infrastructure** implements **Core** interfaces
- **API** depends on both **Application** and **Infrastructure** (for DI registration)

### Layer Responsibilities

- **API Layer**: HTTP endpoints, request/response handling, middleware, Swagger configuration, DI container setup
- **Application Layer**: Business logic, service interfaces/implementations, DTOs, AutoMapper profiles, FluentValidation rules
- **Core Layer**: Domain entities, enums, repository interfaces (no dependencies on other layers)
- **Infrastructure Layer**: EF Core DbContext, repository implementations, entity configurations, database migrations

## Tech Stack

- **.NET 9.0** - Latest framework with improved performance
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 9.0** - ORM with SQL Server provider
- **SQL Server 2022** - Relational database (via Docker)
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Declarative validation rules
- **Swashbuckle (Swagger)** - API documentation

## Features

- **RESTful API Design**: Standard HTTP methods and status codes
- **CRUD Operations**: Create, Read, Update, Delete issues
- **Pagination Support**: Efficient paginated responses with metadata (page number, total count, has next/previous)
- **Cancellation Token Support**: All async operations support cancellation for better resource management
- **Status Management**: Open, InProgress, Resolved workflow
- **RFC 9457 Error Handling**: Problem Details for HTTP APIs with full compliance
- **Request Validation**: FluentValidation for DTO validation
- **API Documentation**: Swagger UI with XML comments
- **Clean Architecture**: Repository Pattern with proper dependency inversion
- **Dependency Injection**: Scoped services and repositories with proper lifetime management
- **Distributed Tracing**: Activity-based correlation IDs for debugging

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for SQL Server)
- [Git](https://git-scm.com/)

## Getting Started

### Local Development

**1. Start SQL Server:**

```bash
# From project root
docker-compose up -d 
```

**2. Apply Database Migrations:**

```bash
cd backend
dotnet ef database update --project src/IssueTracker.Infrastructure --startup-project src/IssueTracker.API
```

**3. Run the API locally:**

```bash
cd backend
dotnet run --project src/IssueTracker.API
```

**4. Build the Solution:**

```bash
cd backend
dotnet build IssueTracker.sln
```

The API will be available at:
- **Swagger UI**: http://localhost:5094/swagger
- **API Base**: http://localhost:5094/api

## API Endpoints

### Issues

| Method | Endpoint | Description | Query Parameters |
|--------|----------|-------------|------------------|
| GET | `/api/issues` | Get paginated issues | `status` (Open/InProgress/Resolved), `pageNumber` (default: 1), `pageSize` (default: 20, max: 100) |
| GET | `/api/issues/{id}` | Get issue by ID | - |
| POST | `/api/issues` | Create new issue | - |
| PUT | `/api/issues/{id}` | Update issue | - |
| PATCH | `/api/issues/{id}/resolve` | Mark issue as resolved | - |
| DELETE | `/api/issues/{id}` | Delete issue | - |

### Pagination

All list endpoints return paginated results with the following structure:

```json
{
  "items": [...],
  "pageNumber": 1,
  "pageSize": 20,
  "totalCount": 157,
  "totalPages": 8,
  "hasPrevious": false,
  "hasNext": true
}
```

### Request/Response Examples

#### Get Paginated Issues

**Request:**
```http
GET /api/issues?status=Open&pageNumber=1&pageSize=20
```

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": 1,
      "title": "Fix login bug",
      "description": "Users cannot log in with special characters in password",
      "status": "Open",
      "createdAt": "2025-11-20T03:46:11.084291Z",
      "resolvedAt": null
    },
    {
      "id": 2,
      "title": "Update documentation",
      "description": "API documentation needs to be updated",
      "status": "Open",
      "createdAt": "2025-11-20T04:15:22.123456Z",
      "resolvedAt": null
    }
  ],
  "pageNumber": 1,
  "pageSize": 20,
  "totalCount": 42,
  "totalPages": 3,
  "hasPrevious": false,
  "hasNext": true
}
```

#### Create Issue

**Request:**
```json
POST /api/issues
{
  "title": "Fix login bug",
  "description": "Users cannot log in with special characters in password"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "title": "Fix login bug",
  "description": "Users cannot log in with special characters in password",
  "status": "Open",
  "createdAt": "2025-11-20T03:46:11.084291Z",
  "resolvedAt": null
}
```

#### Error Response (RFC 9457)

**Request:**
```json
GET /api/issues/99999
```

**Response (404 Not Found):**
```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
  "title": "Resource Not Found",
  "status": 404,
  "detail": "Issue with id '99999' was not found",
  "instance": "/api/issues/99999",
  "traceId": "0HNH7TLRDFVB3:00000001",
  "requestId": "00-6f35c30a9e2f7907d60605ed99cf6b77-e17e346bc6468fa6-00",
  "timestamp": "2025-11-20T04:29:59.277285Z",
  "method": "GET"
}
```

## Project Structure

```
backend/
├── IssueTracker.sln
├── src/
│   ├── IssueTracker.API/
│   │   ├── Controllers/
│   │   │   └── IssuesController.cs          # REST endpoints
│   │   ├── Middleware/
│   │   │   └── GlobalExceptionHandler.cs    # RFC 9457 error handling
│   │   └── Program.cs                       # DI, middleware configuration
│   │
│   ├── IssueTracker.Application/
│   │   ├── DTOs/
│   │   │   ├── IssueDto.cs                  # Response DTO
│   │   │   ├── CreateIssueDto.cs            # Creation DTO
│   │   │   └── UpdateIssueDto.cs            # Update DTO
│   │   ├── Exceptions/
│   │   │   ├── NotFoundException.cs         # 404 exception
│   │   │   └── BadRequestException.cs       # 400 exception
│   │   ├── Interfaces/
│   │   │   └── IIssueService.cs             # Service contract
│   │   ├── Mappings/
│   │   │   └── MappingProfile.cs            # AutoMapper configuration
│   │   ├── Services/
│   │   │   └── IssueService.cs              # Business logic
│   │   └── Validators/
│   │       ├── CreateIssueDtoValidator.cs   # Creation validation
│   │       └── UpdateIssueDtoValidator.cs   # Update validation
│   │
│   ├── IssueTracker.Core/
│   │   ├── Common/
│   │   │   └── PagedResult.cs               # Pagination wrapper
│   │   ├── Entities/
│   │   │   └── Issue.cs                     # Domain entity
│   │   ├── Enums/
│   │   │   └── IssueStatus.cs               # Status enumeration
│   │   └── Interfaces/
│   │       └── IIssueRepository.cs          # Repository contract
│   │
│   └── IssueTracker.Infrastructure/
│       ├── Configurations/
│       │   └── IssueConfiguration.cs        # Fluent API config
│       ├── Data/
│       │   └── IssueTrackerDbContext.cs     # EF Core context
│       ├── Repositories/
│       │   └── IssueRepository.cs           # Repository implementation
│       └── Migrations/                      # EF Core migrations
```

## Database Schema

### Issues Table

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| Title | nvarchar(200) | Not Null |
| Description | nvarchar(max) | Not Null |
| Status | int | Not Null, Default: 1 (Open) |
| CreatedAt | datetime2 | Not Null, Default: GETUTCDATE() |
| ResolvedAt | datetime2 | Nullable |

**Indexes:**
- `IX_Issues_Status` on Status column
- `IX_Issues_CreatedAt` on CreatedAt column (DESC)

## Configuration

### appsettings.Development.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=IssueTrackerDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  },
  "CorsSettings": {
    "AllowedOrigins": [
      "http://localhost:5173",
      "http://localhost:3000"
    ]
  }
}
```

### CORS Configuration

The API is configured to accept requests from:
- `http://localhost:5173` (Vite default)
- `http://localhost:3000` (React default)

Modify your appsettings.{Enviroment}.json or add env variables for additional origins.

## RFC 9457 Error Handling

The API implements **RFC 9457: Problem Details for HTTP APIs** for standardized error responses.

### Problem Types

All error responses include a `type` field referencing RFC 9110 sections:

- **404 Not Found**: `https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5`
- **400 Bad Request**: `https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1`
- **500 Internal Server Error**: `https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1`

### Standard Fields

- **type**: URI reference identifying the problem type
- **title**: Short, human-readable summary
- **status**: HTTP status code (matches response status)
- **detail**: Explanation specific to this occurrence
- **instance**: URI reference identifying this specific occurrence

### Extension Members (Diagnostics)

- **traceId**: `HttpContext.TraceIdentifier` for request correlation
- **requestId**: `Activity.Current.Id` for distributed tracing
- **timestamp**: UTC timestamp when error occurred
- **method**: HTTP method of the request

### Implementation

Uses `IExceptionHandler` interface (.NET 8+) for centralized exception handling:

```csharp
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

app.UseExceptionHandler(); // Must be early in pipeline
```

## Validation Rules

### Create Issue

- **Title**: Required, max 200 characters, cannot be whitespace
- **Description**: Required, max 2000 characters, cannot be whitespace

### Update Issue

- **Title**: Optional, max 200 characters if provided
- **Description**: Optional, max 2000 characters if provided
- **Status**: Optional, must be valid enum value (Open=1, InProgress=2, Resolved=3)

## Development Commands

```bash
# Build solution
dotnet build

# Run API
dotnet run --project src/IssueTracker.API

# Create new migration
dotnet ef migrations add MigrationName --project src/IssueTracker.Infrastructure --startup-project src/IssueTracker.API

# Apply migrations
dotnet ef database update --project src/IssueTracker.Infrastructure --startup-project src/IssueTracker.API

# Remove last migration
dotnet ef migrations remove --project src/IssueTracker.Infrastructure --startup-project src/IssueTracker.API
```

## Architecture Decisions

### Why Repository Pattern?

While EF Core's `DbContext` provides repository-like functionality, we use an explicit Repository Pattern for:

**Clean Architecture Compliance:**
- Application layer depends on abstractions (Core), not concrete implementations (Infrastructure)
- Follows Dependency Inversion Principle (depend on abstractions, not concretions)
- True layer independence - can swap data providers without touching business logic

**Benefits:**
- **Testability**: Easy to mock `IIssueRepository` interface for unit tests
- **Flexibility**: Can swap EF Core for Dapper, MongoDB, or any other data store
- **Separation of Concerns**: Data access logic isolated from business logic
- **Maintainability**: Changes to data access don't affect service layer

**Implementation:**
```csharp
// Core layer defines the contract
public interface IIssueRepository { ... }

// Infrastructure implements it with EF Core
public class IssueRepository : IIssueRepository { ... }

// Application depends on abstraction
public class IssueService
{
    private readonly IIssueRepository _repository; // Not DbContext!
}
```

### Why Service Layer?

The Service Layer pattern provides:
- Business logic encapsulation
- Separation from data access concerns
- Easier unit testing
- Transaction management
- Consistent error handling

### Why AutoMapper?

AutoMapper provides:
- Convention-over-configuration mapping
- Reduced boilerplate code
- Centralized mapping logic
- Type-safe transformations

### Why RFC 9457?

RFC 9457 (published July 2023) obsoletes RFC 7807 with improvements:
- Registry of common problem type URIs
- Clearer guidance on multiple problems
- Better support for distributed tracing
- Stronger recommendations for non-resolvable URIs

## CORS Policy

The API includes CORS configuration for frontend development:

```csharp
var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});
```

## Database Connection

Connection string in `appsettings.Development.json`:

```
Server=localhost,1433;Database=IssueTrackerDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;
```

**Note**: Change password in production and use User Secrets or Azure Key Vault for secure storage.

## Troubleshooting

### Cannot connect to SQL Server

```bash
# Check if SQL Server container is running
docker ps

# View SQL Server logs
docker logs sqlserver

# Restart SQL Server
docker-compose restart
```

### Port 5094 already in use

Edit `src/IssueTracker.API/Properties/launchSettings.json` and change the port number.

### EF Core migrations fail

```bash
# Ensure you're in the backend directory
cd backend

# Drop and recreate database
dotnet ef database drop --project src/IssueTracker.Infrastructure --startup-project src/IssueTracker.API
dotnet ef database update --project src/IssueTracker.Infrastructure --startup-project src/IssueTracker.API
```

## Testing the API

### Using cURL

```bash
# Get paginated issues (default: page 1, size 20)
curl http://localhost:5094/api/issues

# Get specific page with custom size
curl "http://localhost:5094/api/issues?pageNumber=2&pageSize=10"

# Filter by status with pagination
curl "http://localhost:5094/api/issues?status=Open&pageNumber=1&pageSize=20"

# Get specific issue
curl http://localhost:5094/api/issues/1

# Create issue
curl -X POST http://localhost:5094/api/issues \
  -H "Content-Type: application/json" \
  -d '{"title":"Test Issue","description":"Testing the API"}'

# Update issue
curl -X PUT http://localhost:5094/api/issues/1 \
  -H "Content-Type: application/json" \
  -d '{"title":"Updated Title","status":2}'

# Resolve issue
curl -X PATCH http://localhost:5094/api/issues/1/resolve

# Delete issue
curl -X DELETE http://localhost:5094/api/issues/1
```

### Using Swagger UI

Navigate to http://localhost:5094/swagger to access interactive API documentation with built-in testing capabilities.

## References

- [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/rfc9457/)
- [RFC 9110: HTTP Semantics](https://datatracker.ietf.org/doc/html/rfc9110)
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [.NET 9 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/overview)
- [Entity Framework Core 9 Documentation](https://learn.microsoft.com/en-us/ef/core/)

## License

This project is part of a technical assessment.
