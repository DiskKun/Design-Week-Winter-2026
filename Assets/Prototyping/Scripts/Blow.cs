using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blow : MonoBehaviour
{
    Rigidbody blownObjectRB;
    private bool blowingPlayer = false;
    public TextMeshProUGUI blowText;
    public CharacterController playerController;

    public float blowObjectForce;

    public float blowPlayerSpeed;

    // Update is called once per frame
    void Update()
    {
        BlowInput();
    }

    private void BlowInput()
    {
        if (Mouse.current.leftButton.IsPressed()) // Check to see if the left mouse button is held down
        {
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, LayerMask.GetMask("Blowable", "Default"));
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Blowable"))
                {
                    blowingPlayer = false;
                    blownObjectRB = hit.collider.GetComponent<Rigidbody>();
                } else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
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
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
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

        if (blowingPlayer)
        {
            playerController.Move(-transform.forward * Time.deltaTime * blowPlayerSpeed);
        }
    }
}
