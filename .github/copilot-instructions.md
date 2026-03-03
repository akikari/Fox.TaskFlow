# GitHub Copilot Instructions - Fox.TaskFlow

## General Guidelines
- Always ask for permission before committing to Git. Keep commits clean and controlled.
- When adding a NEW date to documentation (that didn't exist before), ALWAYS:
  1. Query current date first using: `Get-Date -Format "yyyy-MM-dd"`
  2. Use the actual current date returned by the system
  3. After making the change, ASK the user if the date is correct before finalizing
- Do NOT assume or guess dates. Do NOT "fix" existing dates without verification.

## Code Style

### Formatting
- **Line Endings**: CRLF
- **Indentation**: 4 spaces
- **File-scoped Namespaces**: Always use `namespace MyNamespace;`
- **Expression-bodied Members**: Prefer for simple properties and methods (e.g., `public bool IsAvailable => Stock > 0;`)
- **Auto-Properties**: Use auto-properties where possible (e.g., `public int Stock { get; set; }`)
- **var Usage**: Use only when type is obvious
- **Max Line Length**: 100 characters
- **Blank Line After Closing Brace**: Add blank line after `}` UNLESS next line is also `}`
- ✅ Add blank line if code follows: `}\n\nDoSomething();`
- ✅ Add blank line between sequential if statements: `}\n\nif (...)`
- ❌ No blank line if another `}` follows: `}\n}`
- **No Line Wrapping**: Do not wrap long lines - keep them on a single line even if they exceed max length

Example:
```csharp
// ✅ Correct - blank line between if statements
if (value < 0)
{
    throw new ArgumentException("Invalid");
}

if (value > 100)
{
    throw new ArgumentException("Too large");
}

// ❌ Incorrect - no blank line between if statements
if (value < 0)
{
    throw new ArgumentException("Invalid");
}
if (value > 100)
{
    throw new ArgumentException("Too large");
}

// ✅ Correct - no blank line when closing brace follows
if (condition)
{
    if (nested)
    {
        DoSomething();
    }
}
```

### Property Guidelines
- **Read-only properties (initialized in constructor)**: Use auto-property with `{ get; }`
  - ✅ `public string Name { get; }`
  - ❌ NOT `private readonly string name; public string Name => name;`
- **Mutable properties**: Use auto-property with setter
  - ✅ `public int Stock { get; set; }` or `public int Stock { get; private set; }`
  - ❌ NOT backing field unless validation is needed
- **Computed properties**: Use expression-bodied syntax
  - ✅ `public bool IsAvailable => Stock > 0;`
  - ✅ `public decimal TotalValue => Price * Stock;`
- **Complex properties with logic**: Use full property with backing field
  - ✅ Use backing field when getter has validation or throws exceptions

### Naming Conventions
- **Private Fields**: Use camelCase **without** underscore prefix (e.g., `value`, not `_value`)
- **Public Members**: PascalCase
- **Local Variables**: camelCase

Example:
```csharp
public sealed class Product
{
    // ✅ Read-only auto-property (initialized in constructor)
    public string Name { get; }
    
    // ✅ Mutable auto-property with private setter
    public int Stock { get; private set; }
    
    // ✅ Computed property (expression-bodied)
    public bool IsAvailable => Stock > 0;
    
    // ✅ Complex property with validation (needs backing field)
    private readonly decimal price;
    public decimal Price
    {
        get
        {
            if (price < 0)
            {
                throw new InvalidOperationException("Invalid price");
            }
            return price;
        }
    }
}
```

### C# Settings
- **Nullable**: Enabled
- **Language Version**: latest
- **Async Suffix**: Prefer `Async` suffix for async methods

## Documentation

### XML Comments
- **Language**: English
- **Generate XML Doc Comments**: Always add XML documentation for public members
- **Avoid Redundant Comments**: Do not add obvious or meta comments

### XML Doc Decorators
- **Enabled**: Yes
- **Width**: 98 characters (total line width including indentation)
- **Character**: `=`
- **Prefix**: `//`
- **No Space After Prefix**: Decorators use `//======` format (no space)
- **Dynamic Indent**: Yes (adjust decorator width based on indentation level)
- **File-Scoped Namespaces Rule**: When using `namespace MyNamespace;` (file-scoped):
  - **File Headers**: 98 characters (no indentation)
  - **Class/Interface/Enum Decorators**: 98 characters (no indentation, same level as namespace)
  - **Method/Property/Member Decorators**: 94 characters + 4-space indentation = 98 total width

Example (file-scoped namespace):
```csharp
//==================================================================================================
// File header: 98 characters (no indentation)
//==================================================================================================

namespace Fox.ResultKit;

//==================================================================================================
/// <summary>
/// Class decorator: 98 characters (no indentation, same level as namespace)
/// </summary>
//==================================================================================================
public sealed class Result
{
    //==============================================================================================
    /// <summary>
    /// Method decorator: 94 characters + 4-space indent = 98 total
    /// </summary>
    //==============================================================================================
    public static Result Success() => new(true, null);
}
```

### File Header
- **Enabled**: Yes
- **Format**: 3-line header (purpose + technical description + decorators)
- **Width**: 98 characters (full line)
- **Character**: `=`
- **Prefix**: `//`
- **No Space After Prefix**: Header decorators use `//======` format (no space)
- **Normal Comments in Header**: Use `// ` with space for content lines
- **Language**: English
- **No Date/Author**: Headers do not include date or author information (git history provides this)

Example:
```csharp
//==================================================================================================
// Represents the result of an operation that can be either success or failure.
// Sealed class implementation for Result pattern without value.
//==================================================================================================
## Comments

### Regular Comments
- **Always use space after prefix**: `// Comment` (not `//Comment`)
- **Decorator lines**: No space - `//==============================`

### Guidelines
- Avoid redundant comments
- Avoid obvious comments
- Avoid meta comments
- Suppress explanatory comments when not needed

## Testing

### Framework & Libraries
- **Test Framework**: xUnit
- **Assertions**: FluentAssertions
- **Naming Convention**: `MethodName_Should_ExpectedBehavior`

Example:
```csharp
[Fact]
public void Success_should_create_success_result()
{
    var result = Result.Success();
    
    result.IsSuccess.Should().BeTrue();
}
```
## Summary

When generating or modifying code in this project:

1. ✅ Use file-scoped namespaces
2. ✅ Add English XML documentation with decorators
3. ✅ Prefer auto-properties and expression-bodied members for simple cases
4. ✅ Add 3-line file headers (purpose + technical description)
5. ✅ Follow 98-character decorator width
6. ✅ Normal comments: `// ` with space
7. ✅ Decorator lines: `//======` without space
8. ✅ Enable nullable reference types
9. ✅ Private fields: camelCase **without** underscore prefix
10. ✅ Use xUnit + FluentAssertions for tests
11. ✅ Follow `MethodName_Should_ExpectedBehavior` test naming
