# D&D Class Abilities - Complete Guide

## 🎮 Updated Controls

| Input | Action |
|-------|--------|
| **Left Mouse Click** | Regular Attack (melee/ranged based on class) |
| **Right Mouse Click** | Special Ability (unique per class) |
| **H Key** | Heal (Cleric only) |

---

## ⚔️ WARRIOR - The Tank

**Role:** Melee fighter, soaks damage, frontline protector

### 📊 Stats:
- **Health:** 150 (HIGHEST - hard to kill!)
- **Speed:** 10 (Slowest - heavy armor)
- **Damage:** 15 (Medium)
- **Range:** 2.5 (Must be close to attack)
- **Type:** Melee

### 🎯 Regular Attack (Left Click):
**"Melee Swing"**
- Uses an **invisible raycast** (like a laser beam) to detect enemies in front
- Range: 2.5 units (very close - must be right in front of enemy)
- Damage: 15 per hit
- How it works:
  1. Click Left Mouse
  2. Shoots invisible line forward
  3. If line hits enemy → damage dealt instantly
  4. No projectile created
- **Strategy:** Get up close and personal, tank damage while swinging

### ⭐ Special Ability (Right Click):
**"Defensive Stance"**
- **Duration:** 3 seconds
- **Effect:** Reduces time between taking damage hits (invulnerability shortened)
- **What it does:**
  - Normally when you take damage, you're invulnerable for a moment
  - This shortens that cooldown, allowing you to take hits more frequently
  - BUT your health is higher, so you can survive longer in combat
- **Visual:** None (just a console message)
- **Cooldown:** None (can spam it, but only lasts 3 seconds)
- **Best Use:**
  - When surrounded by multiple enemies
  - When you need to hold a position
  - Tank for your team while they attack from range

---

## 🏹 RANGER - The Archer

**Role:** Fast ranged attacker, high mobility, precision damage

### 📊 Stats:
- **Health:** 100 (Medium survivability)
- **Speed:** 14 (FASTEST - super mobile!)
- **Damage:** 20 (High damage per arrow)
- **Range:** 20 (Long range sniper)
- **Type:** Ranged (shoots arrows)

### 🎯 Regular Attack (Left Click):
**"Arrow Shot"**
- Shoots a **physical arrow projectile**
- Range: 20 units (can attack from far away)
- Damage: 20 per arrow
- Arrow Speed: 25 (very fast)
- How it works:
  1. Click Left Mouse
  2. Creates arrow GameObject
  3. Arrow flies forward with physics
  4. When arrow hits enemy → damage dealt
  5. Arrow destroyed
- **Projectile Required:** RangerArrow prefab (sphere with Rigidbody)
- **Strategy:** Keep distance, shoot from safety, dodge attacks with high speed

### ⭐ Special Ability (Right Click):
**"Multi-Shot"**
- **Effect:** Fires 3 arrows at once in a spread pattern
- **Spread:** Center arrow straight, left arrow 15° left, right arrow 15° right
- **Damage:** 20 per arrow (3 arrows = 60 total damage if all hit!)
- **Arrow Speed:** 25 (same as regular)
- **Cooldown:** None (can spam, but arrows limited by attack cooldown)
- **Best Use:**
  - When enemy is close and big (all 3 arrows hit)
  - Against groups of enemies
  - Burst damage on bosses
  - When you can't miss

---

## 🔮 MAGE - The Glass Cannon

**Role:** High burst damage, squishy, powerful magic attacks

### 📊 Stats:
- **Health:** 75 (LOWEST - very fragile!)
- **Speed:** 12 (Medium)
- **Damage:** 30 (HIGHEST regular damage!)
- **Range:** 25 (Very long range)
- **Type:** Ranged (shoots fireballs)

### 🎯 Regular Attack (Left Click):
**"Fireball"**
- Shoots a **magical fireball projectile**
- Range: 25 units (longest range!)
- Damage: 30 per fireball
- Fireball Speed: 20 (medium speed)
- How it works:
  1. Click Left Mouse
  2. Creates fireball GameObject
  3. Fireball flies forward
  4. When fireball hits enemy → 30 damage
  5. Fireball destroyed
- **Projectile Required:** MageFireball prefab
- **Strategy:** Stay far back, nuke enemies from safe distance, avoid getting hit

### ⭐ Special Ability (Right Click):
**"Explosive Fireball"**
- **Effect:** Massive fireball that explodes on impact
- **Damage:** 60 (DOUBLE regular damage!)
- **AOE Damage:** 30 in explosion radius (half damage to nearby enemies)
- **Fireball Speed:** 30 (faster than regular: 20 × 1.5)
- **Explosion Radius:** Set in PlayerProjectile script
- **How it works:**
  1. Right Click
  2. Creates super-charged fireball
  3. Fireball flies 50% faster
  4. Hits enemy → 60 damage to main target
  5. Explosion deals 30 damage to all enemies within radius
  6. Perfect for groups!
- **Cooldown:** None
- **Best Use:**
  - When enemies are grouped together
  - Boss burst damage
  - Clearing waves of weak enemies
  - Emergency nuke when health is low

---

## ✨ CLERIC - The Support Healer

**Role:** Healer/support, balanced offense and defense, sustain

### 📊 Stats:
- **Health:** 120 (Good survivability)
- **Speed:** 11 (Medium)
- **Damage:** 12 (Lowest damage)
- **Range:** 15 (Medium range)
- **Type:** Ranged (shoots holy light)

### 🎯 Regular Attack (Left Click):
**"Holy Light Bolt"**
- Shoots a **holy light projectile**
- Range: 15 units (medium range)
- Damage: 12 per bolt (lowest, but you can heal!)
- Light Speed: 20
- How it works:
  1. Click Left Mouse
  2. Creates light bolt GameObject
  3. Light flies forward
  4. When light hits enemy → 12 damage
  5. Light destroyed
- **Projectile Required:** ClericLight prefab
- **Strategy:** Balanced damage and healing, stay mid-range, support role

### 💚 Heal Ability (H Key):
**"Self Heal"**
- **Heal Amount:** 20 HP
- **Cooldown:** 5 seconds
- **Limitation:** Cannot overheal (stops at max health)
- **How it works:**
  1. Press H
  2. Instantly restore 20 HP
  3. Wait 5 seconds before next heal
- **Best Use:**
  - When health is below 50%
  - After taking a big hit
  - Before engaging tough enemies
  - Survival in long fights

### ⭐ Special Ability (Right Click):
**"Area Heal"**
- **Effect:** Heal over time (HOT - Heal Over Time)
- **Total Healing:** 20 HP (same as regular heal)
- **Duration:** 5 seconds
- **Healing per Second:** 4 HP/second (5 ticks × 4 HP)
- **How it works:**
  1. Right Click
  2. Every 1 second for 5 seconds → heal 4 HP
  3. Total: 20 HP restored gradually
  4. Can still move and attack while healing
- **Cooldown:** None (but uses heal cooldown internally)
- **Best Use:**
  - When you can retreat to safety for 5 seconds
  - Sustain healing during long fights
  - More efficient than quick heal if you have time
  - Healing while kiting enemies

---

## 🎯 Combat Strategies Per Class

### **WARRIOR Strategy:**
```
1. Rush into combat (you have most HP)
2. Get close to enemies (2.5 range needed)
3. Left Click to melee attack
4. When surrounded, Right Click for Defensive Stance
5. Tank damage while team attacks
6. Don't worry about dying - you're tanky!
```

### **RANGER Strategy:**
```
1. Stay at 15-20 unit distance
2. Left Click for single arrows
3. When enemy gets close, Right Click for Multi-Shot
4. Use your speed (14) to dodge and kite
5. Keep moving, never stand still
6. Pick off enemies from safe range
```

### **MAGE Strategy:**
```
1. Stay at MAX range (25 units)
2. Left Click for regular fireballs (30 damage)
3. When enemies group up, Right Click for Explosive (60 + AOE)
4. NEVER let enemies get close (only 75 HP!)
5. Play cautious - you die fast but kill faster
6. Burst damage is your strength
```

