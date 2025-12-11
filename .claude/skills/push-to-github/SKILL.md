---
name: push-to-github
description: Execute full git workflow (pull, add, commit, push) with auto-generated commit messages. Use when user wants to push changes to GitHub, save work to remote, or sync with team.
---

# Push to GitHub Skill

This skill automates the complete git workflow for the Uranium Fever Unity project, following the team's conventions from CLAUDE.md.

## Workflow Overview

Execute these phases in sequence:

1. **Pre-flight checks**
2. **Pull latest changes**
3. **Stage changes**
4. **Generate commit message**
5. **Commit changes**
6. **Push to remote**

## Phase 1: Pre-flight Checks

Run these validations BEFORE starting the workflow:

```bash
# Verify git repository
git status
```

If this fails with "not a git repository":
- **STOP** and inform user
- Ask if they want to initialize git repo
- Guide through setup if needed

```bash
# Check remote configuration
git remote -v
```

If no remote configured:
- **STOP** and inform user
- Provide instructions to add remote

```bash
# Get current branch
git branch --show-current
```

If on `main` or `master`:
- **WARN** user about committing directly to main
- Suggest creating feature branch
- Ask for confirmation to proceed

```bash
# Check if Unity editor is running (Windows)
tasklist | findstr /i "Unity.exe"
```

If Unity is running:
- **WARN** user about potential file locks
- Suggest closing Unity first
- Reference CLAUDE.md guideline: "Close Unity before switching branches"

## Phase 2: Pull Latest Changes

This is CRITICAL per CLAUDE.md: "ALWAYS git pull before opening Unity"

```bash
git pull --rebase
```

**Use `--rebase`** to maintain linear history (Unity best practice).

### Handle Pull Results

**If successful**: Continue to Phase 3

**If merge conflicts occur**:
1. Abort the rebase:
   ```bash
   git rebase --abort
   ```
2. **STOP** the workflow
3. Inform user of conflicts
4. Provide resolution steps:
   - Run `git status` to see conflicted files
   - Manually resolve conflicts in each file
   - Run `git add <resolved-files>`
   - Run `git rebase --continue`
   - Then re-run this skill

**If network error**:
- **STOP** and inform user
- Suggest checking internet connection
- Provide retry option

**If divergent branches warning**:
- Choose rebase strategy (align with --rebase flag)
- Continue workflow

## Phase 3: Stage Changes

Show current status, stage all changes, then show updated status:

```bash
# Show unstaged changes
git status

# Stage all changes
git add .

# Verify staged changes
git status
```

### Validate Staging

**If no changes to commit**:
- Inform user: "No changes to commit"
- Exit gracefully

**If >50 files changed**:
- **WARN**: Large changeset detected
- List file count and types
- Suggest reviewing for unrelated changes
- Ask for confirmation to proceed
- Reference CLAUDE.md principle: "Make small, focused changes"

**If critical files detected** (.env, credentials, etc.):
- **WARN** about potential secrets
- List suspicious files
- Ask for explicit confirmation

## Phase 4: Generate Commit Message

Follow conventional commits format adapted for Unity:

```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

### Step 4.1: Analyze Changes

```bash
# Get summary statistics
git diff --cached --stat

# Get detailed diff (use judiciously for large changes)
git diff --cached --unified=3
```

Parse the diff to identify:
- **File locations**: Which Assets/ subdirectories changed
- **File types**: .cs, .unity, .prefab, .meta, .md
- **Change patterns**: New files, deletions, modifications
- **Code patterns**: New classes, methods, tests, bug fixes

### Step 4.2: Determine Commit Type

Based on change analysis:

- **feat**: New functionality (new scripts, game mechanics, systems)
  - Examples: New player movement, new resource system, new UI component

- **fix**: Bug fixes (corrections to existing functionality)
  - Examples: Fix movement speed, correct UI alignment, resolve null reference

- **test**: Test additions or modifications
  - Examples: Add movement tests, update gameplay tests

- **refactor**: Code restructuring without behavior change
  - Examples: Extract method, rename classes, reorganize folders

- **docs**: Documentation only
  - Examples: Update README, add code comments, create setup guides

- **chore**: Build, Unity settings, meta files, dependencies
  - Examples: Update Unity version, regenerate meta files, update packages

**Multiple types detected?**
- Suggest splitting into separate commits
- Follow "incremental development" principle from CLAUDE.md

### Step 4.3: Identify Scope (Optional)

Extract scope from file paths in Assets/:

- `Assets/Scripts/Movement/` → `(movement)`
- `Assets/Scripts/Combat/` → `(combat)`
- `Assets/Scripts/UI/` → `(ui)`
- `Assets/Scripts/Resources/` → `(resources)`
- `Assets/Scripts/Gameplay/` → `(gameplay)`
- `Assets/Scenes/` → `(scene)`
- `Assets/Prefabs/` → `(prefabs)`
- `Assets/Tests/` → `(tests)`

**Multiple scopes?**
- Omit scope if changes span multiple areas
- OR use broader scope like `(gameplay)` or `(core)`

### Step 4.4: Write Description

Rules:
- **Imperative mood**: "add" not "added" or "adds"
- **Lowercase**: Start with lowercase letter
- **Length**: 50-72 characters maximum
- **Specific**: Describe WHAT changed clearly
- **No period**: Don't end with period

Examples:
- ✓ "add player movement controls with WASD input"
- ✓ "fix resource card drawing duplicate bug"
- ✓ "refactor disaster system to use strategy pattern"
- ✗ "updated some stuff"
- ✗ "changes made"
- ✗ "WIP"

### Step 4.5: Write Body (If Needed)

Add body when:
- Change needs explanation of WHY
- Tests were added (follow TDD mention)
- Playability note per Unity Playability Rule
- Breaking change that needs context

Format:
- Blank line after description
- Wrap at 72 characters
- Explain motivation and context
- Reference project principles if relevant

Example:
```
feat(movement): add player movement controls with WASD input

