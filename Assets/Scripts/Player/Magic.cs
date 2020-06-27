using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public UIPlayer uiElement;

    PlayerMovement playerMove;
    Attack playerAttack;
    PlayerElements playerElements;
    Animator anim;
    PlayerStatus playerStatus;

    public bool canUseMagic;

    public Transform specialSpawnPoint;
    public Transform specialSpawnPointFireBall;

    PoolableObject special;

    private void Awake()
    {
        PlayerAnimation.SpawnMagicHitbox += MagicAttack;
    }
    private void OnDestroy()
    {
        PlayerAnimation.SpawnMagicHitbox -= MagicAttack;
    }

    // Start is called before the first frame update
    void Start()
    {
        var canvasText = GameObject.FindObjectOfType(typeof(UIPlayer)) as UIPlayer;
        //uiElement = canvasText.GetComponent<UIPlayer>();

        playerStatus = GetComponent<PlayerStatus>();
        playerElements = GetComponentInParent<PlayerElements>();
        playerMove = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<Attack>();

        anim = GetComponentInChildren<Animator>();

        canUseMagic = true;
    }

    void Update()
    {
        MagicInput();

    }

    void MagicInput()
    {
        if (PauseGame.paused) return;
        if (!Input.GetButtonDown("Special")) return;
        if (anim.GetBool("attacking")) return;
        if (playerMove.dashing) return;
        if (playerAttack.canInputNextAttack) return;
        if (!canUseMagic) return;

        if (playerStatus.mana < playerStatus.maxMana) return;
        if (playerElements.currentElement.ElementName == ElementType.None) return;

        anim.SetTrigger("special");
        playerAttack.DisableNextAttackInput();

        playerStatus.DecreaseAllMana();
    }

    void MagicAttack()
    {
        switch (playerElements.currentElement.ElementName)
        {
            case ElementType.Fire:
                special = GameManager.Instance.FireSpellPool.RequestObject(specialSpawnPointFireBall.position, transform.rotation);
                special.GetComponent<FireBallSpecial>().StartSpell();
                break;
            case ElementType.Water:
                special = GameManager.Instance.WaterSpellPool.RequestObject(specialSpawnPoint.position, transform.rotation);
                special.GetComponent<WaterSpell>().CheckIfCanSpawn();
                break;
            case ElementType.Metal:
                special = GameManager.Instance.MetalSpellPool.RequestObject(transform.position, transform.rotation);
                special.GetComponent<MetalSpell>().StartSpell();
                break;
            case ElementType.Wood:
                special = GameManager.Instance.WoodSpellPool.RequestObject(transform.position, transform.rotation);
                special.GetComponent<WoodSpell>().StartSpell();
                break;
            case ElementType.Earth:
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
