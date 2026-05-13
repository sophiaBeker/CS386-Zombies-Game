using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton AudioManager.
/// Attach to a persistent GameObject. Assign AudioClips via the Inspector.
/// Call AudioManager.Instance.PlaySFX("key") or PlayMusic("key") from anywhere.
/// Volume is driven by SettingsManager.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class SoundEntry
    {
        public string    key;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float     baseVolume = 1f;
    }

    [Header("Sound Effects")]
    public SoundEntry[] sfxEntries;

    [Header("Music")]
    public SoundEntry[] musicEntries;

    // Internal sources
    private AudioSource _sfxSource;
    private AudioSource _musicSource;

    // Lookup dictionaries
    private Dictionary<string, SoundEntry> _sfxMap   = new();
    private Dictionary<string, SoundEntry> _musicMap = new();

    // ---------------------------------------------------------------
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create two AudioSources on this GameObject
        _sfxSource   = gameObject.AddComponent<AudioSource>();
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop = true;

        foreach (var e in sfxEntries)   _sfxMap[e.key]   = e;
        foreach (var e in musicEntries) _musicMap[e.key] = e;

        ApplyVolumes();
    }

    // ---------------------------------------------------------------
    public void PlaySFX(string key)
    {
        if (!_sfxMap.TryGetValue(key, out var entry)) return;
        float vol = entry.baseVolume * SettingsManager.SFXVolume;
        _sfxSource.PlayOneShot(entry.clip, vol);
    }

    public void PlayMusic(string key)
    {
        if (!_musicMap.TryGetValue(key, out var entry)) return;
        if (_musicSource.clip == entry.clip && _musicSource.isPlaying) return;
        _musicSource.clip   = entry.clip;
        _musicSource.volume = entry.baseVolume * SettingsManager.MusicVolume;
        _musicSource.Play();
    }

    public void StopMusic() => _musicSource.Stop();

    // Called by SettingsManager whenever volume changes
    public void ApplyVolumes()
    {
        _sfxSource.volume   = SettingsManager.SFXVolume;
        _musicSource.volume = SettingsManager.MusicVolume;
    }
}
