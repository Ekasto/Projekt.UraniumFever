# Phase 2 Setup Guide - Grid System

This guide will help you set up the playable grid system in Unity.

## What We Built

- ✅ **GridTile** - Data structure for individual tiles (with tests)
- ✅ **GridManager** - Logic for creating and managing 8x8 grid (with tests)
- ✅ **GridVisualizer** - Renders the grid in Unity scene
- ✅ **CameraController** - Navigate the board (pan, zoom, rotate)
- ✅ **TileSelector** - Select and highlight tiles with mouse

## Setup Instructions

### Step 1: Open Unity Project
1. Open Unity Hub
2. Open the project: `C:\Users\ekast\Desktop\Projekt.UraniumFever\UraniumFever`
3. Wait for Unity to load and compile scripts

### Step 2: Run Tests (Optional but Recommended)
1. In Unity, go to **Window > General > Test Runner**
2. Click the **EditMode** tab
3. Click **Run All**
4. You should see all tests pass (green checkmarks)

### Step 3: Set Up the Scene
1. Open `Assets/Scenes/SampleScene.unity`
2. Delete the default objects if you want (or keep them)

### Step 4: Create the Grid System
1. **Create Grid GameObject:**
   - Right-click in Hierarchy → Create Empty
   - Rename it to "Grid"
   - Add Component → Search "Grid Visualizer"
   - The grid will appear when you press Play

2. **Set Up Camera:**
   - Select "Main Camera" in Hierarchy
   - Set Position: `(0, 10, -5)` (or similar to see the board)
   - Set Rotation: `(45, 0, 0)` (looking down at an angle)
   - Add Component → Search "Camera Controller"

3. **Create Tile Selector:**
   - Select the "Grid" GameObject
   - Add Component → Search "Tile Selector"
   - In the Inspector, the GridVisualizer should auto-assign

### Step 5: Test in Play Mode
1. Press the **Play** button (▶) in Unity
2. You should see an 8x8 checkerboard grid!

### Step 6: Test Controls

**Camera Controls:**
- **W/A/S/D or Arrow Keys** - Pan the camera
- **Mouse Scroll Wheel** - Zoom in/out
- **Q/E Keys** - Rotate camera left/right
- **Right Mouse Button + Drag** - Rotate camera
- **Middle Mouse Button + Drag** - Pan camera

**Tile Selection:**
- **Hover over tiles** - They highlight yellow
- **Left Click on tile** - Selects it (turns green)
- Check the Console for "Selected tile at (x, y)" messages

## Expected Result

You should see:
- An 8x8 grid of tiles (checkerboard pattern)
- Camera controls working smoothly
- Tiles highlighting when hovered
- Tiles staying selected when clicked
- Console logging which tile you selected

## Troubleshooting

### Grid doesn't appear
- Make sure GridVisualizer is attached to a GameObject
- Check that the script compiled without errors (check Console)

### Can't select tiles
- Make sure TileSelector component is added
- Check that Main Camera has a Camera component
- Tiles need Colliders (automatically added when using Cube primitives)

### Camera controls don't work
- Make sure CameraController is attached to the Main Camera
- Check the Console for any errors

## Next Steps

Once this is working, we'll move to **Phase 3: Resource System** where you'll be able to draw resource cards!

## Running Tests

To run the unit tests we wrote:
1. **Window > General > Test Runner**
2. Click **EditMode** tab
3. **Run All**

All tests should pass:
- ✅ GridTile_Constructor_SetsPositionCorrectly
- ✅ GridTile_Constructor_SetsWorldPositionCorrectly
- ✅ GridTile_IsOccupied_DefaultsToFalse
- ✅ GridTile_SetOccupied_UpdatesState
- ✅ GridManager_CreateGrid_Creates8x8Grid
- ✅ GridManager_GetTile_ReturnsCorrectTile
- ✅ GridManager_GetTile_InvalidCoordinates_ReturnsNull
- ✅ GridManager_IsValidPosition_ValidCoordinates_ReturnsTrue
- ✅ GridManager_IsValidPosition_InvalidCoordinates_ReturnsFalse
- ✅ GridManager_GetAllTiles_ReturnsAll64Tiles
