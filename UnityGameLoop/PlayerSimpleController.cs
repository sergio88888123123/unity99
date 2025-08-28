using UnityEngine;

[RequireComponent(typeof(SpriteFlipbookAnimator))]
public class PlayerSimpleController : MonoBehaviour
{
    public float speed = 300f;
    public bool useUIRectTransform = false;

    SpriteFlipbookAnimator _anim;
    RectTransform _rt;
    Transform _tr;

    void Awake()
    {
        _anim = GetComponent<SpriteFlipbookAnimator>();
        _rt = GetComponent<RectTransform>();
        _tr = transform;
    }

    void Update()
    {
        if (!GameLoop.Instance || GameLoop.Instance.State != GameLoop.GameState.Playing)
        {
            if (_anim) _anim.Pause();
            return;
        }

        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (input.sqrMagnitude > 0.0001f) _anim.Play();
        else _anim.Pause();

        var delta = input * speed * Time.deltaTime;

        if (useUIRectTransform && _rt)
            _rt.anchoredPosition += delta;
        else
            _tr.position += (Vector3)delta;
    }
}