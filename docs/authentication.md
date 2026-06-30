# Authentication Design

## Purpose

JobAppFlow is a single-user online application that may contain private job search data, recruiter messages, salary notes, AI review notes, uploaded resumes, cover letters, screenshots, PDFs, and other application-related documents.

Even though the MVP is not a multi-user SaaS product, it must prevent unauthorized access to the application, API, database records, and uploaded files.

## Authentication Goals

The authentication design should support:

* one owner account;
* no public user registration;
* protected API endpoints;
* protected access to job application attachments;
* compatibility with an Angular frontend hosted separately from the backend API;
* compatibility with backend deployment to either Azure App Service or Azure Container Apps;
* normal daily use without frequent manual re-login;
* a demo sign-in flow that can authenticate a fixed showcase user without exposing their credentials.

## High-Level Approach

The MVP will use ASP.NET Core Identity for user and password management.

The Angular frontend will authenticate against the ASP.NET Core Web API.

The API will issue JWT access tokens for API requests.

Refresh tokens will be used to keep the user signed in for normal accounts.

Demo access can use the same auth pipeline but should still be treated as a separate role for authorization.

## Authentication Flow

### Login

The user submits email/username and password from the Angular login page.

The API validates credentials using ASP.NET Core Identity.

If credentials are valid, the API returns a short-lived JWT access token and sets a refresh token in a secure HttpOnly cookie.

### Demo Login

The Angular login page can offer a demo mode.

When demo mode is selected, the frontend calls a dedicated demo sign-in endpoint.

The API logs the user in as the configured demo account, which must have the `Demo` role.

### Authenticated API Calls

The Angular application sends the access token with API requests using the Authorization header.

```http
Authorization: Bearer <access_token>
```

All job, ingest, attachment, and settings endpoints require authentication.

Role-sensitive endpoints can additionally require specific roles.

### Token Refresh

When the access token expires, the Angular application calls a refresh endpoint.

The refresh token is sent automatically by the browser as an HttpOnly cookie.

If the refresh token is valid, the API issues a new access token and rotates the refresh token.

This allows normal usage without repeated manual login.

### Logout

On logout, the frontend calls the API logout endpoint.

The API revokes the current refresh token and clears the refresh cookie.

The Angular application removes the in-memory access token.

## Token Storage

### Access Token

The JWT access token should be stored only in Angular application memory.

It should not be stored in localStorage.

### Refresh Token

The refresh token should be stored in a secure HttpOnly cookie.

The cookie should be configured with:

* HttpOnly;
* Secure;
* SameSite=None, if the frontend and API are on different domains;
* limited lifetime.

Refresh tokens should be stored server-side in hashed form and should support revocation.

## Suggested Token Lifetimes

Access token lifetime:

* 10-15 minutes.

Refresh token lifetime:

* 7-14 days.

These values may be adjusted later, but the MVP should avoid forcing frequent manual re-login during normal daily use.

## API Protection Model

The API should be secure by default.

All application endpoints must require authentication unless explicitly marked as anonymous.

Anonymous access should be limited to authentication, health check, and optional one-time setup endpoints.

New endpoints must not be public by default.

## Single-User Mode

The MVP supports one owner account.

Public registration is disabled.

The initial owner account may be created using one of the following approaches:

* seeded during application startup from environment variables;
* created through a one-time setup command;
* created through a protected local initialization process.

The exact setup method can be decided during implementation.

## Cross-Origin Requirements

The Angular frontend may be hosted on Azure Static Web Apps while the API may be hosted on either Azure App Service or Azure Container Apps.

Because of this, the API must support a configurable allowed frontend origin.

Example configuration:

```text
AllowedOrigins__0=https://<xxx>.example.com
AllowedOrigins__1=https://<xxx>-ui.azurestaticapps.net
```

CORS must allow the Angular frontend to call the API.

If refresh tokens are stored in cookies across different origins, the API must allow credentials only for explicitly configured frontend origins.

Wildcard origins should not be used with credentials.

## Attachment Security

Azure Blob Storage containers should be private.

Uploaded files should not be publicly accessible by default.

The frontend should request files through the API.

The API should verify authentication before allowing access to an attachment.

The MVP may stream files through the API.

A later version may generate short-lived SAS URLs after verifying authentication.

## Deployment Compatibility

The authentication design should work the same way whether the API is deployed to:

* Azure App Service Linux;
* Azure Container Apps Consumption.

The application should not depend on hosting-specific authentication features.

All authentication behavior should be implemented in the ASP.NET Core Web API and configured through environment variables.

## Configuration

Relevant configuration values may include:

```text
Jwt__Issuer
Jwt__Audience
Jwt__SigningKey
Jwt__AccessTokenMinutes
Auth__RefreshTokenDays
Auth__AllowedOrigins
Auth__InitialOwnerEmail
Auth__InitialOwnerPassword
Auth__DemoUserLogin
```

Sensitive values should be stored in Azure App Service Configuration, Azure Container Apps secrets, or later in Azure Key Vault.

## Notes

The MVP should not implement public registration, multi-tenant accounts, social login, organization accounts, or billing-related identity features.

The goal is to protect a single-user online application while keeping the authentication model compatible with the planned Azure deployment options.
