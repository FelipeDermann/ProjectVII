using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    [SerializeField]
    private PlayerStatus status;

    [Header("Particles to activate on each stack")]
    public ParticleSystem[] buffParticles;

    [Header("Attack Speed Buff")]
    [Range(0f, 1f)]
    public float bonusSpeedPerStack;
    public int maxAtkSpeedStack;
    public static float atkSpeedToAnimator;

    [Header("Duration")]
    public float buffDuration;

    [Header("Attack Heal Buff")]
    public float healingPerStack;
    public float maxHealingStack;
    public float minimalNumberOfStacksToEnableHeal;

    public int buffStacks;
    bool buffIsActive;

    void OnEnable()
    {
        ComboEffect.GenerationCycleHit += GainStack;
        TravelForward.GenerationCycleHit += GainStack;
        WeaponHitbox.EnemyHit += Heal;
        ComboEffect.EnemyHit += Heal;
    }

    void OnDisable()
    {
        ComboEffect.GenerationCycleHit -= GainStack;
        TravelForward.GenerationCycleHit -= GainStack;
        WeaponHitbox.EnemyHit -= Heal;
        ComboEffect.EnemyHit -= Heal;
    }

    public void GainStack()
    {
        buffStacks += 1;
        if (buffStacks > 5) buffStacks = 5;

        buffParticles[buffStacks-1].Play();

        StopCoroutine(nameof(BuffTime));
        StartCoroutine(nameof(BuffTime));

        if (!buffIsActive)
        {
            buffIsActive = true;
        }

        AttackSpeed();
        if (buffStacks == 5) IgnoreStagger(true);
    }

    void IgnoreStagger(bool _state)
    {
        status.ChangeStagger(_state);
    }

    void Heal()
    {
        if (buffStacks < minimalNumberOfStacksToEnableHeal) return;

        float healStacks = buffStacks - minimalNumberOfStacksToEnableHeal;
        if (healStacks > maxHealingStack) healStacks = maxHealingStack;
        status.IncreaseHealth(healingPerStack * healStacks);
    }

    void AttackSpeed()
    {
        int atkSpeedStacks = buffStacks;
        if (atkSpeedStacks > maxAtkSpeedStack) atkSpeedStacks = maxAtkSpeedStack;
        atkSpeedToAnimator = bonusSpeedPerStack * atkSpeedStacks;
    }

    IEnumerator BuffTime()
    {
        yield return new WaitForSeconds(buffDuration);
        Deactivate();
    }

    public void Deactivate()
    {
        IgnoreStagger(false);

        buffIsActive = false;
        buffStacks = 0;
        atkSpeedToAnimator = 0;

        for (int i = 0; i < buffParticles.Length; i++)
        {
            buffParticles[i].Stop();
        }
    }
}
