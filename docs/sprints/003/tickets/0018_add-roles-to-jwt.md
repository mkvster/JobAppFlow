# Add role claims to access JWTs

## Description

Include Identity role claims in access tokens so the API can use role-based authorization without an extra database lookup on every request.

Keep refresh tokens role-agnostic. Role claims should be added only to access tokens, not to refresh tokens.

## Acceptance

- Access JWTs include the authenticated user's role claims.
- Refresh JWTs do not include role claims.
- The API is configured to recognize the JWT role claim type for authorization.
- Existing login and refresh flows continue to work after the change.
