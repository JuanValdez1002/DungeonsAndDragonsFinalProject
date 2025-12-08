# Class Selection Visual Upgrade Guide

## 🎨 Enhanced Features Added

Your class selection screen now includes:
- ✨ **Hover effects** - Cards glow when you mouse over them
- 🎯 **Selection highlighting** - Selected class gets a golden glow
- 📱 **Button animations** - Smooth scale effects on buttons
- 🎮 **Professional look** - Card-based layout like AAA games

---

## 📦 FREE Assets You Need (No Copyright Issues!)

### 1. **Dungeon Background Image**

**Option A: OpenGameArt.org (Best for 2D)**
- Visit: https://opengameart.org/
- Search: "dungeon background" or "stone wall texture"
- License: CC0 (Public Domain) or CC-BY (give credit)
- Recommended sizes: 1920x1080 or 2560x1440

**Option B: Pixabay (Free Stock Photos)**
- Visit: https://pixabay.com/
- Search: "stone wall", "dungeon", "castle interior"
- License: Free for commercial use, no attribution required
- Download high resolution

**Option C: Create with AI (Free)**
- Visit: https://www.bing.com/images/create (Microsoft Designer)
- Prompt: "dark medieval dungeon stone wall background, torches, atmospheric"
- License: Free to use (Microsoft terms)

**Quick Recommendation:**
```
Search "dungeon stone wall" on Pixabay
Download any dark, atmospheric stone texture
Size: 1920x1080 minimum
```

---

### 2. **Class Icons/Images**

**Option A: Game-Icons.net (BEST FOR D&D)**
- Visit: https://game-icons.net/
- License: CC-BY 3.0 (just credit "game-icons.net")
- Pre-made fantasy icons!

**Recommended icons:**
- **Warrior**: Search "sword shield", "knight helm", "battle axe"
- **Ranger**: Search "bow arrow", "quiver", "archer"
- **Mage**: Search "wizard hat", "magic staff", "fireball"
- **Cleric**: Search "holy symbol", "prayer", "healing"

**How to download:**
1. Search for icon
2. Click icon you like
3. Select size: 512x512 or 1024x1024
4. Download as PNG
5. Pick white or colored version

**Option B: Flaticon (More Stylized)**
- Visit: https://www.flaticon.com/
- Search same terms as above
- Free account required
- Download PNG format

**Option C: Kenny.nl Assets**
- Visit: https://kenney.nl/
- Look for "RPG" or "UI" packs
- License: CC0 (completely free)

---

## 🛠️ Unity Setup Instructions

### Step 1: Import Your Assets

1. **Create folders in Unity:**
   ```
   Assets/
   ├── UI/
   │   ├── Backgrounds/
   │   ├── ClassIcons/
   │   └── Sprites/
   ```

2. **Import your downloaded images:**
   - Drag dungeon background into `Assets/UI/Backgrounds/`
   - Drag class icons into `Assets/UI/ClassIcons/`

3. **Configure images as Sprites:**
   - Select each image in Project window
   - In Inspector, set **Texture Type** to `Sprite (2D and UI)`
   - Click **Apply**

---

### Step 2: Setup Class Selection Scene UI

#### A. Background Setup

1. Open `ClassSelection` scene
2. Find or create **Canvas** object (if not exists: Right-click Hierarchy → UI → Canvas)
3. Add background:
   - Right-click Canvas → UI → **Image**
   - Rename to "Background"
   - In Inspector:
     - **Source Image**: Drag your dungeon background sprite
     - **Color**: Tint slightly darker if needed (RGB: 0.8, 0.8, 0.8)
     - Set to stretch full screen:
       - **Anchor**: Click square icon → Hold Alt+Shift → Click bottom-right (stretch all)
       - **Left/Right/Top/Bottom**: Set all to 0

#### B. Create Title

1. Right-click Canvas → UI → **Text - TextMeshPro** (or legacy Text)
2. Rename to "Title"
3. Settings:
   - **Text**: "Choose Your Class"
   - **Font Size**: 72
   - **Alignment**: Center
   - **Color**: Gold/White (RGB: 1, 0.9, 0.5)
   - **Position**: Top center of screen
   - Add **Outline** effect:
     - Component → UI → Effects → **Outline**
     - Color: Black, Distance: 3

#### C. Create Class Cards (4 Cards)

For each class, create a card:

1. **Create Panel:**
   - Right-click Canvas → UI → **Panel**
   - Rename: "WarriorCard" (or Ranger/Mage/Cleric)
   - Size: Width 300, Height 400

2. **Add Class Icon:**
   - Right-click Panel → UI → **Image**
   - Rename: "ClassIcon"
   - **Source Image**: Your warrior icon sprite
   - Size: 200x200
   - Position: Top of card

3. **Add Class Name:**
   - Right-click Panel → UI → **Text - TextMeshPro**
   - **Text**: "Warrior"
   - **Font Size**: 36
   - **Alignment**: Center
   - Position: Below icon

4. **Add Button:**
   - Right-click Panel → UI → **Button - TextMeshPro**
   - Rename: "SelectButton"
   - **Text**: "Select"
   - Size: Width 200, Height 50
   - Position: Bottom of card
   - Add **UIButtonAnimator** script (Component → Add Component → search "UIButtonAnimator")

5. **Repeat for all 4 classes**

#### D. Layout Cards

Arrange your 4 cards in a nice grid:
- **Option 1**: 2x2 grid (2 top, 2 bottom)
- **Option 2**: 1x4 row (all in a line)
- **Option 3**: Diamond/cross pattern

