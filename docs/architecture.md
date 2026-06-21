# JobAppFlow Architecture Overview

## Purpose

This document describes the high-level architecture of JobAppFlow.

JobAppFlow is a single-user web application for managing a high-volume job search workflow. The application allows the user to ingest job postings, track application status, store AI-assisted review notes, and attach job-related documents.

The architecture is designed to support a practical MVP while also demonstrating a modern .NET, Angular, Azure, and AI-assisted application stack.

## High-Level Architecture

JobAppFlow consists of:

```text
Angular web application
ASP.NET Core Web API
Azure SQL Database
Azure Blob Storage
Optional OpenAI / Azure OpenAI integration
Azure hosting and monitoring services
```

The frontend and backend are separate applications.

The backend owns application data, authentication, authorization, file access, and integration with external services.

The frontend communicates with the backend through REST API calls.

## Frontend

The frontend is an Angular 21 application using Angular Material Design.

It is responsible for:

```text
user interface
routing
forms
job posting paste/preview workflow
job application list and detail views
attachment upload/download UI
authentication UI
calling backend APIs
```

The frontend should not contain business-critical authorization logic.

All protected data access must be enforced by the backend API.

## Backend API

The backend is an ASP.NET Core Web API.

It is responsible for:

```text
authentication and authorization
job posting ingestion
job application management
AI-assisted extraction orchestration
attachment metadata management
secure access to Azure Blob Storage
data access through EF Core
integration with Azure SQL
application diagnostics and logging
```

The backend should be deployable to more than one Azure hosting target without changing application code.

Initial backend deployment target:

```text
Azure App Service Linux
```

Alternative deployment target:

```text
Azure Container Apps Consumption
```

The same `JobAppFlow.Api` application should be used for both deployment options.

## Data Storage

Structured application data is stored in SQL Server / Azure SQL Database.

This includes job records, application status, parsed job posting fields, notes, AI review text, attachment metadata, and authentication-related data.

Database schema changes are managed through the project’s AI-assisted database-first workflow.

EF Core is used for runtime data access.

EF Core Migrations are not used.

## File Storage

Job-related files are stored in Azure Blob Storage.

This includes documents such as:

```text
screenshots
PDF printouts
resumes
cover letters
application confirmations
recruiter messages
other job-related attachments
```

Blob containers should be private.

The frontend should not access private blobs directly without backend authorization.

The backend API verifies access and then streams the file or provides a short-lived access mechanism if implemented later.

## AI Integration

AI integration is used for job posting ingestion.

The primary AI-assisted capability is:

```text
pasted raw job posting text -> structured job posting draft
```

The MVP does not require resume matching.

The system may support either OpenAI API or Azure OpenAI.

AI output should be reviewed or confirmed by the user before being saved as application data.

External AI review notes may also be pasted and stored manually.

## Authentication and Authorization

JobAppFlow is a single-user application, but it is hosted online and contains private data.

All application data and attachments must be protected.

The authentication design is described separately in:

```text
docs/authentication.md
```

The backend API is responsible for enforcing authorization.

The application should not rely on frontend-only protection.

## Azure Services

The intended Azure services are:

```text
Azure Static Web Apps
Azure App Service Linux
Azure SQL Database
Azure Blob Storage
Azure Application Insights
Azure Container Apps Consumption
```

Initial deployment is expected to use Azure App Service Linux for the backend API.

Azure Container Apps Consumption may be added as an alternative backend deployment option.

## Configuration

The application should be configured through environment-specific settings.

Examples include:

```text
database connection string
blob storage connection settings
LLM provider settings
authentication settings
allowed frontend origins
logging and diagnostics settings
```

Sensitive values should not be committed to the repository.

Azure App Service Configuration, Azure Container Apps secrets, or Azure Key Vault may be used for deployed environments.

## Deployment Model

The frontend and backend are deployed separately.

Typical deployment:

```text
Angular frontend -> Azure Static Web Apps

ASP.NET Core API -> Azure App Service Linux

Structured data -> Azure SQL Database

Attachments -> Azure Blob Storage
```

Alternative backend deployment:

```text
ASP.NET Core API -> Azure Container Apps Consumption
```

The deployment target should not change the application code.

Only hosting configuration, deployment scripts, and infrastructure settings should differ.

## Observability

The application should provide basic diagnostics suitable for a small production-like MVP.

This may include:

```text
request logging
error logging
application events
AI extraction failures
attachment upload failures
health checks
```

Azure Application Insights is the preferred Azure monitoring service.

Logging should avoid storing sensitive job search data, personal documents, secrets, or full AI prompts unless intentionally enabled for development.

## Design Principles

The architecture follows these principles:

```text
single-user first
secure by default
Azure-deployable
simple before complex
database-first schema control
private blob-based file storage
AI-assisted but human-confirmed
backend-enforced authorization
deployment-target independence
```

The MVP should remain practical and avoid unnecessary SaaS complexity.
