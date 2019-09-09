using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private MovementInput input;
    public Animator anim;

    public GameObject camera3drPerson;
    public GameObject cameraFar;
    public GameObject cameraFar2;


    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<MovementInput>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.F1))
        {
            camera3drPerson.SetActive(true);
            cameraFar.SetActive(false);
            cameraFar2.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            camera3drPerson.SetActive(false);
            cameraFar.SetActive(true);
            cameraFar2.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            camera3drPerson.SetActive(false);
            cameraFar.SetActive(false);
            cameraFar2.SetActive(true);
        }


        anim.SetFloat("Blend", input.Speed);

    }
}
