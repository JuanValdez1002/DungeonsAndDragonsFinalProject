using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Add this to any button to give it smooth hover and click animations
/// Automatically adds scale effects and optional sound support
/// </summary>
[RequireComponent(typeof(Button))]
public class UIButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Scale Animation")]
    public float hoverScale = 1.1f;
    public float clickScale = 0.95f;
    public float animationSpeed = 10f;
    
    [Header("Optional: Audio")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    [Range(0f, 1f)]
    public float soundVolume = 0.5f;
    
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isHovering = false;
    private bool isPressed = false;
    private AudioSource audioSource;
    
    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        
        // Create audio source if we have sounds
        if (hoverSound != null || clickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = soundVolume;
        }
    }
    
    void Update()
    {
        // Smoothly animate to target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if (!isPressed)
        {
            targetScale = originalScale * hoverScale;
            PlaySound(hoverSound);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (!isPressed)
        {
            targetScale = originalScale;
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        targetScale = originalScale * clickScale;
        PlaySound(clickSound);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        targetScale = isHovering ? originalScale * hoverScale : originalScale;
    }
    
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    // Reset to original scale when disabled
    void OnDisable()
    {
        transform.localScale = originalScale;
    }
}
