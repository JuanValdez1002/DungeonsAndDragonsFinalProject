using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Image foreground;
    public Transform target;
    public Vector3 offset = new Vector3(0, 2.5f, 0);

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }
    }

    public void SetHealth(int max, int current)
    {
        float percent = (float)current / max;
        foreground.fillAmount = percent;
    }
}
