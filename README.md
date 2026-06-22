# JobAppFlow

JobAppFlow is a private, single-user job application tracking tool for managing a high-volume software engineering job search.

The project is designed as a practical self-hosted application and as a portfolio project demonstrating modern C#, ASP.NET Core, Angular, Azure, and AI-assisted application architecture.

## Purpose

The goal is to help one user manage job opportunities copied from LinkedIn and similar job boards.

The application focuses on a practical workflow:

```text
paste raw job posting text
extract structured job information
review and edit the extracted data
save the job record
track status, notes, files, and follow-ups
```

JobAppFlow is not intended to be a full recruiting platform, ATS, or multi-user SaaS product.

## Planned MVP

The MVP is expected to support:

* pasting raw job posting text;
* LLM-based structured data extraction;
* reviewing and editing extracted job data before saving;
* tracking applications by company, role, source, location, workplace type, salary range, status, and priority;
* storing external AI review notes;
* attaching supporting files such as screenshots, PDFs, resumes, cover letters, confirmations, and recruiter messages;
* searching and filtering saved job applications;
* flagging possible duplicate job postings;
* running as a private Azure-hosted single-user application.

## Project Status

Early MVP planning and foundation stage.

Current focus:

* project structure;
* architecture documents;
* authentication model;
* database workflow;
* Azure deployment path;
* sprint specifications.

## Documentation

Project documentation is stored under `docs`.

Planned documentation includes:

```text
docs/mvp-vision.md
docs/architecture.md
docs/authentication.md
docs/db-workflow.md
docs/deployment.md
docs/roadmap.md
docs/sprints/
```

## Solution Format

JobAppFlow uses the `.slnx` solution format.

The project targets .NET 10 and Visual Studio 2026, so the repository does not keep a legacy `.sln` file.

Build the solution with:

```bash
dotnet build JobAppFlow.slnx
```

## Deployment Direction

The initial backend deployment target is Azure App Service Linux.

A later deployment option may use Docker and Azure Container Apps Consumption for lower-cost hosting of the same ASP.NET Core Web API backend.

The frontend is expected to be deployed separately as a static Angular application.

## Technology stack

C#, ASP.NET Core Web API, Entity Framework Core, SQL Server, Azure SQL Database, Angular 21, Angular Material Design, TypeScript, REST API, OpenAPI / Swagger, Azure Blob Storage, Azure App Service Linux, Azure Application Insights, OpenAI API / Azure OpenAI, LLM-based structured data extraction, GitHub Actions, CI/CD, Docker, Azure Container Apps Consumption.
