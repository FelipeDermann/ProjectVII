using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    public bool invincible;

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
        int random = Random.Range(0,3);

        anim.SetInteger("HitVariation", random);
        anim.SetTrigger("Hurt");
    }

    void CanBeHurtAgainBySword()
    {
        invincible = false;
    }

}
