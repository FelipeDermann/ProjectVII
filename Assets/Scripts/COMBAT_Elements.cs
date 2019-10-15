using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Element
{
    Fire,
    Water,
    Metal,
    Wood,
    Earth,
    None
};
public class COMBAT_Elements : MonoBehaviour
{

    public MeshRenderer swordRenderer;
    public Material[] elementColors;
    public ParticleSystem[] elementParticles;

    public GameObject[] comboHitbox;
    public Transform comboSpawnPoint;

    public Element currentElement;
    public Element previousElement;

    //public ScriptableObject playerStats;
    Quaternion zeroed = new Quaternion(0,0,0,0);

    public UIPlayer uiElement;

    // Start is called before the first frame update
    void Start()
    {
        var canvasText = GameObject.FindObjectOfType(typeof(UIPlayer)) as UIPlayer;
        uiElement = canvasText.GetComponent<UIPlayer>();
    }

    public void ElementComboSpawn(Element _element)
    {
        GameObject combo = null;

        switch (_element)
        {
            case Element.Fire:
                combo = Instantiate(comboHitbox[0], comboSpawnPoint.position, transform.rotation);
                Destroy(combo, 1);
                break;
            case Element.Water:
                combo = Instantiate(comboHitbox[1], comboSpawnPoint.position, transform.rotation);
                Destroy(combo, 1);
                break;
            case Element.Metal:
                combo = Instantiate(comboHitbox[2], comboSpawnPoint.position, transform.rotation);
                Destroy(combo, 1);
                break;
            case Element.Wood:
                combo = Instantiate(comboHitbox[3], comboSpawnPoint.position, transform.rotation);
                Destroy(combo, 1);
                break;
            case Element.Earth:
                combo = Instantiate(comboHitbox[4], comboSpawnPoint.position, transform.rotation);
                Destroy(combo, 1);
                break;
        }

        combo.GetComponent<COMBAT_ComboEffect>().playerMagic = GetComponent<COMBAT_Magic>();
    }

    public void ChangeElement(Element _element)
    {
        currentElement = _element;
        uiElement.ChangeElementText(_element);
        ChangeWeaponParticles(_element);

        var materialsToChange = swordRenderer.materials;

        switch (_element)
        {
            case Element.Fire:
                materialsToChange[1] = elementColors[0];
                break;
            case Element.Water:
                materialsToChange[1] = elementColors[1];
                break;
            case Element.Metal:
                materialsToChange[1] = elementColors[2];
                break;
            case Element.Wood:
                materialsToChange[1] = elementColors[3];
                break;
            case Element.Earth:
                materialsToChange[1] = elementColors[4];
                break;
        }

        swordRenderer.materials = materialsToChange;
    }

    void ChangeWeaponParticles(Element _element)
    {
        for(int i = 0; i < elementParticles.Length; i++)
        {
            elementParticles[i].gameObject.SetActive(false);
        }

        switch (_element)
        {
            case Element.Fire:
                elementParticles[0].gameObject.SetActive(true);
                break;
            case Element.Water:
                elementParticles[1].gameObject.SetActive(true);
                break;
            case Element.Metal:
                elementParticles[2].gameObject.SetActive(true);
                break;
            case Element.Wood:
                elementParticles[3].gameObject.SetActive(true);
                break;
            case Element.Earth:
                elementParticles[4].gameObject.SetActive(true);
                break;
        }
    }
}
