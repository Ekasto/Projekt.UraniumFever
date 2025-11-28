# Uranium Fever - Project Architecture

## Overview
Digital Unity implementation of the Uranium Fever board game - a strategic resource management game with disaster events.

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/          # Core game systems (Grid, GameManager, etc.)
│   ├── Game/          # Game-specific logic (Resources, Buildings, Disasters)
│   ├── UI/            # UI components and controllers
│   └── Utilities/     # Helper classes and extensions
├── Tests/
│   ├── EditMode/      # Unit tests (no Unity runtime required)
│   └── PlayMode/      # Integration tests (requires Unity runtime)
├── Prefabs/           # Reusable game objects
├── Materials/         # Visual materials
├── Sprites/           # 2D graphics and UI elements
├── Audio/             # Sound effects and music
└── Scenes/            # Unity scenes
```

## Design Principles

### 1. Test-Driven Development (TDD)
- **Red-Green-Refactor** cycle for all features
- Write failing test first
- Implement minimal code to pass
- Refactor while keeping tests green

### 2. Separation of Concerns
- **Core**: Game-agnostic systems (grid, input, camera)
- **Game**: Domain-specific logic (resources, buildings, disasters)
- **UI**: Presentation layer only, no business logic

### 3. Component-Based Architecture
- Small, focused components with single responsibilities
- Loose coupling between systems
- Events for cross-system communication

## Core Systems

### Grid System
- **GridManager**: Manages 12x12 tile grid
- **GridTile**: Individual tile data (position, occupant, state)
- **GridVisuals**: Handles tile rendering and highlights

### Resource System
- **ResourceType** enum: Electricity, Food, Medicine, PlayerChoice
- **ResourceCard**: Represents a drawable resource card
- **ResourceDeck**: Manages card drawing and shuffling
- **PlayerInventory**: Tracks resources per player on shared board

### Building System
- **BuildingType** enum: Defense, Factory, Research, Road, Bridge, House
- **BuildingData**: Cost, requirements, and effects
- **BuildingPlacer**: Handles building placement logic
- **BuildingValidator**: Validates placement rules

### Turn System
- **TurnManager**: Controls round flow and player turns
- **RoundCounter**: Tracks rounds (disasters start after round 3)
- **PlayerTurnState**: Current player and phase (draw/build/trade)

### Disaster System (Post-MVP)
- **DisasterCard**: Earthquake, Flood, Tornado, Thief, Donkey
- **DisasterEffects**: Applies disaster consequences
- **DisasterDice**: Handles destruction rolls

## Testing Strategy

### Edit Mode Tests
- Pure logic without Unity dependencies
- Fast execution
- Examples: Resource calculations, grid coordinates, building costs

### Play Mode Tests
- Requires Unity runtime
- Tests Unity components and interactions
- Examples: Building placement in scene, UI interactions, GameObject behavior

## Development Workflow

1. **Write test first** (Red)
2. **Implement minimum code** to pass (Green)
3. **Refactor** if needed
4. **Make it playable** - add input/UI so it can be tested in Unity Play mode
5. **Commit** working code
6. **Repeat** for next feature

## Current Phase: Phase 1 - Project Setup
- ✅ Unity project created
- ✅ Folder structure organized
- ✅ Test framework configured
- ✅ Git configured
- 🎯 Next: Grid System development

## Technology Stack
- **Unity 6 (6000.0)**
- **C# (.NET Standard 2.1)**
- **Unity Test Framework 1.6.0**
- **Unity Input System 1.14.2**
- **Universal Render Pipeline (URP) 17.2.0**

## Version Control
- Single shared board (not 3 separate boards)
- Separate inventories per player
- Following claude.md guidelines for TDD and simplicity
