# Project Build Plan Template

> Basis: Statement of Requirements (SOR), Technical Specifications, and `copilot-instructions.md`
> Purpose: Provide a structured, trackable plan for Copilot to build the application.

## Metadata
- Project Name: <enter>
- Iteration ID: <docs/<iteration-id>/>
- Repository/Path: <enter>
- Orchestration/Platform: <Aspire/Containers/Azure/etc>
- Primary Stack: <.NET/C#/MSSQL/React/etc>
- Date: <YYYY-MM-DD>
- Owner: <enter>

## References
- SOR: <link or path>
- Technical Specs: <link or path>
- Copilot Instructions Index: `.github/copilot-instructions.md`
- Architecture Guide: `instructions/architecture.md`
- Folder Structure: `instructions/folder-structure.md`
- File Naming: `instructions/file-naming.md`
- Backend Guide: `instructions/backend.md`
- Frontend Guide: `instructions/frontend.md`
- Docs Workflow: `instructions/docs-workflow.md`

## High-Level Overview
- Goals: <primary outcomes>
- Scope: <services, features, boundaries>
- Non-Goals: <explicit exclusions>
- Assumptions: <key assumptions>
- Risks: <top risks>
- Deliverables: <artifacts/releases>
- Milestones: <timeline>

---

## Work Item

### [ ] Work Item: <Name>
Description: <summary>
Definition of Done: <criteria>
Dependencies: <work items, services, env>

#### Tasks

---

- [ ] Task 1: <Task Name>
- Description: <purpose>
- Inputs: <files, specs, APIs>
- Outputs: <code, config, docs>
- Acceptance: <tests, validations>

Steps:
- [ ] Step 1
- [ ] Step 2
- [ ] Step 3
- [ ] etc   

---

- [ ] Task 2: <Task Name>
- Description: <purpose>
- Inputs: <files, specs, APIs>
- Outputs: <code, config, docs>
- Acceptance: <tests, validations>

Steps:
- [ ] Step 1
- [ ] Step 2
- [ ] Step 3
- [ ] etc  

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
