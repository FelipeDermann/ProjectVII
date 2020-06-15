using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeCollision : MonoBehaviour
{
    public BladeSpecial blade;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            blade.CollisionRegistered(false);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            blade.CollisionRegistered(true);
        }
    }
}
