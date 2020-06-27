using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDebuff : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;

    [Header("Icons to activate on each stack")]
    public DebuffStack[] debuffIcons;
    public Animator[] stackIconsAnims;
    public Animator barAnim;

    [Header("Particles to activate on each stack")]
    public ParticleSystem[] debuffParticles;
    public ParticleSystem debuffBurst;

    [Header("Debuff Attributes")]
    public float damagePerStack;
    public float damageTickTime;
    public float debuffDuration;
    int debuffStacks;
    bool debuffIsActive;

    public void GainStack()
    {
        if (debuffStacks >= 5) return;

        StopCoroutine(nameof(DebuffTime));
        StartCoroutine(nameof(DebuffTime));

        debuffParticles[debuffStacks].Play();
        debuffBurst.Play();

        debuffStacks += 1;

        if (!debuffIsActive)
        {
            debuffIsActive = true;
            DamageOverTime();
        }

        for (int i = 0; i < debuffStacks; i++)
        {
            stackIconsAnims[i].gameObject.SetActive(true);
            stackIconsAnims[debuffStacks - 1].SetBool("Playing", true);
            stackIconsAnims[i].Play("IconDebuff", 0, 0);

            debuffIcons[i].StartTimer(debuffDuration);
        }

        DebuffBurstSound();
        barAnim.SetBool("Playing", true);
        barAnim.Play("BarDebuff", 0, 0);
        
    }

    void DebuffBurstSound()
    {
        var audioEmitter = GameManager.Instance.AudioEnemyDebuffBurst.RequestObject(transform.position, transform.rotation);
        var emitterScript = audioEmitter.GetComponent<AudioEmitter>();

        emitterScript.PlaySoundWithPitch();
        float clipLength = emitterScript.clipToPlay.length;

        GameManager.Instance.AudioEnemyDebuffBurst.ReturnObject(audioEmitter, clipLength + 1);
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

        for (int i = 0; i < stackIconsAnims.Length; i++)
        {
            debuffIcons[i].ResetTimer();
            stackIconsAnims[i].SetBool("Playing", false);
            stackIconsAnims[i].gameObject.SetActive(false);
        }

        barAnim.SetBool("Playing", false);
    }

}
