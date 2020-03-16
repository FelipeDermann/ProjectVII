using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimation : MonoBehaviour
{
    public static event Action StartHitbox;
    public static event Action EndHitbox;

    public static event Action SpawnMagicHitbox;
    public static event Action SpawnComboHitbox;

    public static event Action StartNextAttackInput;

    public static event Action RestartScene;

    public float secondsOfInputDetection;

    public void RestartSceneAfterTime()
    {
        RestartScene?.Invoke();
    }

    public void EnableInput()
    {
        StartNextAttackInput?.Invoke();
        Debug.Log("CAN INPUT NEXT ATTACK");
    }

    public void ActivateHitbox()
    {
        StartHitbox?.Invoke();
        Debug.Log("HITBOX ON");
    }

    public void DeactivateHitbox()
    {
        EndHitbox?.Invoke();
        Debug.Log("HITBOX OFF");
    }

    public void StartCombo()
    {
        SpawnComboHitbox?.Invoke();
    }

    public void StartSpell()
    {
        SpawnMagicHitbox?.Invoke();
    }
}
