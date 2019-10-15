using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMBAT_Animator_Element : MonoBehaviour
{

    COMBAT_Elements playerElement;

    // Start is called before the first frame update
    void Start()
    {
        playerElement = GetComponentInParent<COMBAT_Elements>();

    }

    public void CallFireCombo()
    {
        playerElement.ElementComboSpawn(Element.Fire);
    }

    public void CallWaterCombo()
    {
        playerElement.ElementComboSpawn(Element.Water);

    }

    public void CallEarthCombo()
    {
        playerElement.ElementComboSpawn(Element.Earth);

    }
    public void CallMetalCombo()
    {
        playerElement.ElementComboSpawn(Element.Metal);

    }
    public void CallWoodCombo()
    {
        playerElement.ElementComboSpawn(Element.Wood);

    }

    public void ChangeToFireEvent()
    {
        Debug.Log("CHANGE TO FIRE");
        playerElement.ChangeElement(Element.Fire);
    }

    public void ChangeToWaterEvent()
    {
        Debug.Log("CHANGE TO WATER");
        playerElement.ChangeElement(Element.Water);
    }

    public void ChangeToMetalEvent()
    {
        Debug.Log("CHANGE TO METAL");
        playerElement.ChangeElement(Element.Metal);
    }

    public void ChangeToWoodEvent()
    {
        Debug.Log("CHANGE TO WOOD");
        playerElement.ChangeElement(Element.Wood);
    }

    public void ChangeToEarthEvent()
    {
        Debug.Log("CHANGE TO EARTH");
        playerElement.ChangeElement(Element.Earth);
    }
}
