# Building System Setup Guide

## What's Been Implemented ✅

### Core Systems:
1. **Edge-Based Bridge System** - Bridges placed on grid edges (not tiles)
2. **Building Placement** - Buildings placed on tiles adjacent to bridges
3. **Network Manager** - Calculates bonuses from connected buildings
4. **Building Effects**:
   - Factory: +4 resources when drawing
   - House: +1 resource when drawing
   - Research: Draw twice per turn
   - Defense: -1 disaster strength (logged but not fully implemented)
5. **Shared Networks** - Players share bonuses when bridges connect
6. **UI & Input** - Keyboard shortcuts and buttons for placement

---

## Unity Scene Setup Instructions

### Step 1: Add EdgeVisualizer to Grid

1. In Hierarchy, select the **Grid** GameObject
2. Add Component → **Edge Visualizer** (script)
3. The settings should auto-populate (tileSize: 1, tileSpacing: 0.1)

### Step 2: Update GameManager References

1. In Hierarchy, select **GameManager**
2. In Inspector, find the GameManager component
3. **Drag references:**
   - Grid Visualizer: Drag "Grid" GameObject
   - HQ Visualizer: Should already be assigned
   - **Edge Visualizer**: Drag "Grid" GameObject (it now has EdgeVisualizer component)

### Step 3: Create BuildingInputHandler

1. In Hierarchy, **Create Empty** GameObject
2. Rename to "BuildingInputHandler"
3. Add Component → **Building Input Handler** (script)
4. In Inspector:
   - **Game Manager**: Drag "GameManager" GameObject
   - **Main Camera**: Drag "Main Camera" from hierarchy
   - **Tile Layer**: Leave default
   - **Raycast Distance**: 100

### Step 4: Update UI for Building Controls

#### A. Create Building Buttons

1. Right-click **GameUI Canvas** → UI → Button - TextMeshPro
2. Create **5 buttons** and rename them:
   - BridgeButton
   - HouseButton
   - FactoryButton
   - ResearchButton
   - DefenseButton

3. For each button, update the child Text:
   - Bridge: "Bridge (B) - 2P+1S"
   - House: "House (H) - 2P+1S"
   - Factory: "Factory (F) - 2P+1S"
   - Research: "Research (R) - 5P+1S"
   - Defense: "Defense (D) - 7P+1S"

4. Arrange buttons horizontally or vertically on screen

#### B. Create Info Text Panels

1. Right-click **GameUI Canvas** → UI → Text - TextMeshPro
2. Create and rename:
   - **PlayerInfoText** (shows current player resources)
   - **NetworkBonusText** (shows building bonuses)

3. Position them on screen (e.g., top-left corner)

#### C. Add BuildingUI Component