Implements basic movement system following TDD approach.
Tests verify correct direction and speed calculations.
Playable in Unity editor with WASD keys for immediate testing.
```

### Step 4.6: Add Footer (If Needed)

Use for:
- **Breaking changes**: `BREAKING CHANGE: <description>`
- **Issue references**: `Fixes #123`, `Closes #456`
- **Reviewed by**: `Reviewed-by: Name <email>`

### Step 4.7: Learn from History

Before finalizing, check recent commits for style consistency:

```bash
git log --oneline -10
```

Match existing patterns for:
- Commit type frequency
- Scope naming conventions
- Description style
- Body usage

### Step 4.8: Validate Generated Message

Check:
- [ ] Follows conventional commits format
- [ ] Type is appropriate for changes
- [ ] Scope matches project structure
- [ ] Description is imperative mood
- [ ] Description is 50-72 chars
- [ ] Body explains WHY if needed
- [ ] Aligns with CLAUDE.md principles

**If message quality is uncertain**:
- Reference REFERENCE.md for examples
- Show generated message to user
- Offer to refine

## Phase 5: Commit Changes

Execute commit with generated message:

```bash
git commit -m "$(cat <<'EOF'
<generated-message-here>
EOF
)"
```

**Use HEREDOC format** to preserve multi-line messages correctly.

### Verify Commit Success

```bash
# Show commit was created
git log -1 --oneline

# Verify working tree is clean
git status
```

**If commit fails**:
- Show error message
- Common issues:
  - Empty commit (nothing staged)
  - Pre-commit hook failure
  - Author identity not configured
- Provide resolution steps

## Phase 6: Push to Remote

Push committed changes to remote repository:

```bash
git push
```

### Handle Push Scenarios

**If upstream not set** (first push on new branch):
```bash
git push -u origin <branch-name>
```

**If push rejected** (remote has changes):
1. Inform user remote was updated
2. Run `git pull --rebase`
3. Resolve any conflicts
4. Retry push

**If authentication fails**:
- Guide to credential setup
- Suggest SSH key or credential manager
- Provide GitHub authentication documentation link

**If successful**:
- Confirm push completed
- Show remote branch information
- Note number of commits pushed

## Success Summary

After successful push, display:

```
✓ Changes pushed to GitHub successfully!

Summary:
- Branch: <branch-name>
- Commit: <commit-hash> <commit-message>
- Files changed: <count>
- Remote: <remote-url>
```

## Error Recovery

If workflow fails at any phase:

1. **Show clear error message** with phase name
2. **Preserve work**: Don't discard changes
3. **Provide next steps**:
   - What went wrong
   - How to fix manually
   - How to resume workflow
4. **Offer rollback** if commit was made but push failed:
   ```bash
   git reset --soft HEAD^  # Undo commit, keep changes staged
   ```

## Best Practices Alignment

This skill follows CLAUDE.md principles:

- **Simplicity**: Single command for full workflow
- **TDD**: Highlights test additions in commits
- **Incremental**: Encourages small, focused commits
- **Working code**: Ensures pull before push
- **Unity Playability**: Notes interactive features in messages

## Related Resources

For deeper understanding:
- See REFERENCE.md for commit message examples
- Review CLAUDE.md for project conventions
- Check Unity best practices documentation

## Usage Examples

User says: "push to github"
→ Skill executes full workflow

User says: "save changes to remote"
→ Skill executes full workflow

User says: "sync with team"
→ Skill executes full workflow

User says: "commit and push"
→ Skill executes full workflow
