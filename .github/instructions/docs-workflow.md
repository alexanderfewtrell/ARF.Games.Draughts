# Copilot Instructions: Docs Iteration Workflow

Purpose
- Standardize how project documentation is organized per iteration.
- Ensure SORs and related specifications are stored under `docs/` in consistent locations.

Definitions
- Iteration: A coherent cycle of work (e.g., discovery, SOR creation, planning, design sprint).
- Iteration Id: `yyyy-mm-dd-<short-slug>` (kebab-case, UTC date for the starting day of the iteration).

Top-level Rule
- All generated documentation must live under `docs/` (not `.github/outputs`).

Required Structure
- `docs/<iteration-id>/` — root folder for a single iteration
  - `sor/<sor-slug>/` — a single SOR and its assets
      - `sor.md` — SOR content using `.github/templates/sor-template.md`
  - `specifications/` — technical specifications collected during the iteration
      - `attachments/` — optional images or supporting files
    - other spec types may live in sibling folders (e.g., `brd/`, `user-stories/`)
  - `plans/` — planning artifacts (e.g., `delivery-plan.md`, `milestones.md`)
  - `designs/` — design notes and diagrams (non-ADR)
  - `notes/` — ad-hoc notes, meeting minutes

Additional Docs
- ADRs remain under `docs/adr/` using MADR format and are not tied to a single iteration.
- Diagrams shared across iterations can remain under `docs/diagrams/` and be linked from the iteration folder.

Rules
- Use kebab-case for folder and file names in `docs/`.
- Place multi-file artifacts (like SORs) in their own subfolder; put single-file artifacts directly under their category.
- Keep historical iterations immutable; create a new iteration folder rather than overwriting prior outputs.
- Include a brief `README.md` inside `docs/<iteration-id>/` when helpful to summarize the iteration.

Copilot Output Requirements
- When generating a SOR:
  1) Ask the user to confirm the iteration id (suggest a default like today’s date + short slug).
  2) Write to: `docs/<iteration-id>/specifications/sor/<sor-slug>/sor.md`.
  3) Use `.github/templates/sor-template.md` as the content template.
  4) If the path doesn’t exist, create the directories.
- Do not create or update files in `.github/outputs/`.
