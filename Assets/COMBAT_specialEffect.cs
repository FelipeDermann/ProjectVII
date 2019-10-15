using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_specialEffect : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();

            enemy.GetHurt();
        }

    }
}
