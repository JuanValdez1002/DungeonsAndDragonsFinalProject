# Complete D&D Class System - How It Works

## 🎮 Big Picture Overview

You've created a system where players:
1. Choose a class in a menu (Warrior, Ranger, Mage, or Cleric)
2. Start the game
3. Play with different stats and abilities based on their choice
4. Each class has unique combat style and special powers

---

## 📋 The Flow: What Happens Step-by-Step

### **STEP 1: Player Opens Game**
- Unity loads the **ClassSelection** scene first
- Shows the class selection menu with 4 colored buttons

### **STEP 2: Player Clicks a Class Button**
- **ClassSelectionUI** script detects the click
- Updates the info display to show that class's stats
- Remembers which class was clicked
- Player can click different buttons to compare classes

### **STEP 3: Player Clicks "START GAME"**
- **ClassSelectionUI** saves the chosen class to PlayerPrefs (temporary storage)
- Loads the **MainGameScene** (your dungeon game)

### **STEP 4: Game Scene Loads**
- **GameManager** script runs automatically
- Reads which class was chosen from PlayerPrefs
- Finds the Player GameObject in the scene
- Adds **PlayerClass** component to the player
- Configures the player with correct stats for that class

### **STEP 5: Player Plays the Game**
- Player has stats matching their class (health, speed, damage)
- Combat works differently based on class:
  - **Warrior**: Melee attacks (punch enemies up close)
  - **Ranger**: Shoots arrows at distance
  - **Mage**: Shoots powerful fireballs
  - **Cleric**: Shoots light AND can heal
- Special abilities available (press E key)
- Cleric can heal (press H key)

---

## 🔧 How Each Script Works

### **1. ClassSelectionUI.cs** (The Menu Controller)

**Location:** On ClassSelectionManager GameObject in ClassSelection scene

**What it does:**
- Controls the class selection menu
- Listens for button clicks
- Updates text when you click different classes
- Saves your choice
- Loads the game scene

**Key Functions:**
```csharp
SelectClass(ClassType type)
// Called when you click a class button
// Changes the displayed info to match that class

StartGame()
// Called when you click Start Game button
// Saves choice and loads game scene

UpdateClassDisplay(ClassType type)
// Updates all the text fields to show class info
```

**How button clicks work:**
1. You assigned buttons in Inspector
2. In Start(), script says: "When warriorButton is clicked, call SelectClass(Warrior)"
3. Same for all 4 class buttons
4. Start button calls StartGame()

---

### **2. PlayerClass.cs** (The Class System Brain)

**Location:** Added to Player GameObject by GameManager (in game scene)

**What it does:**
- Stores all class data (stats, abilities, etc.)
- Switches between classes
- Handles attacking
- Handles healing (Cleric)
- Executes special abilities

**Key Variables:**
```csharp
currentClass - Which class is active right now
warriorClass, rangerClass, mageClass, clericClass - Stats for each class
playerHealth - Reference to health system
playerMovement - Reference to movement system
projectileSpawnPoint - Where projectiles shoot from
```

**Key Functions:**
```csharp
SetClass(ClassType type)
// Changes the player to a specific class
// Updates health, speed, color, etc.

TryAttack()
// Called when you click Left Mouse Button
// Warrior: Does melee raycast
// Others: Shoot projectiles

TryHeal()
// Called when you press H (Cleric only)
// Restores health

UseSpecialAbility()
// Called when you press E
// Different effect per class
```

**How combat works:**

**Warrior (Melee):**
```
1. You press Left Click
2. TryAttack() is called
3. Shoots invisible raycast forward (like a laser pointer)
4. If hits enemy, deals damage directly
5. No projectile created
```

**Ranger/Mage/Cleric (Ranged):**
```
1. You press Left Click
2. TryAttack() is called
3. Creates a projectile GameObject (arrow/fireball/light)
4. Adds force to make it fly forward
5. Projectile has PlayerProjectile script
6. When projectile hits enemy, deals damage
7. Projectile destroys itself
```

**Special Abilities (Press E):**
```
Warrior: Defensive stance (takes less damage for 3 seconds)
Ranger: Multi-shot (fires 3 arrows at once)
Mage: Explosive fireball (bigger damage, AOE)
Cleric: Area heal (heal over 5 seconds)
```

---

### **3. GameManager.cs** (The Setup Master)

**Location:** On GameManager GameObject in game scene

**What it does:**
- Runs when game scene loads
- Reads PlayerPrefs to see which class was chosen
- Finds the Player GameObject
- Sets up all the class data with proper stats
- Connects everything together

**Key Function:**
```csharp
SetupClassData()
// Creates the data for all 4 classes
// Sets health, damage, speed, colors, etc.
// Assigns projectile prefabs
```

