using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    public static event Action BuyHealth;
    public static event Action BuyMagnet;
    public static event Action ExitShop;

    public Button buyHealthButton;
    public Button buyMagnetButton;
    public Button exitButton;

    public TextMeshProUGUI healthPriceText;
    public TextMeshProUGUI magnetPriceText;

    PlayerStatus playerStatus;
    ShopArea shop;

    private void OnEnable()
    {
        ShopArea.PlayerInShop += StartShop;
    }

    private void OnDisable()
    {
        ShopArea.PlayerInShop -= StartShop;
    }

    void StartShop(PlayerStatus _player, ShopArea _shop)
    {
        playerStatus = _player;
        shop = _shop;

        healthPriceText.text = shop.healthUpPrice.ToString();
        magnetPriceText.text = shop.magnetPrice.ToString();
    }

    public void Exit()
    {
        ExitShop?.Invoke();
    }

    public void BuyHealthButton()
    {
        if (playerStatus.money < shop.healthUpPrice) return;

        playerStatus.LoseMoney(shop.healthUpPrice);

        playerStatus.maxHealth += shop.healthUpAmount;
        playerStatus.currentHealth += shop.healthUpAmount;

        buyHealthButton.gameObject.SetActive(false);
        BuyHealth?.Invoke();
    }

    public void BuyMagnetButton()
    {
        if (playerStatus.money < shop.magnetPrice) return;

        playerStatus.LoseMoney(shop.magnetPrice);

        playerStatus.coinMagnetIsOn = true;

        buyMagnetButton.gameObject.SetActive(false);
        BuyMagnet?.Invoke();
    }
}
