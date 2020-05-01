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
    public Image barImage;
    public RectTransform bar;
    public RectTransform backBar;

    public Vector3 barRect;
    public Vector3 backBarRect;

    public float playerHealth;
    public float playerMaxHealth;

    [Header("Special HUD")]
    public Image specialBarImage;
    public RectTransform specialBar;
    public RectTransform specialBackBar;

    public Vector3 specialBarRect;
    public Vector3 specialBackBarRect;

    public float playerMana;
    public float playerMaxMana;

    public Color colorNotFilled;
    public Color colorFilled;

    public void StartHUD()
    {
        playerStatus = GetComponent<PlayerStatus>();

        barRect.y = bar.sizeDelta.y;
        backBarRect = backBar.sizeDelta;

        specialBarRect.y = specialBar.sizeDelta.y;
        specialBackBarRect = specialBackBar.sizeDelta;
    }

    public void UpdateCoins(int _coins)
    {
        coinText.text = _coins.ToString();
    }

    public void UpdateHealthBar()
    {
        playerHealth = playerStatus.currentHealth;
        playerMaxHealth = playerStatus.maxHealth;

        barRect.x = (playerHealth / playerMaxHealth) * backBarRect.x;
        bar.sizeDelta = barRect;
    }

    public void UpdateSpecialBar()
    {
        playerMana = playerStatus.mana;
        playerMaxMana = playerStatus.maxMana;

        if (playerMana >= playerMaxMana) specialBarImage.color = colorFilled;
        else specialBarImage.color = colorNotFilled;

        specialBarRect.x = (playerMana / playerMaxMana) * specialBackBarRect.x;
        specialBar.sizeDelta = specialBarRect;
    }
}
