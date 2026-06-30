# AI Agent Instructions

## Purpose

This repository is designed to be used with AI coding agents without depending on a specific vendor or tool.

The project process, architectural decisions, database workflow, authentication model, deployment approach, and ticket workflow are documented in Markdown files under `docs/`.

AI agents must use those documents as the source of truth instead of inventing project rules during implementation.

## Before making code changes

Read the selected ticket first.

Tickets are located in:

* `docs/backlog/`
* `docs/sprints/<sprint-number>/tickets/`

Then read only the project documents relevant to the selected ticket.

Relevant documents:

* Product scope: `docs/mvp-vision.md`
* General architecture: `docs/architecture.md`
* Authentication and roles: `docs/authentication.md`
* Database workflow: `docs/db-workflow.md`
* Deployment: `docs/deployment.md`
* Ticket workflow: `docs/standards/task-tracking.md`

Do not load unrelated documents unless the ticket requires them.

## Source of truth

Do not duplicate or override project decisions in this file.

If a project decision is needed, read the corresponding document under `docs/`.

If project documents conflict with existing code, report the conflict before making broad changes.

If a project document appears outdated, do not silently replace it with a new approach. Call out the issue and propose a separate documentation or architecture update.

## Scope control

Keep changes limited to the selected ticket.

Prefer small, reviewable diffs.

Do not introduce new architecture, packages, database migration strategies, authentication approaches, deployment models, or major rewrites unless the selected ticket explicitly requires it.

If extra work is discovered, propose a new backlog ticket instead of silently expanding the current task.

## Verification

After making changes, report:

* changed files;
* what changed and why;
* build/test commands run;
* command results;
* any commands that could not be run;
* any follow-up backlog tickets that should be created.

Do not mark work complete unless the ticket acceptance criteria are satisfied.
