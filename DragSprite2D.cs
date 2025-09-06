using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DragSprite2D : MonoBehaviour
{
    public bool requireColliderHit = true;
    public Camera targetCamera;
    public bool draggable = true;

    bool _dragging;
    Vector3 _offset;

    void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (requireColliderHit && GetComponent<Collider2D>() == null)
            Debug.LogWarning("DragSprite2D: se recomienda un Collider2D si requireColliderHit está activo.");
    }

    void Update()
    {
        if (!draggable) return;
#if UNITY_EDITOR || UNITY_STANDALONE
        HandlePointer(Input.GetMouseButtonDown(0), Input.GetMouseButton(0), Input.mousePosition);
#else
        if (Input.touchCount > 0)
        {
            var t = Input.GetTouch(0);
            HandlePointer(t.phase == TouchPhase.Began, t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary, t.position);
        }
#endif
    }

    void HandlePointer(bool down, bool held, Vector3 screenPos)
    {
        Vector3 world = targetCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(targetCamera.transform.position.z - transform.position.z)));
        if (down)
        {
            if (!requireColliderHit || IsPointerOverMe(world))
            {
                _dragging = true;
                _offset = transform.position - world;
            }
        }

        if (_dragging && held)
        {
            transform.position = new Vector3(world.x + _offset.x, world.y + _offset.y, transform.position.z);
        }

        if (!held) _dragging = false;
    }

    bool IsPointerOverMe(Vector3 world)
    {
        var hit = Physics2D.OverlapPoint(world);
        return hit != null && hit.transform == transform;
    }

    public void EnableDrag()  => draggable = true;
    public void DisableDrag() => draggable = false;
    public void ToggleDrag()  => draggable = !draggable;
}
