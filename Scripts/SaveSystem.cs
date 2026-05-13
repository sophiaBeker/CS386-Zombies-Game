using UnityEngine;

/// <summary>
/// Simple save system using PlayerPrefs.
/// Saves the player's best kill score across sessions.
/// Extend this class to save additional data (levels unlocked, settings, etc.).
/// </summary>
public static class SaveSystem
{
    private const string KEY_BEST_SCORE = "best_score";

    private const string KEY_CURRENT_LEVEL = "current_level";

    // ---------------------------------------------------------------
    public static void SaveBestScore(int score)
    {
        PlayerPrefs.SetInt(KEY_BEST_SCORE, score);
        PlayerPrefs.Save();
        Debug.Log($"[SaveSystem] Best score saved: {score}");
    }

    public static int LoadBestScore()
    {
        return PlayerPrefs.GetInt(KEY_BEST_SCORE, 0);
    }

    // ---------------------------------------------------------------
    // Helper: wipe all saved data (useful for a "Reset Progress" button)
    public static void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    // Save current level index
    public static void SaveCurrentLevel(int levelIndex)
    {
        Debug.Log($"[SaveSystem] Saving level: {levelIndex}");
        PlayerPrefs.SetInt(KEY_CURRENT_LEVEL, levelIndex);
        PlayerPrefs.Save();
    }

    public static int LoadCurrentLevel()
    {
        int level = PlayerPrefs.GetInt(KEY_CURRENT_LEVEL, 0);
        Debug.Log($"[SaveSystem] Loaded level: {level}");
        return level;
    }
}
