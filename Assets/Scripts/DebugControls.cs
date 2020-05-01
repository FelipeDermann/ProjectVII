using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugControls : MonoBehaviour
{
    public GameObject debugUI;
    public PlayerStatus playerStatus;
    public PlayerHUD playerHUD;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerHUD = GetComponent<PlayerHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) playerStatus.IncreaseAllMana();

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerStatus.DecreaseHealth(10);
            playerHUD.UpdateHealthBar();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerStatus.IncreaseHealth(10);
            playerHUD.UpdateHealthBar();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (debugUI.activeSelf) debugUI.SetActive(false);
            else debugUI.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position + Vector3.up * 3, 5);
    }
}
