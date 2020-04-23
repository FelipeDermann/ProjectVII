using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Basic Attributes")]
    public int value;
    public float coinSpeed;
    public float coinVerticalSpeed;

    public float timeToDespawnAfterCollected;

    [SerializeField] private PoolableObject thisObject;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioEmitter audioEmitter;

    public void Activate()
    {
        meshRenderer.enabled = true;
        sphereCollider.enabled = true;
        rb.isKinematic = false;

        Vector3 randomDir = UnityEngine.Random.insideUnitSphere;
        randomDir.y = coinVerticalSpeed;

        rb.AddForce(randomDir * coinSpeed, ForceMode.Impulse);
    }

    void EnableCollectHitbox()
    {
        boxCollider.enabled = true;
    }

    void Deactivate()
    {
        meshRenderer.enabled = false;
        sphereCollider.enabled = false;
        boxCollider.enabled = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().GainMoney(value);
            audioEmitter.PlaySound();

            Deactivate();

            GameManager.Instance.CoinPool.ReturnObject(thisObject, timeToDespawnAfterCollected);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            EnableCollectHitbox();
        }
    }
}
