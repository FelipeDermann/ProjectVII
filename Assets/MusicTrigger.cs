using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicTrigger : MonoBehaviour
{
    public AudioSource audioSourceAmbient;
    public AudioSource audioSourceBattle;

    public bool isInCombat;
    public LayerMask enemyLayerIndex;

    private void OnEnable()
    {
        Enemy.EnemyDead += CheckIfEnemiesAreNearby;
    }
    private void OnDisable()
    {
        Enemy.EnemyDead -= CheckIfEnemiesAreNearby;
    }

    void CheckIfEnemiesAreNearby()
    {
        SphereCollider sphere = GetComponent<SphereCollider>();
        Collider[] enemies = Physics.OverlapSphere(transform.position, sphere.radius, enemyLayerIndex);
        bool stopBattleMusic = true;

        foreach (Collider currentEnemy in enemies)
        {
            if (currentEnemy.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("HIT ENEMY THAT MAYBE IS DEAD");
                stopBattleMusic = false;
            }
        }

        if (stopBattleMusic)
        {
            isInCombat = false;
            Music.Instance.StartMusicFade(false);
        }
        else
        {
            isInCombat = true;
            Music.Instance.StartMusicFade(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isInCombat = true;
            Music.Instance.StartMusicFade(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isInCombat = false;
            Music.Instance.StartMusicFade(false);
        }
    }
}
