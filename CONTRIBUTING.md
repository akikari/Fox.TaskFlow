# Contributing to Fox.TaskFlow

Thank you for considering contributing to Fox.TaskFlow! This document provides guidelines and instructions for contributing to the project.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How Can I Contribute?](#how-can-i-contribute)
- [Development Setup](#development-setup)
- [Coding Standards](#coding-standards)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)
- [Testing Guidelines](#testing-guidelines)
- [Documentation](#documentation)

## Code of Conduct

### Our Pledge

We are committed to providing a welcoming and inclusive environment for all contributors, regardless of experience level, background, or identity.

### Expected Behavior

- Be respectful and considerate in all interactions
- Welcome newcomers and help them get started
- Provide constructive feedback
- Focus on what is best for the project and community

### Unacceptable Behavior

- Harassment, discrimination, or offensive comments
- Trolling, insulting comments, or personal attacks
- Publishing others' private information
- Any conduct that would be inappropriate in a professional setting

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check existing issues to avoid duplicates.

**When reporting bugs, include:**
- Clear, descriptive title
- Exact steps to reproduce
- Expected vs actual behavior
- .NET version, OS, and relevant environment details
- Code samples or error messages (use markdown code blocks)
- Screenshots if applicable

**Bug Report Template:**
```markdown
**Description:**
A clear description of the bug.

**Steps to Reproduce:**
1. Go to '...'
2. Click on '....'
3. See error

**Expected Behavior:**
What you expected to happen.

**Actual Behavior:**
What actually happened.

**Environment:**
- .NET SDK: 10.0.x
- OS: Windows 11 / macOS / Linux
- Browser (if Blazor): Chrome 120

**Additional Context:**
Any other relevant information.
```

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues.

**When suggesting enhancements, include:**
- Clear, descriptive title
- Detailed description of the proposed functionality
- Rationale: Why is this enhancement valuable?
- Examples of how the feature would be used
- Mockups or diagrams if applicable

### Contributing Code

1. **Fork the Repository**
   ```bash
   git clone https://github.com/akikari/Fox.TaskFlow.git
   cd Fox.TaskFlow
   ```

2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/amazing-feature
   ```

3. **Make Your Changes**
   - Follow coding standards (see below)
   - Write tests for new functionality
   - Update documentation as needed

4. **Commit Your Changes**
   ```bash
   git add .
   git commit -m "feat: add amazing feature"
   ```

5. **Push to Your Fork**
   ```bash
   git push origin feature/amazing-feature
   ```

6. **Open a Pull Request**
   - Provide a clear description of changes
   - Reference related issues
   - Ensure all checks pass

## Development Setup

### Prerequisites

- **.NET 10 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/10.0))
- **Visual Studio 2025** or **VS Code** with C# Dev Kit
- **Git** for version control

### Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/akikari/Fox.TaskFlow.git
   cd Fox.TaskFlow
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the solution:**
   ```bash
   dotnet build
   ```

4. **Run tests:**
   ```bash
   dotnet test
   ```
   All 35 tests should pass.

5. **Run the application:**
   ```bash
   # Terminal 1 - API
   cd src/Fox.TaskFlow.Api
   dotnet run

   # Terminal 2 - Blazor
   cd src/Fox.TaskFlow.Blazor
   dotnet run
   ```

### IDE Configuration

#### Visual Studio 2025

1. Open `Fox.TaskFlow.slnx`
2. Set multiple startup projects:
   - Fox.TaskFlow.Api
   - Fox.TaskFlow.Blazor
3. Press F5 to run

#### VS Code

1. Install extensions:
   - C# Dev Kit
   - .NET Install Tool
2. Open folder in VS Code
3. Use integrated terminal to run projects

## Coding Standards

### C# Style Guide

#### General Formatting
```csharp
// ✅ Line Endings: CRLF
// ✅ Indentation: 4 spaces (no tabs)
// ✅ Max Line Length: 100 characters
// ✅ File-scoped Namespaces: Always
namespace Fox.TaskFlow.Application;

// ✅ Expression-bodied Members: For simple properties/methods
public bool IsValid => Status == TaskStatus.Completed;

// ✅ Auto-Properties: Where possible
public string Title { get; set; } = string.Empty;

// ✅ var Usage: Only when type is obvious
var items = new List<string>();  // ✅
var result = await GetAsync();    // ❌ (unclear type)
```

#### Naming Conventions
```csharp
// ✅ Private Fields: camelCase (NO underscore prefix)
private readonly ITaskRepository repository;  // ✅
private readonly ITaskRepository _repository; // ❌

// ✅ Public Members: PascalCase
public string Title { get; set; }

// ✅ Local Variables: camelCase
var taskItem = new TaskItem();

// ✅ Constants: PascalCase
public const string DefaultTitle = "Untitled";
```

#### XML Documentation
```csharp
//==================================================================================================
// File header comment: Purpose + technical description
//==================================================================================================

//==================================================================================================
/// <summary>
/// Class/Interface/Enum summary.
/// </summary>
//==================================================================================================
public sealed class MyClass
{
    //==============================================================================================
    /// <summary>
    /// Property description.
    /// </summary>
    //==============================================================================================
    public string MyProperty { get; set; }

    //==============================================================================================
    /// <summary>
    /// Method description.
    /// </summary>
    /// <param name="id">Parameter description.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Return value description.</returns>
    //==============================================================================================
    public async Task<Result> MyMethodAsync(Guid id, CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

**XML Documentation Requirements:**
- ✅ All public, internal, protected, **and private** members
- ✅ All parameters documented
- ✅ All return values documented
- ✅ Decorator width: 98 characters (no space after `//`)

#### Code Organization with Regions
```csharp
public sealed class MyClass
{
    #region Constants

    public const int MaxRetries = 3;

    #endregion

    #region Fields

    private readonly IService service;
    private readonly ILogger logger;

    #endregion

    #region Constructors

    public MyClass(IService service, ILogger logger)
    {
        this.service = service;
        this.logger = logger;
    }

    #endregion

    #region Properties

    public string Name { get; set; }

    #endregion

    #region Public Methods

    public void DoSomething() { }

    #endregion

    #region Internal Methods

    internal void InternalMethod() { }

    #endregion

    #region Protected Methods

    protected virtual void ProtectedMethod() { }

    #endregion

    #region Private Methods

    private void HelperMethod() { }

    #endregion
}
```

**Region Order:**
1. Constants
2. Fields
3. Constructors
4. Properties
5. Public Methods
6. Internal Methods
7. Protected Methods
8. Private Methods

**Note**: Simple classes (DTOs, configuration) don't need regions.

### Project-Specific Guidelines

#### Clean Architecture
- **Domain**: No external dependencies except Fox.ResultKit
- **Application**: Interfaces only, no infrastructure references
- **Infrastructure**: Implementations only, depends on Application
- **Presentation**: API/Blazor, depends on Application

#### SOLID Principles
- **Single Responsibility**: One class, one reason to change
- **Open/Closed**: Extend via ChainKit handlers, not modification
- **Liskov Substitution**: Avoid problematic inheritance
- **Interface Segregation**: Small, focused interfaces
- **Dependency Inversion**: Depend on abstractions, not concretions

#### DRY (Don't Repeat Yourself)
- Extract duplicate logic to helper methods
- Use base classes for shared validator rules (when possible)
- Create reusable components in Blazor

### Testing Standards

```csharp
// ✅ Test Naming: MethodName_Should_ExpectedBehavior
[Fact]
public void CreateTask_Should_ReturnSuccess_WhenValidRequest()
{
    // Arrange
    var request = new CreateTaskRequest { Title = "Test", Description = "Description" };

    // Act
    var result = service.CreateTaskAsync(request);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Title.Should().Be("Test");
}

// ✅ Use FluentAssertions for readability
result.Should().NotBeNull();
result.IsSuccess.Should().BeTrue();
result.Value.Should().BeOfType<TaskResponse>();

// ❌ Avoid raw assertions
Assert.NotNull(result);
Assert.True(result.IsSuccess);
```

## Commit Guidelines

### Conventional Commits

We follow the [Conventional Commits](https://www.conventionalcommits.org/) specification.

**Format:**
```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation only
- `style`: Formatting, missing semicolons, etc. (no code change)
- `refactor`: Code refactoring (no functionality change)
- `perf`: Performance improvement
- `test`: Adding or updating tests
- `chore`: Maintenance tasks (dependencies, build, etc.)
- `ci`: CI/CD changes

**Examples:**
```bash
feat(api): add task filtering endpoint

fix(blazor): correct date picker localization

docs(readme): update Fox.ChainKit integration section

refactor(service): extract validation message helper

test(application): add OptionKit filter tests

chore(deps): upgrade Fox.ChainKit to 1.0.1
```

### Commit Best Practices

- ✅ Write in present tense ("add feature" not "added feature")
- ✅ Use imperative mood ("move cursor to..." not "moves cursor to...")
- ✅ Limit first line to 72 characters
- ✅ Reference issues: `fix(api): resolve timeout issue (closes #123)`
- ✅ Make atomic commits (one logical change per commit)

## Pull Request Process

### Before Submitting

1. **Update your branch:**
   ```bash
   git checkout main
   git pull origin main
   git checkout feature/your-feature
   git rebase main
   ```

2. **Ensure all tests pass:**
   ```bash
   dotnet test
   ```
   All 35 tests must pass.

3. **Build successfully:**
   ```bash
   dotnet build
   ```
   No warnings or errors.

4. **Format code:**
   - Use IDE formatter (Visual Studio: Ctrl+K, Ctrl+D)
   - Check for trailing whitespace
   - Ensure CRLF line endings

5. **Update documentation:**
   - Add/update XML comments
   - Update README.md if adding features
   - Update CHANGELOG.md

### Pull Request Template

```markdown
## Description
Brief description of changes.

## Type of Change
- [ ] Bug fix (non-breaking change)
- [ ] New feature (non-breaking change)
- [ ] Breaking change (fix or feature that breaks existing functionality)
- [ ] Documentation update

## Related Issues
Closes #123

## Changes Made
- Added X feature
- Fixed Y bug
- Refactored Z component

## Testing
- [ ] Unit tests added/updated
- [ ] All tests pass (35/35)
- [ ] Manual testing completed

## Screenshots (if applicable)
[Add screenshots for UI changes]

## Checklist
- [ ] Code follows project coding standards
- [ ] XML documentation added for new code
- [ ] Tests added for new functionality
- [ ] README.md updated (if needed)
- [ ] CHANGELOG.md updated
- [ ] No compiler warnings
- [ ] All tests pass
```

### Review Process

1. **Automated Checks**: CI pipeline runs tests and build
2. **Code Review**: Maintainer reviews code for quality
3. **Feedback**: Address review comments
4. **Approval**: Once approved, PR will be merged

## Testing Guidelines

### Test Structure

```csharp
public class MyServiceTests
{
    private readonly ITaskRepository repository;
    private readonly TaskService service;

    public MyServiceTests()
    {
        // Arrange: Setup test dependencies
        repository = new FakeTaskRepository();
        service = new TaskService(repository, ...);
    }

    [Fact]
    public async Task MethodName_Should_ExpectedBehavior()
    {
        // Arrange
        var request = new CreateTaskRequest { ... };

        // Act
        var result = await service.MethodAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
```

### Test Coverage Requirements

- ✅ All public methods should have tests
- ✅ Edge cases should be tested
- ✅ Validation rules should be verified
- ✅ Error handling should be tested
- ✅ Integration between components should be tested

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~TaskServiceTests"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Generate coverage report
dotnet test --collect:"XPlat Code Coverage"
```

## Documentation

### XML Documentation

- **Required for all members** (public, internal, protected, private)
- **Parameters**: Describe each parameter
- **Returns**: Describe return value
- **Exceptions**: Document thrown exceptions
- **Remarks**: Add additional context if needed

### README Updates

When adding features:
1. Update **Features** section
2. Add to **Architecture** section if architectural
3. Update **API Endpoints** if adding endpoints
4. Add configuration examples if needed
5. Update **Getting Started** if setup changes

### CHANGELOG Updates

For every PR:
1. Add entry to **[Unreleased]** section
2. Use appropriate category (Added/Changed/Fixed/Removed)
3. Link to issue/PR if applicable

## Questions?

- **GitHub Discussions**: https://github.com/akikari/Fox.TaskFlow/discussions
- **GitHub Issues**: https://github.com/akikari/Fox.TaskFlow/issues

## License

By contributing to Fox.TaskFlow, you agree that your contributions will be licensed under the MIT License.

---

**Thank you for contributing to Fox.TaskFlow! 🙏**
