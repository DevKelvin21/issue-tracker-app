# Issue Tracker Application

A full-stack issue tracking application demonstrating modern software engineering practices with Clean Architecture, enterprise patterns, and production-ready features.

## Overview

This project showcases a complete issue management system built with:
- **Backend**: .NET 9 REST API following Clean Architecture principles
- **Frontend**: React 19 + TypeScript with Container/Presentation patterns
- **Infrastructure**: Docker-based SQL Server deployment

The application implements industry best practices including layered architecture, dependency inversion, type-safe validation, RFC 9457 compliant error handling, and comprehensive API documentation.

## Technology Stack

### Backend
- .NET 9 with ASP.NET Core
- Entity Framework Core 9 with SQL Server
- AutoMapper, FluentValidation, Swagger/OpenAPI
- Repository Pattern with Clean Architecture

### Frontend
- React 19 with TypeScript
- Vite, React Query (TanStack), React Hook Form
- Tailwind CSS, Zod validation, Axios
- Container/Presentation architecture with custom hook composition

### Infrastructure
- Docker & Docker Compose
- SQL Server 2022
- CORS-enabled API for local development

## Project Structure

```
issue-tracker-app/
├── backend/                    # .NET 9 REST API
│   ├── src/
│   │   ├── IssueTracker.API/              # Controllers, middleware
│   │   ├── IssueTracker.Application/      # Business logic, services
│   │   ├── IssueTracker.Core/             # Domain entities, interfaces
│   │   └── IssueTracker.Infrastructure/   # Data access, repositories
│   └── README.md              # Backend documentation
│
├── frontend/                   # React + TypeScript SPA
│   ├── src/
│   │   ├── components/ui/     # Reusable UI component library
│   │   ├── features/issues/   # Issue feature module
│   │   ├── hooks/             # Shared custom hooks
│   │   ├── lib/               # API client, utilities
│   │   └── types/             # TypeScript definitions
│   └── README.md              # Frontend documentation
│
└── docker-compose.yml          # SQL Server container
```

## Quick Start

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 20.19+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Setup

**1. Clone the repository:**
```bash
git clone <repository-url>
cd issue-tracker-app
```

**2. Start the database:**
```bash
docker-compose up -d
```

**3. Start the backend:**
```bash
cd backend
dotnet ef database update --project src/IssueTracker.Infrastructure --startup-project src/IssueTracker.API
dotnet run --project src/IssueTracker.API
```

The API will be available at:
- Swagger UI: http://localhost:5094/swagger
- API endpoint: http://localhost:5094/api

**4. Start the frontend:**
```bash
cd frontend
npm install
npm run dev
```

The frontend will be available at http://localhost:5173

## Key Features

### Full CRUD Operations
Create, read, update, and delete issues with complete validation and error handling.

### Status Workflow Management
Track issues through their lifecycle: Open → In Progress → Resolved.

### Pagination Support
Efficient data loading with server-side pagination including metadata (total count, page info, navigation flags).

### Advanced Error Handling
RFC 9457 compliant Problem Details responses with distributed tracing support (correlation IDs, request IDs).

### Type Safety Throughout
- Backend: C# with strict null checks and comprehensive validation
- Frontend: TypeScript strict mode with runtime validation via Zod
- API contract: Swagger/OpenAPI specification

### Professional Architecture
- **Backend**: Clean Architecture with Repository Pattern and dependency inversion
- **Frontend**: Feature-based organization with Container/Presentation separation
- **Separation of concerns** across all layers

## Architecture Highlights

### Backend Architecture
The backend follows Clean Architecture with clear dependency flow:
```
Core (Domain) ← Application (Services) ← API (Controllers)
      ↑
Infrastructure (Repositories)
```

Key patterns:
- Repository Pattern for data access abstraction
- Service Layer for business logic
- FluentValidation for declarative validation rules
- Global exception handling with RFC 9457 Problem Details
- Cancellation token support for async operations

See [backend/README.md](backend/README.md) for detailed documentation.

### Frontend Architecture
The frontend implements enterprise patterns:
```
UI Components → Presentation Views → Containers → Hooks → API Client
```

Key patterns:
- Container/Presentation component separation
- Custom hook composition for business logic
- Feature-based modular organization
- Centralized API client with interceptors
- React Query for server state management

See [frontend/README.md](frontend/README.md) for detailed documentation.

## API Documentation

Interactive API documentation is available via Swagger UI at http://localhost:5094/swagger when the backend is running.

Key endpoints:
- `GET /api/issues` - Get paginated issues with optional status filter
- `POST /api/issues` - Create new issue
- `PUT /api/issues/{id}` - Update issue
- `PATCH /api/issues/{id}/resolve` - Mark as resolved
- `DELETE /api/issues/{id}` - Delete issue

All endpoints support cancellation tokens and return RFC 9457 compliant error responses.

## Development

### Running Tests
```bash
# Backend (when implemented)
cd backend
dotnet test

# Frontend (when implemented)
cd frontend
npm test
```

### Database Migrations
```bash
cd backend

# Create migration
dotnet ef migrations add <MigrationName> --project src/IssueTracker.Infrastructure --startup-project src/IssueTracker.API

# Apply migrations
dotnet ef database update --project src/IssueTracker.Infrastructure --startup-project src/IssueTracker.API
```

### Environment Configuration

**Backend** (`backend/src/IssueTracker.API/appsettings.Development.json`):
- Connection strings
- CORS allowed origins
- Logging levels

**Frontend** (`frontend/.env`):
```env
VITE_API_BASE_URL=http://localhost:5094/api
```

## Design Decisions

### Why Clean Architecture?
Enforces strict separation of concerns with dependency inversion. The domain layer (Core) has zero dependencies, while outer layers depend on abstractions, making the system highly testable and maintainable.

### Why Repository Pattern?
Even with EF Core's DbContext, explicit repositories provide:
- True abstraction over data access
- Easy mocking for unit tests
- Flexibility to swap data providers
- Clean Architecture compliance

### Why Container/Presentation Pattern?
Separates business logic (containers) from UI rendering (presentations), improving:
- Component reusability
- Testing (test logic and UI separately)
- Code maintainability
- Team collaboration (logic vs UI development)

### Why RFC 9457?
Standardized error responses improve:
- API consistency
- Client error handling
- Debugging with traceIds
- HTTP semantic compliance

## Contributing

Both the backend and frontend have detailed contribution guidelines in their respective README files:
- [Backend Contributing Guidelines](backend/README.md#architecture-decisions)
- [Frontend Contributing Guidelines](frontend/README.md#contributing)

## Project Status

This is a demonstration project showcasing modern full-stack development practices. It includes:
- ✅ Complete CRUD operations
- ✅ Clean Architecture implementation
- ✅ Type-safe frontend with validation
- ✅ RFC 9457 error handling
- ✅ Pagination support
- ✅ API documentation
- ⏳ Unit tests (planned)
- ⏳ Integration tests (planned)

## License

This project is part of a technical assessment.
