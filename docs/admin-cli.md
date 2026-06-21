# Admin CLI Usage

The admin CLI is used to connect to the database and run command-line administration tasks.

## Example usage

- Register a user:
  - `dotnet run --project src/AdminCli -- register-user`
- Register the local development users:
  - `dotnet run --project src/AdminCli -- register-user --name owner`
  - `dotnet run --project src/AdminCli -- register-user --name tester`
  - `dotnet run --project src/AdminCli -- register-user --name demo`

## Notes

- Connection settings should come from secure configuration.
- Passwords should come from secure configuration.

