# JobAppFlow Roadmap

## Purpose

This roadmap describes the planned delivery sequence for JobAppFlow.

It is not a detailed sprint specification. Detailed sprint plans are stored under `docs/sprints`.

## Sprint 1 - Online Foundation

Goal:

Establish the project structure and prove that the application can run online.

High-level scope:

- repository and solution structure;
- Angular application skeleton;
- ASP.NET Core Web API skeleton;
- Azure App Service Linux deployment;
- basic configuration and health checks;

## Sprint 2 - Auth and Login

Goal:

Add the first database and authentication layer, then deliver login on top of the online foundation.

High-level scope:

- local database identity tables;
- CLI admin tool for user registration;
- local database application tables;
- Azure SQL setup;
- backend database wiring;
- authentication model and endpoints;
- login UI;

## Sprint 3 - Job Ingestion and Tracking MVP

Goal:

Implement the first useful job tracking workflow.

High-level scope:

- paste raw job posting text;
- LLM-based extraction into structured draft;
- review and edit extracted data;
- save and list job records;
- track core job fields and application status;
- store external AI review notes;
- basic duplicate detection.

## Sprint 4 - Attachments and Search Workflow

Goal:

Add file-based job application context and make saved opportunities easier to manage.

High-level scope:

- upload files to job applications;
- store files in Azure Blob Storage;
- store attachment metadata in Azure SQL;
- authenticated attachment access;
- search and filter saved job applications;
- search attachment metadata.

## Sprint 5 - Azure Polish and Deployment Alternatives

Goal:

Improve the Azure deployment story and add a cost-optimized backend deployment option.

High-level scope:

- improve GitHub Actions CI/CD;
- add Application Insights diagnostics;
- add Docker support for the backend API;
- add Azure Container Apps Consumption deployment option;
- document App Service vs Container Apps deployment choices;
- improve README and portfolio presentation.

## Later Candidates

- advanced dashboard;
- better tagging and categorization;
- full-text attachment search;
- resume version tracking;
- built-in match analysis;
- prompt templates for external AI review;
- Azure Key Vault integration;
- audit/history tracking;
- import/export;
- public demo mode with sanitized sample data.

