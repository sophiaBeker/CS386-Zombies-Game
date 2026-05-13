using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles pausing the game with Escape.
/// Assign the pause panel GameObject in the Inspector.
/// Buttons on the panel call the public methods below.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject settingsPanel;   // Optional: nested settings inside pause

    private bool _isPaused = false;

    private bool _inputReady = false;

    void Start()
    {
        SetPaused(false);
        StartCoroutine(EnableInputNextFrame());
    }

    IEnumerator EnableInputNextFrame()
    {
        yield return new WaitForSeconds(0.1f);
        _inputReady = true;
    }

    // ---------------------------------------------------------------
    void Update()
    {
        if (!_inputReady) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    // ---------------------------------------------------------------
    public void TogglePause()
    {
        _isPaused = !_isPaused;
        SetPaused(_isPaused);
    }

    void SetPaused(bool paused)
    {
        _isPaused      = paused;
        Time.timeScale = paused ? 0f : 1f;
        if (pausePanel != null) pausePanel.SetActive(paused);
        if (!paused && settingsPanel != null) settingsPanel.SetActive(false);
    }

    // ---------------------------------------------------------------
    // Button: Resume
    public void Resume()  => SetPaused(false);

    // Button: Open Settings (inside pause menu)
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // Button: Quit to Main Menu
    public void QuitToMenu()
    {
        SetPaused(false);
        GameManager.GetOrCreate().GoToMainMenu();
    }

    // Button: Quit Application
    public void QuitGame()
    {
        GameManager.GetOrCreate().QuitGame();
    }
}
