# Architecture Overview

> **Document Type:** Technical Architecture  
> **Version:** 1.0.0  
> **Last Updated:** 2025

## Table of Contents

- [System Context](#system-context)
- [Container Architecture](#container-architecture)
- [Component Architecture](#component-architecture)
- [Data Flow](#data-flow)
- [Technology Decisions](#technology-decisions)
- [Cross-Cutting Concerns](#cross-cutting-concerns)

---

## System Context

The ARF.Games.Draughts system is a self-contained desktop application with no external dependencies. It provides a browser-based interface for playing Spanish Draughts against an AI opponent.

```mermaid
C4Context
    title System Context Diagram - Draughts Game

    Person(player, "Player", "A person who wants to play Draughts")
    
    System(draughts, "Draughts Game System", "Browser-based Spanish Draughts game with AI opponent")
    
    Rel(player, draughts, "Plays game via", "HTTPS/WebSocket")
```

### Key Actors

| Actor | Description | Interactions |
|-------|-------------|--------------|
| Player | Human user playing the game | Interacts with UI, makes moves, starts new games |

### External Systems

None - This is a standalone MVP with no external integrations.

---

## Container Architecture

The system is composed of multiple containers orchestrated by .NET Aspire:

```mermaid
flowchart TB
    subgraph Host["??? Local Machine"]
        subgraph Aspire["Draughts.AppHost<br/>.NET Aspire Orchestrator"]
            direction TB
            
            subgraph Web["Draughts.Web"]
                BlazorUI["Blazor Server<br/>Interactive UI"]
                SignalR["SignalR Hub<br/>Real-time Updates"]
            end
            
            subgraph Api["Draughts.Api"]
                MinimalApi["Minimal API<br/>AI Endpoints"]
                AiService["AI Service<br/>Move Selection"]
            end
        end
        
        subgraph Shared["Shared Libraries"]
            Domain["Draughts.Domain<br/>Rules Engine"]
            ServiceDefaults["ServiceDefaults<br/>Observability"]
        end
        
        Browser["?? Browser"]
    end
    
    Browser -->|HTTPS/WSS| BlazorUI
    BlazorUI --> SignalR
    BlazorUI --> Domain
    BlazorUI -->|HTTP POST| MinimalApi
    MinimalApi --> AiService
    AiService --> Domain
    Web --> ServiceDefaults
    Api --> ServiceDefaults
```

### Container Descriptions

| Container | Technology | Purpose | Port (Dev) |
|-----------|------------|---------|------------|
| **Draughts.AppHost** | .NET Aspire | Orchestration, service discovery, dashboard | Dynamic |
| **Draughts.Web** | Blazor Server | Game UI, user interaction, local rules validation | HTTPS |
| **Draughts.Api** | Minimal API | AI move computation endpoint | HTTPS |
| **Draughts.Domain** | Class Library | Pure domain logic, rules engine | N/A |
| **ServiceDefaults** | Class Library | Shared Aspire configurations | N/A |

---

## Component Architecture

### Draughts.Web Components

```mermaid
flowchart TB
    subgraph Web["Draughts.Web"]
        subgraph Pages["Pages"]
            GameBoard["GameBoard.razor<br/>Main game page"]
        end
        
        subgraph Components["Shared Components"]
            StatusBar["StatusBar.razor<br/>Turn/status display"]
            RestartDialog["RestartDialog.razor<br/>Game over dialog"]
        end
        
        subgraph Services["Injected Services"]
            RulesEngine["IRulesEngine<br/>Move validation"]
            HttpClient["HttpClient<br/>API communication"]
        end
    end
    
    GameBoard --> StatusBar
    GameBoard --> RestartDialog
    GameBoard --> RulesEngine
    GameBoard --> HttpClient
```

### Draughts.Api Components

```mermaid
flowchart TB
    subgraph Api["Draughts.Api"]
        subgraph Endpoints["API Endpoints"]
            HealthEndpoint["/api/health<br/>Health check"]
            AiMoveEndpoint["/api/ai/move<br/>AI move computation"]
        end
        
        subgraph Services["Services"]
            AiService["AiService<br/>Move selection logic"]
        end
        
        subgraph Dto["Data Transfer Objects"]
            AiMoveRequest["AiMoveRequest"]
            BoardStateDto["BoardStateDto"]
            MoveDto["MoveDto"]
            PieceDto["PieceDto"]
        end
    end
    
    AiMoveEndpoint --> AiService
    AiMoveEndpoint --> AiMoveRequest
    AiService --> BoardStateDto
    AiService --> MoveDto
```

### Draughts.Domain Components

```mermaid
flowchart TB
    subgraph Domain["Draughts.Domain"]
        subgraph Models["Models"]
            Board["Board<br/>8x8 grid state"]
            Piece["Piece<br/>Man/King with owner"]
            Move["Move<br/>From/To with captures"]
            Player["Player<br/>White/Black enum"]
            PieceType["PieceType<br/>Man/King enum"]
        end
        
        subgraph Rules["Rules"]
            IRulesEngine["IRulesEngine<br/>Contract interface"]
            RulesEngineStub["RulesEngineStub<br/>Spanish rules impl"]
        end
    end
    
    RulesEngineStub --> Board
    RulesEngineStub --> Move
    RulesEngineStub --> Piece
    IRulesEngine --> RulesEngineStub
```

---

## Data Flow

### Player Move Sequence

```mermaid
sequenceDiagram
    participant Player
    participant GameBoard
    participant RulesEngine
    participant Api
    participant AiService

    Player->>GameBoard: Click piece
    GameBoard->>RulesEngine: GetLegalMoves(board, White)
    RulesEngine-->>GameBoard: List<Move>
    GameBoard->>GameBoard: Highlight legal destinations

    Player->>GameBoard: Click destination
    GameBoard->>RulesEngine: ApplyMove(board, move)
    RulesEngine-->>GameBoard: Updated board
    GameBoard->>RulesEngine: IsGameOver(board)
    RulesEngine-->>GameBoard: false

    GameBoard->>GameBoard: Switch to AI turn
    GameBoard->>Api: POST /api/ai/move
    Api->>AiService: GetMove(boardState, Black)
    AiService->>RulesEngine: GetLegalMoves(board, Black)
    RulesEngine-->>AiService: List<Move>
    AiService->>AiService: Select best move
    AiService-->>Api: MoveDto
    Api-->>GameBoard: MoveDto

    GameBoard->>RulesEngine: ApplyMove(board, aiMove)
    RulesEngine-->>GameBoard: Updated board
    GameBoard->>Player: Render updated board
```

### Game State Lifecycle

```mermaid
stateDiagram-v2
    [*] --> Initialized: Page Load
    Initialized --> PlayerTurn: Board Setup
    
    PlayerTurn --> ValidatingMove: Click Destination
    ValidatingMove --> PlayerTurn: Invalid Move
    ValidatingMove --> ApplyingMove: Valid Move
    
    ApplyingMove --> CheckingGameOver: Move Applied
    CheckingGameOver --> GameOver: Game Ended
    CheckingGameOver --> AiTurn: Continue
    
    AiTurn --> WaitingForAi: Request AI Move
    WaitingForAi --> ApplyingAiMove: AI Response
    ApplyingAiMove --> CheckingGameOver: Move Applied
    
    GameOver --> Initialized: Restart Game
    GameOver --> [*]: Close
```

---

## Technology Decisions

### Architecture Style: Clean Architecture

```mermaid
flowchart TB
    subgraph Layers["Clean Architecture Layers"]
        subgraph Presentation["Presentation Layer"]
            Web["Draughts.Web"]
            Api["Draughts.Api"]
        end
        
        subgraph Application["Application Layer"]
            Services["AI Service<br/>Move Selection"]
        end
        
        subgraph Domain["Domain Layer"]
            Models["Domain Models"]
            Rules["Rules Engine"]
        end
    end
    
    Presentation --> Application
    Application --> Domain
    Presentation --> Domain
```

### Key Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| **UI Framework** | Blazor Server | Rich interactivity, server-side state, SignalR real-time updates |
| **API Style** | Minimal APIs | Lightweight, fast, sufficient for single endpoint |
| **Orchestration** | .NET Aspire | Service discovery, health checks, observability out-of-box |
| **State Management** | In-Memory | MVP scope, no persistence requirement |
| **Rules Engine** | Pure Domain Service | Testable, framework-agnostic, reusable |

### Dependency Direction

```
Draughts.AppHost
    ??? Draughts.Web
    ?   ??? Draughts.Domain
    ?   ??? Draughts.ServiceDefaults
    ??? Draughts.Api
        ??? Draughts.Domain
        ??? Draughts.ServiceDefaults
```

---

## Cross-Cutting Concerns

### Observability

```mermaid
flowchart LR
    subgraph Apps["Applications"]
        Web["Draughts.Web"]
        Api["Draughts.Api"]
    end
    
    subgraph Telemetry["OpenTelemetry"]
        Logs["Structured Logs"]
        Metrics["Metrics"]
        Traces["Distributed Traces"]
    end
    
    subgraph Dashboard["Aspire Dashboard"]
        LogViewer["Log Viewer"]
        MetricCharts["Metrics"]
        TraceExplorer["Trace Explorer"]
    end
    
    Web --> Logs
    Web --> Metrics
    Web --> Traces
    Api --> Logs
    Api --> Metrics
    Api --> Traces
    Logs --> LogViewer
    Metrics --> MetricCharts
    Traces --> TraceExplorer
```

### Service Defaults Configuration

The `ServiceDefaults` project provides shared configuration:

| Feature | Description |
|---------|-------------|
| **Health Checks** | `/health` and `/alive` endpoints |
| **Service Discovery** | Automatic endpoint resolution |
| **Resilience** | Standard HTTP client retry policies |
| **OpenTelemetry** | Logging, metrics, and tracing |

### Security Model

| Aspect | Implementation | Notes |
|--------|----------------|-------|
| Authentication | None | MVP single-player |
| Authorization | None | No roles required |
| HTTPS | Required | Development certificates |
| CORS | Development policy | AllowAnyOrigin for local dev |
| Input Validation | API-level | Board state validation |

---

## Deployment Model

### Development Environment

```mermaid
flowchart TB
    subgraph Dev["Developer Machine"]
        IDE["Visual Studio / VS Code"]
        Aspire["Aspire Dashboard"]
        
        subgraph Containers["Running Services"]
            WebDev["Draughts.Web<br/>localhost:XXXXX"]
            ApiDev["Draughts.Api<br/>localhost:XXXXX"]
        end
    end
    
    IDE --> Containers
    Aspire --> Containers
```

### Running the Application

```bash
# Option 1: Aspire orchestration (recommended)
dotnet run --project src/Draughts.AppHost

# Option 2: Individual projects
dotnet run --project src/Draughts.Api &
dotnet run --project src/Draughts.Web
```

---

## Future Considerations

### Potential Enhancements

1. **Multiplayer Support** - Add SignalR hub for real-time games
2. **Persistence** - Add PostgreSQL for game history
3. **AI Improvements** - Implement minimax with alpha-beta pruning
4. **Cloud Deployment** - Azure Container Apps with Aspire

### Scalability Path

```mermaid
flowchart LR
    MVP["MVP<br/>(In-Memory)"] --> V2["V2<br/>(Persistence)"] --> V3["V3<br/>(Multiplayer)"] --> V4["V4<br/>(Cloud)"]
```

---

*See also: [Code Overview](code-overview.md) | [API Reference](api-reference.md) | [Domain Model](domain-model.md)*
