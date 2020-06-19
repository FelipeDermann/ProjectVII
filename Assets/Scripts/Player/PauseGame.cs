using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseGame : MonoBehaviour
{
    public AudioMixer mixer;
    public GameObject pauseMenu;
    public bool paused;
    PlayerStatus playerStatus;

    //event system
    public GameObject firstMenuButton;
    EventSystem eventSystem;

    private void Awake()
    {
        if (playerStatus == null) playerStatus = GameObject.FindObjectOfType<PlayerStatus>();
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            if (playerStatus == null) return;
            if (playerStatus.shopping) return;

            if (!paused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }

    private void Pause()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        eventSystem.SetSelectedGameObject(firstMenuButton);

        AudioListener.pause = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    private void Unpause()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        AudioListener.pause = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void MasterVolume(float _sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10 (_sliderValue) * 20);
    }

    public void MusicVolume(float _sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(_sliderValue) * 20);
    }

    public void SoundEffectsVolume(float _sliderValue)
    {
        mixer.SetFloat("SoundEffectVolume", Mathf.Log10(_sliderValue) * 20);
    }

    public void Resume()
    {
        Unpause();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Title");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
