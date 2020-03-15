using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    PlayerStatus playerStatus;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartHUD()
    {
        playerStatus = GetComponent<PlayerStatus>();

        barRect.y = bar.sizeDelta.y;
        backBarRect = backBar.sizeDelta;

        specialBarRect.y = specialBar.sizeDelta.y;
        specialBackBarRect = specialBackBar.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerStatus.DecreaseHealth(10);
            UpdateHealthBar();
        }
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