**What the stats mean:**

**Warrior:**
- maxHealth: 150 (hardest to kill)
- moveSpeed: 10 (slow but tanky)
- attackDamage: 15 (medium damage)
- attackRange: 2.5 (must be close)
- hasRangedAttack: false (melee only)

**Ranger:**
- maxHealth: 100 (medium survivability)
- moveSpeed: 14 (fastest!)
- attackDamage: 20 (high damage)
- attackRange: 20 (long range)
- hasRangedAttack: true (shoots arrows)
- projectilePrefab: RangerArrow
- projectileSpeed: 25 (fast arrows)

**Mage:**
- maxHealth: 75 (lowest - glass cannon)
- moveSpeed: 12 (medium)
- attackDamage: 30 (HIGHEST damage!)
- attackRange: 25 (very long range)
- hasRangedAttack: true (shoots fireballs)
- projectilePrefab: MageFireball
- projectileSpeed: 20 (slower but powerful)

**Cleric:**
- maxHealth: 120 (good survivability)
- moveSpeed: 11 (medium)
- attackDamage: 12 (lowest damage)
- attackRange: 15 (medium range)
- hasRangedAttack: true (shoots light)
- canHeal: true (ONLY class that can heal!)
- healAmount: 20
- healCooldown: 5 seconds

---

### **4. PlayerProjectile.cs** (Projectile Damage Handler)

**Location:** On each projectile prefab (RangerArrow, MageFireball, ClericLight)

**What it does:**
- Detects when projectile hits something
- Deals damage to enemies
- Destroys the projectile after hitting

**How it works:**
```
1. Projectile flies through air (Rigidbody physics)
2. OnTriggerEnter() is called when it touches something
3. Checks: "Did I hit an enemy?"
4. If yes: Call enemy.TakeDamage()
5. If Mage fireball: Also do AOE explosion damage
6. Destroy the projectile GameObject
```

**Why Trigger not Collision:**
- Trigger = passes through, detects hit, no physics bounce
- Collision = bounces off, more complex
- We want projectiles to "hit and stick" not bounce

---

### **5. ClassSwitcherDebug.cs** (Testing Tool)

**Location:** On Player GameObject (ONLY for testing!)

**What it does:**
- Lets you press 1,2,3,4 to instantly switch classes
- Bypasses the menu
- For testing only - remove in final game!

**How it works:**
```
Update() runs every frame
Checks: "Did player press 1, 2, 3, or 4?"
If yes: Calls playerClass.SetClass(that class)
```

---

## 🎯 How Systems Connect

### **Health System Connection:**
```
PlayerClass.SetClass()
  ↓
playerHealth.maxHealth = currentClass.maxHealth
playerHealth.currentHealth = currentClass.maxHealth
  ↓
Player now has correct health for their class
```

### **Movement System Connection:**
```
PlayerClass.SetClass()
  ↓
playerMovement.speed = currentClass.moveSpeed
  ↓
Player now moves at correct speed for their class
```

### **Combat System Connection:**
```
You press Left Click
  ↓
PlayerClass.Update() detects input
  ↓
PlayerClass.TryAttack()
  ↓
If melee: PerformMeleeAttack() → Raycast → Hit enemy
If ranged: PerformRangedAttack() → Create projectile → Projectile hits enemy
  ↓
Enemy.TakeDamage() is called
  ↓
Enemy loses health
```

### **Visual Feedback Connection:**
```
PlayerClass.SetClass()
  ↓
ApplyClassVisuals()
  ↓
Gets all Renderers on player
  ↓
Changes material color to class color
  ↓
Player turns red/green/blue/yellow
```

---

## 🔄 Data Flow Diagram

```
CLASS SELECTION SCENE:
┌─────────────────────┐
│ Player clicks       │
│ "Ranger" button     │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ ClassSelectionUI    │
│ selectedClass = 2   │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ PlayerPrefs         │
│ Save: "Class = 2"   │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Load MainGameScene  │
└──────────┬──────────┘
           │
           ▼
GAME SCENE:
┌─────────────────────┐
│ GameManager.Start() │
│ Read PlayerPrefs    │
│ "Class = 2"         │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Find Player object  │
│ Add PlayerClass     │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ SetupClassData()    │
│ Configure Ranger    │
│ stats               │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ playerClass.        │
│ SetClass(Ranger)    │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Player now has:     │
│ - 100 HP            │
│ - Speed 14          │
│ - Ranged attack     │
│ - Green color       │
└─────────────────────┘
```

---

## 🎮 Player Input → Output

