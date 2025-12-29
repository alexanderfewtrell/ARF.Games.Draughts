# Statement of Requirements (SOR)

1. Executive Summary

- Project name: Draughts MVP (Spanish rules)
- Overview: A browser-based draughts game enabling a single player to play against a basic computer opponent following Spanish draughts rules.
- Business goals:
  - Validate user interest with a minimal, playable experience.
  - Establish core gameplay loop and rules fidelity.
  - Gather feedback to inform future iterations (UI/UX and difficulty expansion).
 - UI Style: Accessibility-first visuals (strong contrast, larger icons, clear typography).

2. Objectives

| Ref | Objective Description | Priority | Success Metric |
|-----|------------------------|----------|----------------|
| OBJ1| Deliver a playable browser game vs basic AI using Spanish rules | High | User completes full games end-to-end |
| OBJ2| Provide clear rule guidance (mandatory capture prompts, move highlights) | High | Players make legal moves without confusion |
| OBJ3| Communicate outcomes (win/loss/draw) and basic game status | Medium | Outcomes shown reliably at game end |

3. Scope

| Scope Item Ref | Item Description | In/Out | Notes |
|----------------|------------------|--------|-------|
| S1             | Single-player vs computer with one difficulty | In     | Basic AI sufficient for MVP |
| S2             | Spanish draughts rules (8x8, mandatory capture, flying kings) | In     | Rules fidelity prioritized |
| S3             | Browser-based UI with board, piece movement, turn indicator | In     | Minimal, clear interaction |
| S4             | Move highlights and mandatory capture prompts | In     | Reduce user errors |
| S5             | Outcome display: win/loss/draw with simple summary | In     | Basic status only |
| S10            | Restart/new game with confirmation dialog | In     | Prevent accidental resets |
| S6             | Online multiplayer or local two-player mode | Out    | Future iteration |
| S7             | Multiple AI levels, adaptive difficulty | Out    | Not required for MVP |
| S8             | Accounts, profiles, persistence, leaderboards | Out    | Not required for MVP |
| S9             | Tutorial, hints, undo, save/resume | Out    | Keep MVP focused |

4. Functional Requirements

| Ref | Requirement Description | Priority | Notes |
|-----|--------------------------|----------|-------|
| FR1 | Render an 8x8 draughts board and initial Spanish setup | High | Visual clarity and correct starting positions |
| FR2 | Enforce legal movement, mandatory captures, and flying king movement | High | Rule engine must prevent illegal moves |
| FR3 | Provide move highlights for selectable pieces and legal destinations | High | Reduce confusion and misclicks |
| FR4 | Prompt user when capture is mandatory and restrict non-capturing moves accordingly | High | Rules compliance |
| FR5 | Implement a basic computer opponent that makes legal moves | High | Single difficulty, reasonable responsiveness |
| FR6 | Display turn indicator and game status updates | Medium | Current player, captures, kinging events |
| FR7 | Detect game completion (win/loss/draw) and show simple result summary | Medium | Clear end-of-game message |
| FR8 | Provide restart/new game functionality with confirm dialog | Medium | Prevent accidental reset |

User Roles and Capabilities
- Roles: Player (single role)
- Capabilities:
  - Start a new game vs AI.
  - Select and move pieces with rules enforcement.
  - View turn state and outcome.

5. Nonfunctional Requirements

| Ref | Category        | Requirement Description           | Target/Measure | Notes |
|-----|-----------------|-----------------------------------|----------------|-------|
| NF1 | Availability    | MVP should be accessible during normal usage times | Best effort | Non-critical prototype |
| NF2 | Performance     | Responsive move processing and AI reply | AI turn completes within 2 seconds | UX focus |
| NF3 | Scalability     | Supports concurrent browser users for demo | Small-scale demo | No large-scale targets for MVP |
| NF4 | Security        | No sensitive data; minimal exposure | Basic safeguards appropriate to MVP | No accounts |
| NF5 | Compliance      | No collection of regulated data | Not applicable | MVP avoids PII collection |
| NF6 | Usability       | Clear visual cues for legal moves and outcomes | Users complete games without confusion | Highlights and prompts |
| NF7 | Accessibility   | Basic keyboard/tab navigation and color contrast | Minimum practical | MVP-level accommodations |
| NF8 | Usability       | Accessibility-first visual design | Strong contrast, larger icons | Consistent, readable UI |

