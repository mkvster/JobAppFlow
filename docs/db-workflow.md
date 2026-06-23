# AI-Assisted Database-First Workflow

## Purpose

JobAppFlow uses a database-first approach where the database schema is managed through explicit SQL scripts rather than EF Core Migrations.

The goal is to keep database changes clear, reviewable, and under developer control, while still using EF Core as the application data access layer.

This workflow is also designed to work well with AI-assisted development, where an agentic AI tool helps keep the C# model and EF Core mappings in sync with database schema changes.

## Core Principles

The database schema is managed through SQL scripts.

SQL scripts are treated as the source of truth for schema changes.

EF Core is used for runtime data access, not for managing database schema migrations.

EF Core Migrations are not used.

The application does not include custom database update utilities.

Applying database scripts is the responsibility of the developer or DBA.

AI may assist with updating C# code after a database script changes, but AI does not automatically apply schema changes to any database.

## Database Scripts

Database scripts are stored in the repository under:

```text
backend/Database/SqlServer
```

Each script should describe an intentional database change.

Scripts should use sequential numeric prefixes such as `001_`, `002_`, `003_` so that the intended order of execution is obvious.

If the number of scripts becomes very large, a new baseline database snapshot may be created and numbering can restart from `001_`.

The target database may be local development, Azure SQL development, staging, or production.

## Script Application

Database scripts are applied manually to the appropriate target database by the developer or DBA.

The application does not apply schema changes on startup, and CI/CD does not automatically update the database schema.

## EF Core Usage

EF Core is used as the application ORM and data access layer.

The project uses POCO classes and explicit EF Core configuration classes.

POCO classes should remain simple C# classes.

POCO classes should not contain EF-specific behavior.

Virtual navigation properties should not be used.

Lazy loading should not be used.

Entity mapping should be configured through explicit EF Core configuration classes.

## Project Structure

The expected project structure is:

```text
backend/
  database/
    scripts/

  JobAppFlow.Server/
    Libraries/
      JobAppFlow.Domain/
        Models/

      JobAppFlow.AppDataAccess/
        Data/
          JobAppFlowDbContext.cs
        Configurations/
```

`JobAppFlow.Domain` contains POCO classes.

`JobAppFlow.AppDataAccess` contains the EF Core `DbContext` and entity configuration classes.

The exact folder names may change, but the separation should remain clear:

```text
Core model classes
EF Core DbContext and mappings
SQL database scripts
```

## AI-Assisted Synchronization

After a database script is created or changed, an AI coding assistant may be used to update the C# model and EF Core configuration.

The AI coding assistant receives the SQL script path as the main input and uses the repository structure to find the related POCO classes, `DbContext`, and EF Core configuration files.

A typical instruction should be short and practical:

```text
Take the script at <path-to-sql-script> and update the POCO models in <path-to-models-folder> and EF Core configurations in <path-to-configurations-folder>.
```

For example:

```text
Take the script at backend/database/scripts/004_add_job_attachments.sql and update the POCO models in backend/JobAppFlow.Server/Libraries/JobAppFlow.Domain/Models and EF Core configurations in backend/JobAppFlow.Server/Libraries/JobAppFlow.AppDataAccess/Configurations.
```

The AI assistant should update the C# code to match the database change.

This may include:

```text
adding or updating POCO properties
adding or updating entity configuration
updating relationships
updating column names, lengths, nullability, indexes, and constraints
updating related tests
```

The developer must review the AI-generated changes before committing them.

## Review Expectations

AI-assisted changes must be reviewed by the developer.

The review should confirm:

```text
the POCO model matches the intended schema
EF Core mappings match the SQL script
nullability is correct
string lengths and column types are correct
relationships are correct
indexes and constraints are represented where useful
no EF Core migration was created
no unwanted lazy loading or virtual navigation properties were introduced
```

##