1. Select **GameUI** GameObject (or create if it doesn't exist under Canvas)
2. Add Component → **Building UI** (script)
3. In Inspector, assign references:
   - **Game Manager**: Drag "GameManager"
   - **Input Handler**: Drag "BuildingInputHandler"
   - **Bridge Button**: Drag "BridgeButton"
   - **House Button**: Drag "HouseButton"
   - **Factory Button**: Drag "FactoryButton"
   - **Research Button**: Drag "ResearchButton"
   - **Defense Button**: Drag "DefenseButton"
   - **Info Text**: Drag "PlayerInfoText"
   - **Network Bonus Text**: Drag "NetworkBonusText"

4. **Update Draw Card Button**:
   - Select "DrawCardButton" in hierarchy
   - In Inspector, find Button component
   - Click "+" in OnClick() event
   - Drag **GameUI** GameObject to the object field
   - Select: BuildingUI → OnDrawCardClicked()

### Step 5: Test in Play Mode

Press **Play** (▶) and test the system!

---

## Controls & Usage

### Keyboard Shortcuts:
- **B** - Select Bridge mode
- **H** - Select House
- **F** - Select Factory
- **R** - Select Research
- **D** - Select Defense
- **ESC** - Cancel selection

### Placing Bridges:
1. Press **B** or click "Bridge" button
2. **Click first tile** (shows in console)
3. **Click adjacent tile** to complete bridge
4. Bridge appears as colored bar between tiles
5. **IMPORTANT Rules**:
   - **Bridges CANNOT be placed ON the HQ tile itself**
   - First bridge must be placed on tiles **NEXT TO** your HQ
   - Example: HQ at (0,6) → Place bridge from (1,6) to (2,6) ✓
   - Example: HQ at (0,6) → Cannot place bridge from (0,6) to (1,6) ✗
   - Subsequent bridges must connect to your existing network

### Placing Buildings:
1. Press hotkey (H/F/R/D) or click button
2. **Click tile** that is:
   - **Directly on** a bridge tile (one of the two tiles the bridge connects), OR
   - **Adjacent to** a tile that has a bridge
3. Building appears as colored cube on tile
4. Bonuses automatically apply when drawing cards

### Drawing Cards:
1. Click **Draw Card** button
2. Resources gained with bonuses:
   - Base: 1 resource
   - Factory: +4 per factory in network
   - House: +1 per house in network
   - Research: Draw twice if present
3. Check console for detailed breakdown

---

## Testing Checklist

### Basic Functionality:
- ✅ 12x12 grid appears
- ✅ 3 HQs appear at edges (Green, Yellow, Red)
- ✅ Building buttons visible with costs
- ✅ Player info shows resources

### Bridge Placement:
- ✅ Click two tiles to place bridge
- ✅ Bridge appears as colored bar
- ✅ Must connect to HQ (first bridge)
- ✅ Can extend from existing bridges
- ✅ Costs 2 Primary + 1 Secondary

### Building Placement:
- ✅ Select building type
- ✅ Click tile adjacent to bridge
- ✅ Building appears as colored cube
- ✅ Cannot place if no adjacent bridge
- ✅ Costs resources (check costs on buttons)

### Building Effects:
- ✅ Place Factory → Draw card → Get +4 resources
- ✅ Place House → Draw card → Get +1 resource
- ✅ Place Research → Draw card → Draw twice
- ✅ Check console for detailed logging

### Network Sharing:
- ✅ Player 1 builds Factory
- ✅ Player 2 connects bridge to Player 1's network
- ✅ Player 2 draws card → Gets Factory bonus
- ✅ Both players benefit from shared network

---

## Known Limitations

1. **No visual feedback** for valid/invalid placement (check console)
2. **Broken car system** not implemented yet
3. **House Upgrade** not implemented (button would need to be added)
4. **Car mechanics** not implemented
5. **Disaster effects** partially implemented (Defense logged but not applied)
6. **No undo/redo**
7. **Buildings don't show ownership** (all same color per type)

---

## Troubleshooting

### "Cannot place bridge"
- First bridge must be adjacent to your HQ (tiles next to HQ work!)
- Later bridges must connect to existing network
- Check console for exact error message
- Ensure tiles are adjacent (no diagonals)

### "Cannot place building"
- Must be on a bridge tile OR next to a tile with a bridge
- Tile must be empty
- Must have enough resources

### "Bridge doesn't appear"
- Check EdgeVisualizer is on Grid GameObject
- Check EdgeVisualizer is initialized (console shows no errors)
- Try clicking "Refresh All Bridges" in EdgeVisualizer Inspector (Play mode)

### "Bonuses not applying"
- Check console - it logs "+X from network"
- Building must be connected to your HQ via bridges
- Use BFS pathfinding to trace network

### "UI buttons don't work"
- Check BuildingUI component references are assigned
- Check BuildingInputHandler is in scene
- Check OnClick events are properly configured

---

## Next Steps (Future Implementation)

1. **Broken Car System** - Spawn at start, repair to activate
2. **Visual Feedback** - Green/red highlighting for valid/invalid placement
3. **Building Ownership** - Color-code buildings by player
4. **Upgrade System** - Add HouseUpgrade button (3P+1S makes building immune)
5. **Disaster Effects** - Fully implement earthquakes, tornadoes, floods
6. **Car Movement** - Resource transport mechanics
7. **Advanced UI** - Preview placement, show connection paths, better visuals

---

## Key Code Locations

- **GridEdge.cs**: Edge data structure
- **GridManager.cs**: Edge arrays and network traversal
- **EdgeVisualizer.cs**: Bridge rendering
- **BridgeConnectivity.cs**: Network pathfinding (BFS)
- **BuildingPlacer.cs**: Placement validation
- **NetworkManager.cs**: Building bonus calculations
- **GameManager.cs**: Applies bonuses when drawing
- **BuildingInputHandler.cs**: Mouse/keyboard input
- **BuildingUI.cs**: UI controls and display

---

**Ready to test! Follow the setup steps above, press Play, and start building!**
