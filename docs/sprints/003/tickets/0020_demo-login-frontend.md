# Demo Login Frontend

## Description

Add a frontend demo sign-in path to the Angular login experience.

The login page should let the user choose demo mode and sign in without typing credentials. The frontend should call the demo login endpoint, store the returned session, and navigate into the authenticated app.

When a demo session is active, the UI should clearly indicate that the user is in demo mode.

## Acceptance

- The login page offers a demo sign-in option.
- Demo sign-in does not require manual username or password entry.
- The frontend calls the dedicated demo login endpoint.
- A successful demo login stores the returned session the same way as a normal login.
- The app can detect that the current session belongs to a demo user.
- The authenticated UI shows a clear demo-mode indicator.
- Existing normal login, refresh, and logout flows continue to work.
