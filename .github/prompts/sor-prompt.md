# Prompt: Generate a Statement of Requirements (SOR)

Purpose
- Guide the creation of a clear, complete Statement of Requirements for a software project.
- Ensure the SOR focuses purely on user requirements and business outcomes, not technical solutions.

Instructions for Assistant Behavior
- Present thinking in the chat window prior to engaging. Briefly outline assumptions, risks, and an initial approach.
- Ask for the project overview first, then progressively clarify via an open, collaborative dialogue.
- Keep language concise and impersonal.
- Avoid collecting secrets or credentials; focus on functional and nonfunctional requirements.
- Keep discussion and output non-technical: avoid proposing architectures, technologies, or implementation details in the SOR.

Flow
1) Initial Input
- "Please describe the project you want to build (one paragraph)."

2) Assistant Visible Thinking
- Summarize initial understanding.
- Identify potential risks or unknowns.
- Do not propose technical solutions within the SOR context; reserve technical exploration for later design phases.

3) Collaborative Clarification
- Engage in an iterative Q&A to refine understanding across: goals and scope, functional requirements, architecture choices, data, integrations, nonfunctional needs (availability, performance, security, compliance), delivery (environments, deployment, IaC), and observability.
- Ask one focused question at a time, adapting based on prior answers.
- Capture decisions and rationale as they emerge.
  
Note: For SOR purposes, frame nonfunctional needs from the user/business perspective (e.g., availability expectations, performance targets) and avoid naming specific technologies or solution patterns.

4) Confirmation
- Confirm assumptions and summarize agreed decisions.
- Ask for constraints, timelines, or budget considerations.

5) Output
- Generate SOR using the template in `.github/templates/sor-template.md`.
- Ensure sections: Executive Summary, Objectives, Scope, Functional Requirements, Nonfunctional Requirements, Risks & Assumptions, Timeline, Acceptance Criteria.
- Omit technical solution content (e.g., architecture diagrams, specific technologies, deployment targets, IaC, observability tooling). If the template includes such sections, leave them as placeholders to be completed in a separate design document.

Notes
- Use concise bullets. Link choices to rationale.
