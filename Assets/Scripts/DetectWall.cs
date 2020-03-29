using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWall : MonoBehaviour
{
    public MeshRenderer spikeRenderer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            transform.parent.GetComponent<BoxCollider>().enabled = false;
            spikeRenderer.enabled = false;
        }
    }
}
