using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectible2D : MonoBehaviour
{
    public string playerTag = "Player";
    public int scoreValue = 1;
    public bool deactivateOnCollect = true;

    public System.Action<int> OnCollected;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        OnCollected?.Invoke(scoreValue);
        if (deactivateOnCollect) gameObject.SetActive(false);
    }
}
