using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Animator endMessage;
    PlayerStatus player;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStatus>();
    }

    private void Awake()
    {
        WaveSpawnerController.FinalWaveEnd += FadeOut;
        ScreenTransitions.FadeOutEnd += Message;
    }

    private void OnDestroy()
    {
        WaveSpawnerController.FinalWaveEnd -= FadeOut;
        ScreenTransitions.FadeOutEnd -= Message;
    }

    void FadeOut()
    {
        Invoke(nameof(StartFadeOut), 3);
    }

    void StartFadeOut()
    {
        ScreenTransitions.Instance.StartFade();
        player.CanAttackState(false);
        player.CanMoveState(false);
    }

    void Message()
    {
        AudioListener.pause = true;
        endMessage.SetTrigger("Start");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }
}
