# Fix Camera Moving in Circle - Conflicting Mouse Scripts

## The Problem
You have 2 mouse look scripts that are likely both active:
1. `MouseMovement.cs` (uses old Input system)
2. `MyMouseLook.cs` (uses new Input system)

**Camera moving in a circle = scripts conflicting or wrong hierarchy setup**

## Immediate Fix Steps

### Step 1: Disable One Script
**Choose ONE mouse script to use:**

**Option A: Use MouseMovement.cs (Recommended - simpler)**
- In Unity, find your Camera GameObject
- Disable the `MouseLook` component (uncheck the box next to it)
- Keep only `MouseMovement` active

**Option B: Use MyMouseLook.cs**
- In Unity, find your Camera GameObject  
- Disable the `MouseMovement` component
- Keep only `MouseLook` active

### Step 2: Fix Hierarchy (CRITICAL)
Your hierarchy should be:
```
Player (has PlayerMovement, CharacterController)
├── Main Camera (has ONE mouse script)
└── GroundCheck (empty GameObject)
```

**Common mistakes:**
- Camera as separate object (not child of Player)
- Mouse script on Player instead of Camera
- Camera position/rotation wrong

### Step 3: Check References
Whichever script you keep active:
- **playerBody field** = Drag the Player GameObject (parent)
- **mouseSensitivity** = 100 (start with this)

### Step 4: Reset Camera Transform
With Camera selected:
- Position: (0, 1.6, 0) relative to Player
- Rotation: (0, 0, 0)
- Make sure it's a CHILD of Player

## Why This Happens

### Camera Moving in Circle = Wrong Pivot Point
- Camera should rotate around its own center when looking up/down
- Player should rotate around its center when looking left/right
- If hierarchy is wrong, camera rotates around Player's center instead

### Both Scripts Active = Double Rotation
- Each script adds mouse input
- Result: twice the rotation speed and weird behavior

## Quick Diagnostic

**Test this:**
1. Disable BOTH mouse scripts
2. Manually set Camera rotation in Inspector while in Play mode
3. Camera should stay in position relative to Player
4. If Camera moves away from Player, hierarchy is wrong

## Recommended Setup (MouseMovement.cs)

### Why MouseMovement is simpler:
- Uses built-in Input.GetAxis (no package dependencies)
- Cleaner code, easier to debug
- Works with default Unity input setup

### Setup for MouseMovement:
1. Camera as child of Player
2. MouseMovement script on Camera
3. playerBody = Player GameObject
4. mouseSensitivity = 100

## Alternative: Fix the Hierarchy Issue

If you want to keep both scripts, the real issue might be:

### Camera Transform Setup:
- Position: Should be at Player's eye level (0, 1.6, 0)
- Rotation: Should start at (0, 0, 0)
- Parent: MUST be child of Player

### Player Transform Setup:
- Position: World position where player spawns
- Rotation: (0, 0, 0) usually
- Scale: (1, 1, 1)

## Expected Result After Fix:
- ✅ Camera stays attached to Player
- ✅ Looking up/down tilts camera smoothly
- ✅ Looking left/right rotates Player (and camera follows)
- ✅ No circular motion or detachment

**Most likely solution: Disable one mouse script and check Camera is child of Player!**