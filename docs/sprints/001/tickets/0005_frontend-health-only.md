# Frontend Health Only

## Description

Create a minimal Angular 21 frontend skeleton that can connect to the backend through configuration and secrets.
The UI should check the backend health endpoint and show whether the system is healthy.
The UI should display a simple success state such as `OK` or `Online`, and a clear failure state when the health check does not pass.

## Acceptance

- An Angular 21 frontend skeleton exists.
- The frontend can use configured backend connection values and secrets.
- The frontend checks the backend health endpoint.
- The UI shows a clear healthy state such as `OK` or `Online`.
- The UI shows a clear unhealthy state when the health check fails.
