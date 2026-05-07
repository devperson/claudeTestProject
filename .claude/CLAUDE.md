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

## Codebase Overview

A .NET 10 e-commerce order-processing library with a console demo app and xunit test suite. All business logic lives in `CommonServices`; both the console app and test project consume it independently.

**Stack:** .NET 10, C#, xunit 2.9.3, GitHub Actions (Claude-powered CI only — no automated build/test pipeline).
**Structure:** `CommonServices/` (class library) → consumed by `claudeTestProject/` (console exe) and `claudeTestProject.Tests/` (tests). Solution file: `claudeTestProject.slnx`.

For detailed architecture, see [docs/CODEBASE_MAP.md](../docs/CODEBASE_MAP.md).

## Architecture

Three projects grouped by `claudeTestProject.slnx` (`.slnx` XML solution format, .NET 9+):

- **`CommonServices/`** — Class library containing all business logic. Referenced by both the console app and the test project.
  - `DiscountService` — Applies a 10% discount to orders over $100.
  - `OrderService` — Handles tax calculation, shipping cost ($5.99 flat, free at $50+), order status by stage (1–4), delivery day estimates by shipping method, and a full `GenerateOrderSummary` that returns an itemized `Dictionary<string, decimal>`.
- **`claudeTestProject/`** — Console app (`Program.cs`) that demonstrates `DiscountService` usage (hardcoded $150 order).
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
