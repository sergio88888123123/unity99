using UnityEngine;
using UnityEngine.Events;

public class RectOverlapDetectorUI : MonoBehaviour
{
    public RectTransform a;
    public RectTransform b;

    public UnityEvent onOverlapEnter;
    public UnityEvent onOverlapExit;

    bool _wasOverlapping;

    void Update()
    {
        if (!a || !b) return;
        var rectA = GetScreenRect(a);
        var rectB = GetScreenRect(b);
        bool overlapping = rectA.Overlaps(rectB);

        if (overlapping && !_wasOverlapping) onOverlapEnter?.Invoke();
        if (!overlapping && _wasOverlapping) onOverlapExit?.Invoke();
        _wasOverlapping = overlapping;
    }

    Rect GetScreenRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 min = corners[0];
        Vector3 max = corners[2];
        return new Rect(min, max - min);
    }
}
