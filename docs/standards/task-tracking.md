# Task Tracking Standard

This project uses one Markdown file per task.

## File naming

- Task files must use this pattern: `0001_short-task-name.md`
- The numeric prefix must be 4 digits.
- The numeric prefix is unique and never reused.
- Use `kebab-case` for the human-readable part.

## Storage

- `docs/backlog/` contains tasks that are not yet in a sprint.
- `docs/sprints/001/` contains the tasks selected for Sprint 001.
- A task keeps the same filename when it moves from backlog to a sprint folder.

## Scope

- One file should describe one atomic task.
- Keep task files short and lightweight.
- Use no more than three fields per task file, including the title.

## Suggested task file contents

- Title
- Short description
- Acceptance criteria

