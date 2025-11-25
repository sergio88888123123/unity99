using UnityEngine;

/// <summary>
/// Sistema simple de guardado utilizando PlayerPrefs.
/// Funciona en PC y dispositivos móviles.
/// </summary>
public static class SaveSystem
{
    private const string KEY_BEST_LEVEL = "BestLevel";
    private const string KEY_BEST_SCORE = "BestScore";
    private const string KEY_DIFFICULTY = "Difficulty";

    public static void SaveProgress(int levelIndex, int score)
    {
        int bestLevel = PlayerPrefs.GetInt(KEY_BEST_LEVEL, 0);
        int bestScore = PlayerPrefs.GetInt(KEY_BEST_SCORE, 0);

        if (levelIndex > bestLevel)
        {
            PlayerPrefs.SetInt(KEY_BEST_LEVEL, levelIndex);
        }

        if (score > bestScore)
        {
            PlayerPrefs.SetInt(KEY_BEST_SCORE, score);
        }

        PlayerPrefs.Save();
    }

    public static int GetBestLevel()
    {
        return PlayerPrefs.GetInt(KEY_BEST_LEVEL, 0);
    }

    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt(KEY_BEST_SCORE, 0);
    }

    public static void SaveDifficulty(Difficulty difficulty)
    {
        PlayerPrefs.SetInt(KEY_DIFFICULTY, (int)difficulty);
        PlayerPrefs.Save();
    }

    public static Difficulty GetDifficulty()
    {
        int value = PlayerPrefs.GetInt(KEY_DIFFICULTY, (int)Difficulty.Normal);
        return (Difficulty)value;
    }
}
