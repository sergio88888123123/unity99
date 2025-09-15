using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class CollisionEvents2D : MonoBehaviour
{
    [Header("Filtros")]
    public LayerMask layerMask = ~0;
    public string requiredTag = "";

    [Header("Eventos de TRIGGER")]
    public UnityEvent<GameObject> onTriggerEnter;
    public UnityEvent<GameObject> onTriggerStay;
    public UnityEvent<GameObject> onTriggerExit;

    [Header("Eventos de COLISIÓN (no trigger)")]
    public UnityEvent<GameObject> onCollisionEnter;
    public UnityEvent<GameObject> onCollisionStay;
    public UnityEvent<GameObject> onCollisionExit;

    bool PassFilters(GameObject other)
    {
        if ((layerMask.value & (1 << other.layer)) == 0) return false;
        if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag)) return false;
        return true;
    }

    void OnTriggerEnter2D(Collider2D other)  { if (PassFilters(other.gameObject)) onTriggerEnter?.Invoke(other.gameObject); }
    void OnTriggerStay2D(Collider2D other)   { if (PassFilters(other.gameObject)) onTriggerStay?.Invoke(other.gameObject); }
    void OnTriggerExit2D(Collider2D other)   { if (PassFilters(other.gameObject)) onTriggerExit?.Invoke(other.gameObject); }

    void OnCollisionEnter2D(Collision2D other) { if (PassFilters(other.gameObject)) onCollisionEnter?.Invoke(other.gameObject); }
    void OnCollisionStay2D(Collision2D other)  { if (PassFilters(other.gameObject)) onCollisionStay?.Invoke(other.gameObject); }
    void OnCollisionExit2D(Collision2D other)  { if (PassFilters(other.gameObject)) onCollisionExit?.Invoke(other.gameObject); }
}
