using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunHandling : MonoBehaviour
{
    // Gun Stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap, reserveAmmo;
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

    // audio
    public AudioClip fireSFX;
    public AudioClip reloadSFX;

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
        if (ammoText != null)
            ammoText.SetText(bulletsLeft + "/" + reserveAmmo);

        if (reloadText != null) {
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

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && reserveAmmo != 0)
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
            transform.localPosition = Vector3.Lerp(transform.localPosition, standardPosition - new Vector3(0, 0, 0.6f), 10f * Time.deltaTime);
        } else {
            transform.localPosition = Vector3.Lerp(transform.localPosition, standardPosition, 10f * Time.deltaTime);
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
                rayHit.collider.gameObject.GetComponent<EnemyHit>().TakeDamage(damage);
            } else {
                // only place bullethole if there are no enemies
                Instantiate(bulletHole, rayHit.point, Quaternion.Euler(0, 180, 0)); // this only works on walls in certain directions
            }
        }

        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        // Count down shots
        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

        // audio
        AudioSource.PlayClipAtPoint(fireSFX, transform.position);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        AudioSource.PlayClipAtPoint(reloadSFX, transform.position);
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        // remove ammo from reserves
        if (reserveAmmo >= magazineSize - bulletsLeft)
        {
            reserveAmmo -= (magazineSize - bulletsLeft);
            bulletsLeft = magazineSize;
        }
        else if (reserveAmmo < magazineSize - bulletsLeft)
        {
            bulletsLeft += reserveAmmo;
            reserveAmmo = 0;
        }
        reloading = false;
    }

    // Adds ammo to the reserve
    public void AddReserveAmmo(int amount)
    {
        reserveAmmo += amount;
        reserveAmmo  = Mathf.Clamp(reserveAmmo, 0, magazineSize);
    }
}
