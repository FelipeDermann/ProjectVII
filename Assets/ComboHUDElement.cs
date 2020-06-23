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

    public Image[] icons;
    public Color obscuredColor;

    public bool canBeShown;

    public void ObscureIcons()
    {
        for (int i = 0; i < icons.Length; i++) icons[i].color = obscuredColor;
        canBeShown = true;
    }

    public void RestoreColor(int _index)
    {
        icons[_index].color = Color.white;
        if (_index == attackTypes.Length) icons[icons.Length].color = Color.white;
    }
}
