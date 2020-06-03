using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimationSounds : MonoBehaviour
{
    int stepToPlayIndex = 0;

    public void PlayStepSound()
    {
        PoolableObject audioEmitter = null;

        if (stepToPlayIndex == 0)
        {
            audioEmitter = GameManager.Instance.AudioStepPool.RequestObject(transform.position, transform.rotation);
            var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

            emitterScript.PlaySoundWithPitch();
            float clipLength = emitterScript.clipToPlay.length;

            GameManager.Instance.AudioStepPool.ReturnObject(audioEmitter, clipLength + 1);
        }

        if (stepToPlayIndex == 1)
        {
            audioEmitter = GameManager.Instance.AudioStep2Pool.RequestObject(transform.position, transform.rotation);
            var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

            emitterScript.PlaySoundWithPitch();
            float clipLength = emitterScript.clipToPlay.length;

            GameManager.Instance.AudioStep2Pool.ReturnObject(audioEmitter, clipLength + 1);
        }

        stepToPlayIndex += 1;
        if (stepToPlayIndex > 1) stepToPlayIndex = 0;
    }

    public void PlaySwingSound()
    {
        var audioEmitter = GameManager.Instance.AudioLightAttackPool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioLightAttackPool.ReturnObject(audioEmitter, clipLength + 1);
    }
    public void PlaySwingSound2()
    {
        var audioEmitter = GameManager.Instance.AudioLightAttack2Pool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioLightAttack2Pool.ReturnObject(audioEmitter, clipLength + 1);
    }
    public void PlaySwingSound3()
    {
        var audioEmitter = GameManager.Instance.AudioLightAttack3Pool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioLightAttack3Pool.ReturnObject(audioEmitter, clipLength + 1);
    }

    public void PlayHeavySwingSound()
    {
        var audioEmitter = GameManager.Instance.AudioHeavyAttack1Pool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioHeavyAttack1Pool.ReturnObject(audioEmitter, clipLength + 1);
    }
    public void PlayHeavySwingSound2()
    {
        var audioEmitter = GameManager.Instance.AudioHeavyAttack2Pool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioHeavyAttack2Pool.ReturnObject(audioEmitter, clipLength + 1);
    }
    public void PlayHeavySwingSound3()
    {
        var audioEmitter = GameManager.Instance.AudioHeavyAttack3Pool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioHeavyAttack3Pool.ReturnObject(audioEmitter, clipLength + 1);
    }

    public void PlayDashSound()
    {
        var audioEmitter = GameManager.Instance.AudioDashPool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioDashPool.ReturnObject(audioEmitter, clipLength + 1);
    }

    public void PlayHurtSound()
    {
        int randomNumber = UnityEngine.Random.Range(0,2);
        if (randomNumber == 0)
        {
            var audioEmitter = GameManager.Instance.AudioHurtPool.RequestObject(transform.position, transform.rotation);
            var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

            emitterScript.PlaySoundWithPitch();
            float clipLength = emitterScript.clipToPlay.length;

            GameManager.Instance.AudioHurtPool.ReturnObject(audioEmitter, clipLength + 1);
        }
        else
        {
            var audioEmitter = GameManager.Instance.AudioHurt2Pool.RequestObject(transform.position, transform.rotation);
            var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

            emitterScript.PlaySoundWithPitch();
            float clipLength = emitterScript.clipToPlay.length;

            GameManager.Instance.AudioHurt2Pool.ReturnObject(audioEmitter, clipLength + 1);
        }
    }

    public void PlayDeathSound()
    {
        var audioEmitter = GameManager.Instance.AudioDeathPool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioDeathPool.ReturnObject(audioEmitter, clipLength + 1);
    }
}
