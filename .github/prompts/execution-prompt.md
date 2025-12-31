# Build Plan Execution Prompt

Purpose
- Execute an existing project build plan in strict order and track progress.
- Mark off every work item, task, and step as they are completed.
- Build the solution and run tests continuously to keep the codebase healthy.

References
- `.github/copilot-instructions.md`
- `instructions/architecture.md`
- `instructions/folder-structure.md`
- `instructions/file-naming.md`
- `instructions/backend.md`
- `instructions/frontend.md`
- `instructions/docs-workflow.md`
- Plan template: `.github/templates/plan-template.md`

Inputs
- Plan file path (markdown following `.github/templates/plan-template.md`).
- Iteration ID (`docs/<iteration-id>/`).
- Repository root path.
- Solution/project context (e.g., solution file, projects, test projects).

Strict Sequence Rules
- Follow the plan exactly as ordered: Work Items ? Tasks ? Steps.
- Do not reorder, skip, or batch steps. Complete the next unchecked step only.
- Mark checkboxes only when the corresponding work item/task/step is fully complete.

Build & Test Discipline
- Before starting: perform a clean build and run all tests to establish a baseline.
- After every step: build the solution and run unit/integration tests.
- Fix build or test failures immediately before proceeding.
- After every task and every work item: run a full build and all tests again.

Workflow
1) Load Plan
   - Read the plan file and validate sections and checklists.
   - Identify the next unchecked Work Item, then Task, then Step.
2) Prepare
   - Gather required inputs/files/specs for the step.
   - Confirm acceptance criteria from the plan.
3) Implement Step
   - Apply minimal, conventional changes aligned to `instructions/*`.
   - Update or add tests as required by acceptance criteria.
4) Validate
   - Build the solution.
   - Run unit and integration tests.
   - If failures occur, fix and re-run until green.
5) Mark Progress
   - Check the checkbox for the completed Step in the plan.
   - When all steps in a Task are complete, check the Task.
   - When all tasks in a Work Item are complete, check the Work Item.
   - Add a brief status note (what changed, links to files, test summary) beneath the completed item.
6) Persist Docs
   - Store progress notes under `docs/<iteration-id>/plans/` if not already captured in the plan file.
   - Keep naming per `instructions/file-naming.md` and `instructions/docs-workflow.md`.
7) Iterate
   - Proceed to the next unchecked Step and repeat until the plan is fully completed.

Failure Handling
- If a step is ambiguous or blocked, ask one focused question and wait for an answer before continuing.
- If build/tests cannot be fixed within the step scope, record the issue under Risks/Notes in the plan, stop, and request guidance.

Outputs
- Updated plan file with completed checkboxes and concise status notes.
- Passing builds and tests after each completed step, task, and work item.
- Optional progress log under `docs/<iteration-id>/plans/`.

Example Usage
"""
Inputs:
- Plan: docs/2025-01-05-draughts-mvp/plans/delivery-plan.md
- Iteration ID: docs/2025-01-05-draughts-mvp
- Repo: C:\Repos\ARF.Games

Process:
1) Initial build and tests.
2) Execute Work Item 1 ? Task 1 ? Step 1; implement; build; test; mark done; note results.
3) Continue strictly in order; repeat build/test after each step.
4) When complete, ensure Tracking & Progress section is all checked.
"""
