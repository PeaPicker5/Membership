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

- **DataModel** — plain POCO class mapped by Dapper (property names match column names)
- **IRepository** — interface defining data access operations
- **Repository** — concrete implementation using Dapper against SQL Server
- **IView** — interface exposing UI state properties and events that the presenter drives
- **Presenter** — coordinates between the view and repository; owns all business logic. Takes the IView in its constructor (it does not implement the IView itself)
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

Uses **Dapper 2.0.30** with hand-written SQL — no Entity Framework, no Dapper.Contrib (removed).

- All repositories open a `SqlConnection` from the `"MembershipDB"` connection string defined in `App.config`
- `App.config` connection string: `Server=.;Database=Membership;Trusted_Connection=True;`
- Use `connection.Query<T>()` for SELECT and `connection.Execute(sql, param)` for INSERT/UPDATE/DELETE, always with `@parameters` (never string interpolation of values)
- Wrap multi-statement writes (e.g. a record plus its child rows) in a transaction
- The database has primary keys and foreign keys (`Database/001_AddKeys.sql`); child tables of `MEMBER_List` and `MEETING_List` cascade on delete
- Table/column names don't always match property casing (e.g. `Lastname`, `EMailAddress`, `MemberID`); SQL Server is case-insensitive here, but write the column names as the table defines them

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

LOSAP tracks annual service credits for pension eligibility.

**Data rules**
- `LOSAP_History` has one row per member per year (primary key `MemberID, Year`) with a `LosapCredit` bit.
- Only rows with `LosapCredit = 1` count as a credited year. Rows with `0` mean the year was reviewed but not credited.
- Crediting a member upserts their row for that year to `1`; un-crediting sets it to `0` (the row is kept, not deleted).
- A member's total = credited years (`LosapRecord.LosapYears`) + `MEMBER_List.LegacyLosapYears` (years earned before the app tracked LOSAP), exposed as `LosapRecord.CombinedTotalYears`.

**Code**
- `LosapRecord` is a view model (not a table mapping): `MemberRec`, `Year`, `HasCredit`, `OriginalHasCredit` (what the database holds), `IsChanged`, `LosapYears`.
- `LosapRepository` (explicit SQL): `GetCreditedYearCounts()`, `GetCreditedMemberIds(year)`, `GetYearsOnFile()`, `SaveCredits(year, toCredit, toUncredit)` (single transaction).
- **Credit entry** — `LosapCredit.xaml` + `LosapRecordPresenter`: a year picker defaults to last year (`DefaultYear`) and offers the last 10 years plus any year on file. It lists current Active members (`MemberTypeId == 1`) plus anyone already credited that year. Double-click toggles credit; changed rows are highlighted; APPLY saves only changed rows, then reloads. Switching years with unsaved changes asks first.
- **Listing** — `LosapListing.xaml` + `LosapListingPresenter`: every current Active member with credited, legacy and total years, sorted by total descending.
