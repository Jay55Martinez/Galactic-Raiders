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
                if (lookAt.CompareTag("ControlPanel"))
                {
                    FindObjectOfType<LevelManager>().LevelBeat();
                }
                else if (lookAt.CompareTag("Ammo"))
                {
                    FindObjectOfType<GunHandling>().AddReserveAmmo(5);
                    FindObjectOfType<LevelManager>().UpdateAmmoCounter(5);
                } 
                else if (lookAt.CompareTag("Heal"))
                {
                    FindObjectOfType<LevelManager>().UpdateHealCounter(1);
                }
                else if (lookAt.CompareTag("Loot")) {
                    lookAt.GetComponent<LootBehaviour>().Die();
                }
                else if (lookAt.CompareTag("Purchase")) {
                    lookAt.GetComponent<GunPurchaseBehaviour>().Purchase();
                }
                // add gun pickup
                else {
                    Destroy(lookAt);
                }
            }
        }
    }

    void FixedUpdate() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, lootDistance)) {
            if (hit.collider.CompareTag("Loot") || hit.collider.CompareTag("Ammo") || hit.collider.CompareTag("Heal")) {
                lookAt = hit.collider.gameObject;
                pickupText.text = "Press 'E' to pickup";
                pickupText.gameObject.SetActive(true);
            }
            else if (hit.collider.CompareTag("ControlPanel")) {
                lookAt = hit.collider.gameObject;
                pickupText.text = "Press 'E' to warp out";
                pickupText.gameObject.SetActive(true);
            }
            else if (hit.collider.CompareTag("Purchase")) {
                lookAt = hit.collider.gameObject;
                pickupText.text = "Press 'E' to purchase for " + 
                    lookAt.GetComponent<GunPurchaseBehaviour>().price;
                pickupText.gameObject.SetActive(true);
            }
            else 
            {
                pickupText.gameObject.SetActive(false);
                lookAt = null;
            }
        } else {
            pickupText.gameObject.SetActive(false);
            lookAt = null;
        }
    }
}
