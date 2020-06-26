using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IconType
{
    Heavy,
    Light,
    Element
}

public class ComboHUDElement : MonoBehaviour
{
    public AttackType[] attackTypes;

    public Element element;

    public Image[] icons;
    public Animator[] iconsAnims;
    public Color obscuredColor;

    public bool canBeShown;

    [Header("Cycles")]
    public Animator rectangleAnim;
    public Image rectangleImage;
    public GameObject controlIcon;
    public GameObject generationIcon;

    public void ObscureIcons()
    {
        for (int i = 0; i < icons.Length; i++) icons[i].color = obscuredColor;
        canBeShown = true;

        controlIcon.SetActive(false);
        generationIcon.SetActive(false);

        rectangleAnim.SetBool("Control", false);
        rectangleAnim.SetBool("Generation", false);

        rectangleImage.color = Color.white;
    }

    public void RestoreColor(int _index)
    {
        icons[_index].color = Color.white;
        iconsAnims[_index].SetTrigger("Shine");
        if (_index == 2)
        {
            iconsAnims[_index+1].SetTrigger("Element");
            icons[icons.Length-1].color = Color.white;
            icons[icons.Length-2].color = Color.white;
        }
    }

    public void TurnObjectOff()
    {
        gameObject.SetActive(false);
    }
}
