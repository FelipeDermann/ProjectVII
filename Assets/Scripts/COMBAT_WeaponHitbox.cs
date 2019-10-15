using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_WeaponHitbox : MonoBehaviour
{
    public bool active;
    public COMBAT_Magic playerMagic;

    Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        playerMagic = GetComponentInParent<COMBAT_Magic>();
        playerPos = FindObjectOfType<COMBAT_PlayerMovement>().transform;
    }

    public void ActivateHitbox()
    {
        active = true;
    }

    public void DeactivateHitbox()
    {
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<Enemy>();
                var enemyMove = other.GetComponent<EnemyMove>();

                Vector3 knockbackDirection = playerPos.position - other.transform.position;
                knockbackDirection.Normalize();
                knockbackDirection.y = 0;

                enemyMove.KnockBack(-knockbackDirection);
                enemy.GetHurt();
                playerMagic.GainEnergy();
            }
        }
    }
}
