using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarEnemy : MonoBehaviour
{
    Camera mainCamera;
    Image bar;
    public Image barCurrentHealth;
    public float initialDistance;
    public Transform camTransform;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) return;
        Debug.Log("CHAOS CHAOS CHAOS");
        var dist = Vector3.Distance(transform.position, camTransform.position);
        transform.localScale = Vector3.one * dist / initialDistance;

        transform.LookAt(transform.position + camTransform.rotation * Vector3.back, camTransform.rotation * Vector3.down);
        transform.forward = camTransform.forward;
    }

    public void UpdateLifeBar(float currentHealth, float maxHealth)
    {
        var ratioToUpdate = currentHealth / maxHealth;

        barCurrentHealth.fillAmount = ratioToUpdate;
    }

    public void DisableBar()
    {
        gameObject.SetActive(false);
    }

    public void EnableBar()
    {
        gameObject.SetActive(true);
    }
}
