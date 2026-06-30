# Role management CLI

## Description

Extend the admin CLI with explicit role-management commands without changing the existing user lifecycle commands.

The CLI should support:
1. adding a role;
jaf db -add-role <role_name> 

2. deleting a role;
jaf db -remove-role <role_name> 

3. assigning any role to any user;
jaf db -add-user-role -login <login> -role <role_name>

4. removing any role from any user.
jaf db -remove-user-role -login <login> -role <role_name>

## Acceptance

- Existing CLI user commands keep their current behavior and syntax.
- The CLI can create a new role.
- The CLI can delete a role.
- The CLI can assign a role to a specified user.
- The CLI can remove a role from a specified user.
