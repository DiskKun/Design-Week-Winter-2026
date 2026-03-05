using UnityEngine;

public class Boat : MonoBehaviour
{

    public GameObject PirateBoat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BlowTest BlowTestScript = GetComponent<BlowTest>();
        
        //if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Forward"))
        //{
            transform.position += transform.forward * Time.deltaTime;
        //}
        
        
    }
}
