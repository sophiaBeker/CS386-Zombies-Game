using TMPro;
using UnityEngine;

/// <summary>
/// Used on both the Win and Lose scenes.
/// Displays final score and provides menu/retry navigation.
/// </summary>
public class EndScreenController : MonoBehaviour
{
    [Header("Labels")]
    public TextMeshProUGUI messageLabel;     // "You Win!" or "Game Over"
    public TextMeshProUGUI scoreLabel;       // Final kill count
    public TextMeshProUGUI bestScoreLabel;   // All-time best

    [Header("Is this the Win screen?")]
    public bool isWinScreen = true;

    // ---------------------------------------------------------------
    void Start()
    {
        Time.timeScale = 1f;    // Ensure unpaused

        if (messageLabel != null)
            messageLabel.text = isWinScreen ? "YOU WIN!" : "GAME OVER";

        int best = SaveSystem.LoadBestScore();
        if (bestScoreLabel != null)
            bestScoreLabel.text = $"Best Score: {best}";

        // Play appropriate music
        AudioManager.Instance?.PlayMusic(isWinScreen ? "WinMusic" : "LoseMusic");
    }

    // ---------------------------------------------------------------
    // Button: Play Again
    public void PlayAgain()
    {
        Debug.Log("PlayAgain clicked");

        SaveSystem.SaveCurrentLevel(0);

        GameManager.Instance?.StartGame(false);
    }

    // Button: Main Menu
    public void GoToMenu()
    {
        AudioManager.Instance?.PlaySFX("ButtonClick");
        AudioManager.Instance?.StopMusic();
        GameManager.Instance?.GoToMainMenu();
    }

    // Button: Quit
    public void QuitGame()
    {
        GameManager.Instance?.QuitGame();
    }
}
