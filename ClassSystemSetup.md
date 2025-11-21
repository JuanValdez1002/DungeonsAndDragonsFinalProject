# D&D Class Selection System - Setup Guide

## Overview
Complete class selection system with 4 unique classes:
- **Warrior**: Melee tank, high health (150 HP), moderate damage (15)
- **Ranger**: Ranged DPS, high damage (20), fast movement
- **Mage**: Magic burst, very high damage (30), low health (75)
- **Cleric**: Support/Healer, can heal (20 HP), balanced stats

## Files Created
1. `PlayerClass.cs` - Main class system and abilities
2. `PlayerProjectile.cs` - Player projectile damage
3. `ClassSelectionUI.cs` - Class selection menu UI
4. `GameManager.cs` - Initializes player with selected class

## Setup Instructions

### Part 1: Create Projectile Prefabs

#### 1. Ranger Arrow Prefab
1. Create **GameObject → 3D Object → Capsule**
2. Scale it: (0.1, 0.5, 0.1) to look like an arrow
3. Add **Rigidbody**: 
   - Use Gravity: OFF
   - Mass: 0.1
4. Add **Sphere Collider** or keep Capsule Collider
   - Is Trigger: ON
5. Add **PlayerProjectile** script
6. Set color to green (Material)
7. Save as prefab: `RangerArrow`
8. Delete from scene

#### 2. Mage Fireball Prefab
1. Create **GameObject → 3D Object → Sphere**
2. Scale: (0.3, 0.3, 0.3)
3. Add **Rigidbody**: 
   - Use Gravity: OFF
   - Mass: 0.2
4. Keep Sphere Collider
   - Is Trigger: ON
5. Add **PlayerProjectile** script
6. Set color to red/orange (Material)
7. Save as prefab: `MageFireball`
8. Delete from scene

#### 3. Cleric Light Prefab
1. Create **GameObject → 3D Object → Sphere**
2. Scale: (0.2, 0.2, 0.2)
3. Add **Rigidbody**: 
   - Use Gravity: OFF
   - Mass: 0.15
4. Keep Sphere Collider
   - Is Trigger: ON
5. Add **PlayerProjectile** script
6. Set color to yellow/gold (Material)
7. Optional: Add **Light** component for glow effect
8. Save as prefab: `ClericLight`
9. Delete from scene

### Part 2: Setup Game Scene

#### 1. Add GameManager
1. In your main game scene (Dungeon scene)
2. Create empty GameObject named `GameManager`
3. Add **GameManager** script
4. Assign projectile prefabs:
   - Ranger Arrow Prefab
   - Mage Fireball Prefab
   - Cleric Light Prefab
5. Optional: Assign player spawn point

#### 2. Setup Player GameObject
Your player should have:
- **CharacterController** (for movement)
- **PlayerMovement** script
- **PlayerHealth** script
- **PlayerClass** script (will be added automatically by GameManager)
- Tag: "Player"

Make sure player has a Camera child with mouse look script.

### Part 3: Create Class Selection Scene

#### 1. Create New Scene
1. **File → New Scene**
2. Save as `ClassSelection`

#### 2. Create UI Canvas
1. **GameObject → UI → Canvas**
2. Canvas Scaler: Scale With Screen Size
3. Reference Resolution: 1920x1080

#### 3. Create Selection Panel
1. Under Canvas, create **UI → Panel**
2. Name it "SelectionPanel"
3. Add background image/color

#### 4. Create Title Text
1. **UI → Text** (or TextMeshPro)
2. Name: "TitleText"
3. Text: "Choose Your Class"
4. Font size: 48
5. Position at top center

#### 5. Create Class Buttons
Create 4 buttons, one for each class:

**Warrior Button:**
- Text: "Warrior"
- Color: Red tint
- Position: Left side

**Ranger Button:**
- Text: "Ranger"
- Color: Green tint
- Position: Center-left

**Mage Button:**
- Text: "Mage"
- Color: Blue tint
- Position: Center-right

