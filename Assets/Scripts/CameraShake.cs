using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    float shakeForce;
    float shakeTime;

    public SignalSourceAsset signalAsset;
    CinemachineImpulseSource impulseSource;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        impulseSource.m_ImpulseDefinition.m_RawSignal = signalAsset;

        Shake();
    }

    public void Shake()
    {
        impulseSource.GenerateImpulse();
    }
}
