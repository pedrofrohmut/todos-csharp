# Todos C# v2 - QWEN.md

## Project Overview

This is a **Todos API application** built with **C# / .NET 8.0**. It provides a RESTful API for managing users and todo items, including authentication via tokens. The project follows a **Clean Architecture / Hexagonal Architecture** pattern with a clear separation of concerns.

### Architecture

The solution is organized into three main projects under `src/`:

| Project | Purpose |
|---------|---------|
| **Core** | Domain layer containing entities, use cases, commands, queries, services interfaces, and error types. This is the innermost layer with no external dependencies. |
| **Infra** | Infrastructure layer providing concrete implementations of interfaces defined in Core: database connections (PostgreSQL via Npgsql + Dapper), service implementations (password hashing, JWT token handling), and command/query handlers. |
| **WebApi** | Presentation layer - ASP.NET Core Web API with controllers exposing the REST endpoints. |

### Key Domain Concepts

- **Users** - Can sign up, sign in, and authenticate via tokens
- **Todos** - Owned by users; support CRUD operations (create, read, update, delete)
- **Todo Items** - Sub-items within a todo (table defined but not yet implemented)

### Database

- **PostgreSQL** is the database (Npgsql + Dapper)
- Schema defined in `sql/0001_create_tables.sql` with three tables: `users`, `todos`, `todo_items`

### Technology Stack

- **.NET 8.0** (C#)
- **ASP.NET Core Web API**
- **Npgsql** - PostgreSQL ADO.NET provider
- **Dapper** - Micro ORM
- **Nullable Reference Types** enabled
- **Implicit Usings** enabled

## Building and Running

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL database

### Build
```bash
dotnet build Todos.sln
```

### Run
```bash
dotnet run --project src/WebApi/WebApi.csproj
```

### Restore dependencies
```bash
dotnet restore Todos.sln
```

## API Endpoints

| Method | Path | Description | Auth Required |
|--------|------|-------------|---------------|
| POST | `/api/v2/users` | Sign up a new user | No |
| POST | `/api/v2/users/signin` | Sign in and get token | No |
| GET | `/api/v2/users/verify` | Verify auth token | Yes |
| POST | `/api/v2/todos` | Create a todo | Yes |
| GET | `/api/v2/todos` | List all todos for user | Yes |
| GET | `/api/v2/todos/{id}` | Get a todo by ID | Yes |
| PUT | `/api/v2/todos/{id}` | Update a todo | Yes |
| DELETE | `/api/v2/todos/{id}` | Delete a todo | Yes |

## Project Structure

```
v2/
├── Todos.sln                    # Solution file
├── sql/
│   └── 0001_create_tables.sql   # Database schema
└── src/
    ├── Core/                    # Domain layer
    │   ├── Commands/            # Command definitions and handler interfaces
    │   ├── Queries/             # Query definitions and handler interfaces
    │   ├── Entities/            # Domain entities with validation logic
    │   ├── UseCases/            # Application use cases (Todo, User)
    │   ├── Services/            # Service interfaces (IPasswordService, IAuthTokenService)
    │   ├── DB/                  # Database-related types
    │   └── Errors/              # Error types
    ├── Infra/                   # Infrastructure layer
    │   ├── Handlers/            # Concrete command/query handlers
    │   ├── Services/            # Concrete service implementations
    │   ├── DbConnectionManager.cs
    │   ├── InfraConfig.cs
    │   └── UseCasesFactory.cs   # Factory for instantiating use cases
    └── WebApi/                  # Presentation layer
        ├── Controllers/         # API controllers
        ├── Program.cs           # Application entry point
        └── appsettings.json     # Configuration
```

## Development Conventions

- **Result Pattern**: Operations return `Result` or `Result<T>` types to indicate success/failure with error details, rather than throwing exceptions
- **CQRS Pattern**: Commands (write) and Queries (read) are separated with corresponding handler interfaces
- **Use Case Pattern**: Each use case is a class with an `Execute` method that orchestrates the business logic
- **Entity Validation**: Validation logic lives in entity classes (e.g., `TodoEntity.ValidateName`)
- **Nullable Reference Types**: Enabled across all projects
- **Middleware**: Custom timing middleware logs request duration

## Notes

- The `UseCasesFactory` has several TODO comments indicating that database connection management is still a work in progress
- The SQL schema defines tables but column definitions are not yet specified
- Error handling maps errors to appropriate HTTP status codes (400, 401, 404, 500)
