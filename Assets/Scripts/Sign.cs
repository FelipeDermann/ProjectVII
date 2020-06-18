using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cinemachine;

public class Sign : MonoBehaviour
{
    [Header("Dialogues")]
    public Dialogue dialogue;

    [Header("Outline")]
    public GameObject outlineObject;

    [Header("Utilities - do not alter")]
    public bool playerInRange;
    public bool talking;
    PlayerStatus playerStatus;
    CinemachineFreeLook cam;
    string camX;
    string camY;

    bool canInteract;

    private void OnEnable()
    {
        DialogueManager.DialogueEnd += ReturnPlayerToNormal;
    }

    private void OnDisable()
    {
        DialogueManager.DialogueEnd -= ReturnPlayerToNormal;
    }

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("FreeLook").GetComponent<CinemachineFreeLook>();
        canInteract = true;
    }

    void Update()
    {
        CheckDialogue();
    }

    void CheckDialogue()
    {
        if (!Input.GetButtonDown("Interact")) return;
        if (!playerInRange) return;
        if (playerStatus.GetComponent<PlayerMovement>().dashing) return;
        if (talking) return;
        if (!canInteract) return;

        DialogueManager.Instance.StartDialogue(dialogue);

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
    }

    void ReturnPlayerToNormal()
    {
        if (!talking) return;
        Music.Instance.NormalizeMusicVolume();

        Cursor.lockState = CursorLockMode.Locked;

        cam.m_XAxis.m_InputAxisName = camX;
        cam.m_YAxis.m_InputAxisName = camY;

        playerStatus.CanMoveState(true);

        canInteract = false;
        talking = false;
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
            if (playerStatus == null) playerStatus = other.GetComponent<PlayerStatus>();
            playerStatus.CanAttackState(false);
            outlineObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerStatus.CanAttackState(true);
            outlineObject.SetActive(false);
        }
    }
}
