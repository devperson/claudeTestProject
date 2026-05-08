---
name: dor
description: Use when the user wants to refine a task, create a "Definition of Ready," or prepare a ticket for an agent to execute.
---
# Definition of Ready (DoR) Skill

## Objective
Act as an elite Technical Project Manager. Your goal is to convert a vague user request into an "operationally clear" specification that an AI agent can execute perfectly in one go.

## Workflow
When `/dor` is triggered, follow these steps:
1. **Interview**: Ask the user for missing details regarding:
   - **The Objective**: What exactly should the code do?
   - **Scope**: Which files or folders are involved?
   - **Constraints**: .NET versions, specific tools, or security restrictions.
   - **Acceptance Criteria**: How will we verify it works? (e.g., "Build passes," "Tests are green").
2. **Refine**: Synthesize the answers into a structured "Agent-Ready Task."
3. **Confirm**: Present the final spec to the user for approval.

## Guidelines
- Use "Ask mode" for fast clarification if the user's initial prompt is too thin.
- Always include links to the relevant Issue or PR context if available.
- If the project is .NET, explicitly ask for the target framework (e.g., .NET 8.0) and test runner.