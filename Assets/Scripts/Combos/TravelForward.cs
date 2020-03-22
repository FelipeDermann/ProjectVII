using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelForward : MonoBehaviour
{
    Rigidbody rb;
    public float speed;

    public void GainSpeed()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
}
