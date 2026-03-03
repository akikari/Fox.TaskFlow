# Changelog

All notable changes to the Fox.TaskFlow project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

_No unreleased changes yet._

## [1.0.0] - 2026-03-03

### Added

#### Complete Fox.\*Kit Integration
- All 7 Fox.\*Kit packages integrated and demonstrated:
  - Fox.ResultKit 1.3.0 for Railway-Oriented Programming
  - Fox.ValidationKit.ResultKit 1.0.0 for fluent validation
  - Fox.MapKit 1.0.0 for compile-time mapping (local DLL reference, not yet published)
  - Fox.OptionKit 1.0.0 for optional types
  - Fox.ChainKit 1.0.1 for Chain of Responsibility pattern
  - Fox.ConfigKit + Fox.ConfigKit.ResultKit for configuration validation
  - Fox.RetryKit 1.0.0 for resilient HTTP calls
- Fox.MapKit included as direct assembly reference (`libs/Fox.MapKit.dll`) until public release

#### ChainKit Workflow Pipeline
- Three-handler chain for task status transitions:
  - ValidateTransitionHandler: Checks if transition is allowed
  - ExecuteTransitionHandler: Applies the status change
  - WebhookNotificationHandler: Sends HTTP notifications with retry
- Extensible pipeline following Open/Closed Principle

#### OptionKit Advanced Filtering
- Five optional filter parameters with explicit value absence:
  - SearchTitle: Case-insensitive title search
  - FilterByStatus: Status-based filtering
  - DueBefore/DueAfter: Date range filtering
  - ShowCompleted: Toggle completed tasks visibility
- Pattern matching with `.Match()` for type-safe null handling
- JSON converter for seamless API integration

#### RetryKit Integration
- Automatic retry for webhook notifications:
  - Configurable retry attempts (MaxRetries from appsettings, default: 3)
  - Configurable delay between retries (RetryDelayMs from appsettings, default: 1000ms)
  - Handles HttpRequestException and TaskCanceledException
  - Never breaks ChainKit pipeline (always returns HandlerResult.Continue)
- Retry attempt logging for observability

#### Blazor Filter UI
- Collapsible filter panel with:
  - Text search input
  - Status dropdown (Created, InProgress, Completed, Cancelled)
  - Date pickers for due date range
  - Show/hide completed toggle
  - Apply and Clear buttons
- Real-time filtered results with total count display

#### Task Management System
- Full CRUD operations with validation
- State-based workflow transitions (Created → InProgress → Completed/Cancelled)
- Server-side localization (English/Hungarian)
- SQLite database with EF Core 10.0.3
- RESTful API with Blazor Server UI

#### Validation System
- Fail-fast validation with cascading rules
- Type-safe message keys (ValidationMessageKeys constants)
- Localized error messages via IStringLocalizer
- Configuration-driven validation rules (appsettings.json)
- Parameterized error messages (e.g., "Title must be at least {0} characters")

#### Clean Architecture Implementation
- Domain layer: Entities, Enums, Workflows
- Application layer: Services, Handlers, Validators, DTOs, Interfaces
- Infrastructure layer: Repositories, Services, Data Access, Configuration
- Presentation layer: API and Blazor UI
- Dependency rule: Inner layers have no knowledge of outer layers

#### Documentation
- Comprehensive README.md with examples and usage patterns
- XML documentation for all types, properties, and methods (including private)
- Code organized with regions (Constants → Fields → Constructors → Properties → Methods)
- Contributing guidelines with build policy and code quality standards
- CHANGELOG.md following Keep a Changelog format

#### Tests
- Comprehensive unit test suite covering:
  - Domain logic (task transitions, validation)
  - Application services (CRUD operations, error handling)
  - OptionKit filtering (all filter combinations)
  - ChainKit handlers (webhook notifications)
  - Validators (CreateTaskRequest, UpdateTaskRequest, TransitionTaskRequest)
- xUnit 3.1.4 test framework
- FluentAssertions for expressive assertions
- Moq for mocking dependencies

### Initial Release Notes
- Production-ready demonstration application
- Clean Architecture with SOLID principles
- 7/7 Fox.\*Kit packages integrated
- All nullable reference types enabled
- Follows Microsoft coding conventions
- Zero warnings, zero errors build policy
