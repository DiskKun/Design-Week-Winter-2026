using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blow : MonoBehaviour
{
    InputAction blow;
    Rigidbody blownObjectRB;
    public TextMeshProUGUI blowText;

    //Collider BlowCldr;
    //RaycastHit BlowHit;

    public float MaxBlowDist;
    public float BlowSpeed;
    private bool BlowDetect;

    public float blowForce;
    public AudioSource Trumpet;
    //private bool TrumpetSFXPlay = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blow = InputSystem.actions.FindAction("Blow");
        //BlowCldr = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
       // TrumpetSFXPlay = false;
        if (blow.WasPressedThisFrame())
        {
            //BlowDetect = Physics.BoxCast(BlowCldr.bounds.center, transform.localScale * 1, transform.forward, out BlowHit, transform.rotation, MaxBlowDist); 
            //if (BlowDetect && BlowHit.collider != null)
            //{
            //    blownObjectRB = BlowHit.collider.GetComponent<Rigidbody>();
            //}
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, 100f, LayerMask.GetMask("Blowable"));
            if (hit.collider != null)
            {
                blownObjectRB = hit.collider.GetComponent<Rigidbody>();
            }

            //else if (hit.collider != null && GameObject.FindWithTag("Trumpet"))
            //{
            //    TrumpetSFXPlay = true;
            //    if (TrumpetSFXPlay == true)
            //    {
            //        Trumpet.Play();
            //        Debug.Log("Trumpet sound is playing");
            //    }
            //}

            blowText.text = "Blowing...";
        }

        if (blow.WasReleasedThisFrame())
        {
            blownObjectRB = null;
            blowText.text = "Click to Blow";
        }
    }

    private void FixedUpdate()
    {
        if (blownObjectRB != null)
        {
            blownObjectRB.AddForce(transform.forward * Time.deltaTime * blowForce);
        }
    }
}
