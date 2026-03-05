using System;
using System.Runtime.CompilerServices;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Blow : MonoBehaviour
{
    Rigidbody blownObjectRB;

    public GameObject umbrella;

    private ParticleSystem particleSystem;
    private InputAction blowAction;
    
    public TextMeshProUGUI blowText;
    public TextMeshProUGUI umbrellaText;
    public CharacterController playerController;

    public bool blowingPlayer = false;
    public bool blowingBubble = false;
    public bool blowingUmbrella = false;
    public Slider lungMeter;
    public RectTransform lungSize;
    float lungSizeFloat;

    private bool umbrellaEnabled = false;

    public float blowObjectForce;
    public float minObjectForce;
    public float maxObjectForce;

    public float breath;
    public float maxBreath = 10;
    public float breathChangeRate;
    //public float minBreath;

    public float blowPlayerSpeed;

    public AudioSource wind;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        blowAction = InputSystem.actions.FindAction("Blow");

        //maxBreath = 10;
        lungMeter.minValue = minObjectForce - 100;
        lungMeter.maxValue = maxObjectForce + 100;

    }

    // Update is called once per frame
    void Update()
    {
        BlowInput();

        blowPlayerSpeed = blowObjectForce / 100;

        lungSize.localScale = Vector3.one * (breath / 10);
        lungMeter.value = blowObjectForce;

        UmbrellaInput();
        
    }

    void UmbrellaInput()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            SetUmbrella(!umbrella.activeInHierarchy);
        }
    }

    void SetUmbrella(bool umbrellaEnabled = true)
    {
        FirstPersonController fpc = playerController.gameObject.GetComponent<FirstPersonController>();

        umbrella.SetActive(umbrellaEnabled);
        if (umbrella.activeInHierarchy)
        {
            umbrellaText.text = "Close Umbrella";
            fpc.Gravity = -0.5f;
        }
        else
        {
            umbrellaText.text = "Open Umbrella";
            fpc.Gravity = -7.5f;
        }
    }

    private void BlowInput()
    {
        blowObjectForce = Mathf.Clamp(blowObjectForce -= Mouse.current.scroll.ReadValue().y, minObjectForce, maxObjectForce);
        var main = particleSystem.main;
        main.simulationSpeed = blowObjectForce / 200;
        
        
        if (blowAction.WasPressedThisFrame())
        {
            particleSystem.Play(); // play the blow animation particles
            wind.Play();
        }
        if (blowAction.IsPressed()) // Check to see if the left mouse button is held down
        {
            // perform a raycast to see what's on the player's reticle, only include blowable and default layers
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, LayerMask.GetMask("Blowable", "Default", "Wand", "Umbrella"));
            if (hit.transform != null) // if hit something, set the state of this fuckass state machine
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Blowable")) // and that thing is blowable...
                {
                    blowingPlayer = false; // stop blowing the player
                    blownObjectRB = hit.collider.GetComponent<Rigidbody>(); // set the rigidbody of the hit object
                    blowingBubble = false;
                    blowingUmbrella = false;
                } else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Default") && blownObjectRB == null) // if that thing is a solid object...
                {
                    SetUmbrella(false);
                    blowingPlayer = true; // blow the player
                    blownObjectRB = null; // stop blowing the object
                    blowingBubble = false;
                    blowingUmbrella = false;
                } else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wand"))
                {
                    blowingBubble = true;
                    blowingPlayer = false;
                    blownObjectRB = null;
                    blowingUmbrella = false;
                } else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Umbrella"))
                {
                    blowingBubble = false;
                    blowingPlayer = false;
                    blownObjectRB = null;
                    blowingUmbrella = true;
                }
            }

            breath -= blowObjectForce/maxObjectForce * Time.deltaTime;
            if (breath < 0)
            {
                breath = 0;
            }
            blowText.text = "Blowing...";
        }
        else
        {
            breath += 1f * Time.deltaTime;
            if(breath > maxBreath)
            {
                breath = maxBreath;
            }
        }

        if (blowAction.WasReleasedThisFrame() || breath <= 0) // when they let go
        {
            particleSystem.Stop();
            blowText.text = "Blow";
            blowingPlayer = false; // stop blowing the player
            blownObjectRB = null; // stop blowing the object
            blowingBubble = false;
            blowingUmbrella = false;
            wind.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (blownObjectRB)
        {
            blownObjectRB.AddForce(Time.deltaTime * blowObjectForce * transform.forward);
        }
        
        if (blowingUmbrella)

        {
            playerController.Move(Time.deltaTime * blowPlayerSpeed * Vector3.up * 2);
        }

        if (blowingPlayer)
        {
            playerController.Move(Time.deltaTime * blowPlayerSpeed * -transform.forward);
        }
    }
}
