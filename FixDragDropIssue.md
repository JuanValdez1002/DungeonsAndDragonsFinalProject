# Fix: Can't Drag Text Into ClassSelectionManager

## Quick Diagnosis

### Step 1: Check What Type of Text You Have

1. Select any text object (like TitleText) in Hierarchy
2. Look in Inspector
3. Find the text component - it will say one of:
   - **Text** (Legacy)
   - **TextMeshPro - Text (UI)** or **TMP_Text**

## Solution A: If You Have Legacy Text (Most Common)

The updated script now has separate sections for both types!

### In Unity:
1. Select **ClassSelectionManager** in Hierarchy
2. In Inspector, scroll down to find **ClassSelectionUI (Script)**
3. You'll now see TWO sections:
   - **"Text Fields (Use Legacy Text OR TextMeshPro)"**
   - **"Class Info Display (Legacy Text)"** ← Use this one
   - **"Class Info Display (TextMeshPro)"** ← Or this one

4. Drag your text objects into the **Legacy Text** section (ignore TextMeshPro section)

### Assignment:
```
Legacy Text Section:
├─ Class Name Display → Drag your ClassNameText here
├─ Health Display → Drag your HealthText here
├─ Damage Display → Drag your DamageText here
├─ Speed Display → Drag your SpeedText here
└─ Special Ability Display → Drag your SpecialText here
```

## Solution B: If You Have TextMeshPro

1. Select **ClassSelectionManager**
2. Find **ClassSelectionUI (Script)**
3. Use the **"TextMeshPro"** section instead
4. Drag your TMP text objects there
5. Leave Legacy Text section empty

## Solution C: Super Simple Workaround

If dragging still doesn't work, use the **circle selector**:

1. Select **ClassSelectionManager**
2. Find the field you want to fill (e.g., "Class Name Display")
3. Click the **small circle** to the right of the field
4. A selection window pops up
5. Find your text object in the list
6. **Double-click** it

**Visual:**
```
Class Name Display: [None  ⊙]
                          ↑ Click this circle!
```

## Solution D: Check If Object Can Be Assigned

Sometimes Unity prevents dragging for valid reasons:

### Common Issues:

**Issue 1: Text is on a different scene**
- Make sure your ClassSelection scene is open
- Make sure you're dragging from the SAME scene

**Issue 2: Prefab vs Scene object**
- Don't drag prefab from Project window
- Drag the instance from Hierarchy

**Issue 3: Script hasn't compiled**
- Wait for Unity to finish compiling
- Check bottom-right of Unity for spinning icon
- Wait until it stops before dragging

**Issue 4: Wrong component type**
- Script expects `Text` but you have `TMP_Text`
- Use the correct section (Legacy vs TextMeshPro)

## Manual Assignment (If All Else Fails)

You can also manually assign via script:

1. Select ClassSelectionManager
2. In Inspector, find the ClassSelectionUI component
3. Click the **gear icon** (⚙️) at top-right of component
4. Choose **Edit Script**
5. You'll see the code

But with the updated script, you should now have fields for both types!

## Test If It Works

After assigning:
1. Press **Play**
2. Click a class button
3. The text should update
4. If it does, everything is working!

## Visual Guide: What You Should See

```
Inspector for ClassSelectionManager:
┌─────────────────────────────────────┐
│ Class Selection UI (Script)        │
├─────────────────────────────────────┤
│ UI References                       │
│ └─ Selection Panel: [InfoPanel]    │
│                                      │
│ Text Fields (Legacy OR TMP)         │
│ ├─ Title Text: [TitleText]         │ ← Drag here if legacy
│ ├─ Title Text TMP: [None]          │ ← Or here if TMP
│ ├─ Description Text: [DescText]    │
│ └─ Description Text TMP: [None]    │
│                                      │
│ Class Buttons                       │
│ ├─ Warrior Button: [WarriorBtn]    │
│ ├─ Ranger Button: [RangerBtn]      │
│ ├─ Mage Button: [MageBtn]          │
│ ├─ Cleric Button: [ClericBtn]      │
│ └─ Start Button: [StartBtn]        │
│                                      │
│ Class Info Display (Legacy Text)   │ ← Use this section
│ ├─ Class Name Display: [?]         │ ← Drag ClassNameText
│ ├─ Health Display: [?]             │ ← Drag HealthText
│ ├─ Damage Display: [?]             │ ← Drag DamageText
│ ├─ Speed Display: [?]              │ ← Drag SpeedText
│ └─ Special Ability Display: [?]    │ ← Drag SpecialText
│                                      │
│ Class Info Display (TextMeshPro)   │ ← Or use this section
│ ├─ Class Name Display TMP: [?]     │
│ ├─ Health Display TMP: [?]         │
│ └─ ... (etc)                        │
└─────────────────────────────────────┘
```

## Still Not Working?

### Checklist:
- [ ] ClassSelectionUI script is on ClassSelectionManager GameObject
- [ ] Text objects exist in Hierarchy (same scene)
- [ ] Unity finished compiling (no spinning icon)
- [ ] Using correct section (Legacy vs TextMeshPro)
- [ ] Tried using circle selector instead of dragging

### Nuclear Option: Recreate Text
If one specific text won't work:
1. Delete it from Hierarchy
2. Create new: Right-click Canvas → UI → Text
3. Set it up again
4. Try dragging again

### Alternative: Skip Optional Fields
Not all fields are required! You can leave some empty:
- Buttons are REQUIRED
- Text displays are OPTIONAL (just won't update)
- Game Scene Name is REQUIRED

Minimum working setup:
- 4 class buttons assigned
- Start button assigned
- Game scene name filled in
- Everything else can be empty (just won't show info)

## Success Check

You'll know it works when:
1. No red errors in Inspector
2. Fields show assigned objects (not "None")
3. Press Play → Click button → Text updates

If text updates when you click buttons, success! 🎉