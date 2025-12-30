# Project Build Plan Template

> Basis: Statement of Requirements (SOR), Technical Specifications, and `copilot-instructions.md`
> Purpose: Provide a structured, trackable plan for Copilot to build the application.

## Metadata
- Project Name: ARF.Games.Draughts
- Iteration ID: docs/001-mvp
- Repository/Path: C:\Repos\ARF.Games
- Orchestration/Platform: Blazor Server (desktop-hosted), Minimal APIs (in-process), No DB
- Primary Stack: .NET 10 / C# / Blazor Server / Minimal APIs / No persistence
- Date: 2025-12-30
- Owner: A. Fewtrell

## References
- SOR: docs/001-mvp/SOR/draughts-sor.md
- Technical Specs:
  - docs/001-mvp/specs/overview-spec.md
  - docs/001-mvp/specs/domain-rules-engine-spec.md
  - docs/001-mvp/specs/backend-api-spec.md
  - docs/001-mvp/specs/frontend-ui-spec.md
  - docs/001-mvp/specs/observability-spec.md
- Copilot Instructions Index: `.github/copilot-instructions.md`
- Architecture Guide: `instructions/architecture.md`
- Folder Structure: `instructions/folder-structure.md`
- File Naming: `instructions/file-naming.md`
- Backend Guide: `instructions/backend.md`
- Frontend Guide: `instructions/frontend.md`
- Docs Workflow: `instructions/docs-workflow.md`

## High-Level Overview
- Goals:
  - Deliver a playable browser game vs basic AI using Spanish rules (SOR OBJ1).
  - Provide clear rule guidance with move highlights and mandatory capture prompts (SOR OBJ2).
  - Communicate outcomes and basic game status (SOR OBJ3).
- Scope:
  - In-scope: Single-player vs AI; 8x8 Spanish rules (mandatory capture, flying kings); UI board and movement; move highlights and prompts; status and outcomes; restart/new game (SOR S1-S5, S10; specs overview AD1-AD5).
  - Out-of-scope: Multiplayer; accounts/profiles/persistence/leaderboards; multiple AI levels; tutorial/hints/undo/save (SOR S6-S9).
- Non-Goals: Any persistence, identity, or cloud deployment; advanced AI beyond basic legal-move selection.
- Assumptions: Desktop-hosted Blazor Server; in-memory state; single difficulty AI; no external services (overview-spec SC7, SC8).
- Risks: Rules ambiguity (RA1); AI quality/frustration risk (RA2). Mitigate via examples/tests and setting expectations.
- Deliverables: Blazor Server app; domain rules engine; minimal API for AI move; UI components; unit/integration/E2E tests; minimal console logging; build instructions.
- Milestones: MVP Specification, Core Gameplay, AI Integration, MVP Validation (SOR TL1-TL4).

---

## Work Item

### [ ] Work Item: Repository & Solution Setup
Description: Initialize solution and projects following Clean Architecture and repo conventions.
Definition of Done: Solution builds successfully; baseline tests run; folder structure matches guidance.
Dependencies: `.github/` instruction set; no code dependencies.

#### Tasks

---

- [ ] Task 1: Create solution and projects
- Description: Scaffold projects per `folder-structure.md` with Blazor Server host and domain/application libraries.
- Inputs: `.github/instructions/folder-structure.md`, `.github/instructions/backend.md`, `.github/instructions/file-naming.md`
- Outputs: `src/Draughts.Api` (Blazor Server), `src/Draughts.Domain`, `src/Draughts.Application`, `src/Draughts.Infrastructure` (placeholder), tests under `tests/*`
- Acceptance: `dotnet build` succeeds; baseline test project executes.

Steps:
- [ ] Create `.sln` and projects with correct target (net8.0) and nullable enabled
- [ ] Add references: Api -> Application -> Domain; Api -> Infrastructure
- [ ] Add analyzers and treat warnings as errors in CI profile
- [ ] Commit scaffold; run `dotnet build` and fix issues

---

- [ ] Task 2: Configure Blazor Server host and minimal API area
- Description: Set up `Program.cs` for Blazor Server with endpoint group `/api` for AI.
- Inputs: `instructions/backend.md`, `docs/001-mvp/specs/backend-api-spec.md`
- Outputs: Minimal `Program.cs`, placeholder endpoints, `appsettings.Development.json` logging config
- Acceptance: App starts; `/` renders placeholder board page; `/api/health` returns 200.

Steps:
- [ ] Add Blazor Server services and Razor components
- [ ] Add minimal API route group `/api` and `/api/health`
- [ ] Configure HTTPS redirection, HSTS (dev-appropriate), CORS (if needed)
- [ ] Run and validate locally

---

## Work Item

### [ ] Work Item: Domain Rules Engine
Description: Implement Spanish draughts rules for movement, captures (mandatory), kinging, end-game.
Definition of Done: Unit tests cover legal moves, captures, kinging, end-game; API/UI can query legal moves and apply moves.
Dependencies: Repository & Solution Setup.

#### Tasks

---

- [ ] Task 1: Define domain models and interfaces
- Description: Create `Board`, `Piece`, `Move`, `Player`, `Result` and `IRulesEngine`.
- Inputs: `docs/001-mvp/specs/domain-rules-engine-spec.md` (AD2-AD4)
- Outputs: Domain types and `IRulesEngine` interface with methods described in spec
- Acceptance: Compiles; unit test project references domain.

Steps:
- [ ] Add domain entities/value objects with immutability where practical
- [ ] Define `IRulesEngine` with `GetLegalMoves`, `ApplyMove`, `IsGameOver`
- [ ] Add guards/validation for board states
- [ ] Build solution

---

