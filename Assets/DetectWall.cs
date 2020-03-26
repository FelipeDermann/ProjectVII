using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            transform.parent.GetComponent<BoxCollider>().enabled = false;
            transform.parent.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
