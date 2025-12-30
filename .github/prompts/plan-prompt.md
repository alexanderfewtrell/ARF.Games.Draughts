# Build Plan Generation Prompt

You are a senior software engineer. Produce a concise, actionable project build plan using the `Project Build Plan Template` at `.github/templates/plan-template.md`. The plan must be sufficient for you to follow and build the application.

Constraints
- Follow `.github/copilot-instructions.md` and all referenced guides.
- Keep changes minimal; adhere to project conventions.
- Populate all sections of the template with specific, concrete details from the provided inputs.
- Use clear checklists and ordered steps.
- Store documentation under `docs/<iteration-id>/` per Docs Workflow.
 - Build the code frequently (at minimum after completing each work item) and run all applicable tests.
 - Fix failing builds and failing tests immediately upon identification; document the fix and re-run the build and tests.

Information Gathering
- Read the Statement of Requirements (SOR) and Technical Specifications provided via links/paths.
- Extract and synthesize: goals, scope, non-goals, assumptions, risks, deliverables, milestones.
- Identify work items and map to tasks with clear Definition of Done and dependencies.
- Derive backend, frontend, database, CI/CD, and infrastructure tasks and acceptance criteria from specs.
- If any required field is missing from SOR/specs, request the specific missing input explicitly and proceed with reasonable defaults only when documented in the repository instructions.

Output
- A completed build plan document strictly following `.github/templates/plan-template.md`.
- Fill placeholders with the provided input values.
- Include concrete tasks and steps that are executable by you.
- Reference all instruction files:
  - `.github/copilot-instructions.md`
  - `instructions/architecture.md`
  - `instructions/folder-structure.md`
  - `instructions/file-naming.md`
  - `instructions/backend.md`
  - `instructions/frontend.md`
  - `instructions/docs-workflow.md`
  - SOR and Technical Specifications (links/paths provided in inputs)

Plan Requirements
- High-Level Overview: Clearly define goals, scope, non-goals, assumptions, risks, deliverables, milestones.
- Work Items: Each item has Description, Definition of Done, Dependencies, and produces useful, testable functionality.
- Tasks: For each task, specify Description, Inputs, Outputs, Acceptance; provide ordered Steps.
- Tracking & Progress: Initialize with appropriate checkboxes.
- Notes: Reiterate minimal change principle and validation.
 - Build & Test Discipline: For each work item, include steps to build the solution, run unit/integration tests, address failures, and confirm green before moving on.

Derivation Rules
- Trace each plan element to its source in SOR or Technical Specs where applicable.
- Align architecture, folder structure, naming, backend, and frontend sections with their respective instruction files.
- Prefer explicit requirements from SOR/specs over assumptions; document any assumptions in the plan.
 - For each work item, define verification criteria and test artifacts (e.g., unit tests, integration tests, manual QA scripts) that demonstrate completeness and functionality.

Formatting Rules
- Use the exact structure of `.github/templates/plan-template.md`.
- Use checkboxes for tasks and progress.
- Avoid heavy markdown other than headings and lists.

Example Usage
"""
Inputs:
- Project Name: ARF.Games.Draughts
- Iteration ID: docs/2025-01-build
- Repository/Path: C:\Repos\ARF.Games
- Orchestration/Platform: Aspire
- Primary Stack: .NET 8 / C# / MSSQL / React
- Date: 2025-01-05
- Owner: A. Fewtrell
- SOR: docs/sor.md
- Technical Specs: docs/specs.md
- Goals: MVP playable draughts with auth and persistence
- Scope: Backend API, Frontend UI, DB schema, CI pipeline
- Non-Goals: Mobile app, advanced AI, live matchmaking
- Assumptions: Single-tenant, local dev SQL
- Risks: Data model complexity, auth integration
- Deliverables: API service, UI app, DB migrations, CI pipeline
- Milestones: Design, Implementation, QA, Release
- Work Items/Tasks: Auth, Game Logic, Persistence, UI, CI, Infra

Output: Completed plan following the template and requirements.
"""
