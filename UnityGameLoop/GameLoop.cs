using System;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public static GameLoop Instance { get; private set; }

    public enum GameState { Boot, MainMenu, Playing, Paused }
    [SerializeField] private GameState _state = GameState.Boot;
    public GameState State => _state;

    public static event Action<GameState> OnStateChanged;

    [Header("Opciones")]
    public bool autoPlayOnStart = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetState(GameState.Boot);
    }

    void Start()
    {
        if (autoPlayOnStart) StartPlaying();
        else GoToMainMenu();
    }

    public void GoToMainMenu() => SetState(GameState.MainMenu);
    public void StartPlaying()  => SetState(GameState.Playing);
    public void Pause()         => SetState(GameState.Paused);
    public void Resume()        => SetState(GameState.Playing);

    public void QuitToMenu()
    {
        SetState(GameState.MainMenu);
    }

    void SetState(GameState newState)
    {
        if (_state == newState) return;
        _state = newState;

        switch (_state)
        {
            case GameState.Boot:
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
        }

        OnStateChanged?.Invoke(_state);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_state == GameState.Playing) Pause();
            else if (_state == GameState.Paused) Resume();
        }
        if (Input.GetKeyDown(KeyCode.M)) GoToMainMenu();
    }
#endif
}