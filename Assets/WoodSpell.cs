using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpell : MonoBehaviour
{
    Rigidbody rb;
    Transform playerPos;
    public WoodSpellController woodSpellController;

    public GameObject woodSpellToSpawn;
    public float spawnOffset;
    public float riseSpeed;
    public float downSpeed;

    [Header("knockback")]
    public float knockbackForce;
    public float knockupForce;
    public float knockTime;

    public KnockType knockType;

    public float damage;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.up * riseSpeed;

        playerPos = FindObjectOfType<COMBAT_PlayerMovement>().transform;

        //Destroy(gameObject, 4);

        Invoke(nameof(SpawnNextWoodAttack), 0.3f);
        StartCoroutine(nameof(Rise));
    }

    IEnumerator Rise()
    {
        yield return new WaitForSeconds(.2f);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(.3f);
        rb.velocity = Vector3.down*downSpeed;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void SpawnNextWoodAttack()
    {
        if (woodSpellController == null) woodSpellController = GameObject.FindObjectOfType<WoodSpell>().GetComponent<WoodSpellController>();

        if (woodSpellController.currentWavesToSpawn <= 0) return;
        woodSpellController.DecreaseWaveNumber();

        BoxCollider box = GetComponent<BoxCollider>();
        //box.enabled = false;

        GameObject wood = Instantiate(woodSpellToSpawn, transform.position + (transform.forward * spawnOffset), transform.rotation);
        wood.transform.position = new Vector3(wood.transform.position.x, -2, wood.transform.position.z);
        wood.GetComponent<WoodSpell>().woodSpellController = woodSpellController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            var enemyMove = other.GetComponent<EnemyMove>();

            Vector3 knockbackDirection = playerPos.position - other.transform.position;
            knockbackDirection.Normalize();
            knockbackDirection.y = 0;

            enemy.TakeDamage(damage);

            switch (knockType)
            {
                case KnockType.Back:
                    enemyMove.KnockBack(-knockbackDirection, knockbackForce, knockTime);
                    break;
                case KnockType.Away:
                    enemyMove.KnockAway(-knockbackDirection, knockbackForce, knockTime);
                    break;
                case KnockType.Up:
                    enemyMove.KnockUp(-knockbackDirection, knockbackForce, knockTime);
                    break;
            }

        }

    }
}
