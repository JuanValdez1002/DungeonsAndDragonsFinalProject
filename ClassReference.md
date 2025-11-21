# D&D Class System - Quick Reference

## Class Stats Comparison

| Class   | Health | Damage | Speed | Range  | Attack Type |
|---------|--------|--------|-------|--------|-------------|
| Warrior | 150    | 15     | 10    | 2.5m   | Melee       |
| Ranger  | 100    | 20     | 14    | 20m    | Ranged      |
| Mage    | 75     | 30     | 12    | 25m    | Magic       |
| Cleric  | 120    | 12     | 11    | 15m    | Holy Light  |

## Class Abilities

### 🛡️ Warrior (Tank/Melee)
**Playstyle**: Get close and tank damage
- **Color**: Red
- **Attack**: Melee raycast (close range)
- **Special (E)**: Defensive Stance (3 seconds of damage reduction)
- **Strength**: Highest health pool, good for beginners
- **Weakness**: Must get close to enemies

### 🏹 Ranger (Ranged DPS)
**Playstyle**: Kite enemies from distance
- **Color**: Green
- **Attack**: Fast arrows
- **Special (E)**: Multi-Shot (fires 3 arrows in spread)
- **Strength**: Highest damage + fastest movement
- **Weakness**: Lower health, need to maintain distance

### 🔮 Mage (Burst Damage)
**Playstyle**: Glass cannon - high risk, high reward
- **Color**: Blue
- **Attack**: Powerful fireballs
- **Special (E)**: Explosive Fireball (AOE damage)
- **Strength**: Highest damage output (30 per shot!)
- **Weakness**: Lowest health (75), dies quickly

### ✨ Cleric (Support/Healer)
**Playstyle**: Balanced survivalist
- **Color**: Yellow/Gold
- **Attack**: Holy light projectiles
- **Heal (H)**: Self-heal for 20 HP (5 second cooldown)
- **Special (E)**: Area Heal (heal over 5 seconds)
- **Strength**: Only class that can heal, very survivable
- **Weakness**: Lower damage output

## Controls (All Classes)

### Movement
- **W/A/S/D**: Walk
- **Mouse**: Look around
- **Space**: Jump
- **Left Ctrl**: Crouch
- **Shift**: Sprint (if implemented)

### Combat
- **Left Click**: Basic Attack
  - Warrior: Melee swing
  - Ranger: Shoot arrow
  - Mage: Cast fireball
  - Cleric: Shoot holy light
  
- **E Key**: Special Ability
  - Warrior: Defensive stance
  - Ranger: Multi-shot
  - Mage: Explosive fireball
  - Cleric: Area heal

- **H Key**: Heal (Cleric only)

### Debug (Testing Only)
- **1**: Switch to Warrior
- **2**: Switch to Ranger
- **3**: Switch to Mage
- **4**: Switch to Cleric

## Recommended Strategies

### Warrior Strategy
1. Rush into combat
2. Use melee attacks up close
3. Activate defensive stance when low health
4. Tank for team (if multiplayer later)

### Ranger Strategy
1. Keep distance from enemies
2. Strafe while shooting
3. Use multi-shot on grouped enemies
4. Fast movement = easy kiting

### Mage Strategy
1. Stay far back
2. One-shot weak enemies with fireballs
3. Use explosive fireball on groups
4. Avoid getting hit at all costs (low HP!)

### Cleric Strategy
1. Heal before engaging
2. Use ranged attacks to stay safe
3. Heal during combat (H key)
4. Use area heal for sustained fights
5. Most forgiving class for learning

## Team Composition (Future Multiplayer)
- **Warrior**: Front-line tank
- **Ranger**: Backline DPS
- **Mage**: Burst damage on priority targets
- **Cleric**: Keep everyone alive

## Attack Cooldowns

| Class   | Cooldown | DPS Potential |
|---------|----------|---------------|
| Warrior | 0.8s     | 18.75 DPS     |
| Ranger  | 0.6s     | 33.33 DPS     |
| Mage    | 1.2s     | 25.00 DPS     |
| Cleric  | 1.0s     | 12.00 DPS     |

*Ranger has highest sustained DPS but Mage has highest burst*

## Color Coding
- 🔴 Red = Warrior (Fire/Blood)
- 🟢 Green = Ranger (Nature/Forest)
- 🔵 Blue = Mage (Magic/Arcane)
- 🟡 Yellow = Cleric (Holy/Light)

## Tips
- **Warrior**: Best for new players, very forgiving
- **Ranger**: Best for experienced players who can aim
- **Mage**: High skill cap, high reward
- **Cleric**: Solo survival master, hard to kill

## Enemy Interactions
All projectiles damage enemies with `EnemyAi` component:
- Direct hit = full damage
- Mage explosion = half damage in radius
- Healing does NOT damage enemies (Cleric still needs to attack)