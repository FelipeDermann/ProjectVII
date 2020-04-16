using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopArea : MonoBehaviour
{
    public bool playerInRange;
    public bool shopping;
    public GameObject shopMenu;

    void Start()
    {
        shopMenu = GameObject.FindGameObjectWithTag("ShopMenu").transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Input.GetButtonDown("light") && playerInRange)
        {
            if (shopping)
            {
                Music.Instance.NormalizeMusicVolume();
                shopMenu.SetActive(false);
                shopping = false;
            }
            else
            {
                Music.Instance.TurnAllMusicVolumeDown();
                shopMenu.SetActive(true);
                shopping = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
