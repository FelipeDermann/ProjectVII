using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    //event system
    public GameObject firstMenuButton;
    EventSystem eventSystem;

    private void Awake()
    {
        ShopArea.PlayerInShop += StartShop;
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
    }

    private void OnDestroy()
    {
        ShopArea.PlayerInShop -= StartShop;
    }

    void StartShop(PlayerStatus _player, ShopArea _shop)
    {
        playerStatus = _player;
        shop = _shop;

        eventSystem.SetSelectedGameObject(firstMenuButton);

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
