using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    //public AudioClip clipToPlay;
    //public Transform target;

    public AudioClip clipToPlay;
    [Range(0.0f, 1.0f)]
    public float clipVolume;
    public float timeOfAudioFadeOut;
    public bool loopSound;

    AudioSource audioSource;

    public void PlaySound()
    {
        if(audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.clip = clipToPlay;
        audioSource.loop = loopSound;

        audioSource.volume = clipVolume;
        audioSource.Play();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / timeOfAudioFadeOut;

            yield return null;
        }

        audioSource.Stop();
    }

}
