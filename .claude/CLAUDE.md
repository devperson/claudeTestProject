# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build all projects
dotnet build

# Run all tests
dotnet test claudeTestProject.Tests/

# Run a single test by name
dotnet test claudeTestProject.Tests/ --filter "FullyQualifiedName~MethodName"

# Run the console app
dotnet run --project claudeTestProject.csproj
```

## Architecture

Three projects with no solution file — reference them individually:

- **`CommonServices/`** — Class library containing all business logic. Referenced by both the console app and the test project.
  - `DiscountService` — Applies a 10% discount to orders over $100.
  - `OrderService` — Handles tax calculation, shipping cost ($5.99 flat, free at $50+), order status by stage (1–4), delivery day estimates by shipping method, and a full `GenerateOrderSummary` that returns an itemized `Dictionary<string, decimal>`.
- **`claudeTestProject/`** — Console app (`Program.cs`) that demonstrates `CommonServices` usage.
- **`claudeTestProject.Tests/`** — xunit test project. Tests live alongside the service they cover (e.g., `OrderServiceTests.cs`). Uses `[Theory]` + `[InlineData]` for parameterized cases and `#region` blocks to group tests by method.

## Naming Rules

- Classes: PascalCase
- Methods: PascalCase
- Parameters: camelCase
- Private fields: `_camelCase`

## Development Rules

- Validate inputs and throw `ArgumentException` when invalid.
- Add XML documentation to all public methods.
- Use Arrange-Act-Assert pattern in tests.

## Commit Rules

- Conventional commits: `feat`, `fix`, `test`, `docs`, `refactor`
- Keep messages short and clear.
