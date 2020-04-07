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

public class Elements : MonoBehaviour
{
    public MeshRenderer swordRenderer;
    public Color[] colorsToChange;
    public Material[] elementColors;
    public ParticleSystem[] elementParticles;

    public Transform comboSpawnPoint;

    public Element currentElement;
    public Element previousElement;

    public Combo currentCombo;

    public bool cancelComboHitbox;

    PoolableObject combo;

    //public ScriptableObject playerStats;
    Quaternion zeroed = new Quaternion(0,0,0,0);

    public UIPlayer uiElement;

    public MeleeWeaponTrail weaponTrail;

    private void OnEnable()
    {
        PlayerAnimation.SpawnComboHitbox += ElementComboSpawn;
    }
    private void OnDisable()
    {
        PlayerAnimation.SpawnComboHitbox -= ElementComboSpawn;
    }

    // Start is called before the first frame update
    void Start()
    {
        var canvasText = GameObject.FindObjectOfType(typeof(UIPlayer)) as UIPlayer;
        uiElement = canvasText.GetComponent<UIPlayer>();

        weaponTrail = GetComponentInChildren<MeleeWeaponTrail>();
    }

    void ElementComboSpawn()
    {
        switch (currentCombo)
        {
            case Combo.FireCombo:
                combo = GameManager.Instance.FireComboPool.RequestObject(comboSpawnPoint.position, transform.rotation);
                combo.GetComponent<ComboEffect>().playerPos = transform;
                combo.gameObject.GetComponent<ComboEffect>().playerStatus = GetComponent<PlayerStatus>();
                break;
            case Combo.WaterCombo:
                combo = GameManager.Instance.WaterComboPool.RequestObject(comboSpawnPoint.position, transform.rotation);
                combo.GetComponent<ComboEffect>().playerPos = transform;
                combo.gameObject.GetComponent<ComboEffect>().playerStatus = GetComponent<PlayerStatus>();
                combo.GetComponentInChildren<TravelForward>().GainSpeed();
                combo.GetComponentInChildren<TravelForward>().player = GetComponent<PlayerStatus>();
                break;
            case Combo.MetalCombo:
                combo = GameManager.Instance.MetalComboPool.RequestObject(comboSpawnPoint.position, transform.rotation);
                combo.GetComponent<ComboEffect>().playerPos = transform;
                combo.gameObject.GetComponent<ComboEffect>().playerStatus = GetComponent<PlayerStatus>();
                break;
            case Combo.WoodCombo:
                combo = GameManager.Instance.WoodComboPool.RequestObject(comboSpawnPoint.position, transform.rotation);
                combo.GetComponent<ComboEffect>().playerPos = transform;
                combo.gameObject.GetComponent<ComboEffect>().playerStatus = GetComponent<PlayerStatus>();
                break;
            case Combo.EarthCombo:
                combo = GameManager.Instance.EarthComboPool.RequestObject(comboSpawnPoint.position + new Vector3(0,-0.3f,0), transform.rotation);
                combo.GetComponent<ComboEffect>().playerPos = transform;
                combo.GetComponent<CameraShake>().Shake();
                combo.gameObject.GetComponent<ComboEffect>().playerStatus = GetComponent<PlayerStatus>();
                break;
        }

        ChangeElement(currentCombo);
        //combo.gameObject.GetComponent<ComboEffect>().playerStatus = GetComponent<PlayerStatus>();
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
