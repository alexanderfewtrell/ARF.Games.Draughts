# Copilot Instructions: Front End Development

Context
- .NET developer stack. Favor Blazor (Server/WebAssembly/SSR) or ASP.NET Core MVC/Razor Pages.

General Principles
- Accessibility-first (WCAG 2.1 AA), responsive design, and performance budgets.
- Type-safe APIs and models. Prefer shared contracts via source generators or shared projects.
- Keep state predictable; avoid hidden coupling.

Framework Guidance
- Blazor: use components with `@code`-behind partial classes; prefer DI over singletons. Avoid heavy object graphs in JS interop.
- ASP.NET Core MVC/Razor: use ViewModels/DTOs; avoid domain entities in views. Tag Helpers over raw HTML helpers.

Styling and UX
- Use Microsoft Fluent UI (Web/Blazor) where appropriate for consistency.
- BEM or utility-first (Tailwind) conventions; avoid global CSS conflicts.
- Respect prefers-reduced-motion and color scheme media queries.

Performance
- Lazy-load routes/components; tree-shake; minify; compress (Brotli).
- Optimize images (AVIF/WebP), use `loading="lazy"`.
- Avoid large JS interop payloads in Blazor; stream where possible.

API Integration
- Use generated clients (NSwag) or `HttpClient` with `IHttpClientFactory` and resiliency policies.
- Use problem+json for errors; map to UI messages.

Security
- Use ASP.NET Core Identity or Azure AD/MSAL for auth.
- Never store tokens in localStorage for SPA; prefer MSAL default storage and SameSite cookies.
- Validate all user input; sanitize/encode outputs.

Testing
- Component tests: bUnit (Blazor), Playwright for E2E, Jest/RTL for React.
- Accessibility checks via axe-core/Playwright.

Internationalization
- Use `IStringLocalizer` (Blazor/MVC).

Build and Delivery
- Use .NET SDK for Blazor/MVC.
- CI publishes artifacts with integrity; enable CSP, SRI, and security headers.

Code Review Checklist
- Accessibility and semantics
- Type safety and contracts
- Efficient rendering and state updates
- Error and loading states accounted for
- Security headers and auth flows
