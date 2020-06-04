using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    PlayerMovement playerMovement;
    Attack playerAttack;
    Magic playerMagic;
    PlayerHUD playerHUD;
    Animator animator;   

    [Header("Health Attributes")]
    public float currentHealth;
    public float maxHealth;
    public float invincibilityTime;
    public bool invincible;
    public bool ignoreStagger;
    public bool dead;
    public bool shopping;
    public bool coinMagnetIsOn;

    [Header("Health Attributes")]
    public float lightAttackDamage;
    public float heavyAttackDamage;

    [Header("Spell Attributes")]
    public int mana;
    public int maxMana;
    public int energyToGain;

    [Header("Money")]
    public int money;

    private void OnEnable()
    {
        PlayerAnimation.RestartScene += RestartSceneCountdown;
    }
    private void OnDisable()
    {
        PlayerAnimation.RestartScene -= RestartSceneCountdown;
    }

    void Start()
    {
        currentHealth = maxHealth;

        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<Attack>();
        playerMagic = GetComponent<Magic>();
        animator = GetComponentInChildren<Animator>();
        playerHUD = GetComponent<PlayerHUD>();
        playerHUD.StartHUD();

        playerHUD.UpdateHealthBar();
        playerHUD.UpdateSpecialBar();
    }

    public void ChangeStagger(bool _state)
    {
        ignoreStagger = _state;
    }

    public void GainMoney(int _amount)
    {
        money += _amount;
        playerHUD.UpdateCoins(money);
    }

    public void LoseMoney(int _amount)
    {
        money -= _amount;
        playerHUD.UpdateCoins(money);
    }

    public void IncreaseHealth(float _heal)
    {
        currentHealth += _heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        playerHUD.UpdateHealthBar();
    }

    public void TakeDamage(float _damage)
    {
        if (invincible) return;

        currentHealth -= _damage;

        if (currentHealth <= 0) Death();
        else
        {
            StartCoroutine(nameof(Invincibility));
            if(!animator.GetBool("hurt") && !ignoreStagger) animator.SetTrigger("hit");
        }

        playerHUD.UpdateHealthBar();
    }

    public void DecreaseHealth(float _damage)
    {
        currentHealth -= _damage;

        if (currentHealth <= 0) Death();

        playerHUD.UpdateHealthBar();
    }

    IEnumerator Invincibility()
    {
        invincible = true;

        yield return new WaitForSeconds(invincibilityTime);

        invincible = false;
    }

    public void HurtState(bool _state)
    {

        if (_state)
        {
            playerAttack.CanPlayerAttack(false);
            playerMagic.CanUseMagicAttack(false);
            playerMovement.ChangeDashState(false);
            playerMovement.canMove = false;
        }
        else
        {
            playerAttack.CanPlayerAttack(true);
            playerMagic.CanUseMagicAttack(true);
            playerMovement.ChangeDashState(true);
            playerMovement.canMove = true;
        }

        animator.SetBool("hurt", _state);
    }

    public void Death()
    {
        if (dead) return;

        Debug.Log("YOU ARE DEAD, DEAD, DEAD!");

        dead = true;

        playerMovement.ChangeDashState(false);
        playerMovement.CanWalkOff();

        animator.SetBool("dead", true);
        animator.SetTrigger("death");
    }

    public void CanMoveState(bool _canControl)
    {
        playerMagic.CanUseMagicAttack(_canControl);
        playerMovement.ChangeDashState(_canControl);
        playerMovement.canMove = _canControl;
        playerMovement.velocity = Vector3.zero;
    }

    public void CanAttackState(bool _canControl)
    {
        playerAttack.CanPlayerAttack(_canControl);
    }

    void RestartSceneCountdown()
    {
        Invoke(nameof(RestartScene), 2);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IncreaseMana()
    {
        mana += energyToGain;
        if (mana > maxMana) mana = maxMana;

        playerHUD.UpdateSpecialBar();
    }
    public void IncreaseAllMana()
    {
        mana = maxMana;
        playerHUD.UpdateSpecialBar();
    }

    public void DecreaseMana(int _amountToLose)
    {
        mana -= _amountToLose;
        playerHUD.UpdateSpecialBar();
    }

    public void DecreaseAllMana()
    {
        mana = 0;
        playerHUD.UpdateSpecialBar();
    }
}
