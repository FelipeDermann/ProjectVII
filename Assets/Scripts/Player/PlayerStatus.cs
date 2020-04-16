using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    MovementInput movementInput;
    PlayerMovement playerMovement;
    Attack playerAttack;
    Magic playerMagic;
    PlayerHUD playerHUD;
    Animator animator;   

    [Header("Health Attributes")]
    public int currentHealth;
    public int maxHealth;
    public float invincibilityTime;
    public bool invincible;
    public bool dead;

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

        movementInput = GetComponent<MovementInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<Attack>();
        playerMagic = GetComponent<Magic>();
        animator = GetComponentInChildren<Animator>();
        playerHUD = GetComponent<PlayerHUD>();
        playerHUD.StartHUD();

        playerHUD.UpdateHealthBar();
        playerHUD.UpdateSpecialBar();
    }

    public void GainMoney(int _amount)
    {
        money += _amount;
    }

    public void LoseMoney(int _amount)
    {
        money -= _amount;
    }

    public void IncreaseHealth(int _damage)
    {
        currentHealth += _damage;
        playerHUD.UpdateHealthBar();
    }

    public void TakeDamage(int _damage)
    {
        if (invincible) return;

        currentHealth -= _damage;

        if (currentHealth <= 0) Death();
        else
        {
            StartCoroutine(nameof(Invincibility));
            if(!animator.GetBool("hurt")) animator.SetTrigger("hit");
        }

        playerHUD.UpdateHealthBar();
    }

    public void DecreaseHealth(int _damage)
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
        Debug.Log("PLAYER INJURED");

        if (_state)
        {
            playerAttack.CanPlayerAttack(false);
            playerMagic.CanUseMagicAttack(false);
            movementInput.ChangeMoveState(false);
            playerMovement.ChangeDashState(false);
        }
        else
        {
            playerAttack.CanPlayerAttack(true);
            playerMagic.CanUseMagicAttack(true);
            movementInput.ChangeMoveState(true);
            playerMovement.ChangeDashState(true);
        }

        animator.SetBool("hurt", _state);
    }

    public void Death()
    {
        if (dead) return;

        Debug.Log("YOU ARE DEAD, DEAD, DEAD!");

        dead = true;

        movementInput.ChangeMoveState(false);
        playerMovement.ChangeDashState(false);

        animator.SetBool("dead", true);
        animator.SetTrigger("death");
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
