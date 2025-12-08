using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;

/// <summary>
/// Fixes class image files and prepares them for use
/// </summary>
public class FixClassImageFiles
{
    [MenuItem("Tools/Class Selection/Fix Image Files")]
    public static void FixImageFiles()
    {
        Debug.Log("🔧 Fixing class image files...");
        
        string basePath = "Assets/UI/ClassImages/";
        
        // Fix WarriorImg (no extension, but it's a JPG)
        string warriorPath = Application.dataPath + "/UI/ClassImages/WarriorImg";
        string warriorNewPath = Application.dataPath + "/UI/ClassImages/WarriorImg.jpg";
        
        if (File.Exists(warriorPath) && !File.Exists(warriorNewPath))
        {
            try
            {
                File.Copy(warriorPath, warriorNewPath);
                Debug.Log("✅ Created WarriorImg.jpg");
                
                // Also copy the meta file
                if (File.Exists(warriorPath + ".meta"))
                {
                    File.Copy(warriorPath + ".meta", warriorNewPath + ".meta", true);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to copy WarriorImg: {e.Message}");
            }
        }
        
        AssetDatabase.Refresh();
        
        // Now configure all images
        ConfigureAsSprite(basePath + "WarriorNewImg.jpg"); // NEW
        ConfigureAsSprite(basePath + "WarriorImg.jpg");
        ConfigureAsSprite(basePath + "RangerImg.jpg");
        ConfigureAsSprite(basePath + "MageImg.jpg"); // NEW
        ConfigureAsSprite(basePath + "MageImg.webp");
        ConfigureAsSprite(basePath + "ClericImg.jpg");
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("✅ All class images fixed and configured!");
        Debug.Log("Now run: Tools → Class Selection → Setup Class Images");
    }
    
    private static void ConfigureAsSprite(string assetPath)
    {
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer != null)
        {
            bool needsUpdate = false;
            
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                needsUpdate = true;
            }
            
            if (importer.spriteImportMode != SpriteImportMode.Single)
            {
                importer.spriteImportMode = SpriteImportMode.Single;
                needsUpdate = true;
            }
            
            if (importer.maxTextureSize < 2048)
            {
                importer.maxTextureSize = 2048;
                needsUpdate = true;
            }
            
            if (importer.mipmapEnabled)
            {
                importer.mipmapEnabled = false;
                needsUpdate = true;
            }
            
            if (!importer.alphaIsTransparency)
            {
                importer.alphaIsTransparency = true;
                needsUpdate = true;
            }
            
            if (needsUpdate)
            {
                Debug.Log($"✅ Configured as sprite: {assetPath}");
                importer.SaveAndReimport();
            }
            else
            {
                Debug.Log($"Already configured: {assetPath}");
            }
        }
        else
        {
            Debug.LogWarning($"⚠️ Could not find: {assetPath}");
        }
    }
}
#endif
