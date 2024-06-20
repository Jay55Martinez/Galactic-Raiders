using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOutOfBounds : MonoBehaviour
{
    public PlayerMovement pm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            other.transform.position = pm.lastGroundedPosition;
            Debug.Log("Player teleported to: " + pm.lastGroundedPosition);
        }
        else
        {
            Debug.Log("Other object entered trigger: " + other.gameObject.name);
        }
    }
}
