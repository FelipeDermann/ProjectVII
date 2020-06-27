using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffStack : MonoBehaviour
{
    public Image buffStackImage;

    void Awake()
    {
        PlayerBuff.BuffRefreshed += StartTimer;
        PlayerBuff.BuffEnded += ResetTimer;
    }

    void OnDestroy()
    {
        PlayerBuff.BuffRefreshed -= StartTimer;
        PlayerBuff.BuffEnded -= ResetTimer;
    }

    void StartTimer(float _time)
    {
        StopAllCoroutines();
        buffStackImage.fillAmount = 1;
        if(gameObject.activeSelf) StartCoroutine(Timer(_time));
    }

    IEnumerator Timer(float _time)
    {
        float buffTime = _time;
        float iconTimer = _time;

        while (iconTimer > 0)
        {
            buffStackImage.fillAmount = iconTimer / buffTime;
            iconTimer -= Time.deltaTime;

            yield return null;
        }
    }

    void ResetTimer()
    {
        StopAllCoroutines();
        buffStackImage.fillAmount = 1;
    }
}
