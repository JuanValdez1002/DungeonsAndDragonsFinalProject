# Complete Unity Class System Setup - Beginner Guide

## Overview
We're adding a D&D class system where players can choose between:
- **Warrior** (melee tank)
- **Ranger** (ranged archer)
- **Mage** (magic caster)
- **Cleric** (healer)

This guide assumes you're new to Unity!

---

## PART 1: Test the Basic System (30 minutes)

We'll start simple - just test class switching without the fancy menu first.

### Step 1: Check Your Scripts Are in Unity

1. **Open Unity** (your DungeonsAndDragonsFinalProject)
2. Wait for Unity to load (may take a minute)
3. Look at the **Project window** (bottom of screen, shows your files)
4. Navigate to **Assets → Scripts** folder
5. You should see these NEW scripts I created:
   - `PlayerClass.cs`
   - `PlayerProjectile.cs`
   - `GameManager.cs`
   - `ClassSwitcherDebug.cs`
   - `ClassSelectionUI.cs`
   
**If you DON'T see them:**
- They're in your Downloads folder but Unity hasn't imported them yet
- Click anywhere in Unity, then press **Ctrl+R** (this refreshes)
- Or close and reopen Unity

---

### Step 2: Open Your Game Scene

1. In **Project window** (bottom), find **Assets → Scenes**
2. Double-click your main game scene (probably named "Dungeon" or similar)
3. This opens the scene in the **Scene view** (middle of screen)
4. You should see your dungeon, player, etc.

---

### Step 3: Find Your Player GameObject

1. Look at the **Hierarchy window** (left side of screen)
2. This shows all objects in your scene
3. Find your player object - it might be named:
   - "Player"
   - "PlayerObj"
   - "FPSController"
   - Or something similar
4. **Click on it once** to select it
5. Look at the **Inspector window** (right side) - this shows the player's components

**How to know if you found the right object:**
- Inspector should show components like:
  - Transform
  - Character Controller (or Rigidbody)
  - PlayerMovement
  - PlayerHealth
  - Maybe MouseLook or Camera scripts

---

### Step 4: Add PlayerClass Script to Player

Now we'll add the class system to your player.

1. **With Player selected** (from Step 3)
2. Look at **Inspector** (right side)
3. Scroll to the bottom of Inspector
4. Click the **"Add Component"** button
5. A search box appears - type: **PlayerClass**
6. Click on "PlayerClass" when it appears in the list
7. You should now see "Player Class (Script)" in the Inspector

**What you'll see:**
```
Inspector:
┌─────────────────────────────┐
│ Player (GameObject)         │
├─────────────────────────────┤
│ Transform                   │
│ Character Controller        │
│ Player Movement (Script)    │
│ Player Health (Script)      │
│ Player Class (Script) ← NEW!│
└─────────────────────────────┘
```

---

### Step 5: Add ClassSwitcherDebug Script to Player

This lets you test by pressing keyboard numbers.

1. **Player should still be selected**
2. In Inspector, click **"Add Component"** again
3. Type: **ClassSwitcherDebug**
4. Click it when it appears
5. Now you have both new scripts on Player

---

### Step 6: Create an Empty GameManager

GameManager will set up the classes for us.

1. **In Hierarchy** (left side), right-click in empty space
2. Choose **Create Empty**
3. A new GameObject appears - it's probably named "GameObject"
4. With it selected, look at top of Inspector
5. Change name from "GameObject" to **GameManager**
6. Press Enter

**How to rename:**
- Click on name at top of Inspector
- Type new name
- Press Enter

---

### Step 7: Add GameManager Script

1. **With GameManager selected** in Hierarchy
2. In Inspector, click **Add Component**
3. Type: **GameManager**
4. Click when it appears

**You'll see warnings** - that's OK! We'll fix them next.

---

### Step 8: Test Warrior Class (No Projectiles Needed!)

Let's test the system with just Warrior first (melee class, no projectiles required).

1. Make sure you saved scene: **File → Save** (or Ctrl+S)
2. Click the **Play button** (▶) at top-center of Unity
3. Game should start
4. **Press the "1" key on keyboard**
5. Check the **Console window** (bottom):
   - Go to **Window → General → Console** if you don't see it
   - You should see: "Class set to: Warrior"
   - Player color might change to red

