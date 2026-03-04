using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlowTest : MonoBehaviour
{
    InputAction blow;
    Rigidbody blownObjectRB;
    private bool blowingPlayer = false;
    private bool blowingUmbrella = false;
    public TextMeshProUGUI blowText;
    public CharacterController playerController;

    public float blowObjectForce;

    public float blowPlayerSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blow = InputSystem.actions.FindAction("Blow");
    }

    // Update is called once per frame
    void Update()
    {
        BlowInput();

    }

    private void BlowInput()
    {
        if (Mouse.current.leftButton.IsPressed())
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, 10f, LayerMask.GetMask("Blowable", "Default", "Umbrella"));
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Blowable"))
                {
                    blowingPlayer = false;

                    //if (blownObjectRB.gameObject != hit.transform.gameObject)
                    //{
                    Debug.Log("blowing object");
                    blownObjectRB = hit.collider.GetComponent<Rigidbody>();
                    //}


                }

                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Umbrella"))
                {
                    blowingUmbrella = true;
                    blownObjectRB = null;
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
                {
                    blowingPlayer = true;
                    blownObjectRB = null;
                }
            }


            blowText.text = "Blowing...";
        }
        else
        {
            blowingPlayer = false;
            blownObjectRB = null;
            blowingUmbrella = false;
        }

        if (blow.WasReleasedThisFrame())
        {
            blowText.text = "Click to Blow";
        }
    }

    private void FixedUpdate()
    {
        if (blownObjectRB != null)
        {
            blownObjectRB.AddForce(transform.forward * Time.deltaTime * blowObjectForce);
        }

        if (blowingUmbrella)

        {
            playerController.Move(Vector3.up/2);
        }

        if (blowingPlayer)
        {
            playerController.Move(-transform.forward * Time.deltaTime * blowPlayerSpeed);
        }
    }
}
