using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Editor utility to setup class images in the ClassSelection scene
/// </summary>
public class ClassImagesSetup
{
    [MenuItem("Tools/Class Selection/Setup Class Images")]
    public static void SetupClassImages()
    {
        Debug.Log("🎨 Setting up class images...");
        
        // Refresh asset database
        AssetDatabase.Refresh();
        
        // Find and configure all class images (try both with and without .jpg extension)
        ConfigureClassImage("Assets/UI/ClassImages/WarriorNewImg.jpg"); // NEW warrior image
        ConfigureClassImage("Assets/UI/ClassImages/WarriorImg.jpg");
        ConfigureClassImage("Assets/UI/ClassImages/WarriorImg"); // fallback
        ConfigureClassImage("Assets/UI/ClassImages/RangerImg.jpg");
        ConfigureClassImage("Assets/UI/ClassImages/MageImg.jpg"); // NEW mage image (jpg instead of webp)
        ConfigureClassImage("Assets/UI/ClassImages/MageImg.webp"); // old version
        ConfigureClassImage("Assets/UI/ClassImages/ClericImg.jpg");
        
        Debug.Log("✅ Class images configured as sprites!");
        
        // Now try to apply them to the scene
        ApplyImagesToScene();
    }
    
    private static void ConfigureClassImage(string path)
    {
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            Debug.Log($"Configuring: {path}");
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.maxTextureSize = 2048;
            importer.textureCompression = TextureImporterCompression.Compressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.SaveAndReimport();
        }
        else
        {
            Debug.LogWarning($"Could not find importer for: {path}");
        }
    }
    
    private static void ApplyImagesToScene()
    {
        // Find the Canvas
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("⚠️ No Canvas found. Open ClassSelection scene first!");
            return;
        }
        
        // First, delete old UI elements if they exist
        CleanupOldUI(canvas);
        
        // Load the sprites - try multiple paths
        // Using NEW warrior and mage images
        Sprite warriorSprite = LoadSprite("Assets/UI/ClassImages/WarriorNewImg.jpg") ?? 
                               LoadSprite("Assets/UI/ClassImages/WarriorImg.jpg") ?? 
                               LoadSprite("Assets/UI/ClassImages/WarriorImg");
        Sprite rangerSprite = LoadSprite("Assets/UI/ClassImages/RangerImg.jpg");
        Sprite mageSprite = LoadSprite("Assets/UI/ClassImages/MageImg.jpg") ?? 
                            LoadSprite("Assets/UI/ClassImages/MageImg.webp");
        Sprite clericSprite = LoadSprite("Assets/UI/ClassImages/ClericImg.jpg");
        
        // Create vertical class cards with better spacing - 80% of screen width
        // Assuming 1920px screen, 80% = 1536px, divided by 4 cards = 384px each
        // Card width 300px, spacing calculated for even distribution
        float spacing = 384f; // Distance between card centers
        
        // Using darker, more subtle colors that won't get too bright
        CreateVerticalClassCard(canvas, "WarriorCard", warriorSprite, "Warrior", 
            "A mighty melee fighter with high defense.\nSpecializes in close combat.",
            new Vector2(-spacing * 1.5f, 0), new Color(0.6f, 0.3f, 0.3f)); // Dark red
            
        CreateVerticalClassCard(canvas, "RangerCard", rangerSprite, "Ranger", 
            "A skilled archer with high damage.\nAttacks from distance with precision.",
            new Vector2(-spacing * 0.5f, 0), new Color(0.3f, 0.6f, 0.3f)); // Dark green
            
        CreateVerticalClassCard(canvas, "MageCard", mageSprite, "Mage", 
            "A powerful spellcaster with magic.\nDeals massive area damage.",
            new Vector2(spacing * 0.5f, 0), new Color(0.4f, 0.3f, 0.7f)); // Dark purple
            
        CreateVerticalClassCard(canvas, "ClericCard", clericSprite, "Cleric", 
            "A holy warrior who heals allies.\nBalanced combat and support.",
            new Vector2(spacing * 1.5f, 0), new Color(0.7f, 0.6f, 0.3f)); // Dark gold
        
        // Update ClassSelectionUI references
        UpdateClassSelectionUI(canvas);
        
        Debug.Log("✅ Vertical class cards created!");
        
        // Mark scene as dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
    
    private static void CleanupOldUI(Canvas canvas)
    {
        // Remove old cards if they exist
        string[] oldCards = new string[] { "WarriorCard", "RangerCard", "MageCard", "ClericCard" };
        foreach (string cardName in oldCards)
        {
            Transform oldCard = canvas.transform.Find(cardName);
            if (oldCard != null)
            {
                GameObject.DestroyImmediate(oldCard.gameObject);
                Debug.Log($"Removed old {cardName}");
            }
        }
    }
    
    private static Sprite LoadSprite(string path)
    {
        // First try direct load
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite != null)
        {
            Debug.Log($"✅ Loaded sprite: {path}");
            return sprite;
        }
        
        // Try as Texture2D instead
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (texture != null)
        {
            Debug.Log($"⚠️ Found texture (not sprite): {path} - Converting...");
            
            // Convert to sprite
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.SaveAndReimport();
                AssetDatabase.Refresh();
                
                // Try loading again
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (sprite != null)
                {
                    Debug.Log($"✅ Converted and loaded: {path}");
                    return sprite;
                }
            }
        }
        
        Debug.LogWarning($"⚠️ Could not load sprite: {path}");
        return null;
    }
    
    private static void CreateVerticalClassCard(Canvas canvas, string cardName, Sprite classSprite, string className, string description, Vector2 position, Color tint)
    {
        // Create card panel
        GameObject cardObj = new GameObject(cardName);
        cardObj.transform.SetParent(canvas.transform);
        
        // Setup RectTransform - VERTICAL layout (taller than wide)
        // 80% of 1920px = 1536px, divided by 4 = 384px per card space
        // Card itself is 340px wide (leaving 44px gap between cards)
        RectTransform cardRect = cardObj.AddComponent<RectTransform>();
        cardRect.anchoredPosition = position;
        cardRect.sizeDelta = new Vector2(340, 600); // Larger cards: width 340, height 600
        cardRect.localScale = Vector3.one;
        
        // Add Panel background
        Image cardBg = cardObj.AddComponent<Image>();
        cardBg.color = new Color(0.1f, 0.1f, 0.1f, 0.85f); // Dark semi-transparent background
        
        // Add subtle outline for visual appeal (darker tint)
        Outline outline = cardObj.AddComponent<Outline>();
        outline.effectColor = new Color(tint.r * 0.7f, tint.g * 0.7f, tint.b * 0.7f, 1f); // Darker outline
        outline.effectDistance = new Vector2(3, -3);
        
        // Create Class Image at top - LARGER and SQUARE
        GameObject imageObj = new GameObject("ClassImage");
        imageObj.transform.SetParent(cardObj.transform);
        
        RectTransform imageRect = imageObj.AddComponent<RectTransform>();
        imageRect.anchoredPosition = new Vector2(0, 150); // Top of card
        imageRect.sizeDelta = new Vector2(300, 300); // Bigger square image for larger cards
        imageRect.localScale = Vector3.one;
        
        Image classImage = imageObj.AddComponent<Image>();
        if (classSprite != null)
        {
            classImage.sprite = classSprite;
            classImage.color = Color.white;
            classImage.preserveAspect = true; // Keep aspect ratio
            Debug.Log($"✅ Applied sprite to {cardName}");
        }
        else
        {
            // Colored placeholder if image missing
            classImage.color = tint;
            Debug.LogWarning($"⚠️ No sprite for {cardName}, using colored placeholder");
        }
        
        // Create Class Name below image
        GameObject nameObj = new GameObject("ClassName");
        nameObj.transform.SetParent(cardObj.transform);
        
        RectTransform nameRect = nameObj.AddComponent<RectTransform>();
        nameRect.anchoredPosition = new Vector2(0, -30); // Below image
        nameRect.sizeDelta = new Vector2(320, 45);
        nameRect.localScale = Vector3.one;
        
        AddTextComponent(nameObj, className, 32, TextAnchor.MiddleCenter, Color.white, true);
        
        // Create Description below name - WRAPPED TEXT
        GameObject descObj = new GameObject("Description");
        descObj.transform.SetParent(cardObj.transform);
        
        RectTransform descRect = descObj.AddComponent<RectTransform>();
        descRect.anchoredPosition = new Vector2(0, -110); // Below name
        descRect.sizeDelta = new Vector2(320, 90); // Taller for multiple lines
        descRect.localScale = Vector3.one;
        
        AddTextComponent(descObj, description, 16, TextAnchor.UpperCenter, new Color(0.9f, 0.9f, 0.9f), false);
        
        // Create Select Button at bottom
        GameObject buttonObj = new GameObject("SelectButton");
        buttonObj.transform.SetParent(cardObj.transform);
        
        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.anchoredPosition = new Vector2(0, -240); // Bottom of card
        buttonRect.sizeDelta = new Vector2(280, 60);
        buttonRect.localScale = Vector3.one;
        
        Button button = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(tint.r * 0.5f, tint.g * 0.5f, tint.b * 0.5f, 0.9f); // Darker tinted button
        
        // Button hover colors - keep them subtle and dark
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(tint.r * 0.6f, tint.g * 0.6f, tint.b * 0.6f, 1f); // Dark normal
        colors.highlightedColor = new Color(tint.r * 0.8f, tint.g * 0.8f, tint.b * 0.8f, 1f); // Slightly brighter on hover
        colors.pressedColor = new Color(tint.r * 0.4f, tint.g * 0.4f, tint.b * 0.4f, 1f); // Darker on press
        colors.selectedColor = new Color(tint.r * 0.7f, tint.g * 0.7f, tint.b * 0.7f, 1f); // Medium on select
        button.colors = colors;
        
        // Button text
        GameObject btnTextObj = new GameObject("Text");
        btnTextObj.transform.SetParent(buttonObj.transform);
        
        RectTransform btnTextRect = btnTextObj.AddComponent<RectTransform>();
        btnTextRect.anchorMin = Vector2.zero;
        btnTextRect.anchorMax = Vector2.one;
        btnTextRect.offsetMin = Vector2.zero;
        btnTextRect.offsetMax = Vector2.zero;
        
        AddTextComponent(btnTextObj, "SELECT", 26, TextAnchor.MiddleCenter, Color.white, true);
        
        // Add button animator for smooth effects
        UIButtonAnimator animator = buttonObj.GetComponent<UIButtonAnimator>();
        if (animator == null)
            buttonObj.AddComponent<UIButtonAnimator>();
        
        Debug.Log($"✅ Created vertical card: {cardName}");
    }
    
    // Helper method to add text (handles both TMP and legacy)
    private static void AddTextComponent(GameObject obj, string text, int fontSize, TextAnchor alignment, Color color, bool bold)
    {
        TMPro.TMP_Text tmpText = null;
        Text legacyText = null;
        
        try
        {
            tmpText = obj.AddComponent<TMPro.TMP_Text>();
            tmpText.text = text;
            tmpText.fontSize = fontSize;
            tmpText.alignment = ConvertAlignment(alignment);
            tmpText.color = color;
            tmpText.fontStyle = bold ? TMPro.FontStyles.Bold : TMPro.FontStyles.Normal;
            tmpText.textWrappingMode = TMPro.TextWrappingModes.Normal;
            tmpText.overflowMode = TMPro.TextOverflowModes.Overflow;
        }
        catch
        {
            // Fallback to legacy Text
            if (tmpText != null)
                GameObject.DestroyImmediate(tmpText);
                
            legacyText = obj.AddComponent<Text>();
            legacyText.text = text;
            legacyText.fontSize = fontSize;
            legacyText.alignment = alignment;
            legacyText.color = color;
            legacyText.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
            legacyText.horizontalOverflow = HorizontalWrapMode.Wrap;
            legacyText.verticalOverflow = VerticalWrapMode.Overflow;
        }
    }
    
    // Convert TextAnchor to TMP alignment
    private static TMPro.TextAlignmentOptions ConvertAlignment(TextAnchor anchor)
    {
        switch (anchor)
        {
            case TextAnchor.UpperCenter: return TMPro.TextAlignmentOptions.Top;
            case TextAnchor.MiddleCenter: return TMPro.TextAlignmentOptions.Center;
            case TextAnchor.LowerCenter: return TMPro.TextAlignmentOptions.Bottom;
            default: return TMPro.TextAlignmentOptions.Center;
        }
    }
    
    private static void CreateClassCard(Canvas canvas, string cardName, Sprite classSprite, string className, Vector2 position, Color tint)
    {
        // Find or create card panel
        Transform cardTransform = canvas.transform.Find(cardName);
        GameObject cardObj;
        
        if (cardTransform == null)
        {
            cardObj = new GameObject(cardName);
            cardObj.transform.SetParent(canvas.transform);
        }
        else
        {
            cardObj = cardTransform.gameObject;
        }
        
        // Setup RectTransform
        RectTransform cardRect = cardObj.GetComponent<RectTransform>();
        if (cardRect == null)
            cardRect = cardObj.AddComponent<RectTransform>();
        
        cardRect.anchoredPosition = position;
        cardRect.sizeDelta = new Vector2(250, 350);
        cardRect.localScale = Vector3.one;
        
        // Add/Update Panel Image (background)
        Image cardBg = cardObj.GetComponent<Image>();
        if (cardBg == null)
            cardBg = cardObj.AddComponent<Image>();
        
        cardBg.color = new Color(tint.r, tint.g, tint.b, 0.7f);
        
        // Create/Update Class Image
        Transform imageTransform = cardObj.transform.Find("ClassImage");
        GameObject imageObj;
        
        if (imageTransform == null)
        {
            imageObj = new GameObject("ClassImage");
            imageObj.transform.SetParent(cardObj.transform);
        }
        else
        {
            imageObj = imageTransform.gameObject;
        }
        
        RectTransform imageRect = imageObj.GetComponent<RectTransform>();
        if (imageRect == null)
            imageRect = imageObj.AddComponent<RectTransform>();
        
        imageRect.anchoredPosition = new Vector2(0, 50);
        imageRect.sizeDelta = new Vector2(200, 200);
        imageRect.localScale = Vector3.one;
        
        Image classImage = imageObj.GetComponent<Image>();
        if (classImage == null)
            classImage = imageObj.AddComponent<Image>();
        
        if (classSprite != null)
        {
            classImage.sprite = classSprite;
            classImage.color = Color.white;
            Debug.Log($"✅ Applied sprite to {cardName}");
        }
        else
        {
            // If no sprite, just show a colored placeholder
            classImage.color = tint;
            Debug.LogWarning($"⚠️ No sprite for {cardName}, using colored placeholder");
        }
        
        // Create/Update Class Name Text
        Transform nameTransform = cardObj.transform.Find("ClassName");
        GameObject nameObj;
        
        if (nameTransform == null)
        {
            nameObj = new GameObject("ClassName");
            nameObj.transform.SetParent(cardObj.transform);
        }
        else
        {
            nameObj = nameTransform.gameObject;
        }
        
        RectTransform nameRect = nameObj.GetComponent<RectTransform>();
        if (nameRect == null)
            nameRect = nameObj.AddComponent<RectTransform>();
        
        nameRect.anchoredPosition = new Vector2(0, -80);
        nameRect.sizeDelta = new Vector2(200, 40);
        nameRect.localScale = Vector3.one;
        
        // Try to add text component (TMP preferred, fallback to legacy)
        TMPro.TMP_Text nameText = nameObj.GetComponent<TMPro.TMP_Text>();
        Text legacyText = nameObj.GetComponent<Text>();
        
        if (nameText == null && legacyText == null)
        {
            // Try TMP first
            try
            {
                nameText = nameObj.AddComponent<TMPro.TMP_Text>();
            }
            catch
            {
                // Fallback to legacy Text if TMP fails
                legacyText = nameObj.AddComponent<Text>();
                legacyText.text = className;
                legacyText.fontSize = 32;
                legacyText.alignment = TextAnchor.MiddleCenter;
                legacyText.color = Color.white;
                legacyText.fontStyle = FontStyle.Bold;
            }
        }
        
        if (nameText != null)
        {
            nameText.text = className;
            nameText.fontSize = 32;
            nameText.alignment = TMPro.TextAlignmentOptions.Center;
            nameText.color = Color.white;
            nameText.fontStyle = TMPro.FontStyles.Bold;
        }
        else if (legacyText != null)
        {
            legacyText.text = className;
            legacyText.fontSize = 32;
            legacyText.alignment = TextAnchor.MiddleCenter;
            legacyText.color = Color.white;
            legacyText.fontStyle = FontStyle.Bold;
        }
        
        // Create/Update Select Button
        Transform buttonTransform = cardObj.transform.Find("SelectButton");
        GameObject buttonObj;
        
        if (buttonTransform == null)
        {
            buttonObj = new GameObject("SelectButton");
            buttonObj.transform.SetParent(cardObj.transform);
        }
        else
        {
            buttonObj = buttonTransform.gameObject;
        }
        
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        if (buttonRect == null)
            buttonRect = buttonObj.AddComponent<RectTransform>();
        
        buttonRect.anchoredPosition = new Vector2(0, -130);
        buttonRect.sizeDelta = new Vector2(180, 45);
        buttonRect.localScale = Vector3.one;
        
        Button button = buttonObj.GetComponent<Button>();
        if (button == null)
            button = buttonObj.AddComponent<Button>();
        
        Image buttonImage = buttonObj.GetComponent<Image>();
        if (buttonImage == null)
            buttonImage = buttonObj.AddComponent<Image>();
        
        buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        
        // Button text
        Transform btnTextTransform = buttonObj.transform.Find("Text");
        GameObject btnTextObj;
        
        if (btnTextTransform == null)
        {
            btnTextObj = new GameObject("Text");
            btnTextObj.transform.SetParent(buttonObj.transform);
        }
        else
        {
            btnTextObj = btnTextTransform.gameObject;
        }
        
        RectTransform btnTextRect = btnTextObj.GetComponent<RectTransform>();
        if (btnTextRect == null)
            btnTextRect = btnTextObj.AddComponent<RectTransform>();
        
        btnTextRect.anchorMin = Vector2.zero;
        btnTextRect.anchorMax = Vector2.one;
        btnTextRect.offsetMin = Vector2.zero;
        btnTextRect.offsetMax = Vector2.zero;
        
        // Try to add text component (TMP preferred, fallback to legacy)
        TMPro.TMP_Text btnText = btnTextObj.GetComponent<TMPro.TMP_Text>();
        Text btnLegacyText = btnTextObj.GetComponent<Text>();
        
        if (btnText == null && btnLegacyText == null)
        {
            try
            {
                btnText = btnTextObj.AddComponent<TMPro.TMP_Text>();
            }
            catch
            {
                btnLegacyText = btnTextObj.AddComponent<Text>();
                btnLegacyText.text = "Select";
                btnLegacyText.fontSize = 24;
                btnLegacyText.alignment = TextAnchor.MiddleCenter;
                btnLegacyText.color = Color.white;
            }
        }
        
        if (btnText != null)
        {
            btnText.text = "Select";
            btnText.fontSize = 24;
            btnText.alignment = TMPro.TextAlignmentOptions.Center;
            btnText.color = Color.white;
        }
        else if (btnLegacyText != null)
        {
            btnLegacyText.text = "Select";
            btnLegacyText.fontSize = 24;
            btnLegacyText.alignment = TextAnchor.MiddleCenter;
            btnLegacyText.color = Color.white;
        }
        
        // Add button animator
        UIButtonAnimator animator = buttonObj.GetComponent<UIButtonAnimator>();
        if (animator == null)
            buttonObj.AddComponent<UIButtonAnimator>();
        
        Debug.Log($"✅ Created/Updated {cardName}");
    }
    
    private static void UpdateClassSelectionUI(Canvas canvas)
    {
        ClassSelectionUI selectionUI = GameObject.FindFirstObjectByType<ClassSelectionUI>();
        if (selectionUI == null)
        {
            Debug.LogWarning("⚠️ ClassSelectionUI not found in scene. Create it manually.");
            return;
        }
        
        SerializedObject so = new SerializedObject(selectionUI);
        
        // Assign card GameObjects (for outline highlighting)
        AssignCardGameObject(so, "warriorCardObj", canvas, "WarriorCard");
        AssignCardGameObject(so, "rangerCardObj", canvas, "RangerCard");
        AssignCardGameObject(so, "mageCardObj", canvas, "MageCard");
        AssignCardGameObject(so, "clericCardObj", canvas, "ClericCard");
        
        // Assign card images (legacy - not used but kept for compatibility)
        AssignCardImage(so, "warriorCard", canvas, "WarriorCard");
        AssignCardImage(so, "rangerCard", canvas, "RangerCard");
        AssignCardImage(so, "mageCard", canvas, "MageCard");
        AssignCardImage(so, "clericCard", canvas, "ClericCard");
        
        // Assign buttons
        AssignButton(so, "warriorButton", canvas, "WarriorCard/SelectButton");
        AssignButton(so, "rangerButton", canvas, "RangerCard/SelectButton");
        AssignButton(so, "mageButton", canvas, "MageCard/SelectButton");
        AssignButton(so, "clericButton", canvas, "ClericCard/SelectButton");
        
        so.ApplyModifiedProperties();
        Debug.Log("✅ ClassSelectionUI references updated!");
    }
    
    private static void AssignCardGameObject(SerializedObject so, string propName, Canvas canvas, string path)
    {
        Transform cardTransform = canvas.transform.Find(path);
        if (cardTransform != null)
        {
            SerializedProperty prop = so.FindProperty(propName);
            if (prop != null)
            {
                prop.objectReferenceValue = cardTransform.gameObject;
            }
        }
    }
    
    private static void AssignCardImage(SerializedObject so, string propName, Canvas canvas, string path)
    {
        Transform cardTransform = canvas.transform.Find(path);
        if (cardTransform != null)
        {
            Image img = cardTransform.GetComponent<Image>();
            if (img != null)
            {
                SerializedProperty prop = so.FindProperty(propName);
                if (prop != null)
                {
                    prop.objectReferenceValue = img;
                }
            }
        }
    }
    
    private static void AssignButton(SerializedObject so, string propName, Canvas canvas, string path)
    {
        Transform btnTransform = canvas.transform.Find(path);
        if (btnTransform != null)
        {
            Button btn = btnTransform.GetComponent<Button>();
            if (btn != null)
            {
                SerializedProperty prop = so.FindProperty(propName);
                if (prop != null)
                {
                    prop.objectReferenceValue = btn;
                }
            }
        }
    }
}
#endif
