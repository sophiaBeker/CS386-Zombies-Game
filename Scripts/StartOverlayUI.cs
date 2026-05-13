using UnityEngine;

public class StartOverlayUI : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        // Pause the game at start
        Time.timeScale = 0f;

        if (panel != null)
            panel.SetActive(true);
    }

    public void StartGame()
    {
        // Hide UI and resume game
        if (panel != null)
            panel.SetActive(false);

        Time.timeScale = 1f;
    }
}