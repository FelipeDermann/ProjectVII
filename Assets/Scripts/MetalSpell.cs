using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalSpell : MonoBehaviour
{
    Spell spell;
    PoolableObject thisObject;
    public float timeToRemoveSpell;
    public List<BladeSpecial> blades;
    public Transform bladeParent;

    public void StartSpell()
    {
        if (spell == null) spell = GetComponent<Spell>();

        for (int i = 0; i < bladeParent.childCount; i++)
        {
            blades.Add(bladeParent.GetChild(i).GetComponent<BladeSpecial>());
        }

        foreach (BladeSpecial blade in blades)
        {
            //blade.playerPos = spell.playerPos;
            blade.StartBlade(spell.playerPos);
        }

        EndSpell();
    }

    void EndSpell()
    {
        blades.Clear();

        if (thisObject == null) thisObject = GetComponent<PoolableObject>();
        GameManager.Instance.MetalSpellPool.ReturnObject(thisObject, timeToRemoveSpell);
    }
}
