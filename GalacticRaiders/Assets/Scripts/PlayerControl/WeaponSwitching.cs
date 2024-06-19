using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    // the selected weapon
    public int currentWeapon = 0;
    public bool devMode; // gives player all weapons for testing
    private int prevWeapon;

    public AudioClip switchSFX;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = 3; // out of bounds, nothing will show
        for (int i = 0; i < GameManager.weapons.Length; i++) {
            if (GameManager.weapons[i]) {
                currentWeapon = i;
                break;
            }
        }
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        prevWeapon = currentWeapon;

        // ScrollSelect();
        KeySelect();

        if (prevWeapon != currentWeapon)
        {
            SelectWeapon();
            AudioSource.PlayClipAtPoint(switchSFX, transform.position);
        }
    }

    void ScrollSelect() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (currentWeapon >= transform.childCount - 1)
                currentWeapon = 0;
            else
                currentWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (currentWeapon <= 0)
                currentWeapon = transform.childCount - 1;
            else
                currentWeapon--;
        }
    }

    void KeySelect() {
        if (Input.GetKeyDown(KeyCode.Alpha1) && (GameManager.weapons[0] || devMode)) {
            currentWeapon = 0;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && (GameManager.weapons[1] || devMode)) {
            currentWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && (GameManager.weapons[2] || devMode)) {
            currentWeapon = 2;
        }

        FindObjectOfType<LevelManager>().UpdateWeaponUI(currentWeapon);
    }

    // Sets the weapon to the currentWeapon value
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == currentWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
