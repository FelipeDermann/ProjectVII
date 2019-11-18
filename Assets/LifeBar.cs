using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    Camera mainCamera;
    public GameObject lifeBar;
    Image bar;
    public Image barCurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        bar = Instantiate(lifeBar, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
        barCurrentHealth = bar.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        bar.transform.position = mainCamera.WorldToScreenPoint(transform.position);
    }

    public void UpdateLifeBar(float currentHealth, float maxHealth)
    {
        var ratioToUpdate = currentHealth / maxHealth;

        barCurrentHealth.fillAmount = ratioToUpdate;
    }

    public void DisableBar()
    {
        bar.gameObject.SetActive(false);
    }
}
