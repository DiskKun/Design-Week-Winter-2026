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
            // perform a raycast to see what's on the player's reticle, only include blowable and default layers
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, LayerMask.GetMask("Blowable", "Default"));
            if (hit.transform != null) // if hit something...
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Blowable")) // and that thing is blowable...
                {
                    blowingPlayer = false; // stop blowing the player
                    blownObjectRB = hit.collider.GetComponent<Rigidbody>(); // set the rigidbody of the hit object
                } else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Default")) // if that thing is a solid object...
                {
                    blowingPlayer = true; // blow the player
                    blownObjectRB = null; // stop blowing the object
                }
            }
            

            blowText.text = "Blowing...";
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame) // when they let go
        {
            blowText.text = "Click to Blow";
            blowingPlayer = false; // stop blowing the player
            blownObjectRB = null; // stop blowing the object
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
