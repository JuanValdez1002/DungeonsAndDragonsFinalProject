using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Cleans up old UI elements from the ClassSelection scene
/// </summary>
public class CleanupClassSelectionUI
{
    [MenuItem("Tools/Class Selection/Clean Old UI Elements")]
    public static void CleanupOldUI()
    {
        Debug.Log("🧹 Cleaning up old UI elements...");
        
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("⚠️ No Canvas found. Open ClassSelection scene first!");
            return;
        }
        
        int cleaned = 0;
        
        // List of old UI elements that might be in the way
        string[] oldElements = new string[] 
        { 
            "Panel",
            "Title",
            "DescriptionText",
            "ClassInfoPanel",
            "InfoPanel",
            "ClassStatsPanel",
            "OldWarriorButton",
            "OldRangerButton",
            "OldMageButton",
            "OldClericButton"
        };
        
        foreach (string elementName in oldElements)
        {
            Transform element = canvas.transform.Find(elementName);
            if (element != null)
            {
                GameObject.DestroyImmediate(element.gameObject);
                Debug.Log($"✅ Removed: {elementName}");
                cleaned++;
            }
        }
        
        // Also look for any objects with "Button" in the name that aren't our new cards
        foreach (Transform child in canvas.transform)
        {
            if (child.name.Contains("Button") && 
                !child.name.Contains("SelectButton") &&
                !child.name.Contains("Start"))
            {
                GameObject.DestroyImmediate(child.gameObject);
                Debug.Log($"✅ Removed old button: {child.name}");
                cleaned++;
            }
        }
        
        if (cleaned > 0)
        {
            Debug.Log($"✅ Cleaned up {cleaned} old UI elements!");
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
        else
        {
            Debug.Log("✅ No old elements found - scene is clean!");
        }
    }
}
#endif
