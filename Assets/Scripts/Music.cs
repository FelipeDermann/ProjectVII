using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Music : MonoBehaviour
{
    public static Music Instance;

    [Header("References")]
    public AudioMixer mixer;
    public AudioSource audioSourceAmbient;
    public AudioSource audioSourceBattle;

    [Header("Fade Times")]
    public float timeOfFadeInAmbient;
    public float timeOfFadeOutAmbient;
    public float timeOfFadeInBattle;
    public float timeOfFadeOutBattle;

    [Header("Others")]
    public float timeOfVolumeDownFade;
    public float timeOfVolumeBackToNormalFade;
    [Range(-80f, 0f)]
    public float minimumVolume;
    public float timeToReturnToAmbientMusic;

    bool inBattle;
    public bool shopping;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSourceAmbient.Play();

        AudioListener.pause = false;

        mixer.SetFloat("BattleVolume", -80f);
        mixer.SetFloat("AmbientVolume", 0);
    }

    public void StartMusicFade(bool _state)
    {

        if (_state)
        {
            CancelInvoke();

            if (inBattle) return;
            if (shopping) return;

            StartCoroutine(nameof(FadeOutAmbient));
            StartCoroutine(nameof(FadeInBattle));

            inBattle = true;
        }
        else
        {
            Invoke(nameof(GetOutOfCombat), timeToReturnToAmbientMusic);
        }

    }

    public void TurnAllMusicVolumeDown()
    {
        shopping = true;
        StartCoroutine(nameof(TurnDownVolume));
    }

    public void NormalizeMusicVolume()
    {
        StartCoroutine(nameof(NormalizeVolume));
    }

    void GetOutOfCombat()
    {
        inBattle = false;
        StartCoroutine(nameof(FadeInAmbient));
        StartCoroutine(nameof(FadeOutBattle));
    }

    IEnumerator FadeOutAmbient()
    {
        float currentTime = 0;
        float currentVol;
        mixer.GetFloat("AmbientVolume", out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(-80, 0.0001f, 1);

        while (currentTime < timeOfFadeOutAmbient)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / timeOfFadeOutAmbient);
            mixer.SetFloat("AmbientVolume", Mathf.Log10(newVol) * 20);
            yield return null;
        }

        audioSourceAmbient.Pause();
        yield break;
    }

    IEnumerator FadeInAmbient()
    {
        audioSourceAmbient.UnPause();

        float currentTime = 0;
        float currentVol;
        mixer.GetFloat("AmbientVolume", out currentVol);

        float volTarget = 0;
        if (shopping) volTarget = minimumVolume;

        while (currentTime < timeOfFadeInAmbient)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, volTarget, currentTime / timeOfFadeInAmbient);
            mixer.SetFloat("AmbientVolume", newVol);
            yield return null;
        }

        yield break;
    }

    IEnumerator FadeInBattle()
    {
        audioSourceBattle.Play();
        StopCoroutine(nameof(FadeOutBattle));
        StopCoroutine(nameof(FadeInAmbient));

        float currentTime = 0;
        float currentVol;
        mixer.GetFloat("BattleVolume", out currentVol);

        while (currentTime < timeOfFadeInBattle)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, 0, currentTime / timeOfFadeInBattle);
            mixer.SetFloat("BattleVolume", newVol);
            yield return null;
        }

        yield break;
    }

    IEnumerator FadeOutBattle()
    {
        float currentTime = 0;
        float currentVol;
        mixer.GetFloat("BattleVolume", out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(-80, 0.0001f, 1);

        while (currentTime < timeOfFadeOutBattle)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / timeOfFadeOutBattle);
            mixer.SetFloat("BattleVolume", Mathf.Log10(newVol) * 20);
            yield return null;
        }

        audioSourceBattle.Stop();

        yield break;
    }

    IEnumerator TurnDownVolume()
    {
        float currentTime = 0;
        float currentVolBattle;
        float currentVolAmbient;
        mixer.GetFloat("BattleVolume", out currentVolBattle);
        mixer.GetFloat("AmbientVolume", out currentVolAmbient);

        while (currentTime < timeOfVolumeDownFade)
        {
            currentTime += Time.deltaTime;
            float newVolBattle = Mathf.Lerp(currentVolBattle, minimumVolume, currentTime / timeOfVolumeDownFade);
            float newVolAmbient = Mathf.Lerp(currentVolAmbient, minimumVolume, currentTime / timeOfVolumeDownFade);
            mixer.SetFloat("BattleVolume", newVolBattle);
            mixer.SetFloat("AmbientVolume", newVolAmbient);
            yield return null;
        }

        yield break;
    }

    IEnumerator NormalizeVolume()
    {
        float currentTime = 0;
        float currentVolBattle;
        float currentVolAmbient;
        mixer.GetFloat("BattleVolume", out currentVolBattle);
        mixer.GetFloat("AmbientVolume", out currentVolAmbient);

        float battleVolTarget = 0;
        float ambientVolTarget = -80;
        if (!inBattle)
        {
            ambientVolTarget = 0;
            battleVolTarget = -80;
        }

        while (currentTime < timeOfVolumeBackToNormalFade)
        {
            currentTime += Time.deltaTime;
            float newVolBattle = Mathf.Lerp(currentVolBattle, battleVolTarget, currentTime / timeOfVolumeBackToNormalFade);
            float newVolAmbient = Mathf.Lerp(currentVolAmbient, ambientVolTarget, currentTime / timeOfVolumeBackToNormalFade);
            mixer.SetFloat("BattleVolume", newVolBattle);
            mixer.SetFloat("AmbientVolume", newVolAmbient);
            yield return null;
        }

        shopping = false;

        yield break;
    }

}
