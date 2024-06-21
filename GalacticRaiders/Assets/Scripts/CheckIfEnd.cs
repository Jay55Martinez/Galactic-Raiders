using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfEnd : MonoBehaviour
{
    public GameObject checkIfActive;

    // Update is called once per frame
    void Update()
    {
        if (checkIfActive.active)
        {
            gameObject.SetActive(false);
        }
    }
}
