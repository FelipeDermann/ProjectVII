using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpellManager : MonoBehaviour
{
    public Transform playerPos;
    Animator anim;

    [Header("Basic Attributes")]
    public float timeUntilDeath;
    public float damage;
    public GameObject earthParticle;

    [Header("Knockback")]
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;
    public KnockType knockType;


    // Start is called before the first frame update
    void Start()
    {
        playerPos = GetComponentInParent<Spell>().playerPos;

        anim = GetComponent<Animator>();

        Destroy(transform.parent.gameObject, timeUntilDeath);
    }

    public void PlayParticle()
    {
        GameObject particle = Instantiate(earthParticle, transform.parent.transform.position, transform.rotation);
        particle.transform.eulerAngles = new Vector3(-90,0,0);
        Destroy(particle, 2);
    }

    public void Activate()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
