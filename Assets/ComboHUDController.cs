using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboHUDController : MonoBehaviour
{
    public ComboHUDElement[] combos;
    public int attacksDone;

    private void Awake()
    {
        DisableAttackState.FinishedAttack += ResetCombo;
        PlayerAnimation.HeavyAttackDamage += AddInputOnHUD;
        PlayerAnimation.LightAttackDamage += AddInputOnHUD;

        PlayerElements.AttackTypeInput += AddInputOnHUD;
    }

    private void OnDestroy()
    {
        DisableAttackState.FinishedAttack -= ResetCombo;
        PlayerAnimation.HeavyAttackDamage -= AddInputOnHUD;
        PlayerAnimation.LightAttackDamage -= AddInputOnHUD;

        PlayerElements.AttackTypeInput -= AddInputOnHUD;
    }

    void AddInputOnHUD(AttackType _attackType)
    {
        attacksDone += 1;

        foreach (ComboHUDElement comboElement in combos)
        {
            //combos[attacksDone].gameObject.SetActive(true);
            if (comboElement.attackTypes[attacksDone - 1] == _attackType)
            {
                if (comboElement.canBeShown)
                {
                    comboElement.gameObject.SetActive(true);
                    comboElement.RestoreColor(attacksDone - 1);
                }
            }
            else
            {
                comboElement.canBeShown = false;
                comboElement.gameObject.SetActive(false);
            }
        }
    }

    void ResetCombo()
    {
        for (int i = 0; i < combos.Length; i++)
        {
            combos[i].ObscureIcons();
            combos[i].gameObject.SetActive(false);
        }

        attacksDone = 0;
    }
}
