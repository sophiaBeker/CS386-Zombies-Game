using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the Main Menu scene.
/// Assign button OnClick events to the public methods below.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;

    [Header("Best Score Display")]
    public TextMeshProUGUI bestScoreLabel;

    // ---------------------------------------------------------------
    void Start()
    {
        // Display best score
        int best = SaveSystem.LoadBestScore();
        if (bestScoreLabel != null)
            bestScoreLabel.text = best > 0 ? $"Best Score: {best}" : "";

        // Play menu music
        AudioManager.Instance?.PlayMusic("MenuTheme");

        // Make sure correct panels are active
        if (mainPanel    != null) mainPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // Ensure timescale is normal (could be leftover from a paused quit)
        Time.timeScale = 1f;
    }

    // ---------------------------------------------------------------
    // Button: Play
    public void PlayGame()
    {
        AudioManager.Instance?.PlaySFX("ButtonClick");
        AudioManager.Instance?.StopMusic();
        GameManager.Instance?.StartGame(false);
    }

    // Button: Open Settings
    public void OpenSettings()
    {
        AudioManager.Instance?.PlaySFX("ButtonClick");
        if (mainPanel     != null) mainPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    // Button: Back from Settings
    public void CloseSettings()
    {
        AudioManager.Instance?.PlaySFX("ButtonClick");
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainPanel     != null) mainPanel.SetActive(true);
    }

    // Button: Quit
    public void QuitGame()
    {
        AudioManager.Instance?.PlaySFX("ButtonClick");
        GameManager.Instance?.QuitGame();
    }

    void Update()
    {
        // If settings panel is open, ESC closes it and returns to main panel
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
                CloseSettings();
        }
    }
}
