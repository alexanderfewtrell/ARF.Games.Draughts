# ARF.Games.Draughts

A browser-based Spanish Draughts (Checkers) game built with .NET Aspire, Blazor Server, and AI opponent.

[![.NET](https://img.shields.io/badge/.NET-10.0-purple)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-blue)](https://blazor.net/)
[![Aspire](https://img.shields.io/badge/.NET-Aspire-green)](https://learn.microsoft.com/dotnet/aspire/)

## 🎮 Overview

ARF.Games.Draughts is a single-player draughts game where you play against a basic AI opponent. The game implements authentic **Spanish Draughts rules** including:

- **Mandatory captures** - You must capture when possible
- **Flying kings** - Kings can move any distance diagonally
- **Multi-capture chains** - Complete all captures in a sequence

## 🚀 Quick Start

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later
- [Visual Studio 2026](https://visualstudio.microsoft.com/) (17.12+) or [VS Code](https://code.visualstudio.com/)

### Run the Game

```bash
# Clone the repository
git clone https://github.com/alexanderfewtrell/ARF.Games.Draughts.git
cd ARF.Games

# Run with .NET Aspire orchestration
dotnet run --project src/Draughts.AppHost
```

The Aspire dashboard will open showing the running services. Click on the **Draughts.Web** endpoint to play!

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────┐
│                  .NET Aspire AppHost                     │
│  ┌─────────────────────┐  ┌─────────────────────────┐   │
│  │   Draughts.Web      │  │    Draughts.Api         │   │
│  │   (Blazor Server)   │──│    (Minimal API)        │   │
│  │   Game UI           │  │    AI Move Endpoint     │   │
│  └─────────┬───────────┘  └───────────┬─────────────┘   │
│            │                          │                  │
│            └──────────┬───────────────┘                  │
│                       ▼                                  │
│            ┌─────────────────────┐                       │
│            │  Draughts.Domain    │                       │
│            │  (Rules Engine)     │                       │
│            └─────────────────────┘                       │
└─────────────────────────────────────────────────────────┘
```

### Projects

| Project | Description |
|---------|-------------|
| `Draughts.AppHost` | .NET Aspire orchestrator |
| `Draughts.Web` | Blazor Server game UI |
| `Draughts.Api` | Minimal API for AI moves |
| `Draughts.Domain` | Game rules and models |
| `Draughts.ServiceDefaults` | Shared Aspire configuration |

## 🎯 Features

- ✅ **Spanish Draughts Rules** - Authentic 8x8 gameplay
- ✅ **AI Opponent** - Basic AI that prioritizes captures
- ✅ **Move Highlighting** - Visual cues for legal moves
- ✅ **Mandatory Capture Prompts** - Clear feedback when capture is required
- ✅ **King Promotion** - Pieces promote at the opposite row
- ✅ **Game Over Detection** - Win/loss/draw detection
- ✅ **Accessibility** - Keyboard navigation and ARIA labels
- ✅ **Observability** - OpenTelemetry via Aspire Dashboard

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet-coverage collect -f cobertura dotnet test
```

## 📚 Documentation

Detailed documentation is available in the [`docs/`](docs/) folder:

- [Technical Documentation](docs/technical/README.md)
  - [Architecture Overview](docs/technical/architecture.md)
  - [Code Overview](docs/technical/code-overview.md)
  - [API Reference](docs/technical/api-reference.md)
  - [Domain Model](docs/technical/domain-model.md)
  - [Development Guide](docs/technical/development-guide.md)
- [Statement of Requirements](docs/001-mvp/SOR/draughts-sor.md)
- [Technical Specifications](docs/001-mvp/specs/overview-spec.md)

## 🛠️ Technology Stack

| Layer | Technology |
|-------|------------|
| Orchestration | .NET Aspire 9.0 |
| Frontend | Blazor Server |
| Backend | ASP.NET Core Minimal APIs |
| Domain | Pure C# (.NET 10) |
| Observability | OpenTelemetry |
| Testing | xUnit, bUnit |

## 📝 License

This project is licensed under the MIT License.

## 🤝 Contributing

Contributions are welcome! Please read the [Development Guide](docs/technical/development-guide.md) for details on the codebase and contribution process.

---

*Built with GitHub Copilot* 🤖
