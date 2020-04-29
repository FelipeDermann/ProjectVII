using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    int stepToPlayIndex;

    public void PlayStepSound()
    {
        PoolableObject audioEmitter = null;

        if (stepToPlayIndex == 0)
        {
            audioEmitter = GameManager.Instance.AudioEnemyStep1Pool.RequestObject(transform.position, transform.rotation);
            var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

            emitterScript.PlaySoundWithPitch();
            float clipLength = emitterScript.clipToPlay.length;

            GameManager.Instance.AudioEnemyStep1Pool.ReturnObject(audioEmitter, clipLength + 1);
        }

        if (stepToPlayIndex == 1)
        {
            audioEmitter = GameManager.Instance.AudioEnemyStep2Pool.RequestObject(transform.position, transform.rotation);
            var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

            emitterScript.PlaySoundWithPitch();
            float clipLength = emitterScript.clipToPlay.length;

            GameManager.Instance.AudioEnemyStep2Pool.ReturnObject(audioEmitter, clipLength + 1);
        }

        stepToPlayIndex += 1;
        if (stepToPlayIndex > 1) stepToPlayIndex = 0;
    }

    public void PlayDeathSound()
    {
        var audioEmitter = GameManager.Instance.AudioEnemyDeathPool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioEnemyDeathPool.ReturnObject(audioEmitter, clipLength + 1);
    }

    public void PlayAttackSound()
    {
        var audioEmitter = GameManager.Instance.AudioEnemyAttackPool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioEnemyAttackPool.ReturnObject(audioEmitter, clipLength + 1);
    }

    public void PlayHurtSound()
    {
        var audioEmitter = GameManager.Instance.AudioEnemyHurtPool.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioEnemyHurtPool.ReturnObject(audioEmitter, clipLength + 1);
    }
}
