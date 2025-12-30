# Technical Specification Generation Prompt

Purpose
- Convert a completed SOR into technical specifications (overview + component docs) in markdown.
- Follow `.github/copilot-instructions.md` and related instruction files.

Inputs
- SOR (based on `.github/templates/sor-template.md`).
- Project context from `instructions/*`.

Outputs
- `docs/<iteration-id>/specs/overview-spec.md`.
- `docs/<iteration-id>/specs/<component>-spec.md` (as needed).
- Conform to `.github/templates/techspec-template.md`.

Interactive Workflow
1. Parse SOR
- Summarize key points; note ambiguities.
- Present brief reasoning.
- Ask numbered clarifying questions derived from SOR and repo context.

2. Confirm Architecture & Components
- Propose architecture aligned to `instructions/architecture.md`.
- Ask numbered questions to confirm components, integrations, persistence, APIs/UI, security, deployment.

3. Confirm NFRs & Observability
- Propose targets per SOR and repo norms.
- Ask numbered questions to confirm availability, performance, scalability, observability stack.

4. Confirm Security & Compliance
- Map SC refs to controls.
- Ask numbered questions to confirm protection mechanisms and compliance scope.

5. Confirm Environments & Deployment
- Outline environments from SOR.
- Ask numbered questions to confirm environments and release strategy.

6. Confirm Test Strategy
- Propose strategy; ask numbered questions to confirm test levels and quality gates.

7. Generate Specs Iteratively
- Compile overview and component specs using the template.
- Present reasoning; ask for final confirmation before writing files.
- Include SOR traceability.

8. Output Formatting
- Write under `docs/<iteration-id>/specs/` with frontmatter.
- Link ADRs and diagrams per `instructions/docs-workflow.md`.

Interaction Protocol
- Precede each question block with "Reasoning:".
- Use numbered options; allow comma-separated multi-select.
- Echo decisions in a compact summary for confirmation.
- Ask clarifying questions if conflicts arise.
- Keep responses short and impersonal.

Constraints
- Use `.github/templates/techspec-template.md`.
- Produce overview and per-component specs as needed.
- Preserve SOR ref ids (OBJ, FR, NF, SC, ED, OB, RA, TL, AC).
- Align with repository instruction files.

Steps
1) Summarize SOR.
2) Propose architecture and components.
3) Generate `overview-spec.md`.
4) Generate component specs.
5) Provide SOR traceability.
6) Write files under `docs/<iteration-id>/specs/`.

Example Components
- backend-api, frontend-ui, data-access, identity-auth, messaging-events, observability, ci-cd, infrastructure

Notes
- Keep concise; use tables where helpful.
- Use code blocks for APIs/DTOs/config.
- Prefer Azure/.NET stack patterns.
