using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Conecta los sliders de volumen de la UI con el VolumeManager.
/// Se debe asignar a un GameObject que contenga o referencie los sliders de Música y SFX.
/// </summary>
public class VolumeSliderUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        InitializeSliders();
    }

    private void OnEnable()
    {
        // Sincronizar sliders cada vez que el panel se muestre
        // (ej: al abrir el menú de Settings durante el gameplay)
        if (VolumeManager.Instance != null)
        {
            if (musicSlider != null) musicSlider.value = VolumeManager.Instance.MusicVolume;
            if (sfxSlider != null) sfxSlider.value = VolumeManager.Instance.SFXVolume;
        }
    }

    private void InitializeSliders()
    {
        if (VolumeManager.Instance == null) return;

        if (musicSlider != null)
        {
            musicSlider.value = VolumeManager.Instance.MusicVolume;
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = VolumeManager.Instance.SFXVolume;
            sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
        }
    }

    private void OnMusicSliderChanged(float value)
    {
        if (VolumeManager.Instance != null)
            VolumeManager.Instance.MusicVolume = value;
    }

    private void OnSFXSliderChanged(float value)
    {
        if (VolumeManager.Instance != null)
            VolumeManager.Instance.SFXVolume = value;
    }

    private void OnDestroy()
    {
        if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
        if (sfxSlider != null) sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
    }
}
