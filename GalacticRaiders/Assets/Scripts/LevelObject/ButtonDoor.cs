using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    public GameObject door;
    public AudioClip SFXdoor;

    public void OpenDoor()
    {
        if (SFXdoor != null)
            AudioSource.PlayClipAtPoint(SFXdoor, transform.position);

        Destroy(door);
    }
}
