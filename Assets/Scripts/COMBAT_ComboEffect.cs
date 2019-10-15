using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_ComboEffect : MonoBehaviour
{
    public COMBAT_Magic playerMagic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();

            enemy.GetHurt();
            playerMagic.GainEnergy();
        }

    }
}
