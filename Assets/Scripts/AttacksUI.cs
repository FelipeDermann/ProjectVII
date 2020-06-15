using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksUI : MonoBehaviour
{
    public AttacksIcon[] attacksIcon;

    private void OnEnable()
    {
        Attack.AttackExecuted += AddAttack;
        DisableAttackState.FinishedAttack += ResetAttacks;
    }
    private void OnDisable()
    {
        Attack.AttackExecuted += AddAttack;
        DisableAttackState.FinishedAttack -= ResetAttacks;
    }

    void AddAttack(int _currentAttackIndex, string _attackType)
    {

        if (_attackType == "light") attacksIcon[_currentAttackIndex].lightAttackIcon.SetActive(true);
        else attacksIcon[_currentAttackIndex].heavyAttackIcon.SetActive(true);
    }

    void ResetAttacks()
    {
        for (int i = 0; i < attacksIcon.Length; i++)
        {
            attacksIcon[i].heavyAttackIcon.SetActive(false);
            attacksIcon[i].lightAttackIcon.SetActive(false);
        }
    }
}
