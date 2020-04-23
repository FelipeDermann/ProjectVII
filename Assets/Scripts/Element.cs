using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Element")]
public class Element : ScriptableObject
{
    public ElementType ElementName;
    public Element nextGenerationCycleElement;
    public Element nextControlCycleElement;
}
