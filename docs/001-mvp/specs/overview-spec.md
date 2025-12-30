# Technical Specification Template

---
iteration-id: 001-mvp

version: 1.0.0  

date: 2025-12-30  

status: draft  

---

1. Overview & Goals

- Project name: Draughts MVP (Spanish rules)
- Summary: A Blazor Server desktop-hosted MVP for Spanish draughts enabling a single player to play against a basic computer opponent. Enforces Spanish rules including mandatory capture and flying kings, with clear UI highlights and prompts.
- Business goals:
  - Validate user interest with a minimal, playable experience.
  - Establish core gameplay loop and rules fidelity.
  - Gather feedback to inform future iterations.
- Success metrics:
  - Players complete full games end-to-end.
  - Players make legal moves without confusion.
  - Outcomes shown reliably at game end.

2. Scope & Assumptions

| Ref | Category   | Description | Notes |
|-----|------------|-------------|-------|
| SC1 | In scope   | Single-player vs basic AI | One difficulty |
| SC2 | In scope   | Spanish rules (8x8, mandatory capture, flying kings) | Rules fidelity prioritized |
| SC3 | In scope   | Move highlights, mandatory capture prompts, status/outcomes | Accessibility-first UI |
| SC4 | In scope   | Restart/new game with confirm dialog | Prevent accidental reset |
| SC5 | Out of scope| Multiplayer, accounts, persistence, leaderboards | Future iterations |
| SC6 | Out of scope| Multiple AI levels, adaptive difficulty | Not required for MVP |
| SC7 | Assumption | Desktop-only hosting for MVP | Local machine |
| SC8 | Constraint | No persistence | In-memory per session |

3. Architecture & Design

| Ref  | Area                      | Description | Diagrams/Links |
|------|---------------------------|-------------|----------------|
| AD1  | System context            | Single desktop-hosted Blazor Server app; minimal internal API endpoints for AI moves; no external integrations | |
| AD2  | Component overview        | Components: frontend-ui (Blazor Server), backend-api (minimal ASP.NET Core endpoints for AI engine), domain/rules engine (Spanish draughts), observability (console logging) | |
| AD3  | Design patterns & rationale| Clean Architecture boundaries; rules engine as domain services; minimal API for separation of concerns; DI for services | |
| AD4  | Technology stack          | .NET LTS, C#, ASP.NET Core Blazor Server, minimal APIs, DI; no DB | |
| AD5  | Data flow & sequences     | UI selects piece -> rules engine validates and produces legal moves -> mandatory capture prompts -> user move -> state updates and kinging -> AI endpoint computes legal AI move -> state updates -> repeat -> detect end-game -> summary | |

4. Interfaces & Contracts

| Ref  | Type      | Name/Endpoint | Request | Response | Notes |
|------|-----------|----------------|---------|----------|-------|
| IF1  | Internal  | POST /api/ai/move | { boardState, currentPlayer } | { move, updatedState } | Minimal API for AI move computation |
| IF2  | Internal  | POST /api/game/validate | { boardState, selection } | { legalMoves, mustCapture } | Optional helper, may be encapsulated in UI/domain |

Events/Messages

| Ref  | Topic/Name | Schema | Producer | Consumer | Notes |
|------|------------|--------|----------|----------|-------|
| EV1  | GameStarted | { timestamp, rules } | UI | Logger | Console logging only |
| EV2  | GameEnded | { timestamp, result } | UI | Logger | Console logging only |

5. Data Model & Storage

Entities & Relationships

| Ref  | Entity | Description | Relationships | Notes |
|------|--------|-------------|---------------|-------|
| DM1  | Board | 8x8 grid with Spanish setup | Contains Pieces | In-memory only |
| DM2  | Piece | Man or King, player ownership | On Board | Movement per rules |
| DM3  | Move  | From, To, captures | References Board and Pieces | Derived transient data |

Schemas & Migrations

| Ref  | Store (DB/Cache) | Schema/DDL | Migration Plan | Notes |
|------|-------------------|------------|----------------|-------|
| DM2  | None | N/A | N/A | No persistence |

Data Lifecycle & Performance

| Ref  | Area            | Description | Target/Measure | Notes |
|------|------------------|-------------|----------------|-------|
| DM3  | Lifecycle        | State lives in session memory; reset on restart | N/A | Desktop scope |
| DM4  | Indexing/Perf    | Not applicable | N/A | |

6. Security & Compliance

| Ref | Area                     | Requirement Description | Target/Policy | Notes |
|-----|--------------------------|-------------------------|---------------|-------|
| SEC1| Identity & authentication| None | N/A | Desktop-only |
| SEC2| Authorization & RBAC     | None | N/A | Single role |
| SEC3| Data protection & privacy| No personal data stored | N/A | In-memory |
| SEC4| Compliance               | Avoid regulated data | N/A | Prototype |

7. Nonfunctional Requirements

| Ref | Category       | Requirement Description | Target/Measure | Notes |
|-----|----------------|-------------------------|----------------|-------|
| NFR1| Availability   | Accessible during normal usage | Best effort | Desktop |
| NFR2| Performance    | AI move responsiveness | <= 2 seconds | Per SOR |
| NFR3| Scalability    | Small-scale demo | N/A | Desktop |
| NFR4| Reliability    | Stable gameplay without crashes | Best effort | |
| NFR5| Maintainability| Clean boundaries and DI | Repo norms | |
| NFR6| Operability    | Simple startup and reset | README instructions | |

