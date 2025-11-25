using UnityEngine;

/// <summary>
/// Objeto coleccionable que incrementa la puntuación.
/// Principal mecánica del juego: recoger estos objetos evitando obstáculos.
/// </summary>
public class Collectible : MonoBehaviour
{
    public int points = 1;
    public ParticleSystem collectEffect;
    public AudioSource collectSound;

    private bool collected = false;

    public void Collect()
    {
        if (collected) return;
        collected = true;

        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        if (collectSound != null)
        {
            collectSound.Play();
        }

        GameManager.Instance?.AddScore(points);
        Destroy(gameObject);
    }
}
