# Demo Login Backend

## Description

Add a backend-supported demo login flow that signs the user in without requiring manual credentials.

The API should authenticate a fixed demo account configured through application settings and return a normal authenticated session.

The demo account must be clearly separated from regular users and should be used only for showcase and read-only demo scenarios.

## Acceptance

- The backend exposes a dedicated demo login endpoint.
- The demo login endpoint authenticates the configured demo account without requiring a username or password.
- The demo account is resolved from application configuration, not hardcoded in the controller.
- The demo login flow returns the same session shape as normal login.
- The authenticated demo user can be recognized by its `Demo` role.
- Existing regular login, refresh, and logout flows continue to work unchanged.
