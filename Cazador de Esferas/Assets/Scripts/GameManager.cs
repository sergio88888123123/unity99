using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Control central del videojuego.
/// Maneja estados, puntuación, dificultad, avance del jugador y navegación entre escenas.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Estado del juego")]
    public int currentLevelIndex = 1;
    public int playerScore = 0;
    public int playerLives = 3;
    public Difficulty difficulty = Difficulty.Normal;

    [Header("Progresión")]
    public int pointsToNextLevel = 10;
    public bool isGamePaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        difficulty = SaveSystem.GetDifficulty();
    }

    private void Start()
    {
        UIManager.Instance?.UpdateDifficultyText(difficulty);
        UIManager.Instance?.UpdateScore(playerScore);
        UIManager.Instance?.UpdateLives(playerLives);
    }

    public void SetDifficulty(Difficulty newDifficulty)
    {
        difficulty = newDifficulty;
        SaveSystem.SaveDifficulty(difficulty);
        UIManager.Instance?.UpdateDifficultyText(difficulty);
    }

    public void AddScore(int amount)
    {
        playerScore += amount;
        UIManager.Instance?.UpdateScore(playerScore);

        if (playerScore >= pointsToNextLevel * currentLevelIndex)
        {
            NextLevel();
        }
    }

    public void LoseLife()
    {
        playerLives--;
        UIManager.Instance?.UpdateLives(playerLives);

        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        SaveSystem.SaveProgress(currentLevelIndex, playerScore);

        // Curva de dificultad progresiva: aumentan requisitos y enemigos.
        pointsToNextLevel = Mathf.RoundToInt(pointsToNextLevel * 1.25f);

        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = buildIndex + 1;
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            LoadMainMenu();
        }
        else
        {
            SceneManager.LoadScene(nextIndex);
        }
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        UIManager.Instance?.ShowGameOver();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        SaveSystem.SaveProgress(currentLevelIndex, playerScore);
        Application.Quit();
    }
}
