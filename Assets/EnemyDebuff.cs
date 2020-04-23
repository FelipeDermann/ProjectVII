using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebuff : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;

    [Header("Particles to activate on each stack")]
    public ParticleSystem[] debuffParticles;

    [Header("Debuff Attributes")]
    public float damagePerStack;
    public float damageTickTime;
    public float debuffDuration;
    int debuffStacks;
    bool debuffIsActive;

    public void GainStack()
    {
        debuffParticles[debuffStacks].Play();

        debuffStacks += 1;

        StopCoroutine(nameof(DebuffTime));
        StartCoroutine(nameof(DebuffTime));

        if (!debuffIsActive)
        {
            debuffIsActive = true;
            DamageOverTime();
        }
    }

    IEnumerator DebuffTime()
    {
        yield return new WaitForSeconds(debuffDuration);
        Deactivate();
    }

    void DamageOverTime()
    {
        enemy.TakeDamage(damagePerStack*debuffStacks);

        if (debuffIsActive) Invoke(nameof(DamageOverTime), damageTickTime);
    }

    public void Deactivate()
    {
        CancelInvoke(nameof(DamageOverTime));
        debuffIsActive = false;
        debuffStacks = 0;

        for (int i = 0; i < debuffParticles.Length; i++)
        {
            debuffParticles[i].Stop();
        }
    }

}
