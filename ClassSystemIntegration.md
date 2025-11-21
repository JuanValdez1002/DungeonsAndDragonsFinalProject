# Class System Integration Checklist

## ✅ Quick Start (Minimum Setup)

### 1. Test Without UI First (5 minutes)
Skip the class selection menu initially and test in your existing scene:

1. **Add to existing Player**:
   - Select your Player GameObject
   - Add Component → **PlayerClass** script
   - Add Component → **ClassSwitcherDebug** script (for testing)

2. **Create Projectile Prefabs**:
   - Create 3 simple sphere prefabs (different colors)
   - Add Rigidbody (no gravity) and Collider (trigger on)
   - Add **PlayerProjectile** script to each
   - Save as: RangerArrow, MageFireball, ClericLight

3. **Add GameManager**:
   - Create empty GameObject named "GameManager"
   - Add **GameManager** script
   - Drag projectile prefabs into the 3 slots

4. **Test**:
   - Press Play
   - Press **1, 2, 3, 4** to switch classes
   - Press **Left Click** to attack
   - Press **E** for special abilities
   - Press **H** for heal (Cleric only)

### 2. Create Class Selection Menu (15 minutes)
Once basic system works, add the selection UI:

1. Create new scene "ClassSelection"
2. Add Canvas with 4 buttons (Warrior, Ranger, Mage, Cleric)
3. Add "Start Game" button
4. Add **ClassSelectionUI** script and wire up all buttons
5. Add to Build Settings (scene 0)

## 🔧 Integration with Existing Code

### PlayerHealth.cs Compatibility
✅ Already compatible! The system uses:
```csharp
playerHealth.maxHealth = currentClass.maxHealth;
playerHealth.currentHealth = currentClass.maxHealth;
playerHealth.Heal(amount);
```

### PlayerMovement.cs Compatibility  
✅ Already compatible! The system uses:
```csharp
playerMovement.speed = currentClass.moveSpeed;
```

### EnemyAi.cs Compatibility
✅ Already compatible! Projectiles call:
```csharp
enemy.TakeDamage(damage);
```

## 📋 Full Setup Checklist

### Scene Setup
- [ ] GameManager GameObject exists in game scene
- [ ] GameManager has all 3 projectile prefabs assigned
- [ ] Player GameObject has tag "Player"
- [ ] Player has CharacterController component
- [ ] Player has PlayerMovement script
- [ ] Player has PlayerHealth script
- [ ] Player has PlayerClass script (added by GameManager)

### Projectile Prefabs
- [ ] RangerArrow prefab created
  - [ ] Has Rigidbody (no gravity)
  - [ ] Has Collider (trigger ON)
  - [ ] Has PlayerProjectile script
- [ ] MageFireball prefab created
  - [ ] Has Rigidbody (no gravity)
  - [ ] Has Collider (trigger ON)
  - [ ] Has PlayerProjectile script
- [ ] ClericLight prefab created
  - [ ] Has Rigidbody (no gravity)
  - [ ] Has Collider (trigger ON)
  - [ ] Has PlayerProjectile script

### Class Selection Scene (Optional)
- [ ] ClassSelection scene created
- [ ] Canvas with UI buttons
- [ ] ClassSelectionUI script setup
- [ ] All button references assigned
- [ ] gameSceneName set correctly
- [ ] Scene added to Build Settings (index 0)

### Testing Checklist
- [ ] Can switch classes (keyboard 1-4 or UI)
- [ ] Player color changes per class
- [ ] Health changes per class (check UI)
- [ ] Speed changes per class (movement feels different)
- [ ] Warrior melee attack works
- [ ] Ranger shoots arrows
- [ ] Mage shoots fireballs
- [ ] Cleric shoots light
- [ ] Cleric can heal (H key)
- [ ] Special abilities work (E key)
- [ ] Projectiles damage enemies
- [ ] Player takes damage normally

## 🐛 Common Issues & Fixes

### Issue: "No projectile prefab assigned"
**Fix**: Assign projectile prefabs in GameManager inspector

### Issue: Projectiles don't damage enemies
**Fix**: 
- Check enemy has `EnemyAi` script (not EnemyAI or EnemyAi)
- Check projectile collider is trigger
- Check enemy has collider

### Issue: Player color doesn't change
**Fix**: Player needs MeshRenderer or children with renderers

### Issue: Movement speed doesn't change
**Fix**: Check PlayerMovement script uses public `speed` variable

### Issue: Can't attack
**Fix**: Check Input settings have "Fire1" mapped (default)

### Issue: Heal doesn't work (Cleric)
**Fix**: 
- Check PlayerHealth script has `Heal(int amount)` method
- Press H key (not E)

### Issue: Class selection doesn't transfer to game
**Fix**:
- Check scene name in ClassSelectionUI matches Build Settings
- Check both scenes in Build Settings
- Check GameManager is in game scene

## 🎯 Testing Scenarios

### Test 1: Basic Class Switching
1. Play game scene
2. Press 1, 2, 3, 4 keys
3. Check console for "Class set to: X"
4. Check player color changes

### Test 2: Combat Test
1. Select Ranger (press 2)
2. Left click to shoot
3. Check arrows spawn and fly
4. Hit enemy and check it takes damage

### Test 3: Healing Test
1. Select Cleric (press 4)
2. Take damage from enemy
3. Press H to heal
4. Check health increases

### Test 4: Special Abilities
1. Select Mage (press 3)
2. Press E for explosive fireball
3. Should deal AOE damage

### Test 5: Full Flow
1. Start from ClassSelection scene
2. Click class button
3. Check info displays update
4. Click "Start Game"
5. Spawn with correct class
6. Test combat

## 📝 Quick Reference

### Keyboard Shortcuts (Testing)
- **1**: Warrior
- **2**: Ranger  
- **3**: Mage
- **4**: Cleric
- **Left Click**: Attack
- **E**: Special Ability
- **H**: Heal (Cleric)

### Inspector Values to Check

**PlayerClass Component:**
- Current Class: Shows selected class data
- Warrior/Ranger/Mage/Cleric Class: Auto-filled by GameManager
- Player Health: Reference to PlayerHealth
- Player Movement: Reference to PlayerMovement
- Projectile Spawn Point: Auto-created or assign manually

**GameManager Component:**
- Ranger Arrow Prefab: Green projectile
- Mage Fireball Prefab: Red/orange projectile
- Cleric Light Prefab: Yellow projectile
- Player Spawn Point: Optional

## 🚀 Next Steps

After basic system works:
1. Add particle effects for abilities
2. Add sound effects for attacks
3. Add cooldown UI indicators
4. Add class icons to UI
5. Balance damage numbers
6. Add more special abilities
7. Create class-specific models

## Need Help?

Check these files for reference:
- `ClassSystemSetup.md` - Full detailed setup
- `ClassReference.md` - Class stats and abilities
- Console logs - System outputs debug info

All scripts have debug logs - check Unity console!