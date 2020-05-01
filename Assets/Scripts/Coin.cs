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

    bool flying;
    float flySpeed;

    public void Activate()
    {
        meshRenderer.enabled = true;
        sphereCollider.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;

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

    public void MagnetEffect(Transform _player, float _flySpeed)
    {
        if (flying) return;
        flying = true;

        flySpeed = _flySpeed;

        rb.isKinematic = false;
        rb.useGravity = false;
        EnableCollectHitbox();

        StopCoroutine(nameof(FlyToPlayer));
        StartCoroutine(nameof(FlyToPlayer), _player);
    }

    IEnumerator FlyToPlayer(Transform _playerTransform)
    {
        while (flying)
        {
            Vector3 dir = (_playerTransform.position - transform.position).normalized * flySpeed;
            rb.velocity = dir;

            yield return null;
        }
        yield break;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().GainMoney(value);
            audioEmitter.PlaySound();

            flying = false;
            StopCoroutine(nameof(FlyToPlayer));
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
