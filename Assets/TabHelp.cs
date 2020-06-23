using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabHelp : MonoBehaviour
{
    public GameObject helpUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("LockOnController") <= -0.8f || Input.GetKey(KeyCode.Tab))
        {
            if (helpUI.activeSelf) return;
            else helpUI.SetActive(true);
        }
        else helpUI.SetActive(false);

    }
}
