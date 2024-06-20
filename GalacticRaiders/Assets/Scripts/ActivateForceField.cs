using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateForceField : MonoBehaviour
{
    public GameObject forceField;

    private void OnTriggerEnter(Collider other)
    {
        forceField.SetActive(true);
    }
}
