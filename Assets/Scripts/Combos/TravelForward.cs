using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelForward : MonoBehaviour
{
    Collider collider;
    Rigidbody rb;
    public float speed;
    public float timeToStop;

    public void GainSpeed()
    {
        if (collider == null) collider = GetComponent<Collider>();
        if(rb == null) rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        Invoke(nameof(Stop), timeToStop);
    }

    void Stop()
    {
        rb.velocity = Vector3.zero;
        if (collider != null) collider.enabled = false;
    }
}
