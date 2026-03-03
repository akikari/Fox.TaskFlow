# Fox.TaskFlow

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![Build and Test](https://img.shields.io/github/actions/workflow/status/akikari/Fox.TaskFlow/build-and-test.yml?branch=main&label=build%20and%20test&color=darkgreen)](https://github.com/akikari/Fox.TaskFlow/actions/workflows/build-and-test.yml)
[![License: MIT](https://img.shields.io/badge/license-MIT-orange.svg)](https://opensource.org/licenses/MIT)

A comprehensive demonstration application showcasing the complete **Fox.\*Kit** ecosystem with Clean Architecture, SOLID principles, and modern .NET 10 practices.

## 📑 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Technologies](#technologies)
- [Workflow System (ChainKit Integration)](#workflow-system-chainkit-integration)
- [Advanced Filtering (OptionKit Integration)](#advanced-filtering-optionkit-integration)
- [Webhook Notifications (RetryKit Integration)](#webhook-notifications-retrykit-integration)
- [Validation Rules](#validation-rules)
- [Localization Architecture](#localization-architecture)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)
- [Testing](#testing)
- [Code Quality](#code-quality)
- [Contributing](#contributing)
- [License](#license)
- [Author](#author)
- [Related Projects](#related-projects)
- [Support](#support)

## 📋Overview

Fox.TaskFlow is an enterprise-grade task management system

### 🎯 Purpose

- **Showcase Fox.\*Kit Integration**: Demonstrate how all Fox packages work together in a production-ready application
- **Best Practices**: Clean Architecture, SOLID, DRY, and Clean Code principles
- **Educational**: Well-documented codebase with XML documentation on every type, property, and method
- **Real-World Patterns**: Chain of Responsibility, Repository, Railway-Oriented Programming, Optional Types

## ✨Features

### Core Functionality
- ✅ **Task Management**: Full CRUD operations with validation
- 🔄 **Workflow System**: State-based transitions via ChainKit pipeline (Validate → Execute → Notify)
- 🔍 **Advanced Filtering**: Optional parameters with Fox.OptionKit (5 filter criteria)
- 🔔 **Webhook Notifications**: Resilient HTTP calls with Fox.RetryKit (3 retries, 1000ms delay)
- ✔️ **Validation**: Fail-fast validation with cascading rules
- 🎯 **Error Handling**: Railway-Oriented Programming with Fox.ResultKit
- 🗺️ **Mapping**: Compile-time DTO/Entity mapping with Fox.MapKit
- 🌍 **Localization**: Server-side localization (English/Hungarian)

### Technical Highlights
- 🏛️ **Clean Architecture**: Domain → Application → Infrastructure → API/Blazor
- 🧱 **SOLID Principles**: Single Responsibility, Open/Closed, Dependency Inversion
- 🔒 **Type Safety**: Compile-time validation message keys, no magic strings
- ⚙️ **Configuration-Driven**: RetryKit policies, validation rules, database settings from appsettings
- 📊 **Comprehensive Testing**: Extensive unit test suite with FluentAssertions covering all critical paths
- 📖 **Full Documentation**: XML comments on all types, properties, and methods (including private)
- 🎨 **Code Structure**: Organized regions (Constants → Fields → Constructors → Properties → Methods)

## 🏗️ Architecture

### Clean Architecture Layers

```
Domain (Entities, Enums, Workflows)
    ↓
Application (Services, Handlers, Validators, DTOs, Interfaces)
    ↓
Infrastructure (Repositories, Services, Data Access, Configuration)
    ↓
Presentation (API, Blazor UI)
```

### Projects

| Project | Responsibility | Key Components |
|---------|---------------|----------------|
| **Fox.TaskFlow.Domain** | Business entities and workflows | `TaskItem`, `TaskStatus`, `TaskTransitionContext` |
| **Fox.TaskFlow.Application** | Business logic and orchestration | `TaskService`, 3 ChainKit handlers, Validators, DTOs |
| **Fox.TaskFlow.Infrastructure** | Data access and external services | `TaskRepository`, `WebhookNotificationService`, EF Core |
| **Fox.TaskFlow.Api** | RESTful API endpoints | Controllers, Middleware, Program.cs |
| **Fox.TaskFlow.Blazor** | Interactive UI | Blazor Server components, Services |
| **Fox.TaskFlow.Tests** | Automated testing | Comprehensive unit test suite (xUnit + FluentAssertions) |

### Key Architectural Decisions

1. **Service Layer Pattern**: Direct `ITaskService` injection instead of MediatR for CRUD simplicity
2. **ChainKit Pipeline**: Workflow transitions use Chain of Responsibility (Validate → Execute → Webhook)
3. **Optional Parameters**: `Option<T>` for filtering instead of nullable reference types
4. **Railway-Oriented Programming**: `Result<T>` eliminates exceptions for business logic errors
5. **Compile-Time Mapping**: Fox.MapKit generates mapping code at build time (no reflection)
6. **Resilient HTTP**: Fox.RetryKit with exponential backoff for webhook notifications
7. **Type-Safe Validation**: `ValidationMessageKeys` constants prevent typos in message keys

## 🧰Technologies

### Core Stack
- **.NET 10.0** with C# 14.0
- **EF Core 10.0.3** (SQLite, migrations, seeding)
- **Blazor Server** (@rendermode="InteractiveServer")
- **ASP.NET Core** (Web API with minimal APIs pattern)

### Fox.\*Kit Packages (7/7 Integrated)

| Package | Version | Usage |
|---------|---------|-------|
| **[Fox.ResultKit](https://github.com/akikari/Fox.ResultKit)** | 1.3.0 | `Result<T>` for error handling, Match pattern |
| **[Fox.ValidationKit.ResultKit](https://github.com/akikari/Fox.ValidationKit)** | 1.0.0 | Fluent validation with fail-fast cascading |
| **[Fox.MapKit](https://github.com/akikari/Fox.MapKit)** | 1.0.0 * | Compile-time object mapping `[MapProfile]` |
| **[Fox.OptionKit](https://github.com/akikari/Fox.OptionKit)** | 1.0.0 | Optional values with JSON converter |
| **[Fox.ChainKit](https://github.com/akikari/Fox.ChainKit)** | 1.0.1 | Chain of Responsibility pattern for workflows |
| **[Fox.ConfigKit.ResultKit](https://github.com/akikari/Fox.ConfigKit)** | 1.0.4 | Type-safe configuration validation |
| **[Fox.RetryKit](https://github.com/akikari/Fox.RetryKit)** | 1.0.0 | Automatic retry with exponential backoff |

**\* Fox.MapKit is not yet publicly available**. This project includes the runtime library (`libs/Fox.MapKit.dll`) as a direct assembly reference instead of a NuGet package. The mapping profile classes serve documentation purposes only; actual mapping is provided by the Fox.MapKit runtime.

### Testing & Quality
- **xUnit 3.1.4** - Test framework
- **FluentAssertions** - Assertion library
- **Microsoft.Extensions.Localization** - Standard .NET localization
- **System.Text.Json** - JSON serialization with custom converters

## 🔄Workflow System (ChainKit Integration)

### Task Status States

```
Created → InProgress → Completed
   ↓           ↓
Cancelled   Cancelled
   ↑           ↑
   └─────┬─────┘
       Back
```

### Transition Pipeline

Every status transition flows through a **3-handler ChainKit pipeline**:

```csharp
ValidateTransitionHandler → ExecuteTransitionHandler → WebhookNotificationHandler
```

1. **ValidateTransitionHandler**: Checks if transition is allowed (e.g., Created → InProgress, Created → Completed)
2. **ExecuteTransitionHandler**: Applies the status change if validation passed
3. **WebhookNotificationHandler**: Sends HTTP notification with Fox.RetryKit resilience

**Key Benefit**: New handlers can be added without modifying existing code (Open/Closed Principle)

## 🔍Advanced Filtering (OptionKit Integration)

The application demonstrates **Fox.OptionKit**

```csharp
public sealed class GetTasksRequest
{
    public Option<string> SearchTitle { get; init; } = Option.None<string>();
    public Option<TaskStatus> FilterByStatus { get; init; } = Option.None<TaskStatus>();
    public Option<DateTime> DueBefore { get; init; } = Option.None<DateTime>();
    public Option<DateTime> DueAfter { get; init; } = Option.None<DateTime>();
    public Option<bool> ShowCompleted { get; init; } = Option.None<bool>();
}
```

### Benefits Over Nullable Types

- ✅ **Explicit Intent**: `Option.None<T>()` vs `null` makes "no value provided" clear
- ✅ **Type Safety**: Compiler enforces `.Match()` handling of both cases
- ✅ **No NullReferenceException**: Pattern matching eliminates null checks
- ✅ **Functional Programming**: Railway-oriented style with explicit branching

## 🔔Webhook Notifications (RetryKit Integration)

**WebhookNotificationService** uses Fox.RetryKit

```csharp
RetryPolicy.Retry(configuration.MaxRetries, TimeSpan.FromMilliseconds(configuration.RetryDelayMs))
    .Handle<HttpRequestException>()
    .Handle<TaskCanceledException>()
    .OnRetry((exception, attempt, delay) => logger.LogWarning(...))
```

**Configuration** (`appsettings.json`):
- `MaxRetries`: Number of retry attempts (default: 3)
- `RetryDelayMs`: Delay between retries in milliseconds (default: 1000)
- `TimeoutSeconds`: HTTP request timeout (default: 30)

**Benefits**:
- 🔄 Automatic retry on transient failures
- ⏱️ Configurable delay between attempts (via appsettings)
- 📊 Retry attempt logging for observability
- 🛡️ Never breaks the ChainKit pipeline (always returns `HandlerResult.Continue`)

## ✔️ Validation Rules

### CreateTaskRequest / UpdateTaskRequest
- **Title**: Required, 3-200 characters
- **Description**: Required, 10-2000 characters  
- **DueDate**: Optional, must be future date

### TransitionTaskRequest
- **TargetStatus**: Must be valid `TaskStatus` enum value

All validation messages:
- ✅ Localized in English and Hungarian
- ✅ Type-safe with `ValidationMessageKeys` constants
- ✅ Field-specific (e.g., "Title is required" not "Validation failed")
- ✅ Parameterized (e.g., "Title must be at least {0} characters" with actual value)

## 🌍Localization Architecture

### Server-Side Flow

1. **User Selection**: Blazor UI language selector (English/Magyar)
2. **HTTP Header**: `LanguageState` updates `HttpClient.DefaultRequestHeaders.AcceptLanguage`
3. **Middleware**: API parses `Accept-Language` header, sets `CultureInfo.CurrentCulture`
4. **Validation**: `IStringLocalizerFactory` retrieves localized messages from `ValidationMessages.resx`
5. **Response**: API returns server-localized error messages
6. **Display**: Blazor shows messages directly (no client-side translation)

### Benefits
- **Single Source of Truth**: `ValidationMessages.resx` / `.hu.resx`
- **Standard .NET**: Uses built-in `IStringLocalizer`
- **Simplified Client**: No translation dictionaries or error code mapping
- **Maintainable**: Add new languages by creating `.resx` files

## 🚀Getting Started

### Prerequisites

- **.NET 10 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/10.0))
- **Visual Studio 2026** or **VS Code** with C# Dev Kit
- **Git** for cloning

### Installation

```bash
# Clone the repository
git clone https://github.com/akikari/Fox.TaskFlow.git
cd Fox.TaskFlow

# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test
```

### Running the Application

#### Option 1: Run Both Projects

```bash
# Terminal 1 - API
cd src/Fox.TaskFlow.Api
dotnet run
# API: https://localhost:7144

# Terminal 2 - Blazor UI
cd src/Fox.TaskFlow.Blazor
dotnet run
# Blazor: https://localhost:7138
```

#### Option 2: Visual Studio

1. Right-click solution → **Set Startup Projects**
2. Select **Multiple startup projects**
3. Set both `Fox.TaskFlow.Api` and `Fox.TaskFlow.Blazor` to **Start**
4. Press `F5`

### Testing Localization

1. Open Blazor UI: `https://localhost:7138`
2. Click language selector in header: **English** → **Magyar**
3. Navigate to **"Create Task"**
4. Submit empty form
5. Observe localized errors:
   - **English**: "Title is required"
   - **Magyar**: "A cím kötelező"

### Testing OptionKit Filtering

1. Navigate to **"Tasks"** page
2. Click **"Show Filters"**
3. Enter filter criteria:
   - Search title: "auth"
   - Status: InProgress
   - Due before: 2026-12-31
4. Click **"Apply Filters"**
5. Observe filtered results with total count

## 📂Project Structure

```
Fox.TaskFlow/
├── src/
│   ├── Fox.TaskFlow.Domain/              # Core domain layer
│   │   ├── Entities/
│   │   │   └── TaskItem.cs               # Domain aggregate root
│   │   ├── Enums/
│   │   │   └── TaskStatus.cs             # Workflow states
│   │   └── Workflows/
│   │       ├── TaskTransitionContext.cs  # ChainKit context
│   │       └── TaskWorkflow.cs           # Transition rules
│   ├── Fox.TaskFlow.Application/         # Business logic layer
│   │   ├── Services/
│   │   │   └── TaskService.cs            # Main business service
│   │   ├── Handlers/                     # ChainKit handlers
│   │   │   ├── ValidateTransitionHandler.cs
│   │   │   ├── ExecuteTransitionHandler.cs
│   │   │   └── WebhookNotificationHandler.cs
│   │   ├── Validators/
│   │   │   ├── CreateTaskRequestValidator.cs
│   │   │   ├── UpdateTaskRequestValidator.cs
│   │   │   ├── TransitionTaskRequestValidator.cs
│   │   │   ├── LocalizedMessageProvider.cs
│   │   │   └── ValidationMessageKeys.cs  # Type-safe constants
│   │   ├── DTOs/
│   │   │   ├── Requests/                 # Command DTOs
│   │   │   └── Responses/                # Query DTOs
│   │   ├── Interfaces/
│   │   │   ├── ITaskService.cs
│   │   │   ├── ITaskRepository.cs
│   │   │   └── IWebhookNotificationService.cs
│   │   ├── Configuration/
│   │   │   └── ValidationConfiguration.cs
│   │   └── Resources/
│   │       ├── ValidationMessages.resx    # English
│   │       └── ValidationMessages.hu.resx # Hungarian
│   ├── Fox.TaskFlow.Infrastructure/       # Data access layer
│   │   ├── Data/
│   │   │   ├── TaskFlowDbContext.cs
│   │   │   └── Entities/
│   │   │       └── TaskEntity.cs         # EF Core entity
│   │   ├── Repositories/
│   │   │   └── TaskRepository.cs
│   │   ├── Services/
│   │   │   └── WebhookNotificationService.cs  # RetryKit integration
│   │   ├── Configuration/
│   │   │   ├── TaskFlowConfiguration.cs
│   │   │   ├── DatabaseConfiguration.cs
│   │   │   └── WebhookConfiguration.cs
│   │   ├── Mappings/
│   │   │   └── TaskEntityMappingProfile.cs  # MapKit profile
│   │   └── Migrations/
│   ├── Fox.TaskFlow.Api/                  # Web API layer
│   │   ├── Controllers/
│   │   │   └── TasksController.cs        # RESTful endpoints
│   │   └── Program.cs                    # Startup configuration
│   └── Fox.TaskFlow.Blazor/              # UI layer
│       ├── Components/
│       │   ├── Layout/
│       │   │   ├── MainLayout.razor
│       │   │   └── NavMenu.razor
│       │   └── Pages/
│       │       ├── Home.razor
│       │       └── Tasks.razor            # Task management UI with filters
│       ├── Services/
│       │   ├── TaskService.cs            # HTTP client wrapper
│       │   └── LanguageState.cs          # Localization state
│       └── Models/
│           ├── TaskDto.cs
│           └── FilteredTasksDto.cs
└── tests/
    └── Fox.TaskFlow.Tests/               # Unit tests
        ├── Domain/
        │   └── TaskItemTests.cs          # 11 tests
        ├── Application/
        │   ├── TaskServiceTests.cs       # 15 tests
        │   ├── TaskServiceFilterTests.cs # 6 tests (OptionKit)
        │   └── WebhookNotificationHandlerTests.cs  # 3 tests (ChainKit)
        └── Validators/
            └── ValidatorTests.cs         # Validation tests
```

## 🌐API Endpoints

### Tasks Resource

| Method | Endpoint | Description | Request Body |
|--------|----------|-------------|--------------|
| `GET` | `/api/tasks` | Get all tasks | - |
| `GET` | `/api/tasks/filter` | Get filtered tasks | Query params |
| `GET` | `/api/tasks/{id}` | Get task by ID | - |
| `POST` | `/api/tasks` | Create new task | `CreateTaskRequest` |
| `PUT` | `/api/tasks/{id}` | Update task | `UpdateTaskRequest` |
| `POST` | `/api/tasks/{id}/transition` | Transition status | `TransitionTaskRequest` |
| `DELETE` | `/api/tasks/{id}` | Delete task | - |

### Filter Query Parameters (OptionKit)

```
GET /api/tasks/filter?searchTitle=auth&status=1&dueBefore=2025-12-31&showCompleted=false
```

- `searchTitle`: Search in title (case-insensitive)
- `status`: Filter by TaskStatus enum (0=Created, 1=InProgress, 2=Completed, 3=Cancelled)
- `dueBefore`: Tasks due before date (ISO 8601)
- `dueAfter`: Tasks due after date (ISO 8601)
- `showCompleted`: Include completed tasks (default: true)

### Example: Create Task

**Request:**
```json
POST /api/tasks
Content-Type: application/json

{
  "title": "Implement user authentication",
  "description": "Add JWT-based authentication with refresh tokens",
  "dueDate": "2025-03-15T00:00:00Z"
}
```

**Response (Success):**
```json
HTTP/1.1 201 Created

{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Implement user authentication",
  "description": "Add JWT-based authentication with refresh tokens",
  "dueDate": "2025-03-15T00:00:00Z",
  "status": "Created",
  "createdAt": "2025-03-02T10:00:00Z",
  "updatedAt": "2025-03-02T10:00:00Z"
}
```

**Response (Validation Error):**
```json
HTTP/1.1 400 Bad Request

{
  "errors": [
    { "message": "Title must be at least 3 characters long" }
  ]
}
```

## ⚙️ Configuration

### API (appsettings.json)

```json
{
  "TaskFlow": {
    "Database": {
      "ConnectionString": "Data Source=taskflow.db",
      "CommandTimeoutSeconds": 30,
      "EnableDetailedErrors": false,
      "EnableSensitiveDataLogging": false
    },
    "Webhook": {
      "Url": "https://example.com/webhook",
      "Enabled": false,
      "MaxRetries": 3,
      "RetryDelayMs": 1000,
      "TimeoutSeconds": 30
    }
  },
  "Validation": {
    "TitleMinLength": 3,
    "TitleMaxLength": 200,
    "DescriptionMinLength": 10,
    "DescriptionMaxLength": 2000
  }
}
```

### Blazor (appsettings.json)

```json
{
  "ApiBaseUrl": "https://localhost:7144"
}
```

## 🧪Testing

### Test Framework

- **Framework**: xUnit 3.1.4
- **Assertions**: FluentAssertions
- **Coverage**: Comprehensive test suite covering domain logic, service operations, validators, and Fox.*Kit integrations

### Test Categories

| Category | Coverage |
|----------|----------|
| Domain Logic | Task transitions, workflow validation, entity operations |
| Service Layer | CRUD operations, error handling, business logic orchestration |
| Validators | Request validation rules for Create, Update, and Transition operations |
| OptionKit Filtering | All filter combinations and optional parameter handling |
| ChainKit Handlers | Pipeline execution, webhook notifications, transition handling |

### Running Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test class
dotnet test --filter "FullyQualifiedName~TaskServiceFilterTests"
```

## 📚Code Quality

### SOLID Principles ✅

- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: ChainKit handlers extensible without modification
- **Liskov Substitution**: No problematic inheritance hierarchies
- **Interface Segregation**: Small, focused interfaces (`ITaskService`, `ITaskRepository`)
- **Dependency Inversion**: Interfaces in Application, implementations in Infrastructure

### DRY (Don't Repeat Yourself) ✅

- **Validation Error Handling**: Extracted to `GetLocalizedValidationMessage()` helper
- **Blazor Error Handling**: Extracted to `HandleErrorResponseAsync()` helper
- **Webhook Payloads**: Extracted to `CreateWebhookPayload()` helper
- **Validator Rules**: Shared via base classes where appropriate

### Clean Code ✅

- ✅ **Meaningful Names**: `TaskTransitionContext`, `WebhookNotificationHandler`
- ✅ **Small Functions**: Most methods <30 lines
- ✅ **No Magic Numbers**: `ValidationConfiguration` centralizes all constraints
- ✅ **Type Safety**: `ValidationMessageKeys` constants prevent typos
- ✅ **XML Documentation**: Every type, property, and method documented
- ✅ **Consistent Formatting**: 4-space indentation, CRLF line endings, 100-char line limit
- ✅ **Region Organization**: Constants → Fields → Constructors → Properties → Methods

## 🤝Contributing

Contributions are welcome!

### What We Welcome

- ✅ **Bug fixes** - Issues with existing functionality
- ✅ **Documentation improvements** - Clarifications, examples, typo fixes
- ✅ **Performance optimizations** - Without breaking API compatibility
- ✅ **New features** - Following existing patterns and Clean Architecture

### What We Generally Do Not Accept

- ❌ Breaking API changes without discussion
- ❌ Large feature additions that increase complexity without clear value
- ❌ Changes that violate SOLID principles or Clean Architecture

If you want to propose a significant change, please open an issue first to discuss whether it aligns with the project's philosophy.

### Build Policy

The project enforces a **strict build policy** to ensure code quality:

- ❌ **No errors allowed** - Build must be error-free
- ❌ **No warnings allowed** - All compiler warnings must be resolved
- ❌ **No messages allowed** - Informational messages must be suppressed or addressed

All pull requests must pass this requirement.

### Code Quality Standards

Fox.TaskFlow follows strict coding standards:

- **Comprehensive unit tests for core business logic** (xUnit + FluentAssertions)
- **Test coverage focused on critical paths** - Domain logic, service layer operations, and Fox.*Kit integrations are thoroughly tested
- **XML documentation for all types, properties, and methods** - Clear, concise documentation (including private members)
- **Follow Microsoft coding conventions** - See `.github/copilot-instructions.md` for project-specific style
- **Zero warnings, zero errors build policy** - Strict enforcement

### Code Style

- Follow the existing code style (see `.github/copilot-instructions.md`)
- Use file-scoped namespaces
- Enable nullable reference types
- Add XML documentation with decorators (98 characters width)
- Write unit tests for new features
- Use expression-bodied members for simple properties
- Auto-properties preferred over backing fields
- Organize code with regions (Constants → Fields → Constructors → Properties → Methods)
- Private fields: camelCase without underscore prefix

### How to Contribute

1. Fork the repository
2. Create a feature branch from `main`
3. Follow the coding standards in `.github/copilot-instructions.md`
4. Ensure all tests pass (`dotnet test`)
5. Submit a pull request

## 📝License

This project is licensed

## 👤Author

**Károly Akácz**

- GitHub: [@akikari](https://github.com/akikari)
- Repository: [Fox.TaskFlow](https://github.com/akikari/Fox.TaskFlow)

## 🔗Related Projects

- [Fox.ResultKit](https://github.com/akikari/Fox.ResultKit) - Lightweight Result pattern library for Railway Oriented Programming
- [Fox.ValidationKit.ResultKit](https://www.nuget.org/packages/Fox.ValidationKit.ResultKit) - Fluent validation with Result pattern
- [Fox.MapKit](https://github.com/akikari/Fox.MapKit) - Compile-time object mapping library
- [Fox.OptionKit](https://github.com/akikari/Fox.OptionKit) - Optional types for explicit value absence
- [Fox.ChainKit](https://github.com/akikari/Fox.ChainKit) - Chain of Responsibility pattern implementation
- [Fox.ConfigKit](https://github.com/akikari/Fox.ConfigKit) - Configuration validation at startup
- [Fox.RetryKit](https://github.com/akikari/Fox.RetryKit) - Automatic retry with exponential backoff

## 📞Support

For issues, questions
