using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionDetection : MonoBehaviour
{
    public int layerIndex;
    public EnemyMove enemyMove;

    void Start()
    {
        enemyMove = GetComponentInParent<EnemyMove>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerIndex)
        {
            enemyMove.IsPlayerInRange(true);
            enemyMove.SetTarget(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == layerIndex)
        {
            enemyMove.IsPlayerInRange(false);
        }
    }
}
