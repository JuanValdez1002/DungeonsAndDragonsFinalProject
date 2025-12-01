using UnityEngine;

public class HitBoxRelay : MonoBehaviour
{
    [SerializeField] private SwordHitbox swordHitbox;

    // Called by animation event
    public void EnableHitbox()
    {
        if (swordHitbox != null)
            swordHitbox.EnableHitbox();
    }

    // Called by animation event
    public void DisableHitbox()
    {
        if (swordHitbox != null)
            swordHitbox.DisableHitbox();
    }
}


