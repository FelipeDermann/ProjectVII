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
    public Color[] colorsToChange;
    public Material[] elementColors;
    public ParticleSystem[] elementParticles;

    public GameObject[] comboHitbox;
    public Transform comboSpawnPoint;

    public Element currentElement;
    public Element previousElement;

    public Combo currentCombo;

    public bool cancelComboHitbox;

    //public ScriptableObject playerStats;
    Quaternion zeroed = new Quaternion(0,0,0,0);

    public UIPlayer uiElement;

    public MeleeWeaponTrail weaponTrail;

    private void OnEnable()
    {
        ComboBehaviour.SpawnComboHitbox += CallElementComboSpawn;
        DashBehaviour.CancelSpawnHitbox += CancelComboHitboxSpawn;
        MagicBehaviour.CancelComboHitbox += CancelComboHitboxSpawn;
    }
    private void OnDisable()
    {
        ComboBehaviour.SpawnComboHitbox -= CallElementComboSpawn;
        DashBehaviour.CancelSpawnHitbox -= CancelComboHitboxSpawn;
        MagicBehaviour.CancelComboHitbox -= CancelComboHitboxSpawn;
    }

    // Start is called before the first frame update
    void Start()
    {
        var canvasText = GameObject.FindObjectOfType(typeof(UIPlayer)) as UIPlayer;
        uiElement = canvasText.GetComponent<UIPlayer>();

        weaponTrail = GetComponentInChildren<MeleeWeaponTrail>();
    }

    void CancelComboHitboxSpawn()
    {
        cancelComboHitbox = true;
    }
    public void AllowComboHitboxSpawn()
    {
        cancelComboHitbox = false;
    }

    void CallElementComboSpawn(float time)
    {
        StartCoroutine(ElementComboSpawn(currentCombo, time));
    }

    IEnumerator ElementComboSpawn(Combo _combo, float time)
    {
        yield return new WaitForSeconds(time);

        GameObject combo = null;

        if (!cancelComboHitbox)
        {
            switch (_combo)
            {
                case Combo.FireCombo:
                    combo = Instantiate(comboHitbox[0], comboSpawnPoint.position, transform.rotation);
                    combo.GetComponent<FireWave>().GainSpeed();
                    Destroy(combo, 1);
                    break;
                case Combo.WaterCombo:
                    combo = Instantiate(comboHitbox[1], comboSpawnPoint.position, transform.rotation);
                    Destroy(combo, 1);
                    break;
                case Combo.MetalCombo:
                    combo = Instantiate(comboHitbox[2], comboSpawnPoint.position, transform.rotation);
                    Destroy(combo, 1);
                    break;
                case Combo.WoodCombo:
                    combo = Instantiate(comboHitbox[3], comboSpawnPoint.position, transform.rotation);
                    Destroy(combo, 1);
                    break;
                case Combo.EarthCombo:
                    combo = Instantiate(comboHitbox[4], comboSpawnPoint.position, transform.rotation);
                    Destroy(combo, 1);
                    break;
            }

            ChangeElement(currentCombo);
            combo.GetComponent<COMBAT_ComboEffect>().playerMagic = GetComponent<COMBAT_Magic>();
        }
        else cancelComboHitbox = false;
    }

    public void ChangeElement(Combo combo)
    {
        Element _element = Element.None;
        var materialsToChange = swordRenderer.materials;

        switch (combo)
        {
            case Combo.FireCombo:
                materialsToChange[1] = elementColors[0];
                weaponTrail._colors[0] = colorsToChange[0]; 
                _element = Element.Fire;
                break;
            case Combo.WaterCombo:
                materialsToChange[1] = elementColors[1];
                weaponTrail._colors[0] = colorsToChange[1]; 
                _element = Element.Water;
                break;
            case Combo.MetalCombo:
                materialsToChange[1] = elementColors[2];
                weaponTrail._colors[0] = colorsToChange[2]; 
                _element = Element.Metal;
                break;
            case Combo.WoodCombo:
                materialsToChange[1] = elementColors[3];
                weaponTrail._colors[0] = colorsToChange[3]; 
                _element = Element.Wood;
                break;
            case Combo.EarthCombo:
                materialsToChange[1] = elementColors[4];
                weaponTrail._colors[0] = colorsToChange[4];
                _element = Element.Earth;
                break;
        }

        currentElement = _element;
        uiElement.ChangeElementText(_element);
        ChangeWeaponParticles(_element);

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
