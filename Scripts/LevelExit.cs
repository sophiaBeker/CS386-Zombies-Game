using UnityEngine;

/// <summary>
/// Place this on a trigger zone at the edge of each arena.
/// When the player walks into it, the next level loads (if kill target is met).
/// If kill target isn't met yet, shows a message prompting the player to kill more zombies.
/// </summary>
public class LevelExit : MonoBehaviour
{
    [Header("Optional - UI Message")]
    public GameObject notReadyMessage;   // Assign a UI Text/Panel to show if kills not met
    public float      messageDisplayTime = 2f;

    private bool _messageShowing = false;

    private bool _activated = false;

    // ---------------------------------------------------------------
    void Start()
    {
        // Make sure the not-ready message is hidden at start
        if (notReadyMessage != null)
            notReadyMessage.SetActive(false);
    }

    // ---------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (_activated) return;
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance == null) return;

        // Check if kill target is met
        if (GameManager.Instance.ZombieKills >= GameManager.Instance.ZombiesToKill)
        {
            // Advance to next level
            _activated = true;
            Time.timeScale = 1f;
            GameManager.Instance.AdvanceLevel();
        }
        else
        {
            // Show "not ready" message if kills not met
            if (!_messageShowing)
                StartCoroutine(ShowNotReadyMessage());
        }
    }

    System.Collections.IEnumerator ShowNotReadyMessage()
    {
        _messageShowing = true;
        if (notReadyMessage != null) notReadyMessage.SetActive(true);
        yield return new WaitForSecondsRealtime(messageDisplayTime);
        if (notReadyMessage != null) notReadyMessage.SetActive(false);
        _messageShowing = false;
    }
}