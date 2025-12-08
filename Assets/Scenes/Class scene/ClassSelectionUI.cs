using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Add TextMeshPro support

/// <summary>
/// Manages the class selection UI and scene
/// Works with both legacy Text and TextMeshPro
/// </summary>
public class ClassSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject selectionPanel;
    
    // Support both Text types - assign whichever you have!
    [Header("Text Fields (Use Legacy Text OR TextMeshPro)")]
    public Text titleText;
    public TMP_Text titleTextTMP;
    
    public Text descriptionText;
    public TMP_Text descriptionTextTMP;
    
    [Header("Class Buttons")]
    public Button warriorButton;
    public Button rangerButton;
    public Button mageButton;
    public Button clericButton;
    public Button startButton;
    
    [Header("Class Info Display (Legacy Text)")]
    public Text classNameDisplay;
    public Text healthDisplay;
    public Text damageDisplay;
    public Text speedDisplay;
    public Text specialAbilityDisplay;
    
    [Header("Class Info Display (TextMeshPro) - Use if you have TMP")]
    public TMP_Text classNameDisplayTMP;
    public TMP_Text healthDisplayTMP;
    public TMP_Text damageDisplayTMP;
    public TMP_Text speedDisplayTMP;
    public TMP_Text specialAbilityDisplayTMP;
    
    [Header("Settings")]
    public string gameSceneName = "Dungeon"; // Scene to load after selection
    
    [Header("Alternative: Drag Scene Asset Here")]
    public UnityEngine.Object gameSceneAsset; // Drag scene file from Project window
    
    private ClassType selectedClass = ClassType.Warrior;
    
    void Start()
    {
        // Setup button listeners
        if (warriorButton != null)
            warriorButton.onClick.AddListener(() => SelectClass(ClassType.Warrior));
        if (rangerButton != null)
            rangerButton.onClick.AddListener(() => SelectClass(ClassType.Ranger));
        if (mageButton != null)
            mageButton.onClick.AddListener(() => SelectClass(ClassType.Mage));
        if (clericButton != null)
            clericButton.onClick.AddListener(() => SelectClass(ClassType.Cleric));
        if (startButton != null)
            startButton.onClick.AddListener(StartGame);
        
        // Show warrior by default
        SelectClass(ClassType.Warrior);
        
        // Unlock cursor for menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    /// <summary>
    /// Called when player selects a class
    /// </summary>
    public void SelectClass(ClassType classType)
    {
        selectedClass = classType;
        UpdateClassDisplay(classType);
    }
    
    /// <summary>
    /// Updates the UI to show selected class info
    /// </summary>
    private void UpdateClassDisplay(ClassType classType)
    {
        switch (classType)
        {
            case ClassType.Warrior:
                SetText(classNameDisplay, classNameDisplayTMP, "Warrior");
                SetText(descriptionText, descriptionTextTMP, "A mighty melee fighter with high defense and moderate damage. " +
                    "Specializes in close combat and can activate a defensive stance.");
                SetText(healthDisplay, healthDisplayTMP, "Health: 150");
                SetText(damageDisplay, damageDisplayTMP, "Damage: 15 (Melee)");
                SetText(speedDisplay, speedDisplayTMP, "Speed: 10");
                SetText(specialAbilityDisplay, specialAbilityDisplayTMP, "Special: Defensive Stance (Right Click)");
                break;
                
            case ClassType.Ranger:
                SetText(classNameDisplay, classNameDisplayTMP, "Ranger");
                SetText(descriptionText, descriptionTextTMP, "A skilled archer with high ranged damage and mobility. " +
                    "Attacks from distance and can fire multiple arrows at once.");
                SetText(healthDisplay, healthDisplayTMP, "Health: 100");
                SetText(damageDisplay, damageDisplayTMP, "Damage: 20 (Ranged)");
                SetText(speedDisplay, speedDisplayTMP, "Speed: 14");
                SetText(specialAbilityDisplay, specialAbilityDisplayTMP, "Special: Multi-Shot (Right Click)");
                break;
                
            case ClassType.Mage:
                SetText(classNameDisplay, classNameDisplayTMP, "Mage");
                SetText(descriptionText, descriptionTextTMP, "A powerful spellcaster with devastating magic attacks. " +
                    "Low health but deals massive damage with explosive fireballs.");
                SetText(healthDisplay, healthDisplayTMP, "Health: 75");
                SetText(damageDisplay, damageDisplayTMP, "Damage: 30 (Magic)");
                SetText(speedDisplay, speedDisplayTMP, "Speed: 12");
                SetText(specialAbilityDisplay, specialAbilityDisplayTMP, "Special: Explosive Fireball (Right Click)");
                break;
                
            case ClassType.Cleric:
                SetText(classNameDisplay, classNameDisplayTMP, "Cleric");
                SetText(descriptionText, descriptionTextTMP, "A holy warrior who can heal and fight. " +
                    "Balanced stats with powerful healing abilities for survival.");
                SetText(healthDisplay, healthDisplayTMP, "Health: 120");
                SetText(damageDisplay, damageDisplayTMP, "Damage: 12 (Ranged/Melee)");
                SetText(speedDisplay, speedDisplayTMP, "Speed: 11");
                SetText(specialAbilityDisplay, specialAbilityDisplayTMP, "Special: Area Heal (Right Click) | Heal (Press H)");
                break;
        }
    }
    
    /// <summary>
    /// Helper method to set text on either legacy Text or TextMeshPro
    /// </summary>
    private void SetText(Text legacyText, TMP_Text tmpText, string value)
    {
        if (legacyText != null)
            legacyText.text = value;
        if (tmpText != null)
            tmpText.text = value;
    }    /// <summary>
    /// Starts the game with selected class
    /// </summary>
    public void StartGame()
    {
        // Save selected class to be loaded in game scene
        PlayerPrefs.SetInt("SelectedClass", (int)selectedClass);
        PlayerPrefs.Save();
        
        Debug.Log($"Starting game as {selectedClass}");
        
        // Try to load scene by name first
        string sceneToLoad = gameSceneName;
        
        // If scene asset is assigned, use that instead
        if (gameSceneAsset != null)
        {
            sceneToLoad = gameSceneAsset.name;
            Debug.Log($"Loading scene from asset: {sceneToLoad}");
        }
        
        // Check if scene exists in build
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        bool sceneFound = false;
        
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (sceneName == sceneToLoad)
            {
                sceneFound = true;
                Debug.Log($"Scene '{sceneToLoad}' found at build index {i}");
                break;
            }
        }
        
        if (!sceneFound)
        {
            Debug.LogError($"Scene '{sceneToLoad}' not found in build settings! Add it via File → Build Profiles");
            Debug.LogError($"Scenes in build: {sceneCount}. Make sure to add your game scene!");
            return;
        }
        
        // Load game scene
        SceneManager.LoadScene(sceneToLoad);
    }
    
    /// <summary>
    /// Quit to main menu or exit
    /// </summary>
    public void QuitToMenu()
    {
        // Could load a main menu scene here
        Debug.Log("Quit to menu");
        Application.Quit();
    }
}
