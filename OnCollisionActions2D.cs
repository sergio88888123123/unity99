using UnityEngine;
using UnityEngine.Events;

public class OnCollisionActions2D : MonoBehaviour
{
    [Header("Opcional: cambiar color al colisionar")]
    public SpriteRenderer spriteRenderer;
    public Color onEnterColor = Color.green;
    public Color onExitColor  = Color.white;

    [Header("Opcional: reproducir sonido")]
    public AudioSource audioSource;
    public AudioClip onEnterClip;
    public AudioClip onExitClip;

    [Header("Eventos custom")]
    public UnityEvent onScored;
    public UnityEvent onDamaged;

    public void SetEnterFeedback(GameObject other)
    {
        if (spriteRenderer) spriteRenderer.color = onEnterColor;
        if (audioSource && onEnterClip) audioSource.PlayOneShot(onEnterClip);
    }

    public void SetExitFeedback(GameObject other)
    {
        if (spriteRenderer) spriteRenderer.color = onExitColor;
        if (audioSource && onExitClip) audioSource.PlayOneShot(onExitClip);
    }

    public void AddScore(GameObject other) => onScored?.Invoke();
    public void TakeDamage(GameObject other) => onDamaged?.Invoke();
}
