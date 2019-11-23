using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    float shakeForce;
    float shakeTime;

    public CinemachineFreeLook cameraFL;
    public CinemachineBasicMultiChannelPerlin cameraFLNoise;


    void Start()
    {
        cameraFL = GetComponent<CinemachineFreeLook>();
        //cameraFLNoise = cameraFL.m
    }

    public void Shake(float _shakeForce, float _shakeTime)
    {
       // cameraFL.

    }
}
