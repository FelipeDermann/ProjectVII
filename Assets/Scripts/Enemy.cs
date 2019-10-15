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

    public void GetHurt()
    {
        anim.SetTrigger("hit");
    }

}
