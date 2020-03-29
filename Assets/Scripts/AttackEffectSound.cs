using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectSound : MonoBehaviour
{
    [Header("Customizable variables")]
    public AudioClip clipToPlay;
    [Range(0.0f, 1.0f)]
    public float clipVolume;
    public float timeToDestroyEmitterAfterDeath;
    public float timeOfAudioFadeOut;
    public bool loopSound;

    [Header("Prefab to spawn. Do not alter this.")]
    public GameObject audioEmitterPrefab;
    GameObject emitter;

    // Start is called before the first frame update
    void Start()
    {
        emitter = Instantiate(audioEmitterPrefab, transform.position, transform.rotation);
        emitter.GetComponent<AudioEmitter>().PlaySound(clipVolume, clipToPlay, loopSound);
    }

    private void OnDestroy()
    {
        emitter.GetComponent<AudioEmitter>().DeathCountdown(timeToDestroyEmitterAfterDeath, timeOfAudioFadeOut);
    }
}
