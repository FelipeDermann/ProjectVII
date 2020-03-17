using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOn : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject normalCamera;
    public GameObject lockOnCamera;

    [Header("Target Picking")]
    public List<Transform> screenTargets = new List<Transform>();
    public Transform target;

    public bool isLocked;

    public Image aim;
    public Vector2 uiOffset;

    [Header("Camera Locking")]
    public Transform cameraPoint;
    public float pointDistance;

    private void OnEnable()
    {
        PlayerAnimation.TurnToEnemyIfLockedOn += TurnToLockedEnemy;
    }
    private void OnDisable()
    {
        PlayerAnimation.TurnToEnemyIfLockedOn -= TurnToLockedEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        UserInterface();

        if (isLocked && target == null) CameraLockOff();

        if (screenTargets.Count < 1)
            return;

        if (!isLocked)
        {
            Debug.Log(targetIndex());
            target = screenTargets[targetIndex()];
        }

        if (Input.GetButtonDown("LockOn"))
        {
            if (isLocked) CameraLockOff();
            else CameraLockOn();
        }

        if(isLocked) CameraLocked();

    }

    public void TurnToLockedEnemy()
    {
        if (!isLocked) return;
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
    }

    void CameraLockOff()
    {
        Debug.Log("TARGET NO LONGER LOCKED");
        isLocked = false;

        aim.enabled = false;
        normalCamera.SetActive(true);
        lockOnCamera.SetActive(false);
    }

    void CameraLockOn()
    {
        Debug.Log("TARGET LOCKED");
        isLocked = true;

        aim.enabled = true;
        normalCamera.SetActive(false);
        lockOnCamera.SetActive(true);
    }

    void CameraLocked()
    {
        Vector3 dir = transform.position - target.position;

        Ray ray = new Ray(transform.position, dir);

        Vector3 point = ray.origin + (ray.direction * pointDistance);

        cameraPoint.position = point;
    }

    private void UserInterface()
    {
        if(target != null) aim.transform.position = Camera.main.WorldToScreenPoint(target.position + (Vector3)uiOffset);

        Color c = screenTargets.Count < 1 ? Color.clear : Color.white;
        aim.color = c;
    }

    public int targetIndex()
    {
        float[] distances = new float[screenTargets.Count];

        for (int i = 0; i < screenTargets.Count; i++)
        {
            distances[i] = Vector2.Distance(Camera.main.WorldToScreenPoint(screenTargets[i].position), new Vector2(Screen.width / 2, Screen.height / 2));
        }

        float minDistance = Mathf.Min(distances);
        int index = 0;

        for (int i = 0; i < distances.Length; i++)
        {
            if (minDistance == distances[i])
                index = i;
        }

        return index;
    }
}
