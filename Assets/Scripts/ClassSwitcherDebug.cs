using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Debug/Testing script to quickly switch classes in-game
/// Attach to a UI panel or leave in scene for testing
/// Remove in final build
/// </summary>
public class ClassSwitcherDebug : MonoBehaviour
{
    [Header("For Testing Only")]
    public KeyCode warriorKey = KeyCode.Alpha1;
    public KeyCode rangerKey = KeyCode.Alpha2;
    public KeyCode mageKey = KeyCode.Alpha3;
    public KeyCode clericKey = KeyCode.Alpha4;
    
    [Header("Optional UI")]
    public Text debugText;
    
    private PlayerClass playerClass;
    
    void Start()
    {
        // Find player class component
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerClass = player.GetComponent<PlayerClass>();
        }
        
        if (debugText != null)
        {
            debugText.text = "Class Switcher Active\n1=Warrior 2=Ranger 3=Mage 4=Cleric";
        }
    }
    
    void Update()
    {
        if (playerClass == null)
            return;
        
        // Quick class switching for testing
        if (Input.GetKeyDown(warriorKey))
        {
            playerClass.SetClass(ClassType.Warrior);
            ShowMessage("Switched to Warrior");
        }
        else if (Input.GetKeyDown(rangerKey))
        {
            playerClass.SetClass(ClassType.Ranger);
            ShowMessage("Switched to Ranger");
        }
        else if (Input.GetKeyDown(mageKey))
        {
            playerClass.SetClass(ClassType.Mage);
            ShowMessage("Switched to Mage");
        }
        else if (Input.GetKeyDown(clericKey))
        {
            playerClass.SetClass(ClassType.Cleric);
            ShowMessage("Switched to Cleric");
        }
    }
    
    private void ShowMessage(string message)
    {
        Debug.Log(message);
        if (debugText != null)
        {
            debugText.text = message;
        }
    }
    
    // Can be called from UI buttons
    public void SwitchToWarrior() { playerClass?.SetClass(ClassType.Warrior); }
    public void SwitchToRanger() { playerClass?.SetClass(ClassType.Ranger); }
    public void SwitchToMage() { playerClass?.SetClass(ClassType.Mage); }
    public void SwitchToCleric() { playerClass?.SetClass(ClassType.Cleric); }
}
