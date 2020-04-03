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
    PlayerStatus playerStatus;

    public bool canUseMagic;

    public Transform specialSpawnPoint;

    PoolableObject special;

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

        playerStatus = GetComponent<PlayerStatus>();
        playerElements = GetComponentInParent<Elements>();
        inputs = GetComponentInParent<MovementInput>();
        playerMove = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<Attack>();

        anim = GetComponentInChildren<Animator>();

        canUseMagic = true;
    }

    void Update()
    {
        MagicInput();

        if (Input.GetKeyDown(KeyCode.Alpha1)) playerStatus.IncreaseAllMana();
    }

    void MagicInput()
    {
        if (!Input.GetButtonDown("Special")) return;
        if (anim.GetBool("attacking")) return;
        if (playerMove.dashing) return;
        if (!playerMove.isGrounded || playerAttack.canInputNextAttack) return;
        if (!canUseMagic) return;

        if (playerStatus.mana < playerStatus.maxMana) return;
        if (playerElements.currentElement == Element.None) return;

        anim.SetTrigger("special");
        inputs.canMove = false;
        playerAttack.DisableNextAttackInput();

        playerStatus.DecreaseAllMana();
    }

    void MagicAttack()
    {
        switch (playerElements.currentElement)
        {
            case Element.Fire:
                special = GameManager.Instance.FireSpellPool.RequestObject(specialSpawnPoint.position, transform.rotation);
                special.GetComponent<FireBallSpecial>().StartSpell();
                break;
            case Element.Water:
                special = GameManager.Instance.WaterSpellPool.RequestObject(specialSpawnPoint.position, transform.rotation);
                special.GetComponent<WaterSpell>().CheckIfCanSpawn();
                break;
            case Element.Metal:
                special = GameManager.Instance.MetalSpellPool.RequestObject(transform.position, transform.rotation);
                special.GetComponent<MetalSpell>().StartSpell();
                break;
            case Element.Wood:
                special = GameManager.Instance.WoodSpellPool.RequestObject(transform.position, transform.rotation);
                special.GetComponent<WoodSpell>().StartSpell();
                break;
            case Element.Earth:
                special = GameManager.Instance.EarthSpellPool.RequestObject(transform.position, transform.rotation);
                special.GetComponentInChildren<EarthSpellManager>().StartSpell();
                break;
        }

        special.GetComponent<Spell>().playerPos = transform;

    }

    public void CanUseMagicAttack(bool _state)
    {
        canUseMagic = _state;
    }
}
