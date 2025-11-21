# Pushing Class Selector to GitHub - Complete Guide

## 🎯 What You Need to Push

Your class selector system includes several components that need to be pushed to GitHub:

### **Files to Include:**
1. **Scripts** (5 C# files)
   - `Assets/Scripts/PlayerClass.cs`
   - `Assets/Scripts/ClassSelectionUI.cs`
   - `Assets/Scripts/GameManager.cs`
   - `Assets/Scripts/PlayerProjectile.cs`
   - `Assets/Scripts/ClassSwitcherDebug.cs`

2. **Scenes** (1-2 scene files)
   - `Assets/Scenes/ClassSelection.unity` (your menu scene)
   - `Assets/Scenes/MainGameScene.unity` (or whatever your game scene is called)

3. **Prefabs** (if you created them)
   - `Assets/Prefabs/RangerArrow.prefab`
   - `Assets/Prefabs/MageFireball.prefab`
   - `Assets/Prefabs/ClericLight.prefab`

4. **UI Elements** (if separate)
   - Any UI images, sprites, or materials you created

5. **Documentation** (optional but helpful!)
   - `CLASS_SELECTION_SETUP.md`
   - `CLASS_ABILITIES_GUIDE.md`
   - `HOW_EVERYTHING_WORKS.md`
   - `ENEMY_RESPAWN_SETUP.md`

---

## 📋 Step-by-Step: Push to GitHub

### **OPTION 1: Using VS Code (Recommended - Easiest)**

#### **Step 1: Save Everything in Unity**
1. In Unity: **File → Save** (or Ctrl+S)
2. **File → Save Project**
3. Close any scenes that are open and save them

#### **Step 2: Open VS Code**
1. Open your project folder in VS Code
2. You should see a Source Control icon on the left (looks like a branch symbol)

#### **Step 3: Stage Your Changes**
1. Click the **Source Control** icon (left sidebar)
2. You'll see a list of changed files
3. Click the **+** icon next to each file you want to add
   - Or click **+** next to "Changes" to add all files

#### **Step 4: Commit Your Changes**
1. At the top of Source Control panel, there's a text box
2. Type a commit message like:
   ```
   Add class selection system with 4 playable classes
   
   - Added PlayerClass, ClassSelectionUI, GameManager scripts
   - Created ClassSelection scene with UI menu
   - Implemented Warrior, Ranger, Mage, Cleric classes
   - Added special abilities and combat system
   ```
3. Click the **✓ Commit** button (or Ctrl+Enter)

#### **Step 5: Push to GitHub**
1. Click the **...** (three dots) at top of Source Control panel
2. Select **Push** (or **Sync Changes** if you see that)
3. If prompted, select your branch (probably `DungeonGeneratorWithProcGen`)
4. Wait for upload to complete

#### **Step 6: Verify on GitHub**
1. Go to your GitHub repository in a browser
2. Check that your files are there
3. Your teammates can now pull!

---

### **OPTION 2: Using Git Command Line (PowerShell)**

#### **Step 1: Open PowerShell in Your Project Folder**
1. In VS Code: **Terminal → New Terminal**
2. Or navigate to your project folder in PowerShell

#### **Step 2: Check Current Status**
```powershell
git status
```
This shows all changed files (red = not staged, green = staged)

#### **Step 3: Stage Your Changes**

**Option A: Add specific files**
```powershell
# Add scripts
git add Assets/Scripts/PlayerClass.cs
git add Assets/Scripts/ClassSelectionUI.cs
git add Assets/Scripts/GameManager.cs
git add Assets/Scripts/PlayerProjectile.cs
git add Assets/Scripts/ClassSwitcherDebug.cs

# Add scenes
git add Assets/Scenes/ClassSelection.unity
git add Assets/Scenes/ClassSelection.unity.meta

# Add prefabs (if you made them)
git add Assets/Prefabs/*.prefab
git add Assets/Prefabs/*.prefab.meta

# Add documentation
git add *.md
```

**Option B: Add all changes** (use with caution!)
```powershell
git add .
```

**Option C: Add by folder**
```powershell
git add Assets/Scripts/
git add Assets/Scenes/ClassSelection.unity*
```

#### **Step 4: Commit Your Changes**
```powershell
git commit -m "Add class selection system with 4 playable classes

- Added PlayerClass, ClassSelectionUI, GameManager scripts
- Created ClassSelection scene with UI menu
- Implemented Warrior, Ranger, Mage, Cleric classes
- Added special abilities and combat system
- Updated controls: Left Click = attack, Right Click = special
"
```

#### **Step 5: Push to GitHub**
```powershell
git push origin DungeonGeneratorWithProcGen
```

If you get an error about upstream, use:
```powershell
git push --set-upstream origin DungeonGeneratorWithProcGen
```

#### **Step 6: Verify**
```powershell
git log --oneline -5
```
This shows your recent commits.

---

## 🔍 Important: Unity .meta Files

Unity creates `.meta` files for every asset. **You MUST push these too!**

### **Why .meta files matter:**
- They contain GUIDs (unique IDs) for assets
- Without them, references break (scripts won't connect to GameObjects)
- Your teammates will get "Missing Script" errors

### **How to include .meta files:**
When you add a file, Git should automatically include its `.meta` file if your `.gitignore` is set up correctly.

**Example:**
```powershell
git add Assets/Scripts/PlayerClass.cs
git add Assets/Scripts/PlayerClass.cs.meta  # Don't forget this!
```

**Or use wildcards:**
```powershell
git add Assets/Scripts/PlayerClass.*
```

---

## 📦 What Files to EXCLUDE (Check Your .gitignore)

Your `.gitignore` should already exclude these Unity folders:

```
/Library/          # Unity cache - DO NOT PUSH
/Temp/             # Temporary files - DO NOT PUSH
/Obj/              # Build files - DO NOT PUSH
/Build/            # Build output - DO NOT PUSH
/Builds/           # Build output - DO NOT PUSH
*.csproj           # Auto-generated by Unity
*.unityproj        # Auto-generated by Unity
*.sln              # Auto-generated by Unity
*.suo              # Visual Studio files
*.user             # User-specific files
```

**DO PUSH:**
- `Assets/` folder (all your content)
- `ProjectSettings/` folder (Unity settings)
- `Packages/` folder (package manifest)

---

## 👥 For Your Teammates to Pull

After you push, your teammates should:

### **Step 1: Pull Your Changes**

**In VS Code:**
1. Source Control panel → **...** → **Pull**

**In PowerShell:**
```powershell
git pull origin DungeonGeneratorWithProcGen
```

### **Step 2: Open Unity**
1. Unity should detect new files automatically
2. Wait for it to reimport assets (progress bar at bottom)
3. If you see "Missing Script" errors, they need to pull the `.meta` files

### **Step 3: Verify Setup**
They should check:
- All 5 scripts are in `Assets/Scripts/`
- `ClassSelection` scene exists in `Assets/Scenes/`
- UI elements are connected (no pink "missing" errors)
- GameManager has all class data filled in

---

## 🎮 Scene Setup for Teammates

Your teammates will need to set up the ClassSelection scene in their Unity:

### **1. Add Scenes to Build Profiles**
1. File → Build Profiles
2. Select their build profile
3. Add scenes:
   - `ClassSelection` (index 0)
   - `MainGameScene` (index 1)

### **2. Check ClassSelectionManager GameObject**
In `ClassSelection` scene:
- Should have `ClassSelectionUI` script
- All buttons should be connected
- All text fields should be connected
- "Game Scene Name" field = "MainGameScene" (or correct name)

### **3. Check Player GameObject**
In `MainGameScene`:
- Should have `PlayerClass` script (added by GameManager)
- `GameManager` should exist in scene
- GameManager should have all class data configured

---

## 🛠️ Handling Merge Conflicts

If multiple people edited the same scene:

### **Scene Conflicts (Most Common)**
Unity scenes are text files but hard to merge manually.

**Best Practice:**
1. Communicate before editing scenes
2. One person works on ClassSelection, another on MainGameScene
3. If conflict happens:
   ```powershell
   # Keep your version
   git checkout --ours Assets/Scenes/ClassSelection.unity
   
   # Or keep their version
   git checkout --theirs Assets/Scenes/ClassSelection.unity
   
   # Then manually re-add your changes in Unity
   ```

### **Script Conflicts (Easier)**
If someone else edited `PlayerClass.cs`:
1. VS Code will show conflict markers
2. Choose which version to keep
3. Or manually merge both changes
4. Save and commit

---

## 📝 Recommended Commit Message Format

Use clear, descriptive commit messages:

```
Add class selection system

Changes:
- Created ClassSelection scene with UI menu
- Added 4 playable classes (Warrior, Ranger, Mage, Cleric)
- Implemented special abilities for each class
- Updated controls to Right Click for specials
- Added enemy respawn system

Scripts Added:
- PlayerClass.cs
- ClassSelectionUI.cs
- GameManager.cs
- PlayerProjectile.cs
- ClassSwitcherDebug.cs

Scenes Modified:
- Added ClassSelection.unity
- Updated MainGameScene.unity with GameManager
```

---

## 🔄 Recommended Workflow

### **Before You Start Working:**
```powershell
git pull origin DungeonGeneratorWithProcGen
```
Get latest changes from teammates.

### **While You Work:**
Save often in Unity (Ctrl+S).

### **When You're Done:**
```powershell
git status                    # Check what changed
git add Assets/Scripts/*      # Stage your changes
git commit -m "Your message"  # Commit with message
git pull origin DungeonGeneratorWithProcGen  # Get any new changes
# Fix conflicts if any
git push origin DungeonGeneratorWithProcGen  # Push to GitHub
```

### **Communication is Key:**
Tell your team in Discord/Slack:
> "Hey team! I just pushed the class selection system to GitHub. Pull the latest changes and add ClassSelection + MainGameScene to your Build Profiles. Let me know if you get any errors!"

---

## 🐛 Common Issues & Solutions

### **Issue 1: "Missing Script" Errors**
**Cause:** Teammates didn't pull `.meta` files
**Solution:**
```powershell
git pull origin DungeonGeneratorWithProcGen
# Make sure to pull ALL files, including .meta
```

### **Issue 2: "Scene Not Found" Errors**
**Cause:** Scene not in Build Profiles
**Solution:** File → Build Profiles → Add scenes

### **Issue 3: "References Not Connected"**
**Cause:** Scene references were lost during merge
**Solution:** Manually reconnect in Inspector (drag GameObjects to script fields)

### **Issue 4: "Merge Conflict in Scene"**
**Cause:** Two people edited same scene
**Solution:**
```powershell
# Use one version, then manually add changes
git checkout --theirs Assets/Scenes/ClassSelection.unity
git add Assets/Scenes/ClassSelection.unity
git commit -m "Resolved scene conflict"
```

### **Issue 5: Large Files Won't Push**
**Cause:** Trying to push files in Library/ or Temp/
**Solution:** Check `.gitignore` excludes those folders

---

## ✅ Checklist Before Pushing

- [ ] Saved all scenes in Unity
- [ ] Saved Unity project (File → Save Project)
- [ ] All scripts compile without errors
- [ ] Tested class selection menu works
- [ ] Tested all 4 classes work in game
- [ ] Staged all relevant files (scripts, scenes, prefabs, .meta files)
- [ ] Written clear commit message
- [ ] Pulled latest changes first (to avoid conflicts)
- [ ] Pushed to correct branch
- [ ] Verified files on GitHub website
- [ ] Notified teammates about changes

---

## 📧 Message Template for Your Team

Copy/paste this to your team chat:

```
Hey team! 👋

I just pushed the class selection system to GitHub. Here's what's new:

🎮 Features Added:
- Class selection menu with 4 playable classes
- Warrior, Ranger, Mage, Cleric (each with unique stats)
- Special abilities on Right Click
- Enemy respawn system

📦 What You Need to Do:
1. Pull latest changes from 'DungeonGeneratorWithProcGen' branch
2. Open Unity and wait for reimport
3. Go to File → Build Profiles → Add these scenes:
   - ClassSelection (set as index 0)
   - MainGameScene (set as index 1)
4. Test it out!

📝 Files Changed:
- Added 5 new scripts in Assets/Scripts/
- Added ClassSelection.unity scene
- Updated MainGameScene with GameManager

🐛 If You Get Errors:
- "Missing Script" → Make sure you pulled .meta files
- "Scene not found" → Add scenes to Build Profiles
- Check the documentation files (*.md) I added for setup help

Let me know if you run into issues! 🎯
```

---

## 🎯 Quick Command Reference

```powershell
# Check status
git status

# Stage all changes in Assets/Scripts/
git add Assets/Scripts/

# Stage specific file
git add Assets/Scenes/ClassSelection.unity

# Commit with message
git commit -m "Add class selection system"

# Pull before pushing (avoid conflicts)
git pull origin DungeonGeneratorWithProcGen

# Push to GitHub
git push origin DungeonGeneratorWithProcGen

# See recent commits
git log --oneline -5

# Undo last commit (if you made a mistake)
git reset --soft HEAD~1
```

---

That's everything! Follow these steps and your teammates will have your class selector system working perfectly. Good luck! 🚀✨
