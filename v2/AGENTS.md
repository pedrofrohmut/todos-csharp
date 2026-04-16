# Todos C# v2

## Build & Run

```bash
# Build
dotnet build Todos.sln

# Run (API on http://localhost:5000)
dotnet run --project src/WebApi/WebApi.csproj

# Database (must be running first)
docker compose up -d
```

## Architecture

- **Core** - Domain layer (entities, use cases, commands, queries, service interfaces). No external deps.
- **Infra** - Infrastructure (PostgreSQL via Npgsql + Dapper, password hashing, JWT)
- **WebApi** - ASP.NET Core controllers and Program.cs entrypoint

Uses Result pattern (no exceptions), CQRS pattern, and Clean/Hexagonal architecture.

## Database

- Primary: `localhost:5432` (user/pass: `write_user`/`write_password`)
- Replica: `localhost:5433` (user/pass: `read_user`/`read_password`)
- Schema in `sql/0001_create_tables.sql`

## Style

- `.editorconfig`: `end_of_line = crlf`, `indent_size = 4`
- .NET 8.0, nullable refs enabled

## Tests

No test projects exist. Do not assume a test framework is configured.
