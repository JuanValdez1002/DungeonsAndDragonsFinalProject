using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Editor utility to quickly setup the Class Selection background
/// This will run automatically when you open the ClassSelection scene
/// </summary>
[InitializeOnLoad]
public class ClassSelectionBackgroundSetup
{
    static ClassSelectionBackgroundSetup()
    {
        // Subscribe to scene opened event
        UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += OnSceneOpened;
    }
    
    private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
    {
        // Only run for ClassSelection scene
        if (scene.name != "ClassSelection") return;
        
        // Try to setup background automatically
        SetupBackground();
    }
    
    [MenuItem("Tools/Class Selection/Setup Background")]
    public static void SetupBackground()
    {
        // Find the Canvas
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("No Canvas found in scene. Please add a Canvas first.");
            return;
        }
        
        // Refresh asset database first
        AssetDatabase.Refresh();
        
        // Try multiple possible paths and file extensions
        string[] possiblePaths = new string[]
        {
            "Assets/UI/Backgrounds/ClassSelectionBackground.jpg",
            "Assets/UI/Backgrounds/classselectionbackground.jpg",
            "Assets/UI/Backgrounds/ClassSelectionBackground.png",
            "Assets/UI/Backgrounds/classselectionbackground.png"
        };
        
        Sprite backgroundSprite = null;
        string foundPath = "";
        
        foreach (string path in possiblePaths)
        {
            backgroundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (backgroundSprite != null)
            {
                foundPath = path;
                Debug.Log($"✅ Found background at: {path}");
                break;
            }
        }
        
        // If still not found, try to find any image in the Backgrounds folder
        if (backgroundSprite == null)
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/UI/Backgrounds" });
            if (guids.Length > 0)
            {
                foundPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                Debug.Log($"Found texture at: {foundPath}");
                
                // Make sure it's imported as a sprite
                TextureImporter importer = AssetImporter.GetAtPath(foundPath) as TextureImporter;
                if (importer != null && importer.textureType != TextureImporterType.Sprite)
                {
                    Debug.Log("Converting texture to Sprite...");
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.maxTextureSize = 2048;
                    importer.SaveAndReimport();
                }
                
                AssetDatabase.Refresh();
                backgroundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(foundPath);
            }
        }
        
        if (backgroundSprite == null)
        {
            Debug.LogError("❌ Could not find any background image in Assets/UI/Backgrounds/");
            Debug.LogError("Make sure ClassSelectionBackground.jpg is in the folder and Unity has imported it.");
            Debug.LogError("Try: Right-click on the image in Unity → Reimport");
            return;
        }
        
        // Find or create Background Image
        Transform backgroundTransform = canvas.transform.Find("Background");
        GameObject backgroundObj;
        
        if (backgroundTransform == null)
        {
            // Create new background
            backgroundObj = new GameObject("Background");
            backgroundObj.transform.SetParent(canvas.transform);
            backgroundObj.transform.SetAsFirstSibling(); // Put it behind everything
        }
        else
        {
            backgroundObj = backgroundTransform.gameObject;
        }
        
        // Add/Get Image component
        Image backgroundImage = backgroundObj.GetComponent<Image>();
        if (backgroundImage == null)
        {
            backgroundImage = backgroundObj.AddComponent<Image>();
        }
        
        // Apply the sprite
        backgroundImage.sprite = backgroundSprite;
        backgroundImage.color = Color.white;
        
        // Setup RectTransform to fill the screen
        RectTransform rectTransform = backgroundObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = Vector3.one;
        
        // Also update ClassSelectionUI reference if it exists
        ClassSelectionUI selectionUI = GameObject.FindFirstObjectByType<ClassSelectionUI>();
        if (selectionUI != null)
        {
            SerializedObject so = new SerializedObject(selectionUI);
            SerializedProperty bgProp = so.FindProperty("backgroundImage");
            if (bgProp != null)
            {
                bgProp.objectReferenceValue = backgroundImage;
                so.ApplyModifiedProperties();
                Debug.Log("✅ Background image assigned to ClassSelectionUI!");
            }
        }
        
        // Mark scene as dirty so it saves
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        
        Debug.Log("✅ Class Selection background setup complete!");
        Debug.Log($"Background: {backgroundSprite.name}");
        Debug.Log($"Resolution: {backgroundSprite.texture.width}x{backgroundSprite.texture.height}");
    }
    
    [MenuItem("Tools/Class Selection/Adjust Background Tint")]
    public static void AdjustBackgroundTint()
    {
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
        if (canvas == null) return;
        
        Transform backgroundTransform = canvas.transform.Find("Background");
        if (backgroundTransform == null) return;
        
        Image backgroundImage = backgroundTransform.GetComponent<Image>();
        if (backgroundImage != null)
        {
            // Darken slightly for better text readability
            backgroundImage.color = new Color(0.85f, 0.85f, 0.85f, 1f);
            Debug.Log("Background darkened for better text contrast");
        }
    }
}
#endif
