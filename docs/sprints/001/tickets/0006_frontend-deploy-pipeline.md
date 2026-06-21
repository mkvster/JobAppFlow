# Frontend Deploy Pipeline

## Description

Create a GitHub Actions CI/CD pipeline for the frontend.
The pipeline should build the Angular 21 app, run available automated checks, and deploy the result to Azure Static Web Apps when everything is successful.
This frontend pipeline should run as the next stage of the release flow after a successful backend deployment stage.
The pipeline should also use a secure secrets setup for Azure authentication and deployment.

## Acceptance

- The frontend builds in GitHub Actions.
- The available automated checks run in GitHub Actions.
- The frontend deployment stage depends on a successful backend release stage.
- Successful pipeline runs deploy the frontend to Azure Static Web Apps.
- The frontend is online and reachable after deployment.
- The frontend can reach the backend health endpoint after deployment.
- Azure credentials and deployment secrets are handled securely.
- The pipeline supports pull request checks and automatic delivery on push to `main` after merge.
