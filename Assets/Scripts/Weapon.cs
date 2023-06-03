using UnityEngine;
using System.Collections;
using TMPro;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    public TextMeshProUGUI ammoDisplay;

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        UpdateAmmoDisplay();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoDisplay();
    }

    void Shoot()
    {
        muzzleFlash.Play();

        currentAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    void UpdateAmmoDisplay()
    {
        
        ammoDisplay.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();

    }
}