8. Observability

| Ref | Area       | Requirement Description | Target/Measure | Notes |
|-----|------------|-------------------------|----------------|-------|
| OB1 | Logging    | Minimal structured logging to console (game start, end, errors) | Basic visibility | |
| OB2 | Metrics    | None for MVP | N/A | |
| OB3 | Tracing    | None for MVP | N/A | |
| OB4 | Alerts & SLOs| None for MVP | N/A | |

9. Environments & Deployment

| Ref | Environment | Purpose/Usage | Deployment Approach | Config Mgmt | Rollback | Notes |
|-----|-------------|---------------|---------------------|-------------|----------|-------|
| ED1 | Desktop     | Local MVP usage | Run Blazor Server locally (Kestrel) | appsettings.Development.json | Restart app | No external deps |

10. Test Strategy & Quality Gates

| Ref | Area            | Strategy/Tools | Coverage/Criteria | Pipeline/Stage | Notes |
|-----|-----------------|----------------|-------------------|----------------|-------|
| TS1 | Unit            | xUnit/NUnit for rules engine | Cover move validation, captures, kinging | Local test run | |
| TS2 | Integration     | Aspire for minimal API endpoints | Validate AI move endpoint responses | Local | |
| TS3 | E2E             | Playwright for UI interaction | Validate legal moves, prompts, outcomes | Local | |
| TS4 | Security        | Basic input validation | Ensure robust request models | Local | |
| TS5 | Performance     | Measure AI move time | <= 2 seconds typical | Local | |

11. Risks & Mitigations

| Ref | Type  | Description | Impact | Mitigation/Notes |
|-----|-------|-------------|--------|------------------|
| RK1 | Risk  | Ambiguity in Spanish rules implementation | Medium | Validate against standard references |
| RK2 | Risk  | AI quality may frustrate users | Medium | Keep expectations clear; iterate later |
| RK3 | Assumption | Desktop-only environment | Low | Confirm portability later |

12. Implementation Plan

Milestones

| Ref | Milestone/Phase | Description | Target Date | Dependencies |
|-----|------------------|-------------|-------------|--------------|
| IP1 | MVP Specification | Finalize SOR and design | 2 weeks | Stakeholder approval |
| IP2 | Core Gameplay     | Board, rules engine, UI basics | 4 weeks | IP1 |
| IP3 | AI Integration    | Basic AI moves | 2 weeks | IP2 |
| IP4 | MVP Validation    | Functional testing and fixes | 2 weeks | IP2, IP3 |

Work Breakdown

| Ref | Work Item | Component | Owner | Estimate | Notes |
|-----|-----------|-----------|-------|----------|-------|
| WBS1| Board render and setup | frontend-ui | TBD | 3d | |
| WBS2| Rules engine (movement, captures, kinging) | domain | TBD | 5d | |
| WBS3| Move highlights and prompts | frontend-ui | TBD | 2d | |
| WBS4| AI move endpoint and basic AI | backend-api | TBD | 4d | |
| WBS5| Turn indicator and status | frontend-ui | TBD | 2d | |
| WBS6| End-game detection and summary | domain/frontend-ui | TBD | 2d | |
| WBS7| Restart/new game with confirm | frontend-ui | TBD | 1d | |

13. Traceability to SOR

| SOR Ref | Spec Section | Notes |
|---------|--------------|-------|
| OBJ1    | 1,3,4,12     | Playable vs AI |
| OBJ2    | 3,4,12       | Rules fidelity and prompts |
| OBJ3    | 1,4,6        | Outcome communication |
| FR1     | 3,5,12       | Board render and setup |
| FR2     | 3,5,12       | Rules enforcement |
| FR3     | 4,12         | Move highlights |
| FR4     | 4,12         | Mandatory capture prompts |
| FR5     | 3,4,12       | Basic AI legal moves |
| FR6     | 4,12         | Turn indicator and status |
| FR7     | 3,4,12       | End-game detection and summary |
| FR8     | 4,12         | Restart functionality |
| NF1     | 7            | Availability |
| NF2     | 7,10         | Performance |
| NF3     | 7            | Scalability |
| NF4     | 6            | Security baseline |
| NF5     | 6            | Compliance |
| NF6     | 1,4          | Usability |
| NF7     | 1,4          | Accessibility |
| NF8     | 1,4          | Accessibility-first visuals |
| SC1     | 2            | In scope |
| ED1     | 9            | Desktop environment |
| OB1     | 8            | Logging |
| RA1     | 11           | Rules ambiguity |
| RA2     | 11           | AI quality risk |
| RA3     | 11           | Browser/platform assumption |
| RA4     | 11           | Single difficulty acceptable |
| TL1     | 12           | Spec timeline |
| TL2     | 12           | Core gameplay |
| TL3     | 12           | AI integration |
| TL4     | 12           | Validation |
| AC1     | 10           | Rules enforced |
| AC2     | 10           | AI legal moves |
| AC3     | 10           | AI response time |
| AC4     | 10           | UI clarity |

14. Appendices

- ADR links: docs/adr (future)
- Diagrams: docs/diagrams (future)
- Glossary: Spanish draughts, flying king, mandatory capture
