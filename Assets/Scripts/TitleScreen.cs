using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject creditsWindow;
    public GameObject quitWindow;
    public GameObject loadingText;
    public GameObject creditsButton;
    //      public GameObject[] 

    private void Awake()
    {
        ScreenTransitions.FadeOutEnd += LoadLevel;
        Cursor.visible = true;
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
            else ToggleQuit();
        }
        if (Input.GetButtonDown("Pause") /*|| Input.GetButtonDown("Interact")*/ || Input.GetKeyDown(KeyCode.Return))
        {
            if (creditsWindow.activeSelf) return;
            if (quitWindow.activeSelf) return;
            ScreenTransitions.Instance.StartFade();
        }
        if (Input.GetButtonDown("Credits"))
        {
            Credits();
        }

        if (Input.GetButtonDown("Interact") && quitWindow.activeSelf) QuitGame();
    }

    public void Credits()
    {
        if (quitWindow.activeSelf) return;
        if (creditsWindow.activeSelf)
        {
            creditsButton.SetActive(true);
            creditsWindow.SetActive(false);
        }
        else
        {
            creditsButton.SetActive(false);
            creditsWindow.SetActive(true);
        }
    }

    void LoadLevel()
    {
        loadingText.SetActive(true);
        SceneManager.LoadScene(1);
    }


    public void ToggleQuit()
    {
        if (creditsWindow.activeSelf) return;
        quitWindow.SetActive(!quitWindow.activeSelf);
    }

    public void QuitGame()
    {
        Debug.Log("asoidhnjauodjasn");
        Application.Quit();
    }
}
