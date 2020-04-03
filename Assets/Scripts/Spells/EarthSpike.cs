using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpike : MonoBehaviour
{
    Transform playerPos;
    EarthSpellManager manager;
    public MeshRenderer spikeRenderer;

    bool canAppear = true;
    float damage;
    float knockbackForce;
    float knockupForce;
    float knockTime;
    KnockType knockType;

    public void StartSpike(Transform _playerPos, EarthSpellManager _manager)
    { 
        if (!canAppear) return;
    
        manager = _manager;

        GetComponent<BoxCollider>().enabled = true;
        spikeRenderer.enabled = true;

        playerPos = _playerPos;
        knockbackForce = manager.knockbackForce;
        knockupForce = manager.knockupForce;
        knockTime = manager.knockTime;
        knockType = manager.knockType;
        damage = manager.damage;
    }

    public void ChangeAppearState(bool _state)
    {
        canAppear = _state;
    }

    public void Hide()
    {
        GetComponent<BoxCollider>().enabled = false;
        spikeRenderer.enabled = false;

        canAppear = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            var enemyMove = other.GetComponent<EnemyMove>();

            if (playerPos == null) playerPos = GameObject.FindObjectOfType<PlayerMovement>().transform;

            Vector3 knockbackDirection = playerPos.position - other.transform.position;
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            enemy.DecreaseHealth(damage);

            switch (knockType)
            {
                case KnockType.Back:
                    enemyMove.KnockBack(-knockbackDirection, knockbackForce, knockTime);
                    break;
                case KnockType.Away:
                    enemyMove.KnockAway(-knockbackDirection, knockbackForce, knockTime);
                    break;
                case KnockType.Up:
                    enemyMove.KnockUp(-knockbackDirection, knockbackForce, knockTime);
                    break;
            }

        }
    }
}
