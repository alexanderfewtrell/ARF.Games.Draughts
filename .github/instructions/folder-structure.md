# Copilot Instructions: Folder Structure

Top-level (.sln root)
- `src/` — application code by bounded context
- `tests/` — test projects mirroring `src`
- `deploy/` — IaC (Bicep/Terraform), pipelines, Aspire app definitions
- `.github/` — workflows and instructions
- `docs/` — architecture decision records, diagrams

Within a bounded context (replace `ContextName` with your bounded context name)
- `ContextName.Api/` — ASP.NET Core API/Blazor/MVC
- `ContextName.Application/` — application layer (use cases, DTOs)
- `ContextName.Domain/` — domain model
- `ContextName.Infrastructure/` — EF Core, MSSQL, integrations
- `ContextName.Workers/` — background services (optional)
- `ContextName.Contracts/` — shared contracts (DTOs, events) if needed

Testing
- `ContextName.Domain.Tests/` — pure unit tests
- `ContextName.Application.Tests/` — use case tests
- `ContextName.Api.Tests/` — integration and endpoint tests (WebApplicationFactory)

Aspire
- `AppHost/` — Aspire orchestrator project
- `ServiceA/`, `ServiceB/` — participating services with Aspire extensions
- `Resources/` — local dev resources (SQL container, storage emulator) and configuration

Data and Migrations
- `migrations/` folder within Infrastructure or a dedicated Migrations project per context.

Docs
- `docs/<iteration-id>/` — per-iteration docs root (e.g., `2025-01-05-draughts-mvp/`)
  - `sor/<sor-slug>/sor.md` — SOR content using the SOR template
  - `specifications/` — technical specifications
  - `plans/` — iteration plans and milestones
  - `designs/` — design notes and diagrams for this iteration
  - `notes/` — meeting notes and ad-hoc docs
- `docs/adr/` — ADRs using MADR format (global, not per-iteration)
- `docs/diagrams/` — shared diagrams (PlantUML/Mermaid)

Rules
- Keep projects small and focused.
- Mirror namespace hierarchy to folder structure.
- Avoid cross-context references except via `Contracts`.
