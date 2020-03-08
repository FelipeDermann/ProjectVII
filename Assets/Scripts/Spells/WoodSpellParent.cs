using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpellParent : MonoBehaviour
{
    WoodSpell spell;

    // Start is called before the first frame update
    void Start()
    {
        spell = GetComponentInChildren<WoodSpell>();
    }

    public void CallFunction()
    {
        spell.SpawnNextWoodAttack();
    }
}
