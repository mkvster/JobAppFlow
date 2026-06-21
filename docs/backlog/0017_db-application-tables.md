# DB Application Tables

## Description

Add the application-specific tables needed by JobAppFlow to both the local database and the Azure database schema.
The SQL scripts should define the application tables consistently for both environments.

## Acceptance

- The local database includes the application tables required by the app.
- The Azure database includes the same application tables.
- The result includes DDL scripts for creating the application-specific schema in both environments.
- The schema is separated cleanly from the Identity foundation.
