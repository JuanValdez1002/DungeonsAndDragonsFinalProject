using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Camera cam;                 // drag your Player Camera here
    public GameObject projectilePrefab; // ✅ THIS will accept a prefab
    public float projectileSpeed = 20f;
    public float fireRate = 0.3f;
    public InventoryUIController inventoryUI;

    float nextFireTime;

    void Update()
    {
        if (GameManager.IsGameOver) return;
        // Block shooting when inventory open
        if (inventoryUI != null && inventoryUI.isOpen)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }


    void Shoot()
    {
        if (projectilePrefab == null || cam == null)
        {
            Debug.LogWarning("PlayerAttack: Missing projectilePrefab or cam!");
            return;
        }

        // Spawn just in front of camera
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Vector3 spawnPos = ray.origin + ray.direction * 0.5f;

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(ray.direction));

        // Give it velocity if it has a Rigidbody
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = ray.direction * projectileSpeed;
        }
    }
}
