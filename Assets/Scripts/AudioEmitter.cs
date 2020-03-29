using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    //public AudioClip clipToPlay;
    public Transform target;

    float audiofadeTime;
    AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        if (target != null) transform.position = target.position; 
    }

    public void PlaySound(float _volume, AudioClip _audioCLip, bool _loop)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = _audioCLip;
        audioSource.loop = _loop;

        audioSource.volume = _volume;
        audioSource.Play();
    }

    public void DeathCountdown(float _time, float _audioFadeTime)
    {
        audiofadeTime = _audioFadeTime;
        Invoke(nameof(FadeOutAndDieCall), _time);
    }

    void FadeOutAndDieCall()
    {
        StartCoroutine(FadeOutAndDieCoroutine());
    }

    IEnumerator FadeOutAndDieCoroutine()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / audiofadeTime;

            yield return null;
        }

        Destroy(gameObject);
    }

}
