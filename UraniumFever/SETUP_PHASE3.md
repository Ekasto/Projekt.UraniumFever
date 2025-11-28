# Phase 3 Setup Guide - Resource System

This guide will help you set up the resource drawing system with headquarters and player inventories.

## What We Built (Phase 3)

- ✅ **Resource & Disaster Types** - 4 resources, 5 disasters
- ✅ **Card System** - Resource and disaster cards
- ✅ **Endless Deck** - Reshuffles when empty, 70% resources, 30% disasters
- ✅ **3 Players** - Each with headquarters (Food, Electricity, Medicine)
- ✅ **Player Inventories** - Separate resource storage per player
- ✅ **Round Manager** - Tracks rounds, safe rounds (1-3) filter out disasters
- ✅ **Game Loop** - Draw card → Add to inventory → Next turn

## Setup Instructions

### Step 1: Run Unit Tests First

1. In Unity: **Window > General > Test Runner**
2. Click **EditMode** tab
3. Click **Run All**
4. Verify **~50 tests pass** (all green checkmarks)

### Step 2: Set Up Unity Scene

#### A. Create Game Manager
1. Create Empty GameObject: **GameObject > Create Empty**
2. Rename to "GameManager"
3. Add Component: **Game Manager**
4. In Inspector:
   - **Grid Visualizer**: Drag the "Grid" GameObject from Phase 2
   - **HQ Visualizer**: Will add next

#### B. Create HQ Visualizer
1. Select "GameManager"
2. Add Component: **HQ Visualizer**
3. The GameManager should now have reference to HQVisualizer

#### C. Create UI Canvas
1. **GameObject > UI > Canvas**
2. Rename to "GameUI Canvas"

#### D. Create Draw Card Button
1. Right-click "GameUI Canvas" → **UI > Button - TextMeshPro**
2. Rename to "DrawCardButton"
3. In the Button's child text, change text to: **"Draw Card"**
4. Position button (e.g., bottom center of screen)

#### E. Create UI Text Elements
1. Right-click "GameUI Canvas" → **UI > Text - TextMeshPro**
2. Create these text fields:
   - **"RoundText"** - Shows current round
   - **"CurrentPlayerText"** - Shows whose turn it is
   - **"Player1Inventory"** - Shows Player 1's resources
   - **"Player2Inventory"** - Shows Player 2's resources
   - **"Player3Inventory"** - Shows Player 3's resources
   - **"DeckInfoText"** - Shows deck/discard counts

3. Position them on screen (e.g., top-left corner)

#### F. Create Game UI Component
1. Create Empty GameObject under "GameUI Canvas"
2. Rename to "GameUI"
3. Add Component: **Game UI**
4. In Inspector, assign references:
   - **Game Manager**: Drag "GameManager" GameObject
   - **Draw Card Button**: Drag "DrawCardButton"
   - **Round Text**: Drag "RoundText"
   - **Current Player Text**: Drag "CurrentPlayerText"
   - **Player Inventory Texts** (Array size 3):
     - Element 0: Drag "Player1Inventory"
     - Element 1: Drag "Player2Inventory"
     - Element 2: Drag "Player3Inventory"
   - **Deck Info Text**: Drag "DeckInfoText"

### Step 3: Test in Play Mode

Press **Play** (▶)

**You should see:**
1. **Grid** from Phase 2 (12x12 checkerboard)
2. **3 colored cubes** on the grid (headquarters):
   - Green cube at left edge (0,6) - Food HQ
   - Yellow cube at bottom edge (6,0) - Electricity HQ
   - Red cube at right edge (11,6) - Medicine HQ
3. **UI showing:**
   - "Round: 1 (SAFE)"
   - "Current Player: 1 (Food)"
   - Player inventories (all zeros initially)
   - "Deck: 100 cards | Discard: 0"
4. **"Draw Card" button**

### Step 4: Test Drawing Cards

1. **Click "Draw Card" button**
   - A random resource is drawn
   - Player 1's inventory increases
   - Turn passes to Player 2
   - Console logs what was drawn

2. **Click again**
   - Player 2 draws a card
   - Their inventory updates
   - Turn passes to Player 3

3. **Keep clicking**
   - Players cycle: 1 → 2 → 3 → 1...
   - After all 3 players go, Round increases to 2
   - After Round 3 completes, Round 4 starts

4. **Test Safe Rounds**
   - Rounds 1-3: Should see "(SAFE)" indicator
   - Only resource cards can be drawn
   - Round 4+: "(SAFE)" disappears
   - Can draw disaster cards (shows in Console)

5. **Test Deck Reshuffle**
   - Keep drawing until deck reaches 0
   - Next draw reshuffles discard pile
   - Deck count should jump back up

### Step 5: Expected Behavior

**Round 1-3 (Safe):**
```
Round: 1 (SAFE)
Player 1 draws Food → ⚡0 🍎1 💊0 ⭐0
Player 2 draws Electricity → ⚡1 🍎0 💊0 ⭐0
Player 3 draws Medicine → ⚡0 🍎0 💊1 ⭐0
Round: 2 (SAFE)
...
```

**Round 4+ (Disasters Possible):**
```
Round: 4
Player 1 draws Electricity → ⚡1 🍎3 💊0 ⭐0
Player 2 draws DISASTER: Tornado! (No effects yet)
Player 3 draws PlayerChoice → ⚡0 🍎1 💊2 ⭐1
```

## Troubleshooting

### HQs don't appear
- Check GameManager has references to GridVisualizer and HQVisualizer
- Check Console for errors
- HQs should appear as colored cubes on the grid

### UI doesn't update
- Verify GameUI component has all references assigned
- Check Console for null reference errors
- Make sure GameManager exists and initialized

### Draw button doesn't work
- Check button has GameUI.OnDrawCardClicked assigned
- Check GameUI has reference to GameManager
- Look at Console for error messages

### Tests fail
- Some tests use randomness and might occasionally fail
- Re-run tests if one fails
- Check Console for specific error messages

## Next Steps

Once Phase 3 is working:
- **Phase 4**: Building System (use resources to build structures)
- **Phase 5**: Win/Lose Conditions & Game Loop
- **Phase 6**: Disasters with actual effects, Trading

## Testing Checklist

- ✅ All ~50 unit tests pass
- ✅ 3 HQs appear on grid (different colors)
- ✅ UI shows round, current player, inventories
- ✅ Draw Card button works
- ✅ Inventories update when drawing
- ✅ Players cycle correctly (1→2→3→1)
- ✅ Rounds increment after all 3 players
- ✅ Rounds 1-3 show "(SAFE)"
- ✅ Round 4+ allows disasters
- ✅ Deck reshuffles when empty
- ✅ Console logs card draws

**Ready to test!**
