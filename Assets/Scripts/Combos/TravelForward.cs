using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelForward : MonoBehaviour
{
    Collider collider;
    Rigidbody rb;
    public PlayerStatus player;

    [Header("Basic Attributes")]
    public float speed;
    public float timeToStop;
    public int damage;

    [Header("Knockback")]
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;

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
        transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            var enemyMove = other.GetComponent<EnemyMove>();

            Vector3 knockbackDirection = transform.position - other.transform.position;
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            enemy.TakeDamage(damage);

            if (!enemy.dead && !enemyMove.knockedDown) player.IncreaseMana();
        }

    }
}
