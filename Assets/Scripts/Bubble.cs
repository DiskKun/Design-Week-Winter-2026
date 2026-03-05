using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Destroyed Bubble");
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(gameObject);
        }
    }
}
