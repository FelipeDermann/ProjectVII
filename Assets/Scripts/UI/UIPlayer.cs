using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPlayer : MonoBehaviour
{
    TextMeshProUGUI myText;

    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeElementText(ElementType _element)
    {
        string textToAdd = "Teste";

        switch(_element)
        {
            case ElementType.Fire: 
                textToAdd = "<color=red>Fire</color>";
                break;
            case ElementType.Water: 
                textToAdd = "<color=blue>Water</color>";
                break;
            case ElementType.Wood:
                textToAdd = "<color=green>Wood</color>";
                break;
            case ElementType.Metal:
                textToAdd = "<color=black>Metal</color>";
                break;
            case ElementType.Earth:
                textToAdd = "<color=yellow>Earth</color>";
                break;
        }

        myText.text = textToAdd;
    }
}
