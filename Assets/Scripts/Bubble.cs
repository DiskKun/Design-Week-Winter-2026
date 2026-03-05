using UnityEngine;

public class Bubble : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Destroyed Bubble");
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
