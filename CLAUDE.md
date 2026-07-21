# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

WPF desktop application (.NET Framework 4.7.2) for managing a fire fighters association membership. Tracks members, dues, meetings, officer assignments, and LOSAP (Legacy Of Service Award Program) credits. Generates RDLC reports for operational use.

## Build

Open `Membership.sln` in Visual Studio and press F5, or from the command line:

```
msbuild Membership.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

Configurations: `Debug`, `DebugLocal`, `Release`. Output goes to `Membership\bin\<Config>\Membership.exe`.

There are no automated tests — all testing is manual by running the application.

## Architecture: MVP Pattern

The codebase follows Model-View-Presenter throughout every feature domain:

- **DataModel** — POCO class with Dapper.Contrib attributes (`[Table]`, `[ExplicitKey]`, `[Computed]`)
- **IRepository** — interface defining data access operations
- **Repository** — concrete implementation using Dapper against SQL Server
- **IView** — interface exposing UI state properties and events that the presenter drives
- **Presenter** — coordinates between the view and repository; owns all business logic
- **UserControl (XAML + code-behind)** — implements IView; code-behind is thin and delegates to the presenter

Example flow for LOSAP:
```
LosapCredit.xaml.cs  →  implements ILosapRecordView
LosapRecordPresenter  →  reads/writes via ILosapRepository
LosapRepository       →  executes SQL via Dapper → SQL Server "Membership" database
LosapRecord           →  the DataModel
```

Every feature domain (`Members`, `Officers`, `Dues`, `Meetings`, `LOSAP`, `Reports`) follows this same structure under `Membership/Core/<Domain>/`.

## Data Access

Uses **Dapper 2.0.30** and **Dapper.Contrib 2.0.30** — no Entity Framework.

- All repositories open a `SqlConnection` from the `"MembershipDB"` connection string defined in `App.config`
- `App.config` connection string: `Server=.;Database=Membership;Trusted_Connection=True;`
- Dapper.Contrib attributes on data models:
  - `[Table("TABLE_NAME")]` — maps class to SQL table
  - `[ExplicitKey]` — marks a primary key that is not auto-generated
  - `[Computed]` — marks properties that are not mapped to columns (derived values)
- Use `connection.Query<T>()` for SELECT, `connection.Insert()` / `connection.Update()` / `connection.Delete()` for CUD operations

## UI Layer

- **MahApps.Metro 1.6.5** for modern WPF styling
- **Fluent.Ribbon 6.1.0** for the ribbon toolbar in `MainWindow.xaml`
- **Custom input controls** in `Membership/Common/Controls/` — prefer these over raw WPF controls:
  - `InputTextControl`, `InputDateControl`, `InputComboControl`, `InputCheckBoxControl`
  - `DetailsHeaderControl` for section headers in detail forms
- **WPF value converters** in `Membership/Common/Converters/` handle binding transformations
- `MainWindow.xaml.cs` manages navigation by swapping the active `UserControl` in the main content area

## Reporting

- Report definitions are `.rdlc` files in `Membership/ReportDefinitions/`
- Data source schemas are in `Membership/ReportDataSources/` as `.xsd` files
- Uses `Microsoft.ReportingServices.ReportViewerControl.Winforms` (v150)
- Report data is assembled in `Membership/Core/Reports/` presenters and repositories
- The `ReportViewer` UI control lives in `Membership/UI_Controls/ReportViewer/`

## Key Domain: LOSAP

LOSAP tracks annual service credits for pension eligibility. Each `LosapRecord` represents a member's credit total for a given year. The listing and detail views are `LosapListing.xaml` and `LosapCredit.xaml`. This domain is currently under active development (see modified files in git status).
