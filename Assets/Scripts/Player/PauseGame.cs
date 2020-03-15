using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool paused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            if (!paused)
            {
                paused = true;
                Pause();
            }
            else
            {
                paused = false;
                Unpause();
            }
        }
    }

    private void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    private void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
