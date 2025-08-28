using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SpriteFlipbookAnimator : MonoBehaviour
{
    [Header("Sprites (en orden)")]
    public Sprite[] frames;

    [Header("Parámetros")]
    public float fps = 10f;
    public bool loop = true;
    public bool playOnEnable = true;

    int _index;
    float _accum;
    bool _isPlaying;

    SpriteRenderer _sr;
    Image _img;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _img = GetComponent<Image>();
        ApplyFrame();
    }

    void OnEnable()
    {
        _accum = 0f;
        _index = 0;
        _isPlaying = playOnEnable;
        ApplyFrame();
    }

    void Update()
    {
        if (!_isPlaying || frames == null || frames.Length == 0 || fps <= 0f) return;

        _accum += Time.unscaledDeltaTime * fps;
        while (_accum >= 1f)
        {
            _accum -= 1f;
            _index++;

            if (_index >= frames.Length)
            {
                if (loop) _index = 0;
                else { _index = frames.Length - 1; _isPlaying = false; }
            }
            ApplyFrame();
        }
    }

    public void Play()  { _isPlaying = true; }
    public void Pause() { _isPlaying = false; }
    public void Stop()  { _isPlaying = false; _index = 0; ApplyFrame(); }
    public void SetFPS(float newFps) { fps = Mathf.Max(0f, newFps); }

    void ApplyFrame()
    {
        if (frames == null || frames.Length == 0) return;
        var s = frames[Mathf.Clamp(_index, 0, frames.Length - 1)];
        if (_sr)  _sr.sprite = s;
        if (_img) _img.sprite = s;
    }
}