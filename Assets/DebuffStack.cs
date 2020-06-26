using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffStack : MonoBehaviour
{
    public Image debuffStackImage;

    public void StartTimer(float _time)
    {
        StopAllCoroutines();
        debuffStackImage.fillAmount = 1;
        StartCoroutine(Timer(_time));
    }

    IEnumerator Timer(float _time)
    {
        float buffTime = _time;
        float iconTimer = _time;

        while (iconTimer > 0)
        {
            debuffStackImage.fillAmount = iconTimer / buffTime;
            iconTimer -= Time.deltaTime;

            yield return null;
        }
    }

    public void ResetTimer()
    {
        StopAllCoroutines();
        debuffStackImage.fillAmount = 1;
    }
}
