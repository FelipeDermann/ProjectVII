using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    MovementInput movementInput;
    PlayerMovement playerMovement;
    PlayerHUD playerHUD;
    Animator animator;

    [Header("Health Attributes")]
    public int currentHealth;
    public int maxHealth;
    public bool dead;

    [Header("Spell Attributes")]
    public int mana;
    public int maxMana;
    public int energyToGain;

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
        animator = GetComponentInChildren<Animator>();
        playerHUD = GetComponent<PlayerHUD>();
        playerHUD.StartHUD();

        playerHUD.UpdateHealthBar();
        playerHUD.UpdateSpecialBar();
    }

    public void IncreaseHealth(int _damage)
    {
        currentHealth += _damage;
        playerHUD.UpdateHealthBar();
    }

    public void DecreaseHealth(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth <= 0) Death();

        playerHUD.UpdateHealthBar();
    }

    public void Death()
    {
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
