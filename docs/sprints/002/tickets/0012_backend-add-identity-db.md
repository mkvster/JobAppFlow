# Backend Add Identity ORM Foundation

## Description

Add the backend ORM foundation needed to work with the Identity database.
This ticket establishes the Identity data access layer so it can be used first by the admin CLI for user registration and seeding.
API integration is intentionally out of scope for this ticket.

## Acceptance

- Backend has an Identity ORM/data access layer.
- The layer can connect to the Identity-related database structure.
- Identity-related persistence is wired into the backend skeleton.
- The layer is ready to be consumed by the admin CLI for user registration and seed flows.