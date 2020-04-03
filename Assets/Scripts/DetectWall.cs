using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWall : MonoBehaviour
{
    public MeshRenderer spikeRenderer;
    EarthSpike spike;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (spike == null) spike = GetComponentInParent<EarthSpike>();
            spike.ChangeAppearState(false);
            
            transform.parent.GetComponent<BoxCollider>().enabled = false;
            spikeRenderer.enabled = false;
        }
    }
}
