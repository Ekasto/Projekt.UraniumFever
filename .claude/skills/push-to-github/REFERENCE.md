# Push to GitHub - Commit Message Reference

This reference provides detailed conventions and examples for commit messages in the Uranium Fever Unity project.

## Conventional Commits Format

```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

### Components

**Type** (required): Category of change
**Scope** (optional): Area of codebase affected
**Description** (required): Brief summary of change
**Body** (optional): Detailed explanation
**Footer** (optional): Breaking changes, issue references

## Commit Types

### feat - New Features

Use when adding new functionality or capabilities.

**Unity Examples**:
```
feat(movement): add player movement with WASD controls
feat(resources): implement resource card drawing system
feat(disasters): add tsunami disaster event
feat(ui): create main menu with play button
feat(combat): implement basic attack system
feat(trading): add resource trading between players
```

**Guidelines**:
- Describe the new capability added
- Include user-facing functionality
- Note if it's playable (Unity Playability Rule)

### fix - Bug Fixes

Use when correcting defects or unexpected behavior.

**Unity Examples**:
```
fix(movement): prevent player moving through walls
fix(resources): correct duplicate card draw bug
fix(ui): resolve button click not registering
fix(disasters): fix Don(k)zilla pathfinding error
fix(scene): repair broken prefab references in main scene
```

**Guidelines**:
- Describe what was broken
- Explain the fix if non-obvious
- Reference issue number if available

### test - Tests

Use when adding or modifying tests.

**Unity Examples**:
```
test(movement): add unit tests for player movement
test(resources): verify resource card distribution
test(disasters): test tsunami direction calculation
test(gameplay): add integration tests for full round
```

**Guidelines**:
- Specify what's being tested
- Note if following TDD (write test first)
- Mention test coverage improvements

### refactor - Code Restructuring

Use when improving code structure without changing behavior.

**Unity Examples**:
```
refactor(movement): extract movement logic to separate class
refactor(disasters): apply strategy pattern to disaster events
refactor(ui): reorganize UI scripts into subfolders
refactor(resources): rename ResourceCard to GameResourceCard
```

**Guidelines**:
- Describe the structural change
- Emphasize no behavior change
- Mention if improving testability

### docs - Documentation

Use for documentation-only changes.

**Unity Examples**:
```
docs(readme): update game rules for disasters
docs(architecture): add system overview diagram
docs(setup): create Unity installation guide
docs(code): add XML comments to public API
```

**Guidelines**:
- Specify documentation type
- Note if adding or updating
- Mention audience if relevant

### chore - Maintenance

Use for build, Unity settings, meta files, dependencies.

**Unity Examples**:
```
chore(unity): update Unity version to 2022.3 LTS
chore(meta): regenerate meta files for new assets
chore(packages): update InputSystem package to 1.5.0
chore(build): configure build settings for Windows
chore(scene): update scene meta files
```

**Guidelines**:
- Be specific about what changed
- Include version numbers
- Note if affects team setup

## Scope Guidelines

Scopes identify the area of the codebase affected. Use project folder structure as guide.

### Common Scopes for Uranium Fever

**Game Systems**:
- `(movement)` - Player movement
- `(combat)` - Combat/attack systems
- `(resources)` - Resource management
- `(trading)` - Trading system
- `(disasters)` - Disaster events
- `(gameplay)` - General game mechanics

**Unity Specific**:
- `(scene)` - Scene files
- `(prefabs)` - Prefab assets
- `(ui)` - User interface
- `(tests)` - Test code
- `(build)` - Build configuration

**Project**:
- `(docs)` - Documentation
- `(config)` - Configuration files
- `(ci)` - CI/CD pipelines

### When to Omit Scope

Omit scope when:
- Changes span multiple areas
- Scope is obvious from type
- Change is project-wide

Examples:
```
docs: update README with new game rules
chore: upgrade Unity to 2023.1 LTS
```

## Writing Great Descriptions

### Rules

1. **Imperative mood**: Command form, present tense
   - ✓ "add", "fix", "update", "remove"
   - ✗ "added", "fixing", "updates", "removed"

2. **Lowercase**: Start with lowercase (unless proper noun)
   - ✓ "add player movement"
   - ✗ "Add player movement"

3. **Length**: 50-72 characters ideal
   - Readable in git log --oneline
   - Complete but concise

4. **No period**: Don't end with period
   - ✓ "fix button alignment"
   - ✗ "fix button alignment."

5. **Specific**: Clear about what changed
   - ✓ "add WASD movement controls"
   - ✗ "update movement"

### Good vs Bad Examples

**Good**:
```
feat(movement): add player movement with WASD controls
fix(ui): prevent button overlap in main menu
test(disasters): verify tsunami tile destruction
refactor(resources): extract card shuffling logic
docs(setup): add Unity project setup instructions
```

**Bad**:
```
updated stuff                     # Too vague
Fixed bugs                        # Not specific
WIP                              # Not descriptive
Movement changes                 # Not imperative
Added new feature for players.   # Past tense, has period
```

## Writing Useful Bodies

Add a body when the description alone isn't sufficient.

### When to Add Body

- Explain WHY the change was made
- Provide context for future readers
- Note TDD approach (tests written first)
- Mention playability (Unity Playability Rule)
- Describe alternative approaches considered
- Reference related changes

### Body Format

- Blank line after description
- Wrap at 72 characters per line
- Use multiple paragraphs if needed
- Bullet points are okay

### Examples with Body

```
feat(movement): add player movement with WASD controls

Implements basic grid-based movement system. Players can move
one tile per turn in cardinal directions using WASD keys.

