using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public LockOn lockOn;
    public Transform parentTransform;

    void Start()
    {
        lockOn = FindObjectOfType<LockOn>();
    }

    public void RemoveFromList()
    {
        lockOn.screenTargets.Remove(transform);
    }

    private void OnBecameVisible()
    {
        if (!lockOn.screenTargets.Contains(transform) && !parentTransform.GetComponent<Enemy>().dead)
            lockOn.screenTargets.Add(transform);
    }

    private void OnBecameInvisible()
    {
        if (lockOn.screenTargets.Contains(transform))
            lockOn.screenTargets.Remove(transform);
    }
}
