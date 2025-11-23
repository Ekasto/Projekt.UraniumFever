# Project Guidelines for Claude

## Core Principles

### 1. Simplicity Over Complexity
- **Do NOT overengineer solutions**
- Keep implementations straightforward and pragmatic
- Avoid unnecessary abstractions, patterns, or frameworks
- Start with the simplest solution that works
- Only add complexity when there's a clear, demonstrated need

### 2. Test-Driven Development (TDD)
- Write tests FIRST, then implementation
- Follow the Red-Green-Refactor cycle:
  1. Write a failing test (Red)
  2. Write minimal code to make it pass (Green)
  3. Refactor if needed while keeping tests green
- Every new feature requires tests
- Every bug fix should include a test that reproduces the issue

### 3. Incremental Development
- Break work into small, manageable steps
- Complete one small piece at a time
- Each increment should be functional and testable
- Commit frequently with working code
- Build features gradually, not all at once

### 4. Technology Constraints
- **C# ONLY** - no other programming languages
- Stay within the .NET ecosystem
- Use Unity's built-in systems and components

## Working Style
- Make small, focused changes
- Test after each change
- Commit working code frequently
- Keep the codebase always in a working state

## Unity Playability Rule
- **EVERY feature must be playable in Unity after implementation**
- Add input handling so the user can test by pressing Play
- Don't just write backend code - make it interactive
- User should be able to test mechanics immediately with keyboard/mouse
- If implementing movement, add input controls
- If implementing combat, add attack buttons
- No "headless" features that can't be demonstrated in play mode

## Git Workflow
- **ALWAYS `git pull` before opening Unity**
- This prevents merge conflicts and ensures you have the latest changes
- Close Unity before switching branches
- Communicate with team about scene edits (only one person per scene at a time)
- Use prefabs for parallel work
