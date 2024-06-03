using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    public float lootDistance; // maximum pickup distance
    public Text pickupText;
    GameObject lookAt; // store what the player is looking at

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (lookAt != null) {
                Debug.Log("pickup!");
                // pick up the item being looked at
                Destroy(lookAt);
            }
        }
    }

    void FixedUpdate() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, lootDistance)) {
            if (hit.collider.CompareTag("Loot")) {
                lookAt = hit.collider.gameObject;
                pickupText.text = "Press 'E' to pickup";
                pickupText.gameObject.SetActive(true);
            } else {
                pickupText.gameObject.SetActive(false);
                lookAt = null;
            }
        } else {
            pickupText.gameObject.SetActive(false);
            lookAt = null;
        }
    }
}
