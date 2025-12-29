# Copilot Instructions: Back End Development (.NET/C#/MSSQL/Azure/Aspire)

Purpose
- Provide consistent prompts and guidance for server-side work in this repository.
- Align with Microsoft guidelines and .NET best practices.

Tech Focus
- .NET (latest LTS), C#, ASP.NET Core (minimal APIs and MVC), EF Core, Dapper (where appropriate), MSSQL, Azure services (App Service, Azure SQL, Storage, Key Vault), Aspire for orchestration.

General Principles
- Prefer clarity, immutability, and dependency injection.
- Follow SOLID, Clean Architecture boundaries, and the Pit of Success.
- Fail fast, log meaningfully, and surface actionable errors.
- Security-by-default: least privilege, no secrets in code, validate all inputs, output encoding, HTTPS everywhere.

Project Setup
- Target LTS of .NET. Enable nullable reference types and implicit usings.
- Enforce analyzers: Microsoft.CodeAnalysis.NetAnalyzers, StyleCop (if used), treat warnings as errors in CI.
- Use `Directory.Packages.props` for package version central management if multiple projects exist.

Domain and Application Layers
- Domain layer is POCOs, value objects, domain services, and events—no external dependencies.
- Application layer contains use cases (CQRS where helpful). MediatR is optional; keep handlers thin.
- Input validation via FluentValidation or minimal custom validators. Fail early with ProblemDetails.

Data Access
- EF Core for standard CRUD/relational aggregates; Dapper for high-throughput read models if needed.
- Configure DbContext pooling, proper connection resiliency, and command timeouts.
- Use `DbContext` per scope; avoid long-lived contexts.
- Migrations: one migrations assembly per bounded context. Never auto-migrate on startup in production; run migrations via CI/CD.
- MSSQL: use proper indexing, SARGable queries, and parameterization. Avoid implicit conversions; use appropriate data types.

Transactions and Concurrency
- Use `TransactionScope` or database transactions for multi-operation consistency.
- Implement optimistic concurrency with rowversion/timestamp where appropriate.

Caching
- Use `IMemoryCache` for node-local caching and `IDistributedCache` (e.g., Redis) for cross-node caching.
- Cache idempotent GETs only; set reasonable expirations and eviction policies.

ASP.NET Core APIs
- Prefer minimal APIs for simple endpoints; controllers when complexity grows.
- Return `IResult`/`ActionResult` with RFC 7807 ProblemDetails on errors.
- Version APIs; document with Swashbuckle/NSwag and include XML comments.
- Enable rate limiting, CORS, HTTPS redirection, HSTS, and security headers.

Logging and Observability
- Use `ILogger<T>`, distributed tracing (OpenTelemetry), and structured logging (Serilog if chosen).
- Correlate with `TraceId`/`SpanId`. Emit metrics via `IMeter`.

Configuration and Secrets
- Use `IOptions<T>` and validation via `ValidateOnStart`.
- Store secrets in Azure Key Vault. Local dev uses user-secrets.
- Keep environment-specific config in `appsettings.{Environment}.json`.

Background Work
- Use `IHostedService`/`BackgroundService` for long-running tasks. Prefer Azure Functions for event-driven workloads.

Azure and Aspire
- Use Azure SQL for data, Storage for blobs/queues, Key Vault for secrets.
- Aspire: define resources, environments, and service-to-service wiring declaratively; prefer service discovery over static endpoints.
- Use Managed Identity for service auth; avoid connection strings with secrets.

Testing
- Unit test domain and application logic (xUnit/NUnit). Use `WebApplicationFactory` for API tests.
- Integration tests spin up MSSQL containers (Testcontainers or Aspire dev harness) and apply migrations.
- Mock external services at boundaries; verify contracts with Pact/HTTP-based contract tests if applicable.

Performance
- Prefer async I/O, pagination, and streaming where sensible.
- Profile critical paths; use indexes; avoid chatty database calls.

Code Review Checklist
- Clear boundaries and DI registrations
- Validation and error handling with ProblemDetails
- Async correctness and cancellation tokens
- Secrets and configuration sourced correctly
- Tests present and meaningful
- Azure resource usage and permissions least-privileged
