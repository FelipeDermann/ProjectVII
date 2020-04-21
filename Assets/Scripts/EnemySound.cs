using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip[] stepSounds;

    public AudioClip hurtSound;
    public AudioClip deathSound;

    public AudioClip attackSound;

    [Header("Volumes")]
    [Range(0.0f, 1.0f)]
    public float stepsVolume;
    [Range(0.0f, 1.0f)]
    public float attackVolume;
    [Range(0.0f, 1.0f)]
    public float hurtVolume;
    [Range(0.0f, 1.0f)]
    public float deathVolume;

    [SerializeField]
    AudioSource audioSource;
    int stepToPlayIndex;

    public void PlayStepSound()
    {
        audioSource.PlayOneShot(stepSounds[stepToPlayIndex], stepsVolume);

        stepToPlayIndex += 1;
        if (stepToPlayIndex > 1) stepToPlayIndex = 0;
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound, deathVolume);
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound, attackVolume);
    }

    public void PlayHurtSound()
    {
        audioSource.PlayOneShot(hurtSound, hurtVolume);
    }
}
