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
    float invincibilityTime;

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
        damage = manager.damage;
        invincibilityTime = manager.invincbilityTime;
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
            if (enemy.invincible) return;

            if (playerPos == null) playerPos = GameObject.FindObjectOfType<PlayerMovement>().transform;

            Vector3 knockbackDirection = other.transform.position - playerPos.position;
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            enemyMove.KnockBack(knockbackDirection, knockbackForce, knockupForce, knockTime);
            enemy.TakeDamage(damage);
            enemy.Invincibility(invincibilityTime);

        }
    }
}
