# Fix MouseLook - Only Looking Up/Down Issue

## The Problem
Your MouseLook script is correctly written but likely has a setup issue in Unity.

## Quick Diagnosis Checklist

### 1. Check Script Assignment
- **MouseLook script should be on the CAMERA, not the Player**
- If it's on the Player, move it to the Camera child object

### 2. Check playerBody Reference
- Select the Camera with MouseLook script
- In Inspector, look for MouseLook component
- **CRITICAL: Drag the Player GameObject into the `playerBody` field**
- If this field is empty, horizontal rotation won't work

### 3. Check Hierarchy Setup
Your setup should look like:
```
Player (has PlayerMovement, CharacterController)
  └── Main Camera (has MouseLook script)
      └── GroundCheck (empty GameObject for ground detection)
```

### 4. Check Input System Settings
- Go to Edit → Project Settings → Player → Other Settings
- Set "Active Input Handling" to **"Both"** or **"Input System Package (New)"**
- The script uses `Mouse.current.delta` which requires new Input System

## Step-by-Step Fix

### Step 1: Verify Script Location
1. Find your Camera GameObject in Hierarchy
2. Select it and check if MouseLook script is attached
3. If not, drag `MyMouseLook.cs` from Assets/Scripts to the Camera

### Step 2: Assign Player Reference
1. With Camera selected, find MouseLook component in Inspector
2. Look for "Player Body" field
3. Drag your Player GameObject from Hierarchy into this field
4. **This is the most common cause of the issue!**

### Step 3: Test Settings
- Mouse Sensitivity: Start with 100 (default)
- Make sure Cursor Lock State is working (cursor should disappear in Play mode)

### Step 4: Verify Input System
1. Go to Window → Package Manager
2. Search for "Input System"
3. Make sure it's installed (should show "Remove" button if installed)

## Alternative Quick Fix
If the above doesn't work, here's a simpler MouseLook setup:

```csharp
// Simple version that definitely works
void Update()
{
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    playerBody.Rotate(Vector3.up * mouseX);
}
```

## Expected Result
After fixing:
- ✅ Mouse up/down = Camera tilts up/down
- ✅ Mouse left/right = Player rotates left/right
- ✅ Movement follows camera direction (WASD relative to where you're looking)

## Most Likely Solution
**Check that `playerBody` field has the Player GameObject assigned!** This is usually the issue - without it, only vertical camera rotation works but horizontal player rotation doesn't happen.