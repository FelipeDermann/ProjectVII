using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    PlayerStatus playerStatus;

    [Header("Coin HUD")]
    public TextMeshProUGUI coinText;

    [Header("Health HUD")]
    public Image bar;

    public Vector3 barRect;
    public Vector3 backBarRect;

    public float playerHealth;
    public float playerMaxHealth;

    [Header("Special HUD")]
    public Image specialBar;

    public Vector3 specialBarRect;
    public Vector3 specialBackBarRect;

    public float playerMana;
    public float playerMaxMana;

    public Color colorNotFilled;
    public Color colorFilled;

    public void StartHUD()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    public void UpdateCoins(int _coins)
    {
        coinText.text = _coins.ToString();
    }

    public void UpdateHealthBar()
    {
        playerHealth = playerStatus.currentHealth;
        playerMaxHealth = playerStatus.maxHealth;

        bar.fillAmount = playerHealth / playerMaxHealth;
    }

    public void UpdateSpecialBar()
    {
        playerMana = playerStatus.mana;
        playerMaxMana = playerStatus.maxMana;

        if (playerMana >= playerMaxMana) specialBar.color = colorFilled;
        else specialBar.color = colorNotFilled;

        specialBar.fillAmount = playerMana / playerMaxMana;
    }
}