### **Left Click (Attack):**
```
Input.GetMouseButtonDown(0) detected
  ↓
PlayerClass.TryAttack()
  ↓
Check: Is attack on cooldown? → No
  ↓
Check: Does class have ranged attack? → Yes (Ranger)
  ↓
PerformRangedAttack()
  ↓
Instantiate(rangerArrowPrefab)
  ↓
Add force to Rigidbody → Arrow flies forward
  ↓
Arrow.OnTriggerEnter(enemy)
  ↓
enemy.TakeDamage(20)
  ↓
Destroy(arrow)
```

### **E Key (Special Ability):**
```
Input.GetKeyDown(KeyCode.E) detected
  ↓
PlayerClass.UseSpecialAbility()
  ↓
Check: Which class? → Ranger
  ↓
RangerMultiShot()
  ↓
FOR i = -1 to 1:
  Create arrow at angle (i * 15°)
  Shoot it forward
  ↓
3 arrows fly in spread pattern
```

### **H Key (Heal - Cleric only):**
```
Input.GetKeyDown(KeyCode.H) detected
  ↓
PlayerClass.TryHeal()
  ↓
Check: Can this class heal? → Yes (Cleric)
  ↓
Check: Is heal on cooldown? → No
  ↓
playerHealth.Heal(20)
  ↓
Current health += 20 (max: maxHealth)
  ↓
Start 5 second cooldown
```

---

## 🎨 Why Each File Exists

**ClassSelectionUI.cs:**
- Creates the menu experience
- Makes class selection visual and interactive

**PlayerClass.cs:**
- Core gameplay system
- Makes classes feel different to play
- Handles all combat

**GameManager.cs:**
- Connects menu to game
- Sets up player correctly
- Stores all class definitions in one place

**PlayerProjectile.cs:**
- Makes ranged combat work
- Handles damage application
- Cleans up projectiles

**ClassSwitcherDebug.cs:**
- Makes testing faster
- Skips menu during development
- Remove before final release

---

## 🔍 Common Questions Answered

**Q: Why PlayerPrefs?**
A: Transfers data between scenes. Unity destroys everything when loading new scene, so we save the choice first.

**Q: Why not just have 4 different player prefabs?**
A: More flexible! Can switch classes mid-game, easier to balance, less duplication.

**Q: Why separate GameManager and PlayerClass?**
A: GameManager = setup once at start. PlayerClass = runs continuously during gameplay. Separation of concerns.

**Q: Why Rigidbody with no gravity on projectiles?**
A: We want straight shooting, not arrows that fall. Rigidbody gives physics (movement, collision) without gravity.

**Q: Why Is Trigger on projectile colliders?**
A: Trigger = detect hit without bouncing. We want projectiles to stick/disappear, not ricochet.

**Q: How does melee attack work without projectiles?**
A: Physics.Raycast = invisible laser check. If enemy is in front, hit detected instantly.

**Q: What happens if I press 2 class buttons?**
A: Last one clicked wins. When you click Start Game, whichever class was selected last is used.

---

## 🛠️ Customization Guide

**Change damage:**
Edit GameManager.cs → SetupClassData() → attackDamage value

**Change health:**
Edit GameManager.cs → SetupClassData() → maxHealth value

**Change colors:**
Edit GameManager.cs → SetupClassData() → classColor value

**Change special abilities:**
Edit PlayerClass.cs → WarriorDefensiveStance(), RangerMultiShot(), etc.

**Add new class:**
1. Add to ClassType enum
2. Create new PlayerClassData in GameManager
3. Add button in menu
4. Add case in UpdateClassDisplay()

---

## 🎯 Testing Checklist

To verify everything works:

- [ ] Menu loads with 4 colored buttons
- [ ] Clicking button updates info display
- [ ] Start Game loads game scene
- [ ] Player spawns with correct class
- [ ] Health bar shows correct max health
- [ ] Movement speed feels different per class
- [ ] Warrior can melee attack
- [ ] Ranger shoots arrows
- [ ] Mage shoots fireballs
- [ ] Cleric shoots light
- [ ] Cleric can heal (H key)
- [ ] Special abilities work (E key)
- [ ] Projectiles damage enemies
- [ ] Player color matches class

---

## 📊 Quick Reference Table

| Class   | HP  | Damage | Speed | Type   | Heal? | Special          |
|---------|-----|--------|-------|--------|-------|------------------|
| Warrior | 150 | 15     | 10    | Melee  | No    | Defense Stance   |
| Ranger  | 100 | 20     | 14    | Ranged | No    | Multi-Shot       |
| Mage    | 75  | 30     | 12    | Magic  | No    | Explosive Ball   |
| Cleric  | 120 | 12     | 11    | Holy   | Yes   | Area Heal        |

---

That's the complete system! Every script, every connection, every piece of data flow. You now have a fully functional D&D-style class system! 🎮✨