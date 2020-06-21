using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject creditsWindow;

    private void Awake()
    {
        ScreenTransitions.FadeOutEnd += LoadLevel;

    }
    private void OnDestroy()
    {
        ScreenTransitions.FadeOutEnd -= LoadLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("dash"))
        {
            if (creditsWindow.activeSelf) Credits();
            else Application.Quit();
        }
        if (Input.GetButtonDown("Pause") || Input.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.Return))
        {
            if (creditsWindow.activeSelf) return;

        }
        if (Input.GetButtonDown("Credits"))
        {
            Credits();
        }
    }

    public void Credits()
    {
        if (creditsWindow.activeSelf) creditsWindow.SetActive(false);
        else creditsWindow.SetActive(true);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
