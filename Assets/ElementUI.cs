using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElementUI : MonoBehaviour
{
    public ArrowUI[] UIToActivate;

    private void OnEnable()
    {
        PlayerElements.ElementChanged += ChangeCurrentElementOnUI;
    }
    private void OnDisable()
    {
        PlayerElements.ElementChanged -= ChangeCurrentElementOnUI;
    }

    void ChangeCurrentElementOnUI(Element _newElement)
    {
        for (int i = 0; i < UIToActivate.Length; i++)
        {
            if (UIToActivate[i].elementType == _newElement.ElementName)
            {
                UIToActivate[i].controlArrowAnim.SetBool("Blinking", true);
                UIToActivate[i].controlArrowAnim.transform.SetAsLastSibling();
                UIToActivate[i].generationArrowAnim.SetBool("Blinking", true);
                UIToActivate[i].elementIconAnim.SetBool("Blinking", true);
            }
            else
            {
                UIToActivate[i].controlArrowAnim.SetBool("Blinking", false);
                UIToActivate[i].generationArrowAnim.SetBool("Blinking", false);
                UIToActivate[i].elementIconAnim.SetBool("Blinking", false);
            }
        }
    }
}
