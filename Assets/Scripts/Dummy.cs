using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Enemy enemyScript;
    [SerializeField]
    private LifeBarEnemy lifeBar;

    public bool invincible;

    [Header("Health Regeneration")]
    public float delayToStartRegen;
    public float regenSpeed;
    public bool recovering;

    void OnEnable()
    {
        WeaponHitbox.AttackEnded += CanBeHurtAgainBySword;
    }

    void OnDisable()
    {
        WeaponHitbox.AttackEnded -= CanBeHurtAgainBySword;
    }

    public void GetHit()
    {
        int random = Random.Range(0, 3);

        anim.SetInteger("HitVariation", random);
        anim.SetTrigger("Hurt");

        StopAllCoroutines();
        StartCoroutine(WaitToRecoverHealth());
    }

    void CanBeHurtAgainBySword()
    {
        invincible = false;
    }

    IEnumerator WaitToRecoverHealth()
    {
        yield return new WaitForSeconds(delayToStartRegen);
        if (enemyScript.currentHealth < 0) enemyScript.currentHealth = 0;
        recovering = true;

        while(enemyScript.currentHealth < enemyScript.maxHealth)
        {
            enemyScript.currentHealth += regenSpeed * Time.deltaTime;
            if (enemyScript.currentHealth > enemyScript.maxHealth) enemyScript.currentHealth = enemyScript.maxHealth;
            lifeBar.UpdateLifeBar(enemyScript.currentHealth, enemyScript.maxHealth);

            yield return null;
        }
        
        recovering = false;
    }
}
