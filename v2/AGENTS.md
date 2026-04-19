# Todos C# v2

## Build & Run

```bash
dotnet build Todos.sln
dotnet run --project src/WebApi/WebApi.csproj
```

## Architecture

- **Core** - Domain layer (entities, use cases, commands, queries, service interfaces). No external deps.
- **Infra** - Infrastructure (PostgreSQL via Npgsql + Dapper, password hashing, JWT)
- **WebApi** - ASP.NET Core controllers and Program.cs entrypoint

Uses Result pattern (no exceptions), CQRS pattern, and Clean/Hexagonal architecture.

## Database

- Primary: `localhost:5432` (user/pass: `write_user`/`write_password`)
- Replica: `localhost:5433` (user/pass: `read_user`/`read_password`)
- Schema in `v2/sql/0001_create_tables.sql`
- DB must be running first: `docker compose up -d`

## Style

- `.editorconfig`: `end_of_line = crlf`, `indent_size = 4`
- .NET 8.0, nullable refs enabled

## Development

- Run `dotnet test` - no tests exist, command will succeed with no output
- No lint/typecheck tools configured
- `dotnet build` runs automatically on `dotnet run`
