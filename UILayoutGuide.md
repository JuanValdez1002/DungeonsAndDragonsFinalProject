# Class Selection UI Layout Guide

## Scene Layout

```
ClassSelection Scene
│
└── Canvas
    ├── Background Panel (full screen, dark semi-transparent)
    │
    ├── Title Text (top center)
    │   └── "CHOOSE YOUR CLASS"
    │
    ├── Class Buttons Panel (middle, horizontal)
    │   ├── Warrior Button (red)
    │   ├── Ranger Button (green)
    │   ├── Mage Button (blue)
    │   └── Cleric Button (yellow)
    │
    ├── Info Panel (bottom half)
    │   ├── Class Name Text (large, centered)
    │   ├── Description Text (paragraph)
    │   ├── Stats Panel (vertical list)
    │   │   ├── Health Display
    │   │   ├── Damage Display
    │   │   ├── Speed Display
    │   │   └── Special Ability Display
    │   │
    │   └── Start Game Button (large, green, bottom)
    │
    └── ClassSelectionManager (empty GameObject with script)
```

## Layout Positions (Anchor Presets)

### Title Text
- Anchor: Top Center
- Position: (0, -50, 0)
- Size: 600x100
- Font Size: 48
- Alignment: Center
- Color: White

### Class Buttons (Horizontal Layout)
- Spacing: 20 pixels
- Position: Middle of screen
- Each button: 200x200 pixels

**Button Layout:**
```
[Warrior] [Ranger] [Mage] [Cleric]
   🛡️       🏹       🔮       ✨
```

### Class Button Settings
Each button:
- Width: 200
- Height: 200
- Text Font Size: 24
- Border: 3px
- Hover effect: Scale to 1.1
- Click effect: Scale to 0.95

**Colors:**
- Warrior: RGB(200, 50, 50) Red
- Ranger: RGB(50, 200, 50) Green
- Mage: RGB(50, 50, 200) Blue
- Cleric: RGB(200, 200, 50) Yellow

### Info Panel
- Anchor: Bottom Center
- Width: 800
- Height: 400
- Background: Semi-transparent black
- Padding: 20px

### Class Name Display
- Font Size: 36
- Alignment: Center
- Position: Top of info panel
- Color: Changes based on selected class

### Description Text
- Font Size: 18
- Alignment: Left/Center
- Word Wrap: On
- Width: 700
- Color: Light gray

### Stats Display
```
Health: XXX
Damage: XXX (Type)
Speed: XXX
Special: XXX
```
- Font Size: 20
- Alignment: Left
- Line spacing: 10px
- Color: White

### Start Game Button
- Anchor: Bottom Center
- Width: 300
- Height: 80
- Font Size: 28
- Color: Green
- Position: Bottom of info panel

## Example Hierarchy in Unity

```
Canvas (Screen Space - Overlay)
├── BackgroundPanel
│   └── Image (Color: Black, Alpha: 0.8)
│
├── TitleText
│   └── Text: "CHOOSE YOUR CLASS"
│
├── ButtonsPanel (Horizontal Layout Group)
│   ├── WarriorButton
│   │   └── Text: "WARRIOR"
│   ├── RangerButton
│   │   └── Text: "RANGER"
│   ├── MageButton
│   │   └── Text: "MAGE"
│   └── ClericButton
│       └── Text: "CLERIC"
│
├── InfoPanel
│   ├── ClassNameText
│   │   └── Text: "Warrior"
│   ├── DescriptionText
│   │   └── Text: "A mighty melee fighter..."
│   ├── StatsPanel (Vertical Layout Group)
│   │   ├── HealthText
│   │   ├── DamageText
│   │   ├── SpeedText
│   │   └── SpecialText
│   └── StartButton
│       └── Text: "START GAME"
│
└── ClassSelectionManager (Empty GameObject)
    └── ClassSelectionUI Script
```

## Script Assignment

**ClassSelectionManager GameObject:**
- Add ClassSelectionUI script
- Drag ALL UI elements into inspector fields:
  - Selection Panel → InfoPanel
  - Title Text → TitleText
  - Description Text → DescriptionText
  - Warrior Button → WarriorButton
  - Ranger Button → RangerButton
  - Mage Button → MageButton
  - Cleric Button → ClericButton
  - Start Button → StartButton
  - Class Name Display → ClassNameText
  - Health Display → HealthText
  - Damage Display → DamageText
  - Speed Display → SpeedText
  - Special Ability Display → SpecialText
  - Game Scene Name → "Dungeon" (or your scene name)

## Colors for Copy-Paste

### Warrior (Red)
- Normal: #C83232
- Highlighted: #E64545
- Pressed: #A02828

### Ranger (Green)
- Normal: #32C832
- Highlighted: #45E645
- Pressed: #28A028

### Mage (Blue)
- Normal: #3232C8
- Highlighted: #4545E6
- Pressed: #2828A0

### Cleric (Yellow)
- Normal: #C8C832
- Highlighted: #E6E645
- Pressed: #A0A028

## Font Recommendations
- Title: Bold, 48pt
- Buttons: Bold, 24pt
- Class Name: Bold, 36pt
- Description: Regular, 18pt
- Stats: Regular, 20pt
- Start Button: Bold, 28pt

## Simple Version (If Short on Time)

Minimum viable UI:
```
1. Canvas
2. 4 Buttons (one for each class)
3. 1 Text field (shows class name)
4. 1 Start button
5. ClassSelectionUI script
```

Wire up:
- 4 class buttons → ClassSelectionUI
- Start button → ClassSelectionUI
- Set game scene name
- Done!

## Testing the UI

1. Create the UI elements
2. Add ClassSelectionUI script
3. Wire up all references
4. Press Play
5. Click each class button
6. Info should update
7. Click Start Game
8. Should load game scene

## Tips

- Use **Vertical/Horizontal Layout Groups** for easy arrangement
- Use **Content Size Fitter** for dynamic text sizing
- Use **Anchor Presets** (alt+shift+click) for responsive design
- Test in different resolutions
- Add hover animations using Button → Transition: Animation

## Alternative: Simple Button List

If horizontal layout is too complex:
```
[  WARRIOR  ]
[  RANGER   ]
[   MAGE    ]
[  CLERIC   ]

[Class Info Display]

[ START GAME ]
```

Vertical layout is simpler but less visually appealing.

## Polish Ideas (Optional)

- Class icons/symbols on buttons
- Animated character preview
- Background image (dungeon theme)
- Button hover sound effects
- Selection glow effect
- Class background images
- Stat bars (visual HP/damage indicators)
- Tooltips on hover

## Keyboard Shortcuts in Editor

- Ctrl+Shift+N: New GameObject
- Alt+Shift+Click anchor preset: Set position too
- Ctrl+D: Duplicate
- F: Frame selected
- Delete: Remove selected

Remember: Start simple, test it works, then add polish!