using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    PlayerHUD playerHUD;

    [Header("Health Attributes")]
    public int currentHealth;
    public int maxHealth;

    [Header("Spell Attributes")]
    public int mana;
    public int maxMana;
    public int energyToGain;

    void Start()
    {
        currentHealth = maxHealth;
        //mana = 0;

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

        if (currentHealth <= 0)
        {
            Debug.Log("YOU ARE DEAD, DEAD, DEAD!");
        }
        playerHUD.UpdateHealthBar();
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
