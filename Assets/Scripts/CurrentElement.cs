using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentElementIcon
{
    public GameObject gameObj;
    public Animator anim;
    public Element element;
}

public class CurrentElement : MonoBehaviour
{
    public CurrentElementIcon[] icons;
    public ElementType currentElement;

    private void Awake()
    {
        PlayerElements.ElementChanged += ChangeElement;
    }
    private void OnDestroy()
    {
        PlayerElements.ElementChanged -= ChangeElement;
    }

    void ChangeElement(Element _newElement)
    {
        if (currentElement == _newElement.ElementName) return;
        currentElement = _newElement.ElementName;

        foreach (CurrentElementIcon iconElement in icons)
        {
            if(_newElement.ElementName == iconElement.element.ElementName)
            {
                iconElement.gameObj.SetActive(true);
                iconElement.anim.SetTrigger("Start");
            }
            else iconElement.anim.gameObject.SetActive(false);
        }
    }
}
