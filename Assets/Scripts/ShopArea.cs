using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cinemachine;

public class ShopArea : MonoBehaviour
{
    public static event Action<PlayerStatus, ShopArea> PlayerInShop;

    [Header("Prices and Effects")]
    public int healthUpPrice;
    public int healthUpAmount;
    public int magnetPrice;

    [Header("!")]
    public GameObject exclamationCanvas;

    public bool playerInRange;
    public bool shopping;
    GameObject shopMenu;
    PlayerStatus playerStatus;
    CinemachineFreeLook cam;
    string camX;
    string camY;

    private void OnEnable()
    {
        ShopMenu.ExitShop += EndShop; 
    }

    private void OnDisable()
    {
        ShopMenu.ExitShop -= EndShop;
    }

    void Start()
    {
        shopMenu = GameObject.FindGameObjectWithTag("ShopMenu").transform.GetChild(0).gameObject;
        cam = GameObject.FindGameObjectWithTag("FreeLook").GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        StartShop();
    }

    void StartShop()
    {
        if (!Input.GetButtonDown("light") && playerInRange) return;
        if (!playerInRange) return;
        if (playerStatus.GetComponent<PlayerMovement>().dashing) return;
        if (shopping) return;
            
        Music.Instance.TurnAllMusicVolumeDown();
        shopMenu.SetActive(true);
        shopping = true;
        playerStatus.shopping = true;
        Cursor.lockState = CursorLockMode.None;
        
        camX = cam.m_XAxis.m_InputAxisName;
        camY = cam.m_YAxis.m_InputAxisName;
        
        cam.m_XAxis.m_InputAxisName = "";
        cam.m_YAxis.m_InputAxisName = "";
        
        cam.m_XAxis.m_InputAxisValue = 0;     
        cam.m_YAxis.m_InputAxisValue = 0;
        
        playerStatus.CanMoveState(false);

        PlayerInShop?.Invoke(playerStatus, this);
    }

    void EndShop()
    {
        Music.Instance.NormalizeMusicVolume();
        shopMenu.SetActive(false);
        playerStatus.shopping = false;
        Cursor.lockState = CursorLockMode.Locked;

        cam.m_XAxis.m_InputAxisName = camX;
        cam.m_YAxis.m_InputAxisName = camY;

        playerStatus.CanMoveState(true);

        shopping = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if(playerStatus == null) playerStatus = other.GetComponent<PlayerStatus>();
            playerStatus.CanAttackState(false);
            exclamationCanvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerStatus.CanAttackState(true);
            exclamationCanvas.SetActive(false);
        }
    }
}
