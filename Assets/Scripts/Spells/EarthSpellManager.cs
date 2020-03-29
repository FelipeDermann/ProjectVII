using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpellManager : MonoBehaviour
{
    public Transform playerPos;
    Animator anim;
    public Transform earthParticle;

    [Header("Basic Attributes")]
    public float damage;
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;
    public KnockType knockType;


    // Start is called before the first frame update
    void Start()
    {
        playerPos = GetComponentInParent<Spell>().playerPos;

        anim = GetComponent<Animator>();
    }

    public void Activate()
    {
        earthParticle.gameObject.SetActive(true);
    }

    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }

    public void DestroyParticle()
    {
        Destroy(earthParticle.gameObject, 2);
        earthParticle.parent = null;
    }
}
