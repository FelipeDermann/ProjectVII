using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    AudioSource audioSource;

    [Header("Basic Actions")]
    public AudioClip[] stepSounds;
    public AudioClip slashSound;
    public AudioClip swingSound;
    public AudioClip heavySwingSound;
    public AudioClip dashSound;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    [Header("Combos")]
    public AudioClip waterComboSound;
    public AudioClip fireComboSound;
    public AudioClip earthComboSound;
    public AudioClip metalComboSound;
    public AudioClip woodComboSound;

    [Header("Specials")]
    public AudioClip waterSpecialSound;
    public AudioClip fireSpecialSound;
    public AudioClip earthSpecialSound;
    public AudioClip metalSpecialSound;
    public AudioClip woodSpecialSound;

    int stepToPlayIndex;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stepToPlayIndex = 0;
    }

    private void OnEnable()
    {
        PlayerAnimationSounds.StepSound += PlayStepSound;
        PlayerAnimationSounds.SwingSound += PlaySwingSound;
        PlayerAnimationSounds.HeavySwingSound += PlayHeavySwingSound;
        PlayerAnimationSounds.SlashSound += PlaySlashSound;
        PlayerAnimationSounds.DashSound += PlayDashSound;
        PlayerAnimationSounds.HurtSound += PlayHurtSound;
        PlayerAnimationSounds.DeathSound += PlayDeathSound;
    }
    private void OnDisable()
    {
        PlayerAnimationSounds.StepSound -= PlayStepSound;
        PlayerAnimationSounds.SwingSound -= PlaySwingSound;
        PlayerAnimationSounds.HeavySwingSound -= PlayHeavySwingSound;
        PlayerAnimationSounds.SlashSound -= PlaySlashSound;
        PlayerAnimationSounds.DashSound += PlayDashSound;
        PlayerAnimationSounds.HurtSound -= PlayHurtSound;
        PlayerAnimationSounds.DeathSound -= PlayDeathSound;
    }

    void PlayStepSound()
    {
        audioSource.PlayOneShot(stepSounds[stepToPlayIndex]);

        stepToPlayIndex += 1;
        if (stepToPlayIndex > 1) stepToPlayIndex = 0;
    }

    public void PlaySlashSound()
    {
        audioSource.PlayOneShot(slashSound);
    }

    void PlaySwingSound()
    {
        audioSource.PlayOneShot(swingSound);
    }

    void PlayHeavySwingSound()
    {
        audioSource.PlayOneShot(heavySwingSound);
    }
    void PlayDashSound()
    {
        audioSource.PlayOneShot(dashSound);
    }

    void PlayHurtSound()
    {
        audioSource.PlayOneShot(hurtSound);
    }
    void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }
}
