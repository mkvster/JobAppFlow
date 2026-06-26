# Auth Model Foundation

## Description

Prepare the API authentication foundation for the next auth endpoints implementation.

This ticket establishes the backend wiring for:

- ASP.NET Core Identity integration;
- JWT authentication configuration for access tokens;
- JWT refresh token configuration;
- application settings for auth and secret values;
- Application Insights configuration for the API;

## Acceptance

- API is wired to the DataAccess.
- ASP.NET Core Identity is configured in the API authentication pipeline.
- JWT authentication is configured for the API.
- Refresh token generation is available for the API layer.
- Auth-related configuration values are loaded from application settings and environment variables.
- Application Insights is configured for the API.
