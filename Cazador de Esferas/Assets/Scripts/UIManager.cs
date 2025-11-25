using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Actualiza la interfaz de usuario en pantalla: score, vidas, dificultad, paneles.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    public Text scoreText;
    public Text livesText;
    public Text difficultyText;

    [Header("Paneles")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Puntuación: " + score;
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
            livesText.text = "Vidas: " + lives;
    }

    public void UpdateDifficultyText(Difficulty difficulty)
    {
        if (difficultyText != null)
            difficultyText.text = "Dificultad: " + difficulty.ToString();
    }

    public void TogglePause()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.isGamePaused = !GameManager.Instance.isGamePaused;
        Time.timeScale = GameManager.Instance.isGamePaused ? 0f : 1f;

        if (pausePanel != null)
            pausePanel.SetActive(GameManager.Instance.isGamePaused);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // Botones UI
    public void OnResumeButton()
    {
        TogglePause();
    }

    public void OnRestartButton()
    {
        GameManager.Instance?.RestartLevel();
    }

    public void OnMainMenuButton()
    {
        GameManager.Instance?.LoadMainMenu();
    }
}
