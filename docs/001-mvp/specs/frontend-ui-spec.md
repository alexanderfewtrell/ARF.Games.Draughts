# Technical Specification Template

---
iteration-id: 001-mvp

version: 1.0.0  

date: 2025-12-30  

status: draft  

---

1. Overview & Goals

- Project name: Draughts MVP Frontend UI
- Summary: Blazor Server UI providing board rendering, move selection, highlights, mandatory capture prompts, turn indicator, and end-game summary.
- Business goals: Clear, accessible UI enabling legal play without confusion.
- Success metrics: Users complete games, understand prompts, and see outcomes.

2. Scope & Assumptions

| Ref | Category   | Description | Notes |
|-----|------------|-------------|-------|
| SC1 | In scope   | Board UI, piece movement, highlights, prompts | Accessibility-first |
| SC2 | In scope   | Turn indicator and status messages | |
| SC3 | In scope   | Restart/new game with confirm | |
| SC4 | Out of scope| Accounts, persistence, multiplayer | |
| SC5 | Assumption | Desktop-hosted Blazor Server | |

3. Architecture & Design

| Ref  | Area                      | Description | Diagrams/Links |
|------|---------------------------|-------------|----------------|
| AD1  | Component overview        | Razor components: GameBoard, Piece, MovePrompt, StatusBar, RestartDialog | |
| AD2  | Patterns & rationale      | Componentization with `@code`-behind partial classes; DI for services; state held in scoped services or component state | |
| AD3  | Technology stack          | .NET LTS, Blazor Server, Fluent UI styles (optional), Tailwind or BEM CSS | |
| AD4  | Data flow                 | User selects piece -> UI queries rules engine -> highlights legal moves -> prompts mandatory capture -> executes move -> updates state -> calls AI endpoint -> updates state -> status indicators | |

4. Interfaces & Contracts

| Ref  | Type      | Name/Endpoint | Request | Response | Notes |
|------|-----------|----------------|---------|----------|-------|
| IF1  | Internal  | RulesEngine service | { boardState, selection } | { legalMoves, mustCapture } | DI service |
| IF2  | Internal  | AIClient (HttpClient) POST /api/ai/move | { boardState, currentPlayer } | { move, updatedState } | IHttpClientFactory |

Events/Messages

| Ref  | Topic/Name | Schema | Producer | Consumer | Notes |
|------|------------|--------|----------|----------|-------|
| EV1  | UI.GameStarted | { timestamp, rules } | UI | Logger | Console |
| EV2  | UI.GameEnded | { timestamp, result } | UI | Logger | Console |

5. Data Model & Storage

Entities & Relationships

| Ref  | Entity | Description | Relationships | Notes |
|------|--------|-------------|---------------|-------|
| DM1  | BoardViewModel | Board grid, pieces, selection, highlights | Contains PieceViewModels | |
| DM2  | PieceViewModel | Type, owner, position, isKing | On BoardViewModel | |
| DM3  | MoveViewModel  | From, To, captures | References PieceViewModels | |

Schemas & Migrations

| Ref  | Store (DB/Cache) | Schema/DDL | Migration Plan | Notes |
|------|-------------------|------------|----------------|-------|
| DM4  | None | N/A | N/A | No persistence |

Data Lifecycle & Performance

| Ref  | Area            | Description | Target/Measure | Notes |
|------|------------------|-------------|----------------|-------|
| DM5  | Lifecycle        | Session state only; clear on restart | N/A | |
| DM6  | Rendering perf   | Efficient rendering, avoid unnecessary re-renders | Smooth UX | |

6. Security & Compliance

| Ref | Area                     | Requirement Description | Target/Policy | Notes |
|-----|--------------------------|-------------------------|---------------|-------|
| SEC1| Input validation         | Sanitize user inputs (coords) | Defensive UI | |
| SEC2| Privacy                  | No personal data | N/A | |

7. Nonfunctional Requirements

| Ref | Category       | Requirement Description | Target/Measure | Notes |
|-----|----------------|-------------------------|----------------|-------|
| NFR1| Accessibility  | WCAG 2.1 AA basics (contrast, keyboard nav) | MVP level | |
| NFR2| Performance    | Responsive interaction | Smooth moves | |
| NFR3| Usability      | Clear highlights and prompts | Users avoid illegal moves | |

8. Observability

| Ref | Area       | Requirement Description | Target/Measure | Notes |
|-----|------------|-------------------------|----------------|-------|
| OB1 | Logging    | Log start, end, errors | Console | |

9. Environments & Deployment

| Ref | Environment | Purpose/Usage | Deployment Approach | Config Mgmt | Rollback | Notes |
|-----|-------------|---------------|---------------------|-------------|----------|-------|
| ED1 | Desktop     | Local MVP usage | Run Blazor Server locally | appsettings.Development.json | Restart app | |

10. Test Strategy & Quality Gates

| Ref | Area            | Strategy/Tools | Coverage/Criteria | Pipeline/Stage | Notes |
|-----|-----------------|----------------|-------------------|----------------|-------|
| TS1 | Component       | bUnit          | Render board, highlights, prompts | Local | |
| TS2 | E2E             | Playwright     | Full game flow vs AI | Local | |

11. Risks & Mitigations

| Ref | Type  | Description | Impact | Mitigation/Notes |
|-----|-------|-------------|--------|------------------|
| RK1 | Risk  | UI confusion on mandatory capture | Medium | Clear prompts and disables |

12. Implementation Plan

Milestones

| Ref | Milestone/Phase | Description | Target Date | Dependencies |
|-----|------------------|-------------|-------------|--------------|
| IP1 | UI Components    | Board, prompts, status | 2 weeks | Rules engine |

Work Breakdown

| Ref | Work Item | Component | Owner | Estimate | Notes |
|-----|-----------|-----------|-------|----------|-------|
| WBS1| GameBoard.razor | frontend-ui | TBD | 2d | |
| WBS2| StatusBar.razor | frontend-ui | TBD | 1d | |
| WBS3| RestartDialog.razor | frontend-ui | TBD | 1d | |

13. Traceability to SOR

| SOR Ref | Spec Section | Notes |
|---------|--------------|-------|
| FR1     | 3,5          | Board render |
| FR3     | 3,4          | Highlights |
| FR4     | 3,4          | Prompts |
| FR6     | 3            | Status |
| FR8     | 3            | Restart |
| NF6     | 7            | Usability |
| NF7     | 7            | Accessibility |

14. Appendices

- ADR links: docs/adr (future)
- Diagrams: docs/diagrams (future)
