# Technical Specification Template

---
iteration-id: 001-mvp

version: 1.0.0  

date: 2025-12-30  

status: draft  

---

1. Overview & Goals

- Project name: Draughts MVP Backend API
- Summary: Minimal ASP.NET Core API endpoints supporting AI move computation and optional rule validation, hosted within the Blazor Server app.
- Business goals: Provide clean separation for AI logic; enable testable endpoints.

2. Scope & Assumptions

| Ref | Category   | Description | Notes |
|-----|------------|-------------|-------|
| SC1 | In scope   | POST /api/ai/move | Compute legal AI move |
| SC2 | In scope   | Optional: POST /api/game/validate | Validate moves |
| SC3 | Assumption | No persistence, in-memory state passed in requests | |

3. Architecture & Design

| Ref  | Area                      | Description | Diagrams/Links |
|------|---------------------------|-------------|----------------|
| AD1  | Patterns & rationale      | Minimal APIs for simplicity; DI for AI service; domain-driven rules engine | |
| AD2  | Technology stack          | .NET LTS, ASP.NET Core minimal APIs, DI | |
| AD3  | Contracts                 | Request/Response DTOs for board state and moves | |

4. Interfaces & Contracts

| Ref  | Type      | Name/Endpoint | Request | Response | Notes |
|------|-----------|----------------|---------|----------|-------|
| IF1  | Internal  | POST /api/ai/move | { boardState, currentPlayer } | { move, updatedState } | |
| IF2  | Internal  | POST /api/game/validate | { boardState, selection } | { legalMoves, mustCapture } | Optional |

Events/Messages

| Ref  | Topic/Name | Schema | Producer | Consumer | Notes |
|------|------------|--------|----------|----------|-------|
| EV1  | ApiError | { timestamp, error } | API | Logger | Console logging |

5. Data Model & Storage

Entities & Relationships

| Ref  | Entity | Description | Relationships | Notes |
|------|--------|-------------|---------------|-------|
| DM1  | BoardStateDto | 8x8 cells, pieces | Used by endpoints | |
| DM2  | MoveDto | From, To, captures | Used by endpoints | |

Schemas & Migrations

| Ref  | Store (DB/Cache) | Schema/DDL | Migration Plan | Notes |
|------|-------------------|------------|----------------|-------|
| DM3  | None | N/A | N/A | No persistence |

Data Lifecycle & Performance

| Ref  | Area            | Description | Target/Measure | Notes |
|------|------------------|-------------|----------------|-------|
| DM4  | Performance      | AI computation time budget | <= 2 seconds | |

6. Security & Compliance

| Ref | Area                     | Requirement Description | Target/Policy | Notes |
|-----|--------------------------|-------------------------|---------------|-------|
| SEC1| Input validation         | Validate DTOs and reject invalid boards | ProblemDetails | |
| SEC2| Transport                | HTTPS (even locally where possible) | Default ASP.NET Core | |

7. Nonfunctional Requirements

| Ref | Category       | Requirement Description | Target/Measure | Notes |
|-----|----------------|-------------------------|----------------|-------|
| NFR1| Reliability    | Stable endpoint behavior | Best effort | |
| NFR2| Testability    | Endpoint tests via Aspire | Local | |

8. Observability

| Ref | Area       | Requirement Description | Target/Measure | Notes |
|-----|------------|-------------------------|----------------|-------|
| OB1 | Logging    | Structured logs for errors and endpoint timing | Console | |

9. Environments & Deployment

| Ref | Environment | Purpose/Usage | Deployment Approach | Config Mgmt | Rollback | Notes |
|-----|-------------|---------------|---------------------|-------------|----------|-------|
| ED1 | Desktop     | Local MVP usage | Hosted within Blazor Server | appsettings.Development.json | Restart app | |

10. Test Strategy & Quality Gates

| Ref | Area            | Strategy/Tools | Coverage/Criteria | Pipeline/Stage | Notes |
|-----|-----------------|----------------|-------------------|----------------|-------|
| TS1 | Integration     | WebApplicationFactory | Validate endpoints | Local | |
| TS2 | Performance     | Measure endpoint latencies | AI move <= 2s | Local | |

11. Risks & Mitigations

| Ref | Type  | Description | Impact | Mitigation/Notes |
|-----|-------|-------------|--------|------------------|
| RK1 | Risk  | Complex or ambiguous board states | Medium | Strict validation |

12. Implementation Plan

Milestones

| Ref | Milestone/Phase | Description | Target Date | Dependencies |
|-----|------------------|-------------|-------------|--------------|
| IP1 | Endpoint setup   | Minimal API routes, DI | 1 week | Rules engine |

Work Breakdown

| Ref | Work Item | Component | Owner | Estimate | Notes |
|-----|-----------|-----------|-------|----------|-------|
| WBS1| AI endpoint | backend-api | TBD | 2d | |
| WBS2| Validation endpoint | backend-api | TBD | 1d | Optional |

13. Traceability to SOR

| SOR Ref | Spec Section | Notes |
|---------|--------------|-------|
| FR5     | 1,4          | AI legal moves |
| FR2     | 4            | Rules enforcement support |
| NF2     | 7,10         | Performance |

14. Appendices

- ADR links: docs/adr (future)
- Diagrams: docs/diagrams (future)
