using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasWorldFaceCamera : MonoBehaviour
{
    Vector3 vec1;
    Vector3 vec2;
    Transform camTransform;
    public float initialDistance;
    public bool constantScale;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) return;
        var dist = Vector3.Distance(transform.position, camTransform.position);
        if (!constantScale) transform.localScale = Vector3.one * dist / initialDistance;

        vec1 = transform.position + camTransform.rotation * Vector3.back;
        vec2 = camTransform.rotation * Vector3.down;
        transform.LookAt(vec1, vec2);
        transform.forward = camTransform.forward;
    }
}
