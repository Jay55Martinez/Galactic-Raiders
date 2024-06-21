using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnDestroy : MonoBehaviour
{
    public GameObject other;

    private void OnDestroy()
    {
        other.SetActive(true);
    }
}
