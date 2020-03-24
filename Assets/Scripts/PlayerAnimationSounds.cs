using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimationSounds : MonoBehaviour
{
    public static event Action StepSound;
    public static event Action SwingSound;
    public static event Action HeavySwingSound;
    public static event Action SlashSound;
    public static event Action DashSound;
    public static event Action HurtSound;
    public static event Action DeathSound;

    public void PlayStepSound()
    {
        StepSound?.Invoke();
    }

    public void PlaySwingSound()
    {
        SwingSound?.Invoke();
    }
    public void PlayHeavySwingSound()
    {
        HeavySwingSound?.Invoke();
    }

    public void PlaySlashSound()
    {
        SlashSound?.Invoke();
    }
    public void PlayDashSound()
    {
        DashSound?.Invoke();
    }

    public void PlayHurtSound()
    {
        HurtSound?.Invoke();
    }

    public void PlayDeathSound()
    {
        DeathSound?.Invoke();
    }
}
