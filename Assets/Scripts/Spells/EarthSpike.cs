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

    Vector3 enemyDir;
    Vector3 hitPoint;
    public LayerMask enemyLayerMask;

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
            enemyDir = other.transform.position - transform.position;
            enemyDir.Normalize();
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            //rickmartin
            Ray ray = new Ray(transform.position, enemyDir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayerMask))
            {
                hitPoint = hit.point;
                var hitmarker = GameManager.Instance.hitMarkerPool.RequestObject(hit.point + new Vector3(0, 1, 0), transform.rotation);
                var hitmarkerParticleSystem = hitmarker.GetComponent<ParticleSystem>();
                hitmarkerParticleSystem.Play();

                GameManager.Instance.hitMarkerPool.ReturnObject(hitmarker, 2);
            }
            else
            {
                var hitmarker = GameManager.Instance.hitMarkerPool.RequestObject(other.transform.position + new Vector3(0, 1, 0), transform.rotation);
                var hitmarkerParticleSystem = hitmarker.GetComponent<ParticleSystem>();
                hitmarkerParticleSystem.Play();

                GameManager.Instance.hitMarkerPool.ReturnObject(hitmarker, 2);
            }

            enemyMove.KnockBack(knockbackDirection, knockbackForce, knockupForce, knockTime);
            enemy.TakeDamage(damage);
            enemy.Invincibility(invincibilityTime);

        }
    }
}
