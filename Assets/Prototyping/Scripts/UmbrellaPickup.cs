using UnityEngine;
using UnityEngine.InputSystem;

public class UmbrellaPickup : MonoBehaviour
{
    InputAction drop;
    InputAction grab;
    public GameObject Item;
    public GameObject PlayerCam;
    public Transform playerHoldSpot;

    private float reachLength = 100f;
    RaycastHit ItemHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Item = gameObject;
        grab = InputSystem.actions.FindAction("Grab");
        drop = InputSystem.actions.FindAction("Drop");
    }

    // Update is called once per frame
    void Update()
    {
        PickUpControls();
    }

    void PickUpControls()
    {
        bool PickedUp;

        if (grab.WasPressedThisFrame())
        {
            
            if (Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out ItemHit, reachLength, LayerMask.GetMask("ICanPickUp")))
            {
                Item = ItemHit.collider.gameObject;
                print("I can see the umbrella!");
                PickUpUmbrella();
            }


            else if (drop.WasPressedThisFrame())
            {
                DropUmbrella();
            }
        }
       
    }

    void PickUpUmbrella()
    {
        Debug.Log("You picked up the umbrella.");
        Item.transform.SetParent(playerHoldSpot.transform);
        Item.transform.localPosition = Vector3.zero;
    }

    void DropUmbrella()
    {
        Debug.Log("You dropped the umbrella.");
           Item.transform.SetParent(null);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("ThingICanGrab"))
    //    {
    //        Item = other.gameObject;
    //    }
    //}
}
