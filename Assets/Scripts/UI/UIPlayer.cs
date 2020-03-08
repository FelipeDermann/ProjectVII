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

    public void ChangeElementText(Element _element)
    {
        string textToAdd = "Teste";

        switch(_element)
        {
            case Element.Fire: 
                textToAdd = "<color=red>Fire</color>";
                break;
            case Element.Water: 
                textToAdd = "<color=blue>Water</color>";
                break;
            case Element.Wood:
                textToAdd = "<color=green>Wood</color>";
                break;
            case Element.Metal:
                textToAdd = "<color=black>Metal</color>";
                break;
            case Element.Earth:
                textToAdd = "<color=yellow>Earth</color>";
                break;
        }

        myText.text = textToAdd;
    }
}
