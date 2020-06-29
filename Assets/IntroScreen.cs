using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") || Input.GetButtonDown("dash") || Input.GetButtonDown("light") || Input.GetButtonDown("heavy"))
        {
            SkipCutscene();
        }
    }

    void SkipCutscene()
    {
        SceneManager.LoadScene("Title");
    }
}
