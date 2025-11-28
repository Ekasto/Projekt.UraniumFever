# Quick UI Setup Guide

## What Was Improved

✅ **Cancel Button**: Already exists in code - just needs to be visible in UI
✅ **Resource Colors**: Automatic color-coded display (Yellow=Electricity, Green=Food, Red=Medicine, Purple=Choice)
✅ **Building Costs**: Show on buttons automatically
✅ **Current Selection Indicator**: Shows what building/bridge is selected
✅ **Affordability Check**: Buttons disable when you can't afford them

## Quick Setup Steps (5 minutes in Unity)

### Step 1: Open Unity
```bash
cd C:\Users\ekast\Desktop\Projekt.UraniumFever\UraniumFever
# Open Unity (make sure to git pull first if working with team)
```

### Step 2: Verify Cancel Button in Scene

1. Open `SampleScene`
2. Find `BuildingUI` GameObject in hierarchy
3. Look at Inspector → BuildingUI component
4. Check if `Cancel Button` field is assigned
5. If not assigned:
   - Find or create a UI Button in your Canvas
   - Name it "CancelButton"
   - Style it (red/orange color, text "Cancel (ESC)")
   - Drag to the `cancelButton` field

### Step 3: Add New UI Text Fields

You need to add these TextMeshProUGUI elements to your Canvas:

**A) Selected Building Info** (shows what's selected)
- Create: Right-click Canvas → UI → Text - TextMeshPro
- Name: "SelectedBuildingInfoText"
- Position: Near building menu, prominent
- Style: Large font, bold, white text
- Assign to: BuildingUI → `selectedBuildingInfo` field

**B) Button Cost Labels** (optional but helpful)
For each building button (Bridge, House, Factory, Research, Defense):
- Create child TextMeshProUGUI under each button
- Name: "[Building]ButtonText"
- Assign to corresponding field in BuildingUI:
  - `bridgeButtonText`
  - `houseButtonText`
  - `factoryButtonText`
  - `researchButtonText`
  - `defenseButtonText`

### Step 4: Add ResourceIconLibrary

1. Create empty GameObject in scene
2. Name it "ResourceIconLibrary"
3. Add Component → ResourceIconLibrary
4. (Optional) Add Component → ResourceSpriteGenerator
5. Right-click ResourceSpriteGenerator → "Generate All Resource Sprites"

### Step 5: Test in Play Mode

Press Play and verify:
- ✅ Resource counts show colors (yellow electricity, green food, etc.)
- ✅ Current player highlighted in yellow
- ✅ Select a building (press B key)
- ✅ "SelectedBuildingInfo" shows "SELECTED: [Building]"
- ✅ Ghost preview appears
- ✅ Click Cancel button → ghost disappears
- ✅ Press ESC → also cancels
- ✅ Buttons disable when you don't have enough resources

## Keyboard Shortcuts Reference

| Key | Action |
|-----|--------|
| B | Toggle building menu / Select Bridge |
| H | Select House |
| F | Select Factory |
| R | Select Research |
| D | Select Defense |
| ESC | Cancel selection |

## Visual Example of Resource Display

Before:
```
Player 1 (Food):
  Food: 5
  Electricity: 3
  Medicine: 2
  Choice: 1
```

After:
```
Player 1 (Food)      ← Yellow if current player
⚡ Electricity: 3    ← Yellow text
🍎 Food: 5           ← Green text (bold count)
💊 Medicine: 2       ← Red text
⭐ Choice: 1         ← Purple text
```

## Minimal Setup (Just Cancel Button)

If you only want the cancel button working:

1. Open Unity → SampleScene
2. Find BuildingUI component
3. Find/create Cancel button in UI
4. Assign to `cancelButton` field
5. Done! ESC key and button both work now

## Full Setup (All Improvements)

For the complete enhanced UI experience:

1. ✅ Cancel button (as above)
2. ✅ Add SelectedBuildingInfo text field
3. ✅ Add button label text fields (optional)
4. ✅ Add ResourceIconLibrary GameObject
5. ✅ Enable "Use Colored Text" in GameUI (should be on by default)
6. ✅ Enable "Show Resource Icons" in GameUI (should be on by default)

## Files Created/Modified

New files:
- `Assets/Scripts/UI/ResourceIconLibrary.cs` - Resource visual library
- `Assets/Scripts/UI/ResourceSpriteGenerator.cs` - Generates placeholder sprites
- `UI_IMPROVEMENTS.md` - Detailed documentation
- `QUICK_UI_SETUP.md` - This file

Modified files:
- `Assets/Scripts/UI/BuildingUI.cs` - Enhanced with costs, states, selection info
- `Assets/Scripts/UI/GameUI.cs` - Enhanced with colors and better formatting

## Troubleshooting

**Cancel button does nothing:**
- Check it's assigned in BuildingUI inspector
- Check InputHandler is assigned in BuildingUI inspector

**No colors showing:**
- Check GameUI has "Use Colored Text" enabled
- Verify TextMeshProUGUI components have "Rich Text" enabled

**Building selection info not showing:**
- Add the SelectedBuildingInfo text field
- Assign it to BuildingUI component

## Next Steps

After basic setup works:
1. Style the UI to look better (colors, fonts, layout)
2. Add proper resource icon sprites (replace emojis)
3. Add building icon sprites
4. Consider tooltips for building buttons
5. Add animations for better feedback

---

**Need Help?** Check `UI_IMPROVEMENTS.md` for detailed documentation.
