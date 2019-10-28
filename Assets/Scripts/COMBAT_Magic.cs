﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_Magic : MonoBehaviour
{
    public UIPlayer uiElement;
    COMBAT_PlayerMovement playerMove;
    COMBAT_Attack playerAttack;
    COMBAT_MovementInput inputs;
    COMBAT_Elements playerElements;
    Animator anim;

    public int mana;
    public int maxMana;
    public int energyToGain;

    public GameObject[] specialHitbox;
    public Transform specialSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {

        var canvasText = GameObject.FindObjectOfType(typeof(UIPlayer)) as UIPlayer;
        uiElement = canvasText.GetComponent<UIPlayer>();

        playerElements = GetComponentInParent<COMBAT_Elements>();
        inputs = GetComponentInParent<COMBAT_MovementInput>();
        playerMove = GetComponent<COMBAT_PlayerMovement>();
        playerAttack = GetComponent<COMBAT_Attack>();

        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        MagicInput();
        //MagicAttack();

        if (Input.GetKeyDown(KeyCode.Alpha1)) mana = maxMana;
    }

    void MagicInput()
    {
        if (!Input.GetButtonDown("Special")) return;
        if (!playerMove.isGrounded || playerAttack.canCombo) return;
        if (mana < maxMana) return;
        if (playerElements.currentElement == Element.None) return;

        anim.SetTrigger("special");
        inputs.canMove = false;
        playerAttack.ActivateCanCombo();

        mana = 0;
    }
    public void MagicAttack()
    {
        GameObject special = null;

        switch (playerElements.currentElement)
        {
            case Element.Fire:
                special = Instantiate(specialHitbox[0], specialSpawnPoint.position, transform.rotation);
                Destroy(special, 1);
                break;
            case Element.Water:
                special = Instantiate(specialHitbox[1], specialSpawnPoint.position, transform.rotation);
                Destroy(special, 1);
                break;
            case Element.Metal:
                special = Instantiate(specialHitbox[2], transform.position, transform.rotation);
                Destroy(special, 1);
                break;
            case Element.Wood:
                special = Instantiate(specialHitbox[3], specialSpawnPoint.position, transform.rotation);
                Destroy(special, 1);
                break;
            case Element.Earth:
                special = Instantiate(specialHitbox[4], transform.position, transform.rotation);
                Destroy(special, 1);
                break;
        }
    }

    public void GainEnergy()
    {
        mana += energyToGain;
        if (mana > maxMana) mana = maxMana;
    }
}
