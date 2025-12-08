using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Add TextMeshPro support
using UnityEngine.EventSystems;

/// <summary>
/// Enhanced class selection UI with visual effects and animations
/// Works with both legacy Text and TextMeshPro
/// </summary>
public class ClassSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject selectionPanel;
    public Image backgroundImage; // Main background
    
    [Header("Class Card GameObjects (Assign the card GameObjects, not Images)")]
    public GameObject warriorCardObj;
    public GameObject rangerCardObj;
    public GameObject mageCardObj;
    public GameObject clericCardObj;
    
    [Header("Class Card Panels (DO NOT assign the main card backgrounds!)")]
    [Header("Leave these EMPTY - they are not used anymore")]
    public Image warriorCard;
    public Image rangerCard;
    public Image mageCard;
    public Image clericCard;
    
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
    
    [Header("Visual Effects")]
    public Color normalCardColor = new Color(0.2f, 0.2f, 0.2f, 0.85f); // Dark, subtle
    public Color selectedCardColor = new Color(0.4f, 0.35f, 0.25f, 0.95f); // Slightly brighter but still dark
    public Color hoverCardColor = new Color(0.3f, 0.3f, 0.3f, 0.9f); // Subtle hover
    public float cardAnimationSpeed = 5f;
    public float hoverScale = 1.05f;
    
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
    private Image currentHoveredCard;
    private Image currentSelectedCard;
    private GameObject currentSelectedCardObj;
    
    void Start()
    {
        // Setup button listeners with hover effects
        SetupButton(warriorButton, ClassType.Warrior, warriorCard);
        SetupButton(rangerButton, ClassType.Ranger, rangerCard);
        SetupButton(mageButton, ClassType.Mage, mageCard);
        SetupButton(clericButton, ClassType.Cleric, clericCard);
        
        if (startButton != null)
            startButton.onClick.AddListener(StartGame);
        
        // Show warrior by default
        SelectClass(ClassType.Warrior);
        
        // Initialize all cards to normal state
        InitializeCards();
        
        // Unlock cursor for menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    /// <summary>
    /// Setup button with click and hover effects
    /// </summary>
    private void SetupButton(Button button, ClassType classType, Image cardImage)
    {
        if (button == null) return;
        
        button.onClick.AddListener(() => SelectClass(classType));
        
        // Add hover effects using EventTrigger
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();
        
        // Mouse enter
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => { OnCardHover(cardImage, true); });
        trigger.triggers.Add(enterEntry);
        
        // Mouse exit
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { OnCardHover(cardImage, false); });
        trigger.triggers.Add(exitEntry);
    }
    
    /// <summary>
    /// Initialize all cards to normal color
    /// </summary>
    private void InitializeCards()
    {
        // Cards are already styled by the editor script, don't override them!
        // Leaving this empty to prevent white/bright cards
        // if (warriorCard != null) warriorCard.color = normalCardColor;
        // if (rangerCard != null) rangerCard.color = normalCardColor;
        // if (mageCard != null) mageCard.color = normalCardColor;
        // if (clericCard != null) clericCard.color = normalCardColor;
    }
    
    /// <summary>
    /// Handle card hover effects
    /// </summary>
    private void OnCardHover(Image card, bool isEntering)
    {
        // Disabled - cards keep their original styling
        // No color changes on hover to prevent white/bright backgrounds
        /*
        if (card == null) return;
        
        if (isEntering)
        {
            currentHoveredCard = card;
            // Don't change color if it's the selected card
            if (card != currentSelectedCard)
            {
                card.color = hoverCardColor;
            }
        }
        else
        {
            currentHoveredCard = null;
            // Return to normal color unless it's selected
            if (card != currentSelectedCard)
            {
                card.color = normalCardColor;
            }
        }
        */
    }
    
    /// <summary>
    /// Called when player selects a class
    /// </summary>
    public void SelectClass(ClassType classType)
    {
        selectedClass = classType;
        UpdateClassDisplay(classType);
        UpdateCardSelection(classType);
    }
    
    /// <summary>
    /// Update visual selection of class cards
    /// </summary>
    private void UpdateCardSelection(ClassType classType)
    {
        // Reset all card outlines to normal (darker)
        ResetCardOutline(warriorCardObj);
        ResetCardOutline(rangerCardObj);
        ResetCardOutline(mageCardObj);
        ResetCardOutline(clericCardObj);
        
        // Highlight selected card outline
        GameObject selectedCardObj = null;
        switch (classType)
        {
            case ClassType.Warrior:
                selectedCardObj = warriorCardObj;
                break;
            case ClassType.Ranger:
                selectedCardObj = rangerCardObj;
                break;
            case ClassType.Mage:
                selectedCardObj = mageCardObj;
                break;
            case ClassType.Cleric:
                selectedCardObj = clericCardObj;
                break;
        }
        
        if (selectedCardObj != null)
        {
            HighlightCardOutline(selectedCardObj);
            currentSelectedCardObj = selectedCardObj;
        }
    }
    
    /// <summary>
    /// Highlight a card's outline to show selection
    /// </summary>
    private void HighlightCardOutline(GameObject cardObj)
    {
        if (cardObj == null) return;
        
        Outline outline = cardObj.GetComponent<Outline>();
        if (outline != null)
        {
            // Make outline brighter and thicker
            Color brightColor = outline.effectColor;
            brightColor.r = Mathf.Min(brightColor.r * 2.5f, 1f);
            brightColor.g = Mathf.Min(brightColor.g * 2.5f, 1f);
            brightColor.b = Mathf.Min(brightColor.b * 2.5f, 1f);
            brightColor.a = 1f;
            outline.effectColor = brightColor;
            outline.effectDistance = new Vector2(5, -5); // Thicker outline
        }
        
        // Optional: add slight scale effect
        cardObj.transform.localScale = new Vector3(1.02f, 1.02f, 1f);
    }
    
    /// <summary>
    /// Reset a card's outline to normal state
    /// </summary>
    private void ResetCardOutline(GameObject cardObj)
    {
        if (cardObj == null) return;
        
        Outline outline = cardObj.GetComponent<Outline>();
        if (outline != null)
        {
            // Return to darker outline
            Color darkColor = outline.effectColor;
            darkColor.r = darkColor.r / 2.5f;
            darkColor.g = darkColor.g / 2.5f;
            darkColor.b = darkColor.b / 2.5f;
            darkColor.a = 1f;
            outline.effectColor = darkColor;
            outline.effectDistance = new Vector2(3, -3); // Normal outline thickness
        }
        
        // Reset scale
        cardObj.transform.localScale = Vector3.one;
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
