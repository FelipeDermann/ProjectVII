using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawnCheck : MonoBehaviour
{
    public LayerMask layerMask;
    public Vector3 hitboxBounds;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<BoxCollider>().enabled = false;
        //GetComponent<MeshRenderer>().enabled = false;

        //Collider[] walls = Physics.OverlapBox(transform.position, hitboxBounds, transform.rotation, layerMask);
        //bool mustHide = false;

        //foreach (Collider currentWall in walls)
        //{
        //    if (currentWall.gameObject.CompareTag("Wall")) mustHide = true;
        //    Debug.Log("GOTTEM COACH");
        //}

        //if (!mustHide)
        //{
        //    GetComponent<BoxCollider>().enabled = true;
        //    GetComponent<MeshRenderer>().enabled = true;
        //}
    }
}
