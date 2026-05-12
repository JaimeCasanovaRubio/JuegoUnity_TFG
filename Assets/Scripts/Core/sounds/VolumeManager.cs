using System;
using UnityEngine;

/// <summary>
/// Singleton persistente que gestiona los niveles de volumen de Música y SFX.
/// Persiste los valores en PlayerPrefs y notifica a los suscriptores mediante eventos.
/// </summary>
public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance { get; private set; }

    public event Action<float> OnMusicVolumeChanged;
    public event Action<float> OnSFXVolumeChanged;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(GameConstants.PlayerPrefsKeys.MUSIC_VOLUME, musicVolume);
            PlayerPrefs.Save();
            OnMusicVolumeChanged?.Invoke(musicVolume);
        }
    }

    public float SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(GameConstants.PlayerPrefsKeys.SFX_VOLUME, sfxVolume);
            PlayerPrefs.Save();
            OnSFXVolumeChanged?.Invoke(sfxVolume);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null)
                transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            // Cargar volúmenes guardados de PlayerPrefs
            musicVolume = PlayerPrefs.GetFloat(GameConstants.PlayerPrefsKeys.MUSIC_VOLUME, 1f);
            sfxVolume = PlayerPrefs.GetFloat(GameConstants.PlayerPrefsKeys.SFX_VOLUME, 1f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
