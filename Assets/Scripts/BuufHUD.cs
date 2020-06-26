using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuufHUD : MonoBehaviour
{
    [Header("Icons to activate on each stack")]
    public Animator[] stackIconsAnims;

    void Awake()
    {
        PlayerBuff.BuffStackGained += AddStack;
        PlayerBuff.BuffEnded += Deactivate;
    }

    void OnDestroy()
    {
        PlayerBuff.BuffStackGained -= AddStack;
        PlayerBuff.BuffEnded -= Deactivate;
    }

    public void AddStack(int _stacks)
    {
        stackIconsAnims[_stacks-1].gameObject.SetActive(true);
        stackIconsAnims[_stacks-1].SetBool("Playing", true);

        for (int i = 0; i < _stacks; i++)
        {
            stackIconsAnims[i].Play("IconBuff", 0, 0);

        }
    }

    public void Deactivate()
    {
        for (int i = 0; i < stackIconsAnims.Length; i++)
        {
            stackIconsAnims[i].SetBool("Playing", false);
            stackIconsAnims[i].gameObject.SetActive(false);
        }
    }
}
