using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages and persists audio settings (master, music, SFX volume).
/// Uses PlayerPrefs so settings survive between sessions.
/// Attach to the Settings panel in your scene; hook sliders in the Inspector.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    // Static accessors used by AudioManager
    public static float MasterVolume { get; private set; } = 1f;
    public static float MusicVolume  { get; private set; } = 1f;
    public static float SFXVolume    { get; private set; } = 1f;

    // PlayerPrefs keys
    private const string KEY_MASTER = "vol_master";
    private const string KEY_MUSIC  = "vol_music";
    private const string KEY_SFX    = "vol_sfx";

    [Header("UI Sliders (assign in Inspector)")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    // ---------------------------------------------------------------
    void Awake()
    {
        LoadSettings();
    }

    void Start()
    {
        // Initialize slider positions to match saved values
        if (masterSlider != null) { masterSlider.value = MasterVolume; masterSlider.onValueChanged.AddListener(SetMasterVolume); }
        if (musicSlider  != null) { musicSlider.value  = MusicVolume;  musicSlider.onValueChanged.AddListener(SetMusicVolume);  }
        if (sfxSlider    != null) { sfxSlider.value    = SFXVolume;    sfxSlider.onValueChanged.AddListener(SetSFXVolume);    }
    }

    // ---------------------------------------------------------------
    public void SetMasterVolume(float value)
    {
        MasterVolume = value;
        AudioListener.volume = value;   // Unity global volume
        PlayerPrefs.SetFloat(KEY_MASTER, value);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        PlayerPrefs.SetFloat(KEY_MUSIC, value);
        PlayerPrefs.Save();
        AudioManager.Instance?.ApplyVolumes();
    }

    public void SetSFXVolume(float value)
    {
        SFXVolume = value;
        PlayerPrefs.SetFloat(KEY_SFX, value);
        PlayerPrefs.Save();
        AudioManager.Instance?.ApplyVolumes();
    }

    // ---------------------------------------------------------------
    static void LoadSettings()
    {
        MasterVolume = PlayerPrefs.GetFloat(KEY_MASTER, 1f);
        MusicVolume  = PlayerPrefs.GetFloat(KEY_MUSIC,  1f);
        SFXVolume    = PlayerPrefs.GetFloat(KEY_SFX,    1f);

        AudioListener.volume = MasterVolume;
    }
}
