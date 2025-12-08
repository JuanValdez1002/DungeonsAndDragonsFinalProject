# Dungeons and Dragons Final Project - Game Instructions

## 🎮 Welcome to the Dungeon!

This is a first-person dungeon crawler where you choose a character class and battle enemies in procedurally generated dungeons. Choose your class wisely - each has unique abilities and playstyles!

---

## 🚀 How to Start

### **1. Launch the Game**
- Open the game in Unity and press **Play**
- Or run the built executable

### **2. Choose Your Class**
You'll see the **Class Selection Menu** with 4 options:
- **Warrior** - Tank (high health, melee fighter)
- **Ranger** - Archer (fast, ranged attacks)
- **Mage** - Spellcaster (high damage, fragile)
- **Cleric** - Healer (balanced, can heal)

### **3. Click a Class to See Stats**
- Health, damage, and speed will display
- Read the special ability description

### **4. Click "START GAME"**
- The dungeon will load
- You spawn with your chosen class

---

## 🎯 Game Controls

### **Movement**
- **W** - Move Forward
- **A** - Move Left
- **S** - Move Backward
- **D** - Move Right
- **Mouse** - Look Around
- **Space** - Jump
- **Ctrl** - Crouch

### **Combat**
- **Left Mouse Click** - Regular Attack
  - Warrior: Melee swing (close range)
  - Ranger: Shoot arrow
  - Mage: Shoot fireball
  - Cleric: Shoot holy light
  
- **Right Mouse Click** - Special Ability
  - Warrior: Defensive Stance (reduce damage for 3 seconds)
  - Ranger: Multi-Shot (fire 3 arrows at once)
  - Mage: Explosive Fireball (double damage + area explosion)
  - Cleric: Area Heal (heal 20 HP over 5 seconds)

### **Cleric Only**
- **H Key** - Quick Heal (instant 20 HP, 5 second cooldown)

### **Debug/Testing** (Remove in final build)
- **1 Key** - Switch to Warrior
- **2 Key** - Switch to Ranger
- **3 Key** - Switch to Mage
- **4 Key** - Switch to Cleric

---

## ⚔️ Character Classes Explained

### **🛡️ WARRIOR - The Tank**
**Best For:** Beginners, players who like melee combat

**Stats:**
- Health: 150 (Highest!)
- Speed: 10 (Slowest)
- Damage: 15 (Melee)
- Range: 2.5 units (very close)

**Playstyle:**
- Get up close to enemies
- Tank damage with high health
- Use Defensive Stance when surrounded
- Hold the frontline

**Tips:**
- Don't be afraid to take hits - you have lots of health
- Chase down enemies aggressively
- Use special ability when health drops below 50%

---

### **🏹 RANGER - The Archer**
**Best For:** Players who like mobility and precision

**Stats:**
- Health: 100 (Medium)
- Speed: 14 (Fastest!)
- Damage: 20 (Ranged)
- Range: 20 units (long range)

**Playstyle:**
- Keep distance from enemies
- Shoot arrows from safety
- Use speed to dodge attacks
- Kite enemies around the map

**Tips:**
- Stay mobile - never stand still
- Use Multi-Shot when enemies get close
- Keep 10-15 units between you and enemies
- Circle-strafe while shooting

---

### **🔮 MAGE - The Glass Cannon**
**Best For:** Experienced players who like high risk/reward

**Stats:**
- Health: 75 (Lowest - very fragile!)
- Speed: 12 (Medium)
- Damage: 30 (Highest!)
- Range: 25 units (longest range)

**Playstyle:**
- Stay at maximum distance
- Nuke enemies before they reach you
- Use Explosive Fireball on groups
- Play cautiously - you die fast!

**Tips:**
- Positioning is everything - stay far back
- Save Explosive Fireball for emergencies
- Retreat if enemies get close
- High skill, high reward class

---

### **✨ CLERIC - The Healer**
**Best For:** Players who want balanced gameplay

**Stats:**
- Health: 120 (Good)
- Speed: 11 (Medium)
- Damage: 12 (Lowest)
- Range: 15 units (medium range)

**Playstyle:**
- Balance offense and healing
- Sustain through long fights
- Support role in team play
- Heal before/after combat

**Tips:**
- Use H for quick emergency heals
- Use Right Click heal when you can retreat safely
- Don't waste heals at full HP
- Most forgiving class for beginners

---

## 🎯 Combat Tips

### **General Strategy**
1. **Assess the situation** - How many enemies? Where are they?
2. **Choose your approach** - Aggressive or defensive?
3. **Manage your health** - Don't fight at low HP
4. **Use special abilities** - They're powerful, use them often!
5. **Retreat if needed** - Live to fight another day

### **Fighting Enemies**
- **Enemies patrol** when they don't see you
- **Enemies chase** when you enter their sight range
- **Enemies attack** when you get close
- **Enemies shoot projectiles** at range
- **Enemies respawn** after 5 seconds (they come back!)

### **Damage Numbers**
- **Warrior Melee:** 15 damage per hit
- **Ranger Arrow:** 20 damage per arrow
- **Mage Fireball:** 30 damage (60 with special!)
- **Cleric Light:** 12 damage per hit

### **Special Ability Cooldowns**
- No cooldowns! Use them freely
- Attack cooldown: 1 second between attacks
- Heal cooldown (Cleric): 5 seconds

---

## 💚 Health System

### **Health Bars**
- Your health displays on the UI (if implemented)
- Enemies also have health bars above them

### **Taking Damage**
- You become briefly invulnerable after taking a hit
- Invulnerability time: varies by class
- Red screen flash indicates damage (if implemented)

### **Healing (Cleric Only)**
- **H Key:** Instant 20 HP, 5 second cooldown
- **Right Click Special:** 20 HP over 5 seconds (4 HP per second)
- Cannot overheal (stops at max health)

### **Death & Respawn**
- If your health reaches 0, you respawn (if implemented)
- Respawn at starting location
- Health restored to full

---

## 🏰 Dungeon Navigation

### **Map Layout**
- Procedurally generated dungeon
- Floors and walls create maze-like structure
- Multiple rooms and corridors
- Enemies scattered throughout

### **Tips for Exploring**
- Check corners before entering rooms
- Listen for enemy sounds
- Use sprint to escape danger
- Memorize paths for quick escapes

---

## 🎮 Advanced Techniques

### **Kiting**
Move backwards while attacking to maintain distance from enemies.
- **Best for:** Ranger, Mage, Cleric
- **How:** Walk backwards, shoot, repeat

### **Strafing**
Move sideways to dodge projectiles while attacking.
- **Best for:** All classes
- **How:** Press A/D while fighting

### **Ability Combos**

**Warrior:**
1. Rush into group of enemies
2. Activate Defensive Stance (Right Click)
3. Tank damage while attacking

**Ranger:**
1. Shoot arrows at range (Left Click)
2. When enemy gets close, Multi-Shot (Right Click)
3. Backpedal while shooting

**Mage:**
1. Normal fireballs on single enemies (Left Click)
2. Explosive Fireball on groups (Right Click)
3. Retreat if they get close

**Cleric:**
1. Damage enemies with light (Left Click)
2. Heal when below 70 HP (H or Right Click)
3. Continue fighting while healing

---

## 🏆 Winning Strategy by Class

### **Warrior Victory Path**
1. Aggressive playstyle - charge enemies
2. Use high health to your advantage
3. Tank hits, deal consistent damage
4. Don't overthink - just fight!

### **Ranger Victory Path**
1. Maintain 15-20 unit distance
2. Use speed to reposition constantly
3. Multi-Shot on big targets
4. Never let enemies surround you

### **Mage Victory Path**
1. Stay at max range always
2. Burst down enemies before they reach you
3. Save Explosive Fireball for emergencies
4. Master positioning and spacing

### **Cleric Victory Path**
1. Balance damage and healing
2. Heal proactively at 70% HP
3. Use Area Heal during retreats
4. Outlast enemies with sustain

---

## 🐛 Troubleshooting

### **Controls Not Working**
- Make sure you clicked in the game window
- Check if cursor is locked (press ESC to unlock)

### **Can't Attack**
- Wait for attack cooldown (1 second)
- Make sure you're aiming at an enemy
- Check if you're out of range

### **Healing Not Working (Cleric)**
- Check if you're at full health (can't overheal)
- Wait for cooldown (5 seconds)
- Make sure you selected Cleric class

### **Stuck in Wall**
- Press Space to jump
- Try crouching (Ctrl) and moving
- Restart level if necessary

### **Enemies Not Taking Damage**
- Make sure projectile prefabs are assigned (Ranger/Mage/Cleric)
- Check console for errors
- Verify enemy has EnemyAi script

---

## 🎓 Class Recommendations

**New to the Game?**
→ Start with **Warrior** or **Cleric**
- Forgiving, high health
- Simple mechanics
- Good for learning

**Like Fast-Paced Action?**
→ Try **Ranger**
- Mobile and fun
- Ranged safety
- Skill-based gameplay

**Want a Challenge?**
→ Play **Mage**
- High risk, high reward
- Punishing but powerful
- Master class for experts

**Playing with Friends?**
- Warrior = Tank
- Mage = Damage Dealer
- Cleric = Healer
- Ranger = Mobile DPS

---

## 📊 Quick Reference Card

| Class   | Health | Speed | Damage | Type   | Difficulty |
|---------|--------|-------|--------|--------|------------|
| Warrior | 150    | 10    | 15     | Melee  | ⭐ Easy     |
| Ranger  | 100    | 14    | 20     | Ranged | ⭐⭐ Medium |
| Mage    | 75     | 12    | 30     | Magic  | ⭐⭐⭐ Hard  |
| Cleric  | 120    | 11    | 12     | Holy   | ⭐ Easy     |

---

## 🎯 Controls Summary

```
MOVEMENT                  COMBAT
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
WASD      Move            Left Click    Attack
Mouse     Look            Right Click   Special
Space     Jump            H (Cleric)    Heal
Ctrl      Crouch          

DEBUG (Testing Only)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
1-4       Switch Class
```

---

## 💡 Pro Tips

1. **Learn enemy patterns** - They have sight and attack ranges
2. **Use the environment** - Hide behind walls, use corners
3. **Manage cooldowns** - Don't spam, time your attacks
4. **Special abilities are strong** - Use them often!
5. **Positioning wins fights** - Stay at your optimal range
6. **Respawns mean endless practice** - Enemies come back, keep fighting!
7. **Different classes = different strategies** - Don't play Mage like Warrior!
8. **Healing is powerful** - Cleric can outlast tough fights
9. **Speed is survival** - Ranger can escape anything
10. **Health is a resource** - Warrior should trade HP for damage

---

## 🎮 Have Fun!

This is a student project for Game Development class. Experiment with different classes, master the combat, and explore the dungeon!

**Credits:**
- Class System: PlayerClass.cs, ClassSelectionUI.cs, GameManager.cs
- Combat: PlayerProjectile.cs, EnemyAi.cs
- Movement: PlayerMovement.cs, MouseLook.cs
- Health: PlayerHealth.cs

**For Developers:**
See the following documentation files for technical details:
- `HOW_EVERYTHING_WORKS.md` - System architecture
- `CLASS_ABILITIES_GUIDE.md` - Ability mechanics
- `CLASS_SELECTION_SETUP.md` - Setup guide
- `GITHUB_PUSH_GUIDE.md` - Version control

---

**Good luck in the dungeon, adventurer!** ⚔️🛡️🏹🔮✨