6. Security & Compliance

| Ref | Area                      | Requirement Description | Target/Policy | Notes |
|-----|---------------------------|-------------------------|---------------|-------|
| SC1 | Identity                  | No authentication required | N/A | Single-player prototype |
| SC2 | Authorization             | Not applicable for single-player | N/A | No roles beyond player |
| SC3 | Data Protection & Privacy | No persistent personal data | N/A | Session-only state |
| SC4 | Compliance                | Avoid storing regulated data | N/A | Prototype scope |

7. Environments & Deployment

| Ref | Environment | Purpose/Usage | Deployment Approach | Notes |
|-----|-------------|---------------|---------------------|-------|
| ED1 | Dev         | Iterative development and internal testing | Placeholder | Filled in design document |
| ED2 | Test/QA     | Basic functional validation | Placeholder | Filled in design document |
| ED3 | Staging     | Pre-release checks | Placeholder | Filled in design document |
| ED4 | Prod        | Public MVP access | Placeholder | Filled in design document |

8. Observability

| Ref | Area     | Requirement Description | Target/Measure | Notes |
|-----|----------|-------------------------|----------------|-------|
| OB1 | Logging  | Minimal event logging (game start, end, errors) | Placeholder | Filled in design document |
| OB2 | Monitoring| Basic uptime observation | Placeholder | Filled in design document |
| OB3 | Alerts   | Not required for MVP | N/A | Prototype |
| OB4 | SLOs     | Not defined for MVP | N/A | Prototype |

9. Risks & Assumptions

| Ref | Type        | Description | Impact | Mitigation/Notes |
|-----|-------------|-------------|--------|------------------|
| RA1 | Risk        | Ambiguity in Spanish rules implementation | Medium | Validate rules against standard references |
| RA2 | Risk        | AI quality may frustrate users | Medium | Keep expectations clear; iterate later |
| RA3 | Assumption  | Browser is the initial platform of choice | Low | Confirm with stakeholders |
| RA4 | Assumption  | Single difficulty is acceptable for MVP | Medium | Plan for future expansion |

10. Timeline

| Ref | Phase/Milestone | Description | Target Date | Dependencies |
|-----|------------------|-------------|-------------|--------------|
| TL1 | MVP Specification | Finalize SOR and design | 2 weeks | Stakeholder approval |
| TL2 | Core Gameplay     | Board, rules engine, UI basics | 4 weeks | TL1 |
| TL3 | AI Integration    | Basic AI moves | 2 weeks | TL2 |
| TL4 | MVP Validation    | Functional testing and fixes | 2 weeks | TL2, TL3 |

11. Acceptance Criteria

| Ref | Category        | Criteria Description | Validation Method | Notes |
|-----|-----------------|----------------------|-------------------|-------|
| AC1 | Functional      | Spanish rules enforced including mandatory captures and flying kings | Manual test scenarios | Clear prompts prevent illegal moves |
| AC2 | Functional      | Basic AI plays legal moves and completes games | Test matches vs AI | Single difficulty only |
| AC3 | Nonfunctional   | AI response within target time | Measure typical AI move time | <= 2 seconds |
| AC4 | Nonfunctional   | UI indicates turn state and end-game result clearly | UX review | Highlights and status messages |

Appendix

- Glossary: Spanish draughts rules, flying king, mandatory capture
- ADR links: Placeholder for future architectural decisions
- Diagrams: Placeholder
 - Constraints: No formal constraints for MVP (timeline and budget flexible)
