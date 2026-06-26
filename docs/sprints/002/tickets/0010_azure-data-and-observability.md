# Azure Data and Observability

## Description

Set up the Azure resources that support application data and observability.

Create the following Azure resources manually:

- Azure SQL Database;
- Azure Application Insights;
- Log Analytics Workspace for Application Insights, if required by Azure.

Configure the backend App Service to use these resources (sqldb connection string + appi connection string).

## Acceptance

- The Azure data resources are created and configured manually.
- The Azure observability resource is created if it is included in the task.
- The environment is ready for backend integration with database and telemetry.
