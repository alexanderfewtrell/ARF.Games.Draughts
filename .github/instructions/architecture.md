# Copilot Instructions: Architecture

Goals
- Enable evolvable, testable systems that follow Microsoft guidance and Cloud Design Patterns.
- Favor Clean Architecture with clear boundaries: Presentation, Application, Domain, Infrastructure.

Core Principles
- High cohesion, low coupling, dependency inversion.
- Domain model purity; Infrastructure is replaceable.
- API-first for service contracts; schema/package versioning.
- Security and privacy by design; zero trust assumptions.

Boundaries
- Domain: entities, value objects, domain events, domain services. No external framework coupling.
- Application: use cases, DTOs, validators, orchestration, transactions. No UI or persistence details.
- Infrastructure: EF Core repositories, MSSQL, external services, email, storage, identity providers.
- Presentation: APIs, Blazor/MVC, background workers.

Integration
- Prefer async messaging for cross-service communication (Azure Service Bus/Storage Queues). Idempotent consumers.
- Validate contracts with OpenAPI and consumer-driven contracts.

Orchestration with Aspire
- Model all services, dependencies, and resources declaratively.
- Use service discovery and health checks for resilience.
- Environments: dev/test/prod with consistent topology. Dev harness spins local dependencies.

Data and Persistence
- Use EF Core migrations; keep schema changes reviewed and automated in CI/CD.
- Apply optimistic concurrency and soft deletes where required.
- Partition when needed; use read replicas for reporting.

Observability
- Standardize logging schema (structured), tracing (OTel), and metrics. Correlate across services.
- Centralize logs in Azure Monitor / Application Insights. Define SLOs and alerts.

Security
- Identity via Azure AD/MSAL/Entra. Apply least privilege, managed identity, and Key Vault.
- Defense in depth: validate inputs, enforce authorization, sanitize outputs, encrypt at rest and transit.

Performance and Reliability
- Use caching, rate limiting, circuit breakers, bulkheads, and retries with jitter.
- Capacity plan and load test. Use feature flags for safe rollout.

Delivery
- CI: build, test, analyze, scan. CD: blue/green or canary. Infrastructure as Code (Bicep/Terraform/Azure Developer CLI).
- Treat configuration as code; gate on quality and security checks.

Review Checklist
- Layered boundaries respected
- Contracts versioned and documented
- Resiliency patterns implemented
- Observability and security baselines present
- Deployment strategy defined and repeatable
