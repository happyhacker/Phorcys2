# Phorcys 2.0

![Phorcys](Phorcys2Web/wwwroot/images/Phorcys.webp)

*Phorcys was an ancient sea-god who presided over the hidden dangers of the deep. He and his wife Keto were also the gods of all the large creatures which inhabited the depths of the sea.*

## Overview

Phorcys is a web-based scuba diving logger and planning application. It serves as a comprehensive digital logbook and planning tool for scuba divers, allowing them to track dives, manage equipment, plan future dives, and connect with dive buddies.

**Current Version:** 2.3.4

## Features

### Dive Management
- **Dive Logging** - Record dive details including descent time, average/max depth, temperature, duration, and dive number
- **Dive Planning** - Create and manage planned dives with title, scheduled time, max depth, duration, and notes
- **Dive Sites** - Manage dive locations with geographic coordinates, freshwater/saltwater classification, max depth, and URLs
- **Dive Types** - Categorize dives (technical, recreational, etc.)

### Equipment & Gear
- **Gear Inventory** - Track personal scuba equipment (tanks, BCDs, regulators, wetsuits, etc.)
- **Equipment Details** - Record retail price, actual cost, serial numbers, acquisition/decommission dates, weight, and service notes
- **Tank Management** - Manage tanks on specific dives with gas mix information (O2% and He% composition)

### Contacts & Social
- **Contact Management** - Maintain contacts for dive buddies, instructors, dive agencies, dive shops, and manufacturers
- **Dive Teams** - Assign dive buddies to planned dives
- **SAC Rates** - Track Surface Air Consumption rates for dive buddies

### Certifications
- **Certification Tracking** - Record and manage diving certifications and qualifications

### Checklists
- **Pre-dive Checklists** - Create reusable checklist templates
- **Checklist Instances** - Use checklists during actual dives with item-by-item checkoff

### Tools
- **MOD & END Calculator** - Calculate Maximum Operating Depth and Equivalent Narcotic Depth for technical diving
- **Map Integration** - View dive sites on maps with geographic coordinates

## Technology Stack

- **Framework:** ASP.NET Core 8.0 MVC with Razor Pages
- **Database:** Microsoft SQL Server 2022
- **ORM:** Entity Framework Core 8.0 (Code-First)
- **Authentication:** ASP.NET Core Identity
- **UI:** Bootstrap with Telerik/Kendo UI components
- **Logging:** Serilog (console and file outputs)
- **Cloud:** Azure (Key Vault, App Service)
- **Email:** SendGrid/Postmark integration

## Project Structure

```
Phorcys2/
├── Phorcys2Web/           # ASP.NET Core MVC web application
│   ├── Controllers/       # MVC controllers
│   ├── Views/             # Razor views
│   ├── wwwroot/           # Static files (CSS, JS, images)
│   └── Program.cs         # Application entry point
├── Phorcys.Services/      # Business logic and service layer
├── PhorcysDomain/         # Domain entities and models
├── PhorcysData/           # Data access with EF Core DbContext
├── Phorcys.Tests/         # Unit test suite
└── db/                    # Database scripts and schema
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server 2022 (or Azure SQL)
- Visual Studio 2022 or VS Code

### Configuration

1. Clone the repository
2. Configure your database connection string in Azure Key Vault or user secrets
3. Run database migrations:
   ```bash
   dotnet ef database update --project PhorcysData
   ```
4. Run the application:
   ```bash
   dotnet run --project Phorcys2Web
   ```

### Environment Variables

The application uses Azure Key Vault for secrets management. Configure the following:

- Database connection string
- Email provider credentials (SendGrid/Postmark)
- Identity token settings

## Deployment

The application is configured for Azure App Service deployment. The production instance is available at:

https://phorcys2.azurewebsites.net/

## License

This project is proprietary software.

## Version History

See the [About page](Phorcys2Web/Views/Home/About.cshtml) for detailed release notes.
