using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [Header("Sounds that loop")]
    public AudioSource[] loopSounds; 

    [Header("Pools with sounds to play")]
    public ObjectPool StepSound1;
    public ObjectPool StepSound2;
    public ObjectPool DeathSound;
    public ObjectPool AttackSound;
    public ObjectPool HurtSound;

    int stepToPlayIndex;

    public void StartLoopSounds()
    {
        if (loopSounds.Length < 1) return;
        foreach (AudioSource source in loopSounds)
        {
            source.Play();
        }
    }

    public void StopLoopSounds()
    {
        if (loopSounds.Length < 1) return;
        foreach (AudioSource source in loopSounds)
        {
            source.Stop();
        }
    }

    public void PlayStepSound()
    {
        PoolableObject audioEmitter = null;

        if (stepToPlayIndex == 0)
        {
            audioEmitter = StepSound1.RequestObject(transform.position, transform.rotation);
            var audioEmitterReturn = audioEmitter.GetComponent<ReturnToPool>();
            var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

            emitterScript.PlaySoundWithPitch();
            float clipLength = emitterScript.clipToPlay.length;

            audioEmitterReturn.ReturnObjectToPool(StepSound1, clipLength + 1);
        }

        if (stepToPlayIndex == 1)
        {
            audioEmitter = StepSound2.RequestObject(transform.position, transform.rotation);
            var audioEmitterReturn = audioEmitter.GetComponent<ReturnToPool>();
            var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

            emitterScript.PlaySoundWithPitch();
            float clipLength = emitterScript.clipToPlay.length;

            audioEmitterReturn.ReturnObjectToPool(StepSound2, clipLength + 1);
        }

        stepToPlayIndex += 1;
        if (stepToPlayIndex > 1) stepToPlayIndex = 0;
    }

    public void PlayDeathSound()
    {
        var audioEmitter = DeathSound.RequestObject(transform.position, transform.rotation);
        var audioEmitterReturn = audioEmitter.GetComponent<ReturnToPool>();
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        audioEmitterReturn.ReturnObjectToPool(DeathSound, clipLength + 1);
    }

    public void PlayAttackSound()
    {
        var audioEmitter = AttackSound.RequestObject(transform.position, transform.rotation);
        var audioEmitterReturn = audioEmitter.GetComponent<ReturnToPool>();
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        audioEmitterReturn.ReturnObjectToPool(AttackSound, clipLength + 1);
    }

    public void PlayHurtSound()
    {
        var audioEmitter = HurtSound.RequestObject(transform.position, transform.rotation);
        var audioEmitterReturn = audioEmitter.GetComponent<ReturnToPool>();
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        audioEmitterReturn.ReturnObjectToPool(HurtSound, clipLength + 1);
    }
}
