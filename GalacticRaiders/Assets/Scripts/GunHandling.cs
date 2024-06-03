using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunHandling : MonoBehaviour
{
    // Gun Stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    // Booleans
    bool shooting, readyToShoot, reloading;

    // References
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    // Graphics
    public GameObject muzzleFlash, bulletHole;
    public TextMeshProUGUI ammoText, reloadText;

    // recoil anim
    private Vector3 standardPosition; 

    public void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        standardPosition = transform.localPosition;
    }

    public void Update()
    {
        MyInput();

        // SetText
        ammoText.SetText(bulletsLeft + "/" + magazineSize);

        if (bulletsLeft == 0 && !reloading)
        {
            reloadText.SetText("Reload");
        }
        else if (reloading)
        {
            reloadText.SetText("Reloading");
        }
        else
        {
            reloadText.SetText("");
        }
    }

    private void MyInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }

        // animate gun moving back
        if (shooting && bulletsLeft > 0) {
            transform.localPosition = standardPosition - new Vector3(0, 0, 0.6f);
        } else {
            transform.localPosition = standardPosition;
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Adding Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        // RayCasting
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            if (rayHit.collider.CompareTag("Enemy"))
            {
                // TODO: have enemy take damage
            }
        }

        Instantiate(bulletHole, rayHit.point, Quaternion.Euler(0, 180, 0));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        // animate;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
