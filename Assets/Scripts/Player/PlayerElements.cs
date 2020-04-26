using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    Fire,
    Water,
    Metal,
    Wood,
    Earth,
    None
};

public enum ElementRelation
{
    NONE,
    GENERATION_CYCLE,
    CONTROL_CYCLE
}

public class PlayerElements : MonoBehaviour
{
    public SkinnedMeshRenderer swordRenderer;
    public Color[] colorsToChange;
    public Material[] elementColors;
    public ParticleSystem[] elementParticles;

    public Transform comboSpawnPoint;

    public Element[] allElementsReference;
    public Element currentElement;
    public Element previousElement;
    //public Element nextElement;


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

    public void ChangeElements(Element _newElement)
    {
        previousElement = currentElement;
        currentElement = _newElement;
    }

    void ElementComboSpawn()
    {
        ElementRelation currentCycle = DefineCycle();

        var materialsToChange = swordRenderer.materials;
        ComboEffect comboEffect = null;

        switch (currentElement.ElementName)
        {
            case ElementType.Fire:
                //Spawn Combo Hitbox
                combo = GameManager.Instance.FireComboPool.RequestObject(comboSpawnPoint.position, transform.rotation);
                comboEffect = combo.GetComponent<ComboEffect>();
                comboEffect.playerPos = transform;
                comboEffect.playerStatus = GetComponent<PlayerStatus>();
                if (currentCycle == ElementRelation.CONTROL_CYCLE) comboEffect.ActivateControlCycleEffect();
                if (currentCycle == ElementRelation.GENERATION_CYCLE) comboEffect.ActivateGenerationCycleEffect();

                //Change element
                materialsToChange[0] = elementColors[0];
                weaponTrail._colors[0] = colorsToChange[0];
                break;
            case ElementType.Water:
                //Spawn Combo Hitbox
                combo = GameManager.Instance.WaterComboPool.RequestObject(comboSpawnPoint.position, transform.rotation);
                comboEffect = combo.GetComponent<ComboEffect>();
                comboEffect.playerPos = transform;
                comboEffect.playerStatus = GetComponent<PlayerStatus>();
                if (currentCycle == ElementRelation.CONTROL_CYCLE) combo.GetComponentInChildren<TravelForward>().ActivateControlCycleEffect();
                if (currentCycle == ElementRelation.GENERATION_CYCLE) combo.GetComponentInChildren<TravelForward>().ActivateGenerationCycleEffect();
                combo.GetComponentInChildren<TravelForward>().GainSpeed(GetComponent<PlayerStatus>());

                //Change element
                materialsToChange[0] = elementColors[1];
                weaponTrail._colors[0] = colorsToChange[1];
                break;
            case ElementType.Metal:
                //Spawn Combo Hitbox
                combo = GameManager.Instance.MetalComboPool.RequestObject(transform.position, transform.rotation);
                comboEffect = combo.GetComponent<ComboEffect>();
                comboEffect.playerPos = transform;
                comboEffect.playerStatus = GetComponent<PlayerStatus>();
                if (currentCycle == ElementRelation.CONTROL_CYCLE) comboEffect.ActivateControlCycleEffect();
                if (currentCycle == ElementRelation.GENERATION_CYCLE) comboEffect.ActivateGenerationCycleEffect();

                //Change element
                materialsToChange[0] = elementColors[2];
                weaponTrail._colors[0] = colorsToChange[2];
                break;
            case ElementType.Wood:
                //Spawn Combo Hitbox
                combo = GameManager.Instance.WoodComboPool.RequestObject(comboSpawnPoint.position, transform.rotation);
                comboEffect = combo.GetComponent<ComboEffect>();
                comboEffect.playerPos = transform;
                comboEffect.playerStatus = GetComponent<PlayerStatus>();
                if (currentCycle == ElementRelation.CONTROL_CYCLE) comboEffect.ActivateControlCycleEffect();
                if (currentCycle == ElementRelation.GENERATION_CYCLE) comboEffect.ActivateGenerationCycleEffect();

                //Change element
                materialsToChange[0] = elementColors[3];
                weaponTrail._colors[0] = colorsToChange[3];
                break;
            case ElementType.Earth:
                //Spawn Combo Hitbox
                combo = GameManager.Instance.EarthComboPool.RequestObject(comboSpawnPoint.position + new Vector3(0,-0.3f,0), transform.rotation);
                comboEffect = combo.GetComponent<ComboEffect>();
                comboEffect.playerPos = transform;
                comboEffect.playerStatus = GetComponent<PlayerStatus>();
                if (currentCycle == ElementRelation.CONTROL_CYCLE) comboEffect.ActivateControlCycleEffect();
                if (currentCycle == ElementRelation.GENERATION_CYCLE) comboEffect.ActivateGenerationCycleEffect();

                combo.GetComponent<CameraShake>().Shake();
            

                //Change element
                materialsToChange[0] = elementColors[4];
                weaponTrail._colors[0] = colorsToChange[4];
                break;
        }

        //combo.gameObject.GetComponent<ComboEffect>().playerStatus = GetComponent<PlayerStatus>();
        uiElement.ChangeElementText(currentElement.ElementName);
        ChangeWeaponParticles(currentElement.ElementName);

        swordRenderer.materials = materialsToChange;
    }

    ElementRelation DefineCycle()
    {
        ElementRelation cycle = ElementRelation.NONE;

        if (previousElement.nextGenerationCycleElement == currentElement) cycle = ElementRelation.GENERATION_CYCLE;
        if (previousElement.nextControlCycleElement == currentElement) cycle = ElementRelation.CONTROL_CYCLE;

        Debug.Log(cycle);
        return cycle;
    }

    void ChangeWeaponParticles(ElementType _element)
    {
        for(int i = 0; i < elementParticles.Length; i++)
        {
            elementParticles[i].gameObject.SetActive(false);
        }

        switch (_element)
        {
            case ElementType.Fire:
                elementParticles[0].gameObject.SetActive(true);
                break;
            case ElementType.Water:
                elementParticles[1].gameObject.SetActive(true);
                break;
            case ElementType.Metal:
                elementParticles[2].gameObject.SetActive(true);
                break;
            case ElementType.Wood:
                elementParticles[3].gameObject.SetActive(true);
                break;
            case ElementType.Earth:
                elementParticles[4].gameObject.SetActive(true);
                break;
        }
    }
}
