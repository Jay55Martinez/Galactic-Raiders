using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    public GameObject Door;

    public void OpenDoor()
    {
        Destroy(Door);
    }
}
