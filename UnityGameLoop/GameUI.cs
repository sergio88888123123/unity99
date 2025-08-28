using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Botones (opcionales)")]
    public Button playButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button menuButton;

    [Header("Paneles (opcionales)")]
    public GameObject mainMenuPanel;
    public GameObject hudPanel;
    public GameObject pausePanel;

    void OnEnable()  => GameLoop.OnStateChanged += HandleState;
    void OnDisable() => GameLoop.OnStateChanged -= HandleState;

    void Start()
    {
        if (playButton)  playButton.onClick.AddListener(() => GameLoop.Instance.StartPlaying());
        if (pauseButton) pauseButton.onClick.AddListener(() => GameLoop.Instance.Pause());
        if (resumeButton) resumeButton.onClick.AddListener(() => GameLoop.Instance.Resume());
        if (menuButton)  menuButton.onClick.AddListener(() => GameLoop.Instance.QuitToMenu());

        HandleState(GameLoop.Instance ? GameLoop.Instance.State : GameLoop.GameState.MainMenu);
    }

    void HandleState(GameLoop.GameState s)
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(s == GameLoop.GameState.MainMenu);
        if (hudPanel)      hudPanel.SetActive(s == GameLoop.GameState.Playing);
        if (pausePanel)    pausePanel.SetActive(s == GameLoop.GameState.Paused);

        if (pauseButton)  pauseButton.gameObject.SetActive(s == GameLoop.GameState.Playing);
        if (resumeButton) resumeButton.gameObject.SetActive(s == GameLoop.GameState.Paused);
    }
}