using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundDetect : MonoBehaviour
{
    EnemyMove enemy;
    public bool airborne;
    public float timeToMoveAgain;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyMove>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            if (enemy.airborne)
            {
                enemy.Invoke(nameof(enemy.MoveAgain), timeToMoveAgain);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {

        }
    }
}
