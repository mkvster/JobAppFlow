# Backend Deploy Pipeline

## Description

Create a GitHub Actions CI/CD pipeline for the backend that supports pull request checks and automatic delivery to Azure App Service Linux after merge to `main`.
This backend pipeline should be the first stage of a release flow that can gate later frontend delivery.
The pipeline should build the backend, run available automated checks, and use a secure secrets setup for Azure authentication and deployment.

## Acceptance

- The backend builds in GitHub Actions.
- The available automated checks run in GitHub Actions.
- Successful pipeline runs deploy the backend to Azure App Service Linux.
- The backend is online and reachable after deployment.
- `GET /health` responds successfully through an HTTP client such as Postman or `curl`.
- Azure credentials and deployment secrets are handled securely.
- The pipeline supports pull request checks and automatic delivery on push to `main` after merge.
- The pipeline exposes a clear success signal that a later frontend deployment stage can depend on.
