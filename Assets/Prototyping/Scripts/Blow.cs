using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blow : MonoBehaviour
{
    Rigidbody blownObjectRB;

    private ParticleSystem particleSystem;
    private InputAction blowAction;
    
    public TextMeshProUGUI blowText;
    public CharacterController playerController;
    
    public bool blowingPlayer = false;
    public bool blowingBubble = false;

    public float blowObjectForce;
    public float minObjectForce;
    public float maxObjectForce;

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

        blowPlayerSpeed = blowObjectForce / 100;
    }

    private void BlowInput()
    {
        blowObjectForce = Mathf.Clamp(blowObjectForce -= Mouse.current.scroll.ReadValue().y, minObjectForce, maxObjectForce);
        var main = particleSystem.main;
        main.simulationSpeed = blowObjectForce / 200;
        
        
        if (blowAction.WasPressedThisFrame())
        {
            particleSystem.Play(); // play the blow animation particles
        }
        if (blowAction.IsPressed()) // Check to see if the left mouse button is held down
        {
            // perform a raycast to see what's on the player's reticle, only include blowable and default layers
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, LayerMask.GetMask("Blowable", "Default", "Wand"));
            if (hit.transform != null) // if hit something...
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Blowable")) // and that thing is blowable...
                {
                    blowingPlayer = false; // stop blowing the player
                    blownObjectRB = hit.collider.GetComponent<Rigidbody>(); // set the rigidbody of the hit object
                    blowingBubble = false;
                } else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Default")) // if that thing is a solid object...
                {
                    blowingPlayer = true; // blow the player
                    blownObjectRB = null; // stop blowing the object
                    blowingBubble = false;
                } else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wand"))
                {
                    blowingBubble = true;
                    blowingPlayer = false;
                    blownObjectRB = null;
                }
            }
            

            blowText.text = "Blowing...";
        }

        if (blowAction.WasReleasedThisFrame()) // when they let go
        {
            particleSystem.Stop();
            blowText.text = "Space to Blow";
            blowingPlayer = false; // stop blowing the player
            blownObjectRB = null; // stop blowing the object
            blowingBubble = false;
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
