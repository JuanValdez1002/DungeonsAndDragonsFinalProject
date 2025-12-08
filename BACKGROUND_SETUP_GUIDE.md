# 🎨 Quick Background Setup Guide

## Your Background is Ready!

**File Location:** `Assets/UI/Backgrounds/ClassSelectionBackground.jpg`

---

## 🚀 Two Ways to Apply It:

### **Option 1: Automatic Setup (Recommended)**

1. **Open Unity**
2. **In the top menu, click:** `Tools` → `Class Selection` → `Setup Background`
3. **Done!** ✅ The background will be automatically applied

This will:
- Create a Background Image on your Canvas
- Load your ClassSelectionBackground.jpg
- Stretch it to fill the entire screen
- Connect it to the ClassSelectionUI script

---

### **Option 2: Manual Setup**

1. **Open the ClassSelection scene** (`Assets/Scenes/ClassSelection.unity`)

2. **In Hierarchy, find or create Canvas**
   - If no Canvas exists: Right-click → UI → Canvas

3. **Create Background Image:**
   - Right-click Canvas → UI → **Image**
   - Rename to "Background"

4. **Apply your background:**
   - Select "Background" in Hierarchy
   - In Inspector, find **Image (Script)** component
   - Click the circle next to **Source Image**
   - Search "ClassSelectionBackground"
   - Select it

5. **Make it fill the screen:**
   - With Background selected, find **Rect Transform** in Inspector
   - Click the **Anchor Presets** square (top-left of Rect Transform)
   - Hold **Alt + Shift** and click the **bottom-right** preset (stretch/stretch)
   - Set all position values to **0** (Left, Top, Right, Bottom)

6. **Move it behind everything:**
   - In Hierarchy, drag "Background" to the **top** of the Canvas children
   - This puts it in the back

7. **Connect to script (optional):**
   - Find your **ClassSelectionManager** object
   - Select it
   - In Inspector, find **ClassSelectionUI** component
   - Drag the **Background** Image into the **Background Image** field

---

## 🎨 Adjusting the Look

### Make it Darker (Better text contrast):
```
In Unity Menu: Tools → Class Selection → Adjust Background Tint
```

Or manually:
- Select Background Image
- In Image component, change **Color** to slightly gray (R: 0.85, G: 0.85, B: 0.85)

### Make it Lighter:
- Change Color to white (R: 1, G: 1, B: 1)

### Add Vignette Effect:
1. Select Background
2. Add Component → UI → **Shadow**
3. Change settings for a dark edge effect

---

## ✅ Verification Checklist

- [ ] Background image appears in Scene view
- [ ] Background fills entire Canvas
- [ ] Background is behind all UI elements
- [ ] Text is readable on the background
- [ ] Image looks crisp (not blurry)

---

## 🐛 Troubleshooting

**Image is blurry?**
- Select `ClassSelectionBackground.jpg` in Project
- In Inspector, set **Max Size** to 2048 or 4096
- Click Apply

**Image won't show?**
- Make sure **Texture Type** is set to "Sprite (2D and UI)"
- Click Apply after changing

**Background is in front of buttons?**
- In Hierarchy, drag Background to the TOP of Canvas children list

**Image is stretched weird?**
- Select Background Image
- In Image component, change **Image Type** to "Simple"
- Or try "Filled" or "Sliced"

---

## 🎮 Next Steps

Once the background is working:
1. Add class icons (put in `Assets/UI/ClassIcons/`)
2. Customize card colors
3. Add button animations
4. Test in Play mode

Enjoy your custom background! 🎨✨