**Quick Grid Layout:**
1. Create empty GameObject: "CardContainer"
2. Add Component: **Grid Layout Group**
   - Cell Size: 300 x 400
   - Spacing: 20 x 20
   - Child Alignment: Middle Center
3. Drag all 4 cards into CardContainer

#### E. Create Info Panel (Stats Display)

1. Create Panel: "InfoPanel"
   - Position: Bottom or right side
   - Size: Width 600, Height 300
   - Add slight transparency: Color alpha 0.8

2. Add Text fields inside:
   - "ClassName" (large, bold)
   - "Description" (smaller, wrapping)
   - "Health", "Damage", "Speed", "Special"

---

### Step 3: Wire Up the Script

1. **Find ClassSelectionManager** in your scene (or create it)
2. **Add/Update ClassSelectionUI** script
3. **Assign all fields in Inspector:**

   ```
   Background Image: Drag Background Image component
   
   Class Card Panels:
   - Warrior Card: Drag WarriorCard Image component
   - Ranger Card: Drag RangerCard Image component
   - Mage Card: Drag MageCard Image component
   - Cleric Card: Drag ClericCard Image component
   
   Class Buttons:
   - Warrior Button: Drag WarriorCard SelectButton
   - Ranger Button: Drag RangerCard SelectButton
   - Mage Button: Drag MageCard SelectButton
   - Cleric Button: Drag ClericCard SelectButton
   
   Text Fields:
   - Assign all TMP text fields from InfoPanel
   
   Visual Effects (leave defaults or customize):
   - Normal Card Color: (1, 1, 1, 0.7) - slightly transparent
   - Selected Card Color: (1, 0.85, 0.4, 1) - golden
   - Hover Card Color: (1, 1, 1, 0.9) - brighter
   ```

---

### Step 4: Add Button Animations (Optional)

For extra polish, add `UIButtonAnimator` to ALL buttons:

1. Select a button in Hierarchy
2. Component → Add Component → **UIButtonAnimator**
3. Settings:
   - Hover Scale: 1.1
   - Click Scale: 0.95
   - Animation Speed: 10

Repeat for Start button and all class select buttons!

---

## 🎨 Color Schemes (Copy-Paste Ready!)

### Warrior Theme
- **Card Color**: Red tint (1, 0.8, 0.8)
- **Text Color**: (1, 0.2, 0.2)

### Ranger Theme
- **Card Color**: Green tint (0.8, 1, 0.8)
- **Text Color**: (0.2, 0.8, 0.2)

### Mage Theme
- **Card Color**: Blue/Purple tint (0.9, 0.8, 1)
- **Text Color**: (0.5, 0.3, 1)

### Cleric Theme
- **Card Color**: Yellow/Gold tint (1, 1, 0.8)
- **Text Color**: (1, 0.9, 0.3)

---

## 🎯 Quick Start Checklist

- [ ] Download dungeon background from Pixabay
- [ ] Download 4 class icons from game-icons.net
- [ ] Import into Unity as Sprites
- [ ] Open ClassSelection scene
- [ ] Add background image to Canvas
- [ ] Create 4 class cards with icons
- [ ] Add UIButtonAnimator to buttons
- [ ] Wire up ClassSelectionUI script
- [ ] Test hover effects and selection
- [ ] Adjust colors to your preference

---

## 📸 Example Resource Links

**Dungeon Backgrounds (Free):**
- https://opengameart.org/content/dungeon-tileset-0
- https://pixabay.com/images/search/stone%20wall/
- https://kenney.nl/assets/ui-pack-rpg-expansion

**Class Icons (Free):**
- https://game-icons.net/ (BEST!)
- https://www.flaticon.com/packs/rpg-32
- https://kenney.nl/assets/game-icons

**Attribution (if using CC-BY):**
Create a Credits.txt file:
```
Class Icons: game-icons.net (CC-BY 3.0)
Background: [Artist name if required]
```

---

## 🚀 Advanced Enhancements (Optional)

### Add Particle Effects
1. GameObject → Effects → **Particle System**
2. Position near selected card
3. Make particles match class theme (fire for mage, etc.)

### Add Sound Effects
1. Download free sounds from https://freesound.org/
2. Search: "button click", "sword unsheath", "magic"
3. Drag into UIButtonAnimator's sound fields

### Add Fade-In Animation
1. Select Canvas
2. Component → **Canvas Group**
3. Create script to fade alpha from 0 to 1 on start

### Add Class Preview 3D Models
1. Import free character models from Mixamo
2. Create rotating preview for each class
3. Show when class is selected

---

## 🐛 Troubleshooting

**Cards don't highlight?**
- Make sure you assigned the Image components (not GameObject)
- Check that Image components have a sprite assigned

**Hover doesn't work?**
- Ensure Canvas has "Graphic Raycaster" component
- Check EventSystem exists in scene

**Images look blurry?**
- Select sprite in Project
- Set "Filter Mode" to Bilinear or Point (no filter)
- Increase "Max Size" to 2048

**Attribution needed?**
- If using game-icons.net: Add "Icons from game-icons.net" in credits
- Pixabay: No attribution required
- Kenny.nl: No attribution required (but appreciated)

---

## 📝 Summary

You now have:
1. ✅ Enhanced ClassSelectionUI with hover and selection effects
2. ✅ UIButtonAnimator for smooth button animations
3. ✅ Free resource links for backgrounds and icons
4. ✅ Step-by-step Unity setup guide

**Next Steps:**
1. Download your favorite background and icons
2. Follow the Unity setup instructions
3. Customize colors to match your game's theme
4. Test and enjoy your professional-looking class selection screen!

Happy game dev! 🎮✨
