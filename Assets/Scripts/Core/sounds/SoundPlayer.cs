using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Footsteps")]
    [SerializeField] private AudioClip footstep_1;
    [SerializeField] private AudioClip footstep_2;

    [Header("Attacks")]
    [SerializeField] private AudioClip attackSound;

    [Header("Abilities")]
    [SerializeField] private AudioClip abilitySound;

    [Header("Hurt")]
    [SerializeField] private AudioClip hurtSound;

    [Header("TP")]
    [SerializeField] private AudioClip tpSound;

    private void Start()
    {
        if (VolumeManager.Instance != null)
        {
            VolumeManager.Instance.OnSFXVolumeChanged += OnSFXVolumeChanged;
            OnSFXVolumeChanged(VolumeManager.Instance.SFXVolume);
        }
    }

    private void OnDestroy()
    {
        if (VolumeManager.Instance != null)
        {
            VolumeManager.Instance.OnSFXVolumeChanged -= OnSFXVolumeChanged;
        }
    }

    private void OnSFXVolumeChanged(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    public void PlayFootstep1()
    {
        Debug.Log("Footstep 1");
        audioSource.PlayOneShot(footstep_1);
    }

    public void PlayFootstep2()
    {
        Debug.Log("Footstep 2");
        if (audioSource != null && footstep_2 != null)
        {
            audioSource.PlayOneShot(footstep_2);
        }
    }

    public void PlayAttack()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    public void PlayAbility()
    {
        if (abilitySound != null)
        {
            audioSource.PlayOneShot(abilitySound);
        }
    }

    public void PlayHurt()
    {
        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }

    public void PlayTP()
    {
        if (tpSound != null)
        {
            audioSource.PlayOneShot(tpSound);
        }
    }
}