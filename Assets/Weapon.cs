
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f; 
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    private float nextTimeToFire = 5f; 

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate; 
            Shoot();
        }

        
    }

    void Shoot()
    {
        muzzleFlash.Play(); 
        RaycastHit hit; 
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>(); 

            if (target != null)
            {
                target.TakeDamage(damage); 
            }
        }
    }
}