**Console messages you'll see:**
```
Class set to: Warrior
Player initialized as Warrior
```

**If you see errors:**
- Don't worry yet! Note what they say
- Click Play again to stop the game
- We'll fix in troubleshooting section

6. **Click Play button again** to stop the game

---

### Step 9: Test Class Switching

1. Press **Play** again
2. Try pressing these keys:
   - **1** = Warrior
   - **2** = Ranger (won't shoot yet, no projectiles)
   - **3** = Mage (won't shoot yet)
   - **4** = Cleric (won't shoot yet)
3. Check Console - should say "Class set to: X"
4. Player color should change (red, green, blue, yellow)

**If this works - SUCCESS!** The basic system is running!

---

## PART 2: Create Projectile Prefabs (15 minutes)

Now let's make the 3 projectiles so Ranger, Mage, and Cleric can attack.

### What's a Prefab?
A prefab is a "template" GameObject you can reuse. Think of it like a cookie cutter - you create the shape once, then spawn copies during gameplay.

---

### Step 10: Create Ranger Arrow Prefab

1. **Stop the game** if it's playing (press Play button)
2. In **Hierarchy**, right-click empty space
3. Choose **3D Object → Sphere**
4. A sphere appears in your scene
5. With sphere selected, look at Inspector
6. At top, change name to: **RangerArrow**

**Scale it to look like an arrow:**
7. In Inspector, find **Transform** section
8. Find **Scale** (has X, Y, Z values)
9. Change to: 
   - X: **0.1**
   - Y: **0.5**
   - Z: **0.1**
10. Press Enter

Now it's thin and tall like an arrow!

**Add Physics:**
11. In Inspector, click **Add Component**
12. Type: **Rigidbody**
13. Click it
14. In the Rigidbody component, find **Use Gravity**
15. **UNCHECK the box** (click it so checkmark disappears)
16. Find **Mass**, set to: **0.1**

**Set Collider to Trigger:**
17. In Inspector, find **Sphere Collider** (should already be there)
18. Find **Is Trigger** checkbox
19. **CHECK the box** (click so checkmark appears)

**Add PlayerProjectile Script:**
20. Click **Add Component**
21. Type: **PlayerProjectile**
22. Click it

**Change Color to Green:**
23. In Inspector, find **Mesh Renderer**
24. Click the arrow next to **Materials**
25. You'll see "Element 0" with a material
26. Click the circle next to the material name
27. A window opens - choose any green material
28. Or create new: Right-click in Project → Create → Material
29. Name it "GreenArrow", select it, change color to green in Inspector

**Save as Prefab:**
30. In **Project window** (bottom), create a folder:
    - Right-click in Assets
    - Create → Folder
    - Name it "Projectiles"
31. **Drag** the RangerArrow from **Hierarchy** into the **Projectiles folder**
32. It turns blue in Hierarchy - now it's a prefab!
33. **Right-click** RangerArrow in Hierarchy
34. Choose **Delete** (we don't need it in scene, just the prefab)

---

### Step 11: Create Mage Fireball Prefab

Repeat the process for Mage:

1. **3D Object → Sphere**
2. Name: **MageFireball**
3. Scale: **X: 0.3, Y: 0.3, Z: 0.3** (bigger than arrow)
4. Add **Rigidbody**
   - Use Gravity: **OFF** (unchecked)
   - Mass: **0.2**
5. **Sphere Collider** → Is Trigger: **ON** (checked)
6. Add **PlayerProjectile** script
7. Change color to **Red or Orange**
8. Drag to **Projectiles folder** (saves as prefab)
9. Delete from Hierarchy

---

### Step 12: Create Cleric Light Prefab

One more time for Cleric:

1. **3D Object → Sphere**
2. Name: **ClericLight**
3. Scale: **X: 0.2, Y: 0.2, Z: 0.2**
4. Add **Rigidbody**
   - Use Gravity: **OFF**
   - Mass: **0.15**
5. **Sphere Collider** → Is Trigger: **ON**
6. Add **PlayerProjectile** script
7. Change color to **Yellow or Gold**
8. Optional: Add Component → **Light** (makes it glow!)
9. Drag to **Projectiles folder**
10. Delete from Hierarchy

---

### Step 13: Assign Projectiles to GameManager

Now we tell GameManager which projectiles to use.

1. In **Hierarchy**, click **GameManager**
2. In **Inspector**, find **Game Manager (Script)**
3. You'll see 3 empty slots:
   - Ranger Arrow Prefab
   - Mage Fireball Prefab
   - Cleric Light Prefab

**Assign them:**
4. In **Project window**, open **Assets → Projectiles folder**
5. **Drag RangerArrow** from Project window to **Ranger Arrow Prefab** slot
6. **Drag MageFireball** to **Mage Fireball Prefab** slot
7. **Drag ClericLight** to **Cleric Light Prefab** slot

**Should look like:**
```
Game Manager (Script)
├─ Player Prefab: None
├─ Player Spawn Point: None
├─ Ranger Arrow Prefab: RangerArrow   ← Assigned!
├─ Mage Fireball Prefab: MageFireball ← Assigned!
└─ Cleric Light Prefab: ClericLight   ← Assigned!
```

---

## PART 3: Test All Classes (5 minutes)

### Step 14: Test Ranged Classes

1. Save scene: **Ctrl+S**
2. Press **Play**
3. Press **2** (Ranger class)
4. **Left-click your mouse** (or press Fire1 button)
5. You should see a green arrow shoot forward!
6. Try pressing **3** (Mage) and left-click
7. Try pressing **4** (Cleric) and left-click

**Expected results:**
- Ranger (2): Shoots green arrow
- Mage (3): Shoots red/orange fireball
- Cleric (4): Shoots yellow light
- Warrior (1): Melee attack (no projectile)

**Test healing (Cleric):**
1. Press **4** to be Cleric
2. Take damage from an enemy
3. Press **H key**
4. You should heal!
5. Check Console: "Healed for 20 HP!"

**Test special abilities:**
1. Select any class (1, 2, 3, or 4)
2. Press **E key**
3. Special ability activates (check Console)

---

## PART 4: Common Issues & Solutions

### Issue: "Can't find scripts"
**Solution:**
- Scripts are in your file system but not in Unity yet
- Press **Ctrl+R** to refresh
- Or drag the Scripts folder from Windows Explorer into Unity Assets

### Issue: "Missing references"
**Solution:**
- GameManager needs projectile prefabs assigned
- Follow Step 13 again carefully

### Issue: "Projectiles fall down instead of shooting forward"
**Solution:**
- Rigidbody → Use Gravity must be **OFF** (unchecked)
- Select prefab, check Inspector

### Issue: "Projectiles pass through enemies"
**Solution:**
- Collider must be **Is Trigger = ON**
- Enemy must have collider
- Enemy must have **EnemyAi** script (exact spelling)

### Issue: "Nothing happens when I press 1,2,3,4"
**Solution:**
- Check ClassSwitcherDebug is on Player
- Check Console for errors
- Make sure game is running (Play button pressed)

### Issue: "Can't attack with Left Click"
**Solution:**
- Check Edit → Project Settings → Input Manager
- Make sure "Fire1" exists (should be default)
- Try pressing mouse button in game mode

### Issue: "Projectiles don't damage enemies"
**Solution:**
- Enemy script must be named exactly: `EnemyAi` (not EnemyAI or Enemy)
- Check enemy has collider component
- Check PlayerProjectile script is on projectile prefabs

---

## PART 5: Understanding What Each Part Does

### Scripts Explained:

**PlayerClass.cs**
- Main class system
- Stores stats for each class
- Handles switching between classes
- Controls attacks and abilities

**GameManager.cs**
- Sets up the classes when game starts
- Assigns all the stats (health, damage, speed)
- Links projectile prefabs to classes

**PlayerProjectile.cs**
- Makes projectiles damage enemies
- Handles explosion for Mage fireballs
- Destroys projectile on impact

**ClassSwitcherDebug.cs**
- Testing tool only
- Lets you press 1,2,3,4 to switch classes
- Remove this in final game!

**ClassSelectionUI.cs**
- For the class selection menu (Part 6)
- Shows class info
- Starts game with chosen class

---

## PART 6: Create Class Selection Menu (Optional, 30 minutes)

This creates a menu before the game starts where players choose their class.

### Step 15: Create New Scene

1. **File → New Scene**
2. Choose **Basic (Built-in)** or **2D** (doesn't matter much)
3. **File → Save As**
4. Name it: **ClassSelection**
5. Save in **Assets → Scenes** folder

### Step 16: Create UI Canvas

1. Right-click in **Hierarchy**
2. **UI → Canvas**
3. A Canvas appears (this holds UI elements)

**Set Canvas to scale with screen:**
4. Select Canvas in Hierarchy
5. In Inspector, find **Canvas Scaler** component
6. Change **UI Scale Mode** to: **Scale With Screen Size**
7. Reference Resolution: **1920 x 1080**

### Step 17: Create Background

1. Right-click on **Canvas** in Hierarchy
2. **UI → Panel**
3. Rename it: **BackgroundPanel**
4. In Inspector, find **Image** component
5. Click **Color** box
6. Choose a dark color (dark gray or black)
7. Set Alpha (A) to about 200 (semi-transparent)

### Step 18: Create Title

1. Right-click **Canvas**
2. **UI → Text** (or UI → Text - TextMeshPro if available)
3. Rename: **TitleText**
4. In Inspector:
   - Text: **"CHOOSE YOUR CLASS"**
   - Font Size: **48**
   - Alignment: Center (both horizontal and vertical)
   - Color: White

**Position it at top:**
5. Click **Rect Transform** anchor preset (top-left of Inspector)
6. Hold **Alt+Shift** and click **top-center** preset
7. Set Position Y to: **-50** (moves it down from top)

### Step 19: Create Warrior Button

1. Right-click **Canvas**
2. **UI → Button**
3. Rename: **WarriorButton**
4. Select WarriorButton in Hierarchy
5. Expand it (click arrow) - you'll see "Text" child
6. Click the **Text** child
7. In Inspector, change text to: **"WARRIOR"**

**Position and size:**
8. Click WarriorButton (parent, not text child)
9. In Rect Transform:
   - Width: **200**
   - Height: **200**
   - Pos X: **-300** (left side)
   - Pos Y: **0** (middle)

**Color it red:**
10. In Inspector, find **Image** component
11. Click **Color**
12. Choose red (RGB: 200, 50, 50)

### Step 20: Create Other 3 Buttons

**Ranger Button:**
1. Right-click Canvas → UI → Button
2. Name: **RangerButton**
3. Text child: **"RANGER"**
4. Width: 200, Height: 200
5. Pos X: **-100**, Pos Y: **0**
6. Color: **Green** (RGB: 50, 200, 50)

**Mage Button:**
1. Right-click Canvas → UI → Button
2. Name: **MageButton**
3. Text child: **"MAGE"**
4. Width: 200, Height: 200
5. Pos X: **100**, Pos Y: **0**
6. Color: **Blue** (RGB: 50, 50, 200)

**Cleric Button:**
1. Right-click Canvas → UI → Button
2. Name: **ClericButton**
3. Text child: **"CLERIC"**
4. Width: 200, Height: 200
5. Pos X: **300**, Pos Y: **0**
6. Color: **Yellow** (RGB: 200, 200, 50)

Now you have 4 buttons in a row!

### Step 21: Create Info Panel

1. Right-click Canvas → UI → Panel
2. Name: **InfoPanel**
3. Anchor preset: **bottom-center** (Alt+Shift+click)
4. Width: **800**
5. Height: **300**
6. Pos Y: **150** (moves up from bottom)
7. Color: Dark gray, semi-transparent

**Create text fields inside InfoPanel:**

1. Right-click **InfoPanel** → UI → Text
2. Name: **ClassNameText**
3. Text: "Warrior"
4. Font Size: **36**
5. Alignment: Center
6. Position at top of panel

Repeat for these texts (all children of InfoPanel):
- **DescriptionText** (paragraph about class)
- **HealthText** ("Health: 150")
- **DamageText** ("Damage: 15")
- **SpeedText** ("Speed: 10")
- **SpecialText** ("Special: Defensive Stance")

### Step 22: Create Start Button

1. Right-click Canvas → UI → Button
2. Name: **StartButton**
3. Text child: **"START GAME"**
4. Width: **300**, Height: **80**
5. Anchor: **bottom-center**
6. Pos Y: **50**
7. Color: **Green**

### Step 23: Create ClassSelectionManager

1. Right-click in Hierarchy (outside Canvas)
2. **Create Empty**
3. Name: **ClassSelectionManager**
4. Add Component → **ClassSelectionUI**

**Wire up all the references:**
5. With ClassSelectionManager selected, look at Inspector
6. You'll see MANY slots to fill
7. **Drag each UI element** from Hierarchy to matching slot:

```
Selection Panel → InfoPanel
Title Text → TitleText
Description Text → DescriptionText
Warrior Button → WarriorButton
Ranger Button → RangerButton
Mage Button → MageButton
Cleric Button → ClericButton
Start Button → StartButton
Class Name Display → ClassNameText
Health Display → HealthText
Damage Display → DamageText
Speed Display → SpeedText
Special Ability Display → SpecialText
```

8. **Game Scene Name** field: Type your game scene name (probably "Dungeon")

### Step 24: Add Scenes to Build Settings

1. **File → Build Settings**
2. Click **Add Open Scenes** (adds ClassSelection scene)
3. **File → Open Scene** → Choose your game scene (Dungeon)
4. **File → Build Settings** again
5. Click **Add Open Scenes** (adds game scene)

**Make sure order is:**
- Scene 0: ClassSelection
- Scene 1: Dungeon (your game scene)

6. Close Build Settings

### Step 25: Test the Menu

1. Open **ClassSelection** scene
2. Press **Play**
3. Click each class button
4. Info should update on screen
5. Click **Start Game**
6. Should load your game scene with selected class!

---

## Quick Test Checklist

✅ **Basic System:**
- [ ] GameManager in scene
- [ ] PlayerClass on Player
- [ ] ClassSwitcherDebug on Player
- [ ] Can press 1,2,3,4 to switch classes
- [ ] Player color changes

✅ **Projectiles:**
- [ ] 3 prefabs created (Ranger, Mage, Cleric)
- [ ] Each has Rigidbody (no gravity)
- [ ] Each has Collider (trigger on)
- [ ] Each has PlayerProjectile script
- [ ] Assigned to GameManager

✅ **Combat:**
- [ ] Warrior melee attack works (press 1, left-click)
- [ ] Ranger shoots arrows (press 2, left-click)
- [ ] Mage shoots fireballs (press 3, left-click)
- [ ] Cleric shoots light (press 4, left-click)
- [ ] Cleric can heal (press 4, then H)
- [ ] Special abilities work (press E)

✅ **Menu (Optional):**
- [ ] ClassSelection scene exists
- [ ] UI buttons created
- [ ] ClassSelectionUI script wired up
- [ ] Scenes in Build Settings
- [ ] Menu loads game scene

---

## Controls Reference

**Class Switching (Testing):**
- 1 = Warrior
- 2 = Ranger
- 3 = Mage
- 4 = Cleric

**Combat:**
- Left Click = Attack
- E = Special Ability
- H = Heal (Cleric only)

**Movement** (your existing controls):
- WASD = Move
- Mouse = Look
- Space = Jump
- Left Ctrl = Crouch

---

## What to Do If You Get Stuck

1. **Check Console** (Window → General → Console)
   - Red errors tell you what's wrong
   - Read the first error carefully

2. **Check all references are assigned**
   - GameManager → projectile prefabs
   - ClassSelectionUI → all UI elements

3. **Save and reload**
   - File → Save (Ctrl+S)
   - Close and reopen Unity
   - Sometimes fixes weird issues

4. **Start with Warrior only**
   - Warrior doesn't need projectiles
   - Get that working first
   - Then add projectiles for other classes

5. **Check exact names**
   - Scripts are case-sensitive
   - `EnemyAi` not `EnemyAI`
   - `PlayerClass` not `Playerclass`

---

## Next Steps After This Works

Once you have the basic system working:

1. **Balance the classes** - adjust damage/health in GameManager.cs
2. **Add particle effects** - make abilities look cooler
3. **Add sound effects** - shooting, hitting, healing sounds
4. **Add cooldown UI** - show when abilities are ready
5. **Add class icons** - visual representations of each class
6. **Create class-specific models** - different character appearance per class

---

## Summary

You now have:
- ✅ 4 playable classes with unique stats
- ✅ Melee and ranged combat
- ✅ Healing system
- ✅ Special abilities
- ✅ Class selection menu (optional)
- ✅ Full integration with your existing game

**Remember:** Start simple, test often, and build up complexity!

Good luck! 🎮