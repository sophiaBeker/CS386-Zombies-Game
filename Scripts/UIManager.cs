using TMPro;
using UnityEngine;

/// <summary>
/// Manages the in-game HUD.
/// Assign the TextMeshPro labels in the Inspector.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD Labels")]
    public TextMeshProUGUI killsLabel;
    public TextMeshProUGUI hitsLabel;
    public TextMeshProUGUI levelLabel;
    public TextMeshProUGUI bestScoreLabel;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            UpdateKills(GameManager.Instance.ZombieKills);
            UpdateHits(GameManager.Instance.HitsTaken);
            UpdateLevel();
        }

        int best = SaveSystem.LoadBestScore();
        if (bestScoreLabel != null)
            bestScoreLabel.text = $"Best: {best}";
    }

    // ---------------------------------------------------------------
    public void UpdateKills(int kills)
    {
        int target = GameManager.Instance != null ? GameManager.Instance.ZombiesToKill : 10;
        if (killsLabel != null)
            killsLabel.text = $"Kills: {kills} / {target}";
    }

    public void UpdateHits(int hitsTaken)
    {
        int livesLeft = Mathf.Max(0, GameManager.Instance.maxHits - hitsTaken);
        hitsLabel.text = "Lives: " + livesLeft;
    }

    public void UpdateLevel()
    {
        if (GameManager.Instance == null || levelLabel == null) return;
        int display = GameManager.Instance.CurrentLevel + 1;
        int total   = GameManager.Instance.levels?.Length ?? 1;
        levelLabel.text = $"Level: {display} / {total}";
    }

    public void ShowExitPrompt()
    {
        // You can optionally add a UI label here like "Head to the exit!"
        // For now just update the kills label to show completion
        if (killsLabel != null)
            killsLabel.text = "All zombies killed! Find the exit!";
    }
}