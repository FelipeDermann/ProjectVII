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

    [Header("Basic Sounds Volumes")]
    [Range(0.0f, 1.0f)]
    public float stepsVolume;
    [Range(0.0f, 1.0f)]
    public float slashVolume;
    [Range(0.0f, 1.0f)]
    public float swingVolume;
    [Range(0.0f, 1.0f)]
    public float swingHeavyVolume;
    [Range(0.0f, 1.0f)]
    public float dashVolume;
    [Range(0.0f, 1.0f)]
    public float hurtVolume;
    [Range(0.0f, 1.0f)]
    public float deathVolume;

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
        //audioSource.volume = stepsVolume;
        audioSource.PlayOneShot(stepSounds[stepToPlayIndex], stepsVolume);

        stepToPlayIndex += 1;
        if (stepToPlayIndex > 1) stepToPlayIndex = 0;
    }

    public void PlaySlashSound()
    {
        //audioSource.volume = slashVolume;
        audioSource.PlayOneShot(slashSound, slashVolume);
    }

    void PlaySwingSound()
    {
        //audioSource.volume = swingVolume;
        //audioSource.PlayOneShot(swingSound, swingVolume);
        //GameManager.Instance.WaterComboPool.RequestObject(transform.position, transform.rotation);
        var audioEmitter = GameManager.Instance.AudioLightAttackPool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.EarthComboPool.ReturnObject(audioEmitter, clipLength+1);
    }

    void PlayHeavySwingSound()
    {
        //audioSource.volume = swingHeavyVolume;
        audioSource.PlayOneShot(heavySwingSound, swingHeavyVolume);
    }
    void PlayDashSound()
    {
        //audioSource.volume = dashVolume;
        audioSource.PlayOneShot(dashSound, dashVolume);
    }

    void PlayHurtSound()
    {
        //audioSource.volume = hurtVolume;
        audioSource.PlayOneShot(hurtSound, hurtVolume);
    }
    void PlayDeathSound()
    {
        //audioSource.volume = deathVolume;
        audioSource.PlayOneShot(deathSound, deathVolume);
    }
}
