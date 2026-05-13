using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton GameManager. Supports multiple levels with individual kill targets.
/// Lose condition: 5 hits taken (persists across levels).
/// Win condition: complete all levels.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Win / Lose Settings")]
    public int maxHits = 5;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string winScene      = "WinScene";
    public string loseScene     = "LoseScene";

    [Header("Levels (in order)")]
    public LevelData[] levels;

    // Runtime state
    private int  _zombieKills  = 0;
    private int  _hitsTaken    = 0;
    private bool _gameOver     = false;
    private int  _currentLevel = 0;

    // Properties read by UIManager
    public int ZombieKills   => _zombieKills;
    public int HitsTaken     => _hitsTaken;
    public int CurrentLevel  => _currentLevel;
    public int ZombiesToKill => levels != null && levels.Length > 0
                                ? levels[_currentLevel].zombiesToKill : 10;

    // ---------------------------------------------------------------
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuScene)
        {
            AudioManager.Instance?.PlayMusic("MenuTheme");
            return;
        }

        if (scene.name == winScene || scene.name == loseScene) return;

        AudioManager.Instance?.PlayMusic("GameplayMusic");
        StartCoroutine(RefreshHudNextFrame());
    }

    IEnumerator RefreshHudNextFrame()
    {
        yield return null;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKills(_zombieKills);
            UIManager.Instance.UpdateHits(_hitsTaken);
        }
    }

    // ---------------------------------------------------------------
    public void OnZombieKilled()
    {
        if (_gameOver) return;
        _zombieKills++;
        UIManager.Instance?.UpdateKills(_zombieKills);
        AudioManager.Instance?.PlaySFX("ZombieDeath");

        if (_zombieKills >= ZombiesToKill) TriggerLevelComplete();
    }

    public void OnPlayerHit()
    {
        if (_gameOver) return;
        _hitsTaken++;
        UIManager.Instance?.UpdateHits(_hitsTaken);
        AudioManager.Instance?.PlaySFX("PlayerHurt");

        if (_hitsTaken >= maxHits) TriggerLose();
    }

    // ---------------------------------------------------------------
    void TriggerLevelComplete()
    {
        // Kill target met — unlock the exit but don't auto-advance
        // The player must walk to the exit zone (LevelExit.cs handles this)
        UIManager.Instance?.ShowExitPrompt();
    }

    // Called by LevelExit.cs when the player walks into the exit trigger
    public void AdvanceLevel()
    {
        Time.timeScale = 1f;

        if (_currentLevel + 1 < levels.Length)
        {
            _currentLevel++;

            //SAVE PROGRESS HERE
            SaveSystem.SaveCurrentLevel(_currentLevel);

            _zombieKills = 0;
            _gameOver    = false;

            SceneManager.LoadScene(levels[_currentLevel].sceneName);
        }
        else
        {
            TriggerFinalWin();
        }
    }

    void TriggerFinalWin()
    {
        // Save best score
        int totalKills = 0;
        foreach (var l in levels) totalKills += l.zombiesToKill;
        int best = SaveSystem.LoadBestScore();
        if (totalKills > best) SaveSystem.SaveBestScore(totalKills);

        //Reset level progress
        Debug.Log("Resetting saved level to 0");
        SaveSystem.SaveCurrentLevel(0);

        StartCoroutine(LoadSceneAfterDelay(winScene, 0f));
    }

    void TriggerLose()
    {
        _gameOver = true;
        SaveSystem.SaveCurrentLevel(0);
        StartCoroutine(LoadSceneAfterDelay(loseScene, 0f));
    }

    IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetState();
        SceneManager.LoadScene(sceneName);
    }

    // ---------------------------------------------------------------
    public void StartGame(bool useSave = false)
    {
        Debug.Log("StartGame called");

        ResetState();

        if (useSave)
        {
            _currentLevel = SaveSystem.LoadCurrentLevel();
            Debug.Log("Using saved level: " + _currentLevel);
        }
        else
        {
            _currentLevel = 0;
            Debug.Log("Forcing level 0");
        }

        SceneManager.LoadScene(levels[_currentLevel].sceneName);
    }

    public void GoToMainMenu()
    {
        ResetState();
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    void ResetState()
    {
        _zombieKills  = 0;
        _hitsTaken    = 0;
        _gameOver     = false;
        _currentLevel = 0;
    }

    // ---------------------------------------------------------------
    public static GameManager GetOrCreate()
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("GameManager");
            go.AddComponent<GameManager>();
        }
        return Instance;
    }
}

/// <summary>
/// Holds per-level configuration. Set these up in the GameManager Inspector.
/// </summary>
[System.Serializable]
public class LevelData
{
    public string sceneName;
    public int    zombiesToKill = 10;
}