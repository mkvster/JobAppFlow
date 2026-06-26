# Azure SQL Setup

## Description

Apply the current database scripts to Azure SQL Database.

After the database schema is ready, configure the admin CLI to connect to Azure SQL Database and use it to register:

* a private administrator account defined in local configuration;
* the `tester` development account;
* the `demo` account.

## Acceptance

- Azure SQL Database exists.
- The current database scripts are applied to Azure SQL Database.
- The admin CLI can connect to Azure SQL Database.
- The admin CLI can register an administrator account using values from local configuration.
- The admin CLI can register the `tester` and `demo` users.
- The Azure database is ready for the backend to use.

