using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    public float lootDistance; // maximum pickup distance
    public Text pickupText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, lootDistance)) {
            if (hit.collider.CompareTag("Loot")) {
                GameObject lookAt = hit.collider.gameObject;
                pickupText.text = "Press 'E' to pickup";
                pickupText.gameObject.SetActive(true);
                if (Input.GetKeyUp(KeyCode.E)) {
                    // pick up the item being looked at
                    Destroy(lookAt);
                }
            } else {
                pickupText.gameObject.SetActive(false);
            }
        } else {
            pickupText.gameObject.SetActive(false);
        }
    }
}
