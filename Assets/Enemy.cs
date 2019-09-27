using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HarmEnemy"))
        {
            anim.SetTrigger("hit");
        }

    }

}
