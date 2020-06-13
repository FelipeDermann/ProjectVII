using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cinemachine;

public class ShopArea : MonoBehaviour
{
    public static event Action<PlayerStatus, ShopArea> PlayerInShop;

    [Header("Dialogues")]
    public Dialogue dialogueBeforeShop;
    public Dialogue dialogueAfterShop;

    [Header("Prices")]
    public int healthUpPrice;
    public int magnetPrice;

    [Header("Effects")]
    public int healthUpAmount;

    [Header("!")]
    public GameObject exclamationCanvas;

    [Header("Utilities - do not alter")]
    public bool playerInRange;
    public bool shopping;
    public bool talking;
    [SerializeField]
    private Animator anim;
    GameObject shopMenu;
    PlayerStatus playerStatus;
    CinemachineFreeLook cam;
    string camX;
    string camY;

    bool canInteract;

    private void OnEnable()
    {
        ShopMenu.ExitShop += EndShop;
        DialogueManager.DialogueEnd += EventAfterDialogue;
    }

    private void OnDisable()
    {
        ShopMenu.ExitShop -= EndShop;
        DialogueManager.DialogueEnd -= EventAfterDialogue;
    }

    void Start()
    {
        shopMenu = GameObject.FindGameObjectWithTag("ShopMenu").transform.GetChild(0).gameObject;
        cam = GameObject.FindGameObjectWithTag("FreeLook").GetComponent<CinemachineFreeLook>();
        canInteract = true;
    }

    void Update()
    {
        // StartShop();
        CheckDialogue();
    }

    void CheckDialogue()
    {
        if (!Input.GetButtonDown("light")) return;
        if (!playerInRange) return;
        if (playerStatus.GetComponent<PlayerMovement>().dashing) return;
        if (shopping) return;
        if (talking) return;
        if (!canInteract) return;

        DialogueManager.Instance.StartDialogue(dialogueBeforeShop);

        Music.Instance.TurnAllMusicVolumeDown();
        Cursor.lockState = CursorLockMode.None;

        camX = cam.m_XAxis.m_InputAxisName;
        camY = cam.m_YAxis.m_InputAxisName;

        cam.m_XAxis.m_InputAxisName = "";
        cam.m_YAxis.m_InputAxisName = "";

        cam.m_XAxis.m_InputAxisValue = 0;
        cam.m_YAxis.m_InputAxisValue = 0;

        playerStatus.CanMoveState(false);
        talking = true;
        shopping = true;
    }

    void EventAfterDialogue()
    {
        if (!talking) return;
        if (!shopping) ReturnPlayerToNormal();
        else StartShop();
    }

    void StartShop()
    {
        shopMenu.SetActive(true);
        playerStatus.shopping = true;

        PlayerInShop?.Invoke(playerStatus, this);
    }

    void EndShop()
    {
        shopMenu.SetActive(false);
        playerStatus.shopping = false;
        shopping = false;

        CancelInvoke();
        Invoke(nameof(TriggerFinalDialog), 0.2f);
    }

    void TriggerFinalDialog()
    {
        DialogueManager.Instance.StartDialogue(dialogueAfterShop);
    }

    void ReturnPlayerToNormal()
    {
        Music.Instance.NormalizeMusicVolume();

        Cursor.lockState = CursorLockMode.Locked;

        cam.m_XAxis.m_InputAxisName = camX;
        cam.m_YAxis.m_InputAxisName = camY;

        playerStatus.CanMoveState(true);

        talking = false;
        canInteract = false;
        StartCoroutine(InteractableCooldown());
    }

    IEnumerator InteractableCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        canInteract = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if(playerStatus == null) playerStatus = other.GetComponent<PlayerStatus>();
            playerStatus.CanAttackState(false);
            exclamationCanvas.SetActive(true);

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Hello"))
            anim.SetTrigger("Hello");
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