Follows TDD approach with tests verifying correct direction
calculation and boundary checking. Playable immediately in
Unity editor for testing.

Refs #12
```

```
fix(disasters): correct Don(k)zilla pathfinding algorithm

Previous implementation used Manhattan distance but didn't
account for destroyed tiles. Now uses A* pathfinding to
navigate around obstacles.

This fixes the issue where Don(k)zilla would get stuck when
tsunami destroyed tiles in its path.

Fixes #45
```

```
refactor(resources): apply factory pattern to resource creation

Extracted resource instantiation logic into ResourceFactory.
This improves testability by allowing mock resources in tests
and centralizes resource configuration.

No behavior change - all existing tests still pass.
```

## Footers

Use footers for metadata and references.

### Breaking Changes

Mark breaking changes (API changes, behavior changes affecting others):

```
feat(movement)!: change movement from tile-based to continuous

BREAKING CHANGE: Movement system now uses continuous coordinates
instead of discrete tiles. Existing save files incompatible.
```

**Alternative notation**: `!` after scope indicates breaking change

### Issue References

Link commits to issues/tickets:

```
fix(ui): prevent button overlap in main menu

Fixes #23
```

```
feat(disasters): implement earthquake disaster

Closes #18, #19
```

**Keywords**:
- `Fixes #123` - Marks issue as fixed
- `Closes #123` - Closes issue
- `Refs #123` - References issue without closing

### Multiple Footers

```
feat(combat): add damage calculation system

Implements dice-based damage with modifiers from buildings.

Reviewed-by: TeamMember <email@example.com>
Refs #34, #35
```

## Unity-Specific Guidelines

### Scene Files

Scene changes often affect many files (.unity, .meta). Be descriptive:

```
feat(scene): add main game board with grid layout

chore(scene): update scene meta files after Unity upgrade
```

### Prefabs

When modifying prefabs:

```
feat(prefabs): create resource card prefab with variants

fix(prefabs): restore broken player prefab reference
```

### Meta Files

Meta files usually don't need dedicated commits:

```
chore(meta): regenerate meta files for new assets
```

BUT if meta file changes are the ONLY changes:

```
chore(meta): fix GUID conflict in PlayerController.meta
```

### Test Files

Always mention test additions (TDD principle):

```
test(movement): add tests for diagonal movement blocking

Test verifies players cannot move diagonally per game rules.
Written before implementing the restriction (TDD).
```

## Multi-Change Commits

### When to Split

Split into multiple commits if changes include:
- Multiple unrelated features
- Feature + refactoring
- Multiple bug fixes for different systems

**Example of what to split**:
Instead of:
```
feat: add movement and fix UI bug
```

Split into:
```
feat(movement): add player movement with WASD controls
(commit separately)
fix(ui): prevent button overlap in main menu
```

### When to Combine

Combine related changes:
- Feature + its tests
- Fix + regression test
- Refactoring + updated tests

**Example of what to combine**:
```
feat(movement): add player movement with WASD controls

Implements movement system with comprehensive test coverage.
Tests verify direction calculation, boundary checking, and
tile occupation validation.
```

## Real-World Examples

### Example 1: New Feature with Tests
```
feat(resources): implement resource card drawing system

Players draw 3 resource cards per round from shuffled deck.
Includes validation for deck exhaustion and reshuffling.

Follows TDD approach:
- Tests written first for draw mechanics
- Tests verify card distribution fairness
- Tests check deck reshuffling behavior

Playable in Unity with D key to draw cards for testing.

Refs #8
```

### Example 2: Bug Fix
```
fix(disasters): prevent tsunami from destroying headquarters twice

Previous bug allowed tsunami to trigger game over multiple
times when hitting headquarters at angle. Now checks if
headquarters already destroyed before triggering game over.

Adds regression test to prevent future occurrences.

Fixes #42
```

### Example 3: Refactoring
```
refactor(disasters): extract disaster effects into strategy pattern

Restructures disaster system:
- DisasterEffect interface for polymorphism
- TsunamiEffect, ThiefEffect, DonzillaEffect implementations
- Removes large switch statement in DisasterManager

No behavior change - all existing tests pass without modification.
Improves extensibility for future disaster types.
```

### Example 4: Documentation
```
docs(readme): add disaster mechanics explanation

Expands README with detailed disaster rules:
- Tsunami movement and destruction patterns
- Thief resource stealing mechanics
- Don(k)zilla pathfinding and health system

Includes strength and direction tables for reference.
```

### Example 5: Chore
```
chore(unity): update Unity version from 2021.3 to 2022.3 LTS

Updates project to Unity 2022.3 LTS for long-term support.
Regenerates .csproj and meta files.

Team members must upgrade Unity before pulling this change
to avoid merge conflicts in scene files.
```

## Commit Message Checklist

Before committing, verify:

- [ ] Type is correct (feat, fix, test, refactor, docs, chore)
- [ ] Scope matches project structure (if applicable)
- [ ] Description uses imperative mood
- [ ] Description is 50-72 characters
- [ ] Description is lowercase (unless proper noun)
- [ ] Description has no ending period
- [ ] Body explains WHY if needed
- [ ] TDD mentioned if tests written first
- [ ] Playability noted if feature is interactive
- [ ] Breaking changes marked with `!` or footer
- [ ] Issue references included if applicable
- [ ] Message aligns with CLAUDE.md principles
- [ ] Related changes are grouped together
- [ ] Unrelated changes are in separate commits

## Resources

**Conventional Commits Specification**:
https://www.conventionalcommits.org/

**Unity Version Control Best Practices**:
https://unity.com/how-to/version-control-systems

**Project Guidelines**:
See CLAUDE.md in project root
