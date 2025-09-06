using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DragSpriteUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool constrainToParent = true;
    public bool draggable = true;

    RectTransform _rt;
    RectTransform _parentRt;
    Canvas _canvas;
    Vector2 _offset;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _parentRt = transform.parent as RectTransform;
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null || _canvas.renderMode == RenderMode.WorldSpace)
            Debug.LogWarning("DragSpriteUI: asegúrate de usar un Canvas en ScreenSpace para precisión en UI.");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!draggable) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rt, eventData.position, eventData.pressEventCamera, out var local);
        _offset = local;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!draggable) return;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRt, eventData.position, eventData.pressEventCamera, out var localPos))
        {
            var size = _rt.rect.size;
            Vector2 half = size * 0.5f;

            Vector2 target = localPos - _offset;

            if (constrainToParent && _parentRt != null)
            {
                var parentSize = _parentRt.rect.size;
                target.x = Mathf.Clamp(target.x, -parentSize.x/2f + half.x, parentSize.x/2f - half.x);
                target.y = Mathf.Clamp(target.y, -parentSize.y/2f + half.y, parentSize.y/2f - half.y);
            }

            _rt.anchoredPosition = target;
        }
    }

    public void OnEndDrag(PointerEventData eventData) { }

    public void EnableDrag()  => draggable = true;
    public void DisableDrag() => draggable = false;
    public void ToggleDrag()  => draggable = !draggable;
}
