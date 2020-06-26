using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboHUDController : MonoBehaviour
{
    [Header("Combo HUD elements to show")]
    public ComboHUDElement[] combos;

    int attacksDone;
    [SerializeField]
    int posIndex;
    PlayerElements playerElements;

    [Header("Initial colors for rects")]
    public Color controlRecColor;
    public Color generationRecColor;

    [Header("Pivot Positions for HUD elements to appear")]
    public Transform[] pivotPos;

    bool isControlCycle;
    bool isGenerationCycle;

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

    void Start()
    {
        playerElements = GameObject.FindObjectOfType<PlayerElements>();
    }

    void AddInputOnHUD(AttackType _attackType)
    {
        attacksDone += 1;

        foreach (ComboHUDElement comboElement in combos)
        {
            if (comboElement.attackTypes[attacksDone - 1] == _attackType)
            {
                if (attacksDone == 1)
                {
                    //HUD positions
                    comboElement.transform.position = pivotPos[posIndex].position;
                    posIndex += 1;
                    if (posIndex >= pivotPos.Length) posIndex = pivotPos.Length - 1;

                    //checks if combo is part of the generation or control cycle on the first attack
                    if (playerElements.currentElement.nextGenerationCycleElement.ElementName == comboElement.element.ElementName)
                    {
                        isGenerationCycle = true;
                        comboElement.rectangleImage.color = generationRecColor;
                        comboElement.generationIcon.SetActive(true);
                    }
                    if (playerElements.currentElement.nextControlCycleElement.ElementName == comboElement.element.ElementName)
                    {
                        isControlCycle = true;
                        comboElement.rectangleImage.color = controlRecColor;
                        comboElement.controlIcon.SetActive(true);
                    }
                }

                //shows the combo icons if they're part of the combo
                if (comboElement.canBeShown)
                {
                    comboElement.gameObject.SetActive(true);
                    comboElement.RestoreColor(attacksDone - 1);
                }

                //starts the rectangle animation
                if (attacksDone == 1)
                {
                    comboElement.rectangleAnim.SetBool("Control", isControlCycle);
                    comboElement.rectangleAnim.SetBool("Generation", isGenerationCycle);

                    isControlCycle = false;
                    isGenerationCycle = false;
                }
            }
            else
            {
                comboElement.canBeShown = false;
                comboElement.GetComponent<Animator>().SetTrigger("Fade");
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

        posIndex = 0;
        attacksDone = 0;
    }
}