- [ ] Task 2: Implement move generation and rules
- Description: Implement legal move generation for men and kings with mandatory capture and multi-capture.
- Inputs: SOR FR2, domain spec AD3; Spanish rules references (implicit in SOR)
- Outputs: Rules engine implementation with pure/side-effect-free functions where possible
- Acceptance: Unit tests pass for movement, capture priority, kinging, multi-capture.

Steps:
- [ ] Implement movement for men and kings (flying kings)
- [ ] Enforce mandatory capture and capture chains
- [ ] Implement kinging on reaching last rank
- [ ] Add `IsGameOver` detection (no moves, captures exhausted)
- [ ] Write unit tests (xUnit) per scenarios; run tests and fix failures

---

## Work Item

### [ ] Work Item: Backend API (AI Move)
Description: Provide endpoint to compute AI move based on current board; optional validate endpoint.
Definition of Done: `POST /api/ai/move` returns a legal move within 2 seconds; integration tests pass.
Dependencies: Domain Rules Engine.

#### Tasks

---

- [ ] Task 1: Define DTOs and mapping
- Description: Create request/response DTOs for board state and moves; mapping to domain.
- Inputs: `docs/001-mvp/specs/backend-api-spec.md`
- Outputs: `BoardStateDto`, `MoveDto`, mappers
- Acceptance: Model validation in place; bad requests return ProblemDetails.

Steps:
- [ ] Add DTOs with data annotations/FluentValidation
- [ ] Add mapping helpers to/from domain types
- [ ] Add minimal validation filters and ProblemDetails
- [ ] Build solution

---

- [ ] Task 2: Implement AI service and endpoint
- Description: Implement basic AI (e.g., pick any legal capture else best heuristic) and expose via endpoint.
- Inputs: SOR FR5; backend spec IF1; domain engine APIs
- Outputs: `IAiService` and implementation; `/api/ai/move` endpoint
- Acceptance: Integration test via `WebApplicationFactory` verifies legal move and response time <= 2s (typical).

Steps:
- [ ] Implement `IAiService` using rules engine to enumerate legal moves
- [ ] Choose simple heuristic: prefer capture; otherwise random/legal
- [ ] Add `/api/ai/move` endpoint with validation and timing logs
- [ ] Write integration test; run and fix issues

---

## Work Item

### [ ] Work Item: Frontend UI
Description: Blazor Server UI for board rendering, move selection, highlights, prompts, status, restart.
Definition of Done: User can play full game vs AI with legal moves and clear prompts; E2E test passes.
Dependencies: Domain Rules Engine; Backend API.

#### Tasks

---

- [ ] Task 1: Components and state setup
- Description: Create `GameBoard.razor`, `StatusBar.razor`, `RestartDialog.razor`; set up DI clients/services.
- Inputs: `docs/001-mvp/specs/frontend-ui-spec.md`
- Outputs: Razor components with `@code`-behind; scoped state service
- Acceptance: Board renders with initial setup; keyboard focusable elements; high-contrast styles.

Steps:
- [ ] Build board grid and piece rendering
- [ ] Initialize starting position (Spanish setup)
- [ ] Add status bar and restart dialog skeleton
- [ ] Build and manual verify

---

- [ ] Task 2: Interaction, highlights, prompts
- Description: Wire selection, legal move highlights, mandatory capture prompts, and AI turn flow.
- Inputs: SOR FR3-FR4, FR6-FR8; frontend spec AD4
- Outputs: Interaction logic; HTTP client to call `/api/ai/move`; status updates; end-game summary
- Acceptance: Playwright E2E validates full game path; bUnit tests for component behaviors.

Steps:
- [ ] On select, query rules engine for legal moves; highlight destinations
- [ ] Enforce capture-only when required; show prompt
- [ ] Apply move; update state; request AI move; apply response
- [ ] Detect end-game and display result; confirm restart
- [ ] Add bUnit and Playwright tests; run and stabilize

---

## Work Item

### [ ] Work Item: Observability & Quality Gates
Description: Minimal structured logging; test setup across layers; performance check for AI.
Definition of Done: Logs emitted for key events; unit/integration/E2E tests green; AI typically <= 2s.
Dependencies: All feature work.

#### Tasks

---

- [ ] Task 1: Logging baseline
- Description: Configure `ILogger` with structured messages and event IDs.
- Inputs: `docs/001-mvp/specs/observability-spec.md`, `instructions/architecture.md`
- Outputs: Log statements for GameStarted/GameEnded, errors, AI timing
- Acceptance: Logs visible in console during manual play and tests.

Steps:
- [ ] Add logging configuration in `Program.cs`
- [ ] Emit logs at start/end of games and on errors
- [ ] Add timing logs around AI endpoint
- [ ] Run app and verify logs

---

- [ ] Task 2: Testing setup and gates
- Description: Establish unit (xUnit), integration (WebApplicationFactory), and E2E (Playwright) tests.
- Inputs: `instructions/backend.md`, `instructions/frontend.md`, specs test strategies
- Outputs: Test projects: `Draughts.Domain.Tests`, `Draughts.Api.Tests`, `Draughts.E2E`
- Acceptance: CI/local script runs all tests; all green before release.

Steps:
- [ ] Add xUnit test projects and references
- [ ] Add integration tests for `/api/ai/move`
- [ ] Add Playwright tests for core flow
- [ ] Run all tests and fix failures

---

## Tracking & Progress
- [ ] Plan approved
- [ ] Work item created
- [ ] First build successful
- [ ] QA passed
- [ ] Release prepared

## Notes
- Follow minimal change principle and project coding conventions.
- Validate changes using available tools; avoid breaking existing behavior.
- Store all documentation iterations under `docs/<iteration-id>/`.
- Traceability: Each task ties to SOR/Specs refs noted in Inputs/Acceptance.
