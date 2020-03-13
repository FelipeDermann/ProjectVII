using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public UIPlayer uiElement;
    PlayerMovement playerMove;
    Attack playerAttack;
    MovementInput inputs;
    Elements playerElements;
    Animator anim;

    public int mana;
    public int maxMana;
    public int energyToGain;

    public bool cancelMagicHitbox;

    public GameObject[] specialHitbox;
    public Transform specialSpawnPoint;

    private void OnEnable()
    {
        PlayerAnimation.SpawnMagicHitbox += MagicAttack;

    }
    private void OnDisable()
    {
        PlayerAnimation.SpawnMagicHitbox -= MagicAttack;

    }

    // Start is called before the first frame update
    void Start()
    {
        var canvasText = GameObject.FindObjectOfType(typeof(UIPlayer)) as UIPlayer;
        uiElement = canvasText.GetComponent<UIPlayer>();

        playerElements = GetComponentInParent<Elements>();
        inputs = GetComponentInParent<MovementInput>();
        playerMove = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<Attack>();

        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        MagicInput();

        if (Input.GetKeyDown(KeyCode.Alpha1)) mana = maxMana;
    }

    void MagicInput()
    {
        if (!Input.GetButtonDown("Special")) return;
        if (playerMove.dashing) return;
        if (!playerMove.isGrounded || playerAttack.canInputNextAttack) return;
        if (mana < maxMana) return;
        if (playerElements.currentElement == Element.None) return;

        anim.SetTrigger("special");
        inputs.canMove = false;
        playerAttack.DisableNextAttackInput();

        mana = 0;
    }

    void MagicAttack()
    {
        GameObject special = null;

        switch (playerElements.currentElement)
        {
            case Element.Fire:
                special = Instantiate(specialHitbox[0], specialSpawnPoint.position, transform.rotation);
                break;
            case Element.Water:
                special = Instantiate(specialHitbox[1], specialSpawnPoint.position, transform.rotation);
                break;
            case Element.Metal:
                special = Instantiate(specialHitbox[2], transform.position, transform.rotation);
                break;
            case Element.Wood:
                special = Instantiate(specialHitbox[3], transform.position + new Vector3(0, 0.92f, 0), transform.rotation);
                break;
            case Element.Earth:
                special = Instantiate(specialHitbox[4], transform.position, transform.rotation);
                break;
        }

        special.GetComponent<Spell>().playerPos = transform;

    }

    public void GainEnergy()
    {
        mana += energyToGain;
        if (mana > maxMana) mana = maxMana;
    }
}
