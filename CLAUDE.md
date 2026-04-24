# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Is

Phorcys 2.0 is a web-based SCUBA dive logging and planning application. Current version: **2.3.4**. Production: https://phorcys2.azurewebsites.net/

---

## Commands

All commands run from the repo root unless noted.

```bash
# Build
dotnet build Phorcys2Web/Phorcys.Web.sln

# Run the web app
dotnet run --project Phorcys2Web/Phorcys.Web.csproj

# Run all tests
dotnet test Phorcys.Tests/Phorcys.Tests.csproj

# Run a single test
dotnet test Phorcys.Tests/Phorcys.Tests.csproj --filter "FullyQualifiedName~TestMethodName"

# Add an EF migration
dotnet ef migrations add MigrationName \
  --project PhorcysData/Phorcys.Data.csproj \
  --startup-project Phorcys2Web/Phorcys.Web.csproj

# Apply migrations
dotnet ef database update \
  --project PhorcysData/Phorcys.Data.csproj \
  --startup-project Phorcys2Web/Phorcys.Web.csproj
```

> **Migrations note:** Although `PhorcysContext` lives in `PhorcysData`, the migrations assembly is registered as `"Phorcys.Web"` in `Program.cs`. Always provide both `--project` and `--startup-project` when running EF commands.

---

## Project Structure

Five projects in one solution (`Phorcys2Web/Phorcys.Web.sln`):

| Project | Role |
|---|---|
| `PhorcysDomain` | EF domain entities — no logic, no DTOs |
| `PhorcysData` | `PhorcysContext` (EF DbContext) + DTOs in `Phorcys.Data.DTOs` |
| `Phorcys.Services` | All business logic; services inject `PhorcysContext` + `ILogger<T>` |
| `Phorcys2Web` | ASP.NET Core MVC — controllers, Razor views, ViewModels |
| `Phorcys.Tests` | xUnit + Moq unit tests |

---

## Architecture & Key Patterns

### Layer flow
```
Domain entities → PhorcysContext (EF) → Service layer → Controllers → ViewModels → Views
```

- **Controllers** inject services only — never `PhorcysContext` directly.
- **Services** return domain entities or DTOs — never ViewModels.
- **Views** receive ViewModels — never raw domain entities.
- **No AutoMapper** — all mapping is manual, property by property. This applies at every boundary (domain → DTO, DTO → ViewModel, ViewModel → DTO).

### Naming conventions
- Services: `{Entity}Services` (e.g., `DiveServices`)
- DTOs: `{Entity}Dto` — in `Phorcys.Data.DTOs`
- ViewModels: `{Entity}ViewModel` — in `Phorcys2Web/Models/`
- Domain entities: `{Entity}` — in `PhorcysDomain/`

### Contact entity pattern
`Contact` is the base entity for all person/org types. `Diver`, `Instructor`, `DiveShop`, `Manufacturer`, and `DiveAgency` each have a 1:1 with `Contact` via `ContactId` FK with cascade delete configured in `PhorcysContext.OnModelCreating`.

### DiveComputerLog / LogSample flow
Shearwater CSV imports go through `IShearwaterCsvImportService`, which parses the 4-row CSV format (summary headers → summary values → sample headers → sample rows). The parsed `ShearwaterDiveSummaryDto` is used by `DiveController` to pre-fill the Create form; samples are stored in session (`PendingLogSamples`) and written to `LogSamples` via `DiveServices.SaveLogSamples()` when the dive is saved.

### TempData messages
Page-level success/error messages use:
```csharp
TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = "...";
```

### Secrets & configuration
All secrets are in **Azure Key Vault** (`https://PhorcysKeyVault.vault.azure.net/`), loaded at startup via `DefaultAzureCredential`. There are no secrets in `appsettings.json`. The database connection string is named `PhorcysDbConnection`.

For local development, use `dotnet user-secrets` or set up Azure CLI credentials so `DefaultAzureCredential` can resolve Key Vault access.

---

## UI Conventions

- **Telerik UI for ASP.NET Core** (Kendo UI wrappers, v2024.1.319) — use fluent HTML Helper syntax (`Html.Kendo().Grid(...)`) as the primary pattern.
- **Bootstrap 5** for all layout — no inline styles.
- Per-view JavaScript goes in `@section scripts { }`.
- Pass server-side URLs into JavaScript via `data-*` attributes on a config element — never embed `@Url.Action(...)` inside JS strings.
- `<body>` carries class `k-content` — required for Kendo theme to apply.

---

## Agents

Four specialized agents live in `.claude/agents/`:

- **Matt** — SQL Server: T-SQL, DDL, schema design, naming conventions
- **Matthew** — .NET backend: service layer, EF Core, DTOs, controllers, ViewModels
- **Robert** — CI/CD: GitHub Actions, Azure App Service deployment, EF migration pipelines
- **Thiago** — UI: Razor views, Telerik/Kendo components, Bootstrap 5, jQuery