### **CLERIC Strategy:**
```
1. Balance between offense and defense
2. Left Click to damage enemies (12 per hit)
3. Monitor health - heal at 50-70 HP
4. H Key for quick emergency heal
5. Right Click for sustained healing while retreating
6. Support role - stay alive to keep healing
7. Medium range (15 units) - not too close, not too far
```

---

## 📊 Quick Comparison Table

| Class   | HP  | Speed | Damage | Range | Attack Type | Special                  |
|---------|-----|-------|--------|-------|-------------|--------------------------|
| Warrior | 150 | 10    | 15     | 2.5   | Melee       | Defensive Stance (3s)    |
| Ranger  | 100 | 14    | 20     | 20    | Arrows      | Multi-Shot (3 arrows)    |
| Mage    | 75  | 12    | 30     | 25    | Fireballs   | Explosive (60 + AOE)     |
| Cleric  | 120 | 11    | 12     | 15    | Holy Light  | Area Heal (20 over 5s)   |

---

## 🔧 Technical Details

### **How Melee Works (Warrior):**
```csharp
Physics.Raycast(position, forward, attackRange)
→ Invisible laser shoots forward 2.5 units
→ If hits enemy collider → instant damage
→ No GameObject created
→ Fastest attack type (no travel time)
```

### **How Ranged Works (Ranger/Mage/Cleric):**
```csharp
Instantiate(projectilePrefab, spawnPosition)
→ Creates physical GameObject
→ Rigidbody.linearVelocity = forward * speed
→ Projectile flies through air
→ OnTriggerEnter(enemy) → damage dealt
→ Destroy(projectile)
```

### **How Special Abilities Work:**

**Warrior Defensive:**
```csharp
StartCoroutine()
→ Modify playerHealth.invulnerabilityTime
→ Wait 3 seconds
→ Restore original value
```

**Ranger Multi-Shot:**
```csharp
FOR i = -1 to 1:
    angle = i × 15°
    direction = Quaternion.Euler(0, angle, 0) × forward
    Create arrow at angle
    Shoot with same velocity
```

**Mage Explosive:**
```csharp
damage = attackDamage × 2 (60)
projectileSpeed × 1.5 (faster)
isExplosive = true
→ PlayerProjectile.DealExplosiveDamage()
→ Find all enemies in radius
→ Deal half damage to each
```

**Cleric Area Heal:**
```csharp
FOR 5 iterations:
    Heal(healAmount / 5)  // 4 HP
    Wait 1 second
END
Total: 20 HP over 5 seconds
```

---

## 🎮 Controls Summary

### **ALL CLASSES:**
- **Left Mouse Click** = Regular attack
- **Right Mouse Click** = Special ability
- **WASD** = Move
- **Mouse** = Look around
- **Space** = Jump
- **Ctrl** = Crouch

### **CLERIC ONLY:**
- **H Key** = Quick heal (20 HP, 5s cooldown)

---

## 💡 Pro Tips

### **Warrior:**
- Use Defensive Stance when health is below 50%
- Don't run away - you're meant to take hits
- Chase fast enemies (Ranger) by predicting movement

### **Ranger:**
- Multi-Shot at close range for maximum damage
- Use speed to circle-strafe enemies
- Switch to Warrior if you keep dying (more forgiving)

### **Mage:**
- Save Explosive Fireball for groups or bosses
- Regular fireball spam is very strong (30 damage!)
- Play defensively - positioning is everything
- Most rewarding class but hardest to master

### **Cleric:**
- Use Right Click heal when safe, H key when emergency
- Don't waste heals when at full HP
- Balance offense and healing
- Best for beginners (forgiving, sustain)

---

## 🔄 Class Synergies (If Playing with Friends)

**Best Team Comp:**
- **Warrior** (tank) + **Mage** (damage) + **Cleric** (healer)
- Warrior holds enemies, Mage nukes from back, Cleric keeps everyone alive

**Duo Combos:**
- **Warrior + Cleric** = Unkillable tank with healer support
- **Ranger + Mage** = Double ranged damage, kiting powerhouse
- **Warrior + Ranger** = Tank and spank, classic combo

---

That's everything! Each class has a unique playstyle. Experiment and find your favorite! 🎮✨
