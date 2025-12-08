# 🎨 New Vertical Class Selection Layout

## ✨ What's New:

### **Vertical Card Design**
Each class card is now:
- **240px wide x 500px tall** (vertical layout)
- **Even spacing** across the screen
- **Larger images** (220x220 square)
- **Description text** below each class name
- **Themed colors** with outlines

---

## 🚀 Setup Instructions:

### **Step 1: Clean Old UI (IMPORTANT!)**
Remove the old squares/buttons first:
```
Tools → Class Selection → Clean Old UI Elements
```

### **Step 2: Setup New Vertical Cards**
Create the new design:
```
Tools → Class Selection → Setup Class Images
```

**Done!** ✨

---

## 📐 New Layout:

```
╔════════════════════════════════════════════════════════════╗
║                   DUNGEON BACKGROUND                       ║
║                                                            ║
║   ┌────────┐  ┌────────┐  ┌────────┐  ┌────────┐        ║
║   │        │  │        │  │        │  │        │        ║
║   │ [IMG]  │  │ [IMG]  │  │ [IMG]  │  │ [IMG]  │        ║
║   │        │  │        │  │        │  │        │        ║
║   │        │  │        │  │        │  │        │        ║
║   │Warrior │  │Ranger  │  │ Mage   │  │Cleric  │        ║
║   │        │  │        │  │        │  │        │        ║
║   │A mighty│  │A skilled│  │Powerful│  │A holy  │        ║
║   │fighter │  │archer  │  │caster  │  │warrior │        ║
║   │        │  │        │  │        │  │        │        ║
║   │[SELECT]│  │[SELECT]│  │[SELECT]│  │[SELECT]│        ║
║   └────────┘  └────────┘  └────────┘  └────────┘        ║
║                                                            ║
╚════════════════════════════════════════════════════════════╝
```

---

## 🎨 Card Features:

### **Each Card Contains:**

1. **Class Image** (top, 220x220)
   - Square format
   - Preserves aspect ratio
   - Your uploaded images

2. **Class Name** (bold, 28px)
   - Warrior, Ranger, Mage, Cleric
   - White text

3. **Description** (14px, wrapped)
   - 2 lines of flavor text
   - Light gray color
   - Auto-wraps text

4. **Select Button** (bottom, themed)
   - Color matches class theme
   - Hover effects
   - Animation on click

---

## 🌈 Color Themes:

- 🔴 **Warrior**: Red outline & red button
- 🟢 **Ranger**: Green outline & green button  
- 🟣 **Mage**: Purple outline & purple button
- 🟡 **Cleric**: Yellow/Gold outline & button

---

## 📏 Spacing:

Cards are evenly spaced horizontally:
- Warrior: -400px from center
- Ranger: -133px from center
- Mage: +133px from center
- Cleric: +400px from center

Perfect distribution across a 1920px wide screen!

---

## 🔧 Customization:

### **Want to adjust spacing?**
Edit `ClassImagesSetup.cs`, line ~74:
```csharp
new Vector2(-400, 50)  // Change first number for horizontal position
```

### **Want bigger/smaller images?**
Edit line ~224:
```csharp
imageRect.sizeDelta = new Vector2(220, 220); // Change both numbers
```

### **Want different descriptions?**
Edit lines 74-86 with your own text!

---

## ✅ What You'll See:

- ✨ Clean vertical cards
- 🖼️ Large class images
- 📝 Descriptions explaining each class
- 🎨 Color-coded themes
- 🎮 Hover and click animations
- 🌟 Professional AAA-game look!

---

## 🐛 Troubleshooting:

**Old UI still visible?**
- Run: Tools → Class Selection → Clean Old UI Elements
- Manually delete old objects in Hierarchy

**Images too small/big?**
- Adjust `sizeDelta` values in the script
- Or manually resize in Inspector

**Text not showing?**
- Script automatically uses TMP or legacy Text
- Check that font assets exist

**Cards overlapping?**
- Adjust X positions in CreateVerticalClassCard calls
- Spacing: -400, -133, 133, 400

---

Enjoy your beautiful new class selection screen! 🎮✨
