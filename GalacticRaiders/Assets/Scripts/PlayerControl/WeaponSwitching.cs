using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    // the selected weapon
    public int currentWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {

        int prevWeapon = currentWeapon;

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

        if (prevWeapon != currentWeapon)
        {
            SelectWeapon();
        }
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
