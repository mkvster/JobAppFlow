# Deployment Options

## Purpose

JobAppFlow is designed so that the same ASP.NET Core Web API backend can be deployed to different Azure hosting targets.

The deployment target should not require changes to application code.

This document describes the supported backend deployment options and when each option is appropriate.

## Common Application Components

The application uses the same core components regardless of backend hosting option:

```text
Angular frontend
ASP.NET Core Web API
Azure SQL Database
Azure Blob Storage
OpenAI API or Azure OpenAI
Application diagnostics
```

The frontend is expected to be hosted separately as a static Angular application.

The backend API is responsible for authentication, authorization, data access, file access, and AI-assisted job posting extraction.

## Frontend Deployment

The Angular frontend is intended to be deployed as a static web application.

Preferred frontend hosting:

```text
Azure Static Web Apps
```

The frontend calls the backend API through a configured API base URL.

The API base URL may point to either Azure App Service or Azure Container Apps.

## Backend Deployment Option A: Azure App Service Linux

Azure App Service Linux is the initial backend deployment option.

It is the simplest option for getting the ASP.NET Core Web API online quickly.

Use this option when the priority is:

```text
simple deployment
always-on availability
classic Azure App Service experience
minimal container-related setup
fast MVP delivery
```

Expected usage:

```text
Angular frontend -> Azure Static Web Apps
ASP.NET Core API -> Azure App Service Linux
Structured data -> Azure SQL Database
Attachments -> Azure Blob Storage
```

This option has a predictable fixed monthly hosting cost based on the selected App Service Plan.

## Backend Deployment Option B: Azure Container Apps Consumption

Azure Container Apps Consumption is a cost-optimized container-based backend deployment option.

The same ASP.NET Core Web API application is packaged as a Docker container and deployed to Azure Container Apps.

Use this option when the priority is:

```text
lower cost for light usage
container-based deployment
scale-to-zero behavior
cloud-native deployment experience
```

Expected usage:

```text
Angular frontend -> Azure Static Web Apps
ASP.NET Core API container -> Azure Container Apps Consumption
Structured data -> Azure SQL Database
Attachments -> Azure Blob Storage
```

This option may reduce hosting cost for low-traffic usage but requires Docker/container deployment setup.

## Same Backend Codebase

Both backend deployment options use the same application code:

```text
JobAppFlow.Api
```

The application should not depend on hosting-specific behavior.

Configuration should be provided through environment variables or platform-specific application settings.

Examples:

```text
database connection string
blob storage settings
LLM provider settings
authentication settings
allowed frontend origins
logging settings
```

## Container Image Registry

When using Azure Container Apps, the backend API must be packaged as a Docker image.

The image may be stored in:

```text
GitHub Container Registry
Azure Container Registry
```

GitHub Container Registry is a good default for an open-source project.

Azure Container Registry may be used for a more Azure-native deployment.

## Recommended Initial Path

The recommended initial path is:

```text
1. Deploy the backend API to Azure App Service Linux.
2. Keep the application code container-friendly.
3. Add Docker support.
4. Add Azure Container Apps Consumption as an alternative deployment option.
```

This keeps the first online MVP simple while preserving a path toward a lower-cost container deployment.

## Deployment Decision Summary

Use Azure App Service Linux when:

```text
you want the fastest and simplest always-on Azure deployment
you do not want to deal with Docker yet
a predictable monthly cost is acceptable
```

Use Azure Container Apps Consumption when:

```text
you want lower cost for light usage
you are ready to use Docker
you want scale-to-zero behavior
you want a container-based Azure deployment option
```

## Design Rule

Deployment choice must remain an infrastructure decision.

The application code should remain the same regardless of whether the backend is deployed to Azure App Service Linux or Azure Container Apps Consumption.
