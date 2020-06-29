using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenTransitions : MonoBehaviour
{
    public static ScreenTransitions Instance;

    public Animator transitionsAnimator;

    public static event Action FadeOutEnd;

    public GameObject loadingAnim;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
        }
    }

    public void StartFade()
    {
        transitionsAnimator.SetTrigger("Fade");
    }

    public void FadeOutEnded()
    {
        loadingAnim.SetActive(true);
        FadeOutEnd?.Invoke();
    }

}
