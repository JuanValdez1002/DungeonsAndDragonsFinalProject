using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Image foregroundFill;   // drag Foreground image here
    public Transform target;       // enemy root transform
    public Vector3 offset = new Vector3(0, 2.2f, 0);

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Follow the enemy
        transform.position = target.position + offset;

        // Always face the camera
        transform.LookAt(cam.transform);
    }

    // Called when enemy takes damage
    public void SetHealth(float current, float max)
    {
        float fill = current / max;
        foregroundFill.fillAmount = fill;  // <- THIS is what updates the red fill

        Debug.Log($"Enemy health updated: {current}/{max}");
    }
}
