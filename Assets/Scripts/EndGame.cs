using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Animator endMessage;
    PlayerStatus player;
    public bool canEnd;

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
        if (!canEnd) return;
        Invoke(nameof(StartFadeOut), 3);
    }

    void StartFadeOut()
    {
        if (!canEnd) return;
        ScreenTransitions.Instance.StartFade();
        player.CanAttackState(false);
        player.CanMoveState(false);
    }

    void Message()
    {
        if (!canEnd) return;
        AudioListener.pause = true;
        endMessage.SetTrigger("Start");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canEnd = true;
        }
    }
}
