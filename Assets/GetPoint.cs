using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPoint : MonoBehaviour
{
    public Transform cameraPoint;
    public Transform target;
    public Vector3 offset;
    public float pointDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;

        Ray ray = new Ray(target.position, dir);

        Vector3 point = ray.origin + (ray.direction * pointDistance);

        cameraPoint.position = point;
    }

    private void OnDrawGizmos()
    {
        // Draws a blue line from this transform to the target
        //Gizmos.color = Color.blue;


        //Vector3 dir = target.position - transform.position;

        //Ray ray = new Ray(transform.position, dir);
        //Physics.Raycast(transform.position, dir);

        //Vector3 point = ray.origin + (ray.direction * 20);

        //Gizmos.DrawRay(ray);
        //Gizmos.DrawCube(point, new Vector3(4,4,4));
        //cameraPoint.position = point;
        //Gizmos.DrawLine(transform.position, target.position + offset);
    }
}
