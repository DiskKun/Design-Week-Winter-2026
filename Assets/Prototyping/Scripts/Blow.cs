using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blow : MonoBehaviour
{
    Rigidbody blownObjectRB;

    private ParticleSystem particleSystem;
    private bool blowingPlayer = false;
    private InputAction blowAction;
    
    public TextMeshProUGUI blowText;
    public CharacterController playerController;
    

    public float blowObjectForce;

    public float blowPlayerSpeed;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        blowAction = InputSystem.actions.FindAction("Blow");
    }

    // Update is called once per frame
    void Update()
    {
        BlowInput();
    }

    private void BlowInput()
    {
        if (blowAction.WasPressedThisFrame())
        {
            particleSystem.Play(); // play the blow animation particles
        }
        if (blowAction.IsPressed()) // Check to see if the left mouse button is held down
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

        if (blowAction.WasReleasedThisFrame()) // when they let go
        {
            particleSystem.Stop();
            blowText.text = "Click to Blow";
            blowingPlayer = false; // stop blowing the player
            blownObjectRB = null; // stop blowing the object
        }
    }

    private void FixedUpdate()
    {
        if (blownObjectRB)
        {
            blownObjectRB.AddForce(Time.deltaTime * blowObjectForce * transform.forward);
        }

        if (blowingPlayer)
        {
            playerController.Move(Time.deltaTime * blowPlayerSpeed * -transform.forward);
        }
    }
}
