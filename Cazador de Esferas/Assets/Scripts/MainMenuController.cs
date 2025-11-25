using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla la interfaz de inicio / menú principal.
/// Botones para jugar en distintas dificultades y navegar.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    public void PlayEasy()
    {
        StartGame(Difficulty.Easy);
    }

    public void PlayNormal()
    {
        StartGame(Difficulty.Normal);
    }

    public void PlayHard()
    {
        StartGame(Difficulty.Hard);
    }

    private void StartGame(Difficulty difficulty)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetDifficulty(difficulty);
            GameManager.Instance.currentLevelIndex = 1;
            GameManager.Instance.playerScore = 0;
            GameManager.Instance.playerLives = 3;
        }

        // Asumimos que la escena del primer nivel se llama "Level1".
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        GameManager.Instance?.QuitGame();
    }
}
