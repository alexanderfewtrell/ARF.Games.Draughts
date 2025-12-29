# Copilot Instructions: File Naming

General
- Use PascalCase for C# types and files containing a single type: `OrderService.cs`, `PlayerController.cs`.
- Use kebab-case for markdown and docs: `architecture-decision-record.md`.
- Use snake_case for database artifacts when required by convention; otherwise PascalCase for EF Core entity properties.

Projects and Assemblies
- Project names match folders and root namespaces: `ContextName.Api`, `ContextName.Domain`.
- Test projects end with `.Tests`: `ContextName.Domain.Tests`.

C# Conventions
- Interfaces: `IThing` (file `IThing.cs`).
- Enums: singular names, file `CardSuit.cs`.
- Partial classes: `Component.razor` + `Component.razor.cs` for Blazor.
- Records: `PlayerRecord.cs` or value-based type file name matches type name.
- DTOs end with `Dto` only when needed; prefer meaningful names: `CreateGameRequest`, `GameSummary`.

ASP.NET Core
- Controllers: `PlayersController.cs`.
- Middleware: `RequestTimingMiddleware.cs`.
- Minimal APIs: group endpoints in `PlayersEndpoints.cs` or per feature `Players.Get.cs`/`Players.Post.cs` if splitting.

Data and Migrations
- EF Core migrations: `yyyyMMddHHmmss_Description.cs`.
- SQL files: kebab-case with verb-noun: `create-tables.sql`, `seed-data.sql`.

Front End
- Razor components: `GameBoard.razor` (+ `.razor.css`/`.razor.cs`).
- React: `GameBoard.tsx`, hooks `useGameState.ts`.
- Assets: kebab-case: `game-board.png`, `site.css`.

Azure and Aspire
- Bicep/Terraform: kebab-case with scope prefix: `rg-app.bicep`, `sql-server.bicep`.
- Aspire resources: clear service names: `AppHost`, `ContextName.Api`, `ContextName.Infrastructure`.

Do Not
- Avoid spaces and mixed casing within the same category.
- Avoid abbreviations unless widely known (e.g., `Dto`, `Api`).