**Cleric Button:**
- Text: "Cleric"
- Color: Yellow tint
- Position: Right side

#### 6. Create Info Display
Create text fields to show class stats:
- **Class Name Display** (large text)
- **Description Text** (paragraph)
- **Health Display** ("Health: XXX")
- **Damage Display** ("Damage: XXX")
- **Speed Display** ("Speed: XXX")
- **Special Ability Display** ("Special: XXX")

#### 7. Create Start Button
- Large button at bottom
- Text: "Start Game"
- Color: Green

#### 8. Setup ClassSelectionUI Script
1. Create empty GameObject named `ClassSelectionManager`
2. Add **ClassSelectionUI** script
3. Assign all UI references:
   - Selection Panel
   - Title Text
   - Description Text
   - All 4 class buttons
   - Start button
   - All info display texts
4. Set **Game Scene Name**: "Dungeon" (or your game scene name)

### Part 4: Build Settings

1. **File → Build Settings**
2. Add scenes in order:
   - Scene 0: ClassSelection
   - Scene 1: Dungeon (your game scene)
3. Click "Add Open Scenes" for each

### Part 5: Testing

#### Test Class Selection:
1. Open ClassSelection scene
2. Press Play
3. Click each class button - info should update
4. Click "Start Game" - should load game scene

#### Test In-Game:
1. After selecting class in menu
2. Player should spawn with correct stats
3. Test controls:
   - **Left Click / Fire1**: Attack
   - **H Key**: Heal (Cleric only)
   - **E Key**: Special ability
   - **WASD**: Movement
   - **Mouse**: Look around

#### Expected Results by Class:

**Warrior:**
- Red color
- Melee attacks (close range)
- High health (150)
- E = Defensive stance

**Ranger:**
- Green color
- Shoots green arrows
- Fast movement
- E = Multi-shot (3 arrows)

**Mage:**
- Blue color
- Shoots blue/red fireballs
- High damage, low health (75)
- E = Explosive fireball

**Cleric:**
- Yellow color
- Shoots light projectiles
- H = Heal self
- E = Area heal over time

## Controls Reference

### All Classes:
- **WASD**: Move
- **Mouse**: Look
- **Space**: Jump
- **Left Ctrl**: Crouch
- **Left Click / Fire1**: Basic Attack
- **E**: Special Ability

### Class-Specific:
- **H** (Cleric only): Self heal

## Troubleshooting

### Class not applying:
- Check GameManager has all projectile prefabs assigned
- Check Player has PlayerHealth component
- Check Player tag is "Player"

### Projectiles not working:
- Check Rigidbody is on projectile prefabs
- Check collider is set to trigger
- Check PlayerProjectile script is attached

### UI buttons not working:
- Check ClassSelectionUI has all references assigned
- Check buttons have OnClick listeners (should be automatic)
- Check scene name matches in Build Settings

### Player spawns but has no class:
- Check GameManager is in the game scene
- Check you selected a class before clicking "Start Game"
- Check PlayerPrefs is saving (Play class selection first)

## Customization

### Change Class Stats:
Edit values in `GameManager.cs` → `SetupClassData()` method

### Add New Class:
1. Add to `ClassType` enum in `PlayerClass.cs`
2. Create new `PlayerClassData` in `GameManager.cs`
3. Add button in ClassSelection UI
4. Add case in `ClassSelectionUI.UpdateClassDisplay()`

### Change Colors:
Edit `classColor` values in `GameManager.cs`

### Adjust Abilities:
Edit methods in `PlayerClass.cs`:
- `WarriorDefensiveStance()`
- `RangerMultiShot()`
- `MageFireball()`
- `ClericAreaHeal()`

## Notes
- System uses PlayerPrefs to transfer class selection between scenes
- Projectiles automatically damage enemies with EnemyAi component
- All classes work with existing PlayerHealth and PlayerMovement
- System is compatible with CharacterController-based movement

Enjoy your D&D class system! 🎮