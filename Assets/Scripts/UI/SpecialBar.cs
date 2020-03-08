using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialBar : MonoBehaviour
{
    public Magic playerMagic;

    public Image barImage;
    public RectTransform bar;
    public RectTransform backBar;

    public Vector3 barRect;
    public Vector3 backBarRect;

    public float playerMana;
    public float playerMaxMana;

    public Color colorNotFilled;
    public Color colorFilled;

    // Start is called before the first frame update
    void Start()
    {
        playerMagic = FindObjectOfType<Magic>();

        bar = GetComponent<RectTransform>();
        backBar = GetComponentInParent<RectTransform>();
        barImage = GetComponent<Image>();

        barRect.y = bar.sizeDelta.y;
        backBarRect = backBar.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        playerMana = playerMagic.mana;
        playerMaxMana = playerMagic.maxMana;

        if (playerMana >= playerMaxMana) barImage.color = colorFilled;
        else barImage.color = colorNotFilled;

        barRect.x = (playerMana / playerMaxMana) * backBarRect.x;
        bar.sizeDelta = barRect;
    }
}
