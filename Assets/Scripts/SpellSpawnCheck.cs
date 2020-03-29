using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawnCheck : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject spellToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 boxBounds = GetComponent<BoxCollider>().bounds.extents;
        Collider[] walls = Physics.OverlapBox(transform.position, boxBounds, transform.rotation, layerMask);
        bool mustDestroy = false;

        foreach (Collider currentWall in walls)
        {
            if (currentWall.gameObject.CompareTag("Wall")) mustDestroy = true;
        }

        if (mustDestroy) Destroy(gameObject);
        else
        {
            GameObject waterSpell = Instantiate(spellToSpawn, transform.position, transform.rotation);
            waterSpell.GetComponent<Spell>().playerPos = GetComponent<Spell>().playerPos;
            Destroy(gameObject);
        }
    }
}
