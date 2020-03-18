using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public int layerIndex;
    public EnemyAttack enemyAttack;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == layerIndex)
        {

            enemyAttack.PlayerInRange(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == layerIndex)
        {
            enemyAttack.PlayerInRange(false);
        }
    }
}
