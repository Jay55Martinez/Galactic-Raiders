using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPurchaseBehaviour : MonoBehaviour
{
    public int slot;
    public int price;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.weapons[slot]) { // disappear if the player already has this item
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // rotate for visuals
        transform.Rotate(Vector3.up * 90f * Time.deltaTime);
    }

    public void Purchase() {
        if (GameManager.totalCurrency >= price) {
            GameManager.weapons[slot] = true;
            Destroy(gameObject);
        }
    }
}