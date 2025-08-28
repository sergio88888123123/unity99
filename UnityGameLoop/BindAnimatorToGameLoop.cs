using UnityEngine;

[RequireComponent(typeof(SpriteFlipbookAnimator))]
public class BindAnimatorToGameLoop : MonoBehaviour
{
    SpriteFlipbookAnimator _anim;

    void Awake() => _anim = GetComponent<SpriteFlipbookAnimator>();
    void OnEnable()  => GameLoop.OnStateChanged += HandleState;
    void OnDisable() => GameLoop.OnStateChanged -= HandleState;

    void HandleState(GameLoop.GameState s)
    {
        if (_anim == null) return;
        if (s == GameLoop.GameState.Paused || s == GameLoop.GameState.MainMenu) _anim.Pause();
        if (s == GameLoop.GameState.Playing) _anim.Play();
    }
}