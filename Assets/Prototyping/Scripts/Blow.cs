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

    public float blowForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blow = InputSystem.actions.FindAction("Blow");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.forward * 100, Color.red);
        if (blow.WasPressedThisFrame())
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, 100f, LayerMask.GetMask("Blowable"));
            if (hit.collider != null)
            {
                blownObjectRB = hit.collider.GetComponent<Rigidbody>();
            }

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
