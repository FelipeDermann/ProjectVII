using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpellManager : MonoBehaviour
{
    public Transform playerPos;
    Animator anim;

    CameraShake cameraShaking;
    PoolableObject thisObject;
    public List<EarthSpike> spikes;

    [Header("Set these up")]
    public Transform spikeParent;
    public AudioEmitter audioEmitter;

    [Header("Basic Attributes")]
    public float damage;
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;
    public float invincbilityTime;

    public float timeToRemoveSpell;
    public ParticleSystem earthParticle;


    // Start is called before the first frame update
    public void StartSpell()
    {
        if (playerPos == null) playerPos = GetComponentInParent<Spell>().playerPos;
        if(anim == null) anim = GetComponent<Animator>();

        anim.Play("earthSpikes");
    }

    public void Activate()
    {
        if (cameraShaking == null) cameraShaking = GetComponentInParent<CameraShake>();
        cameraShaking.Shake();

        audioEmitter.PlaySoundWithPitch();
        
        earthParticle.Play();

        for (int i = 0; i < spikeParent.childCount; i++)
        {
            spikes.Add(spikeParent.GetChild(i).GetComponent<EarthSpike>());
        }

        foreach (EarthSpike spike in spikes)
        {
            spike.StartSpike(playerPos, this);
        }
    }

    public void DestroySelf()
    {
        if (thisObject == null) thisObject = GetComponentInParent<PoolableObject>();
        GameManager.Instance.EarthSpellPool.ReturnObject(thisObject, timeToRemoveSpell);

        foreach (EarthSpike spike in spikes)
        {
            spike.Hide();
        }

        spikes.Clear();
    }
}
