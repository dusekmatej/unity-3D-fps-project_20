using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.2f; // Time between shots

    public Camera fpsCam;
    public ParticleSystem muzzleFlash1;
    public ParticleSystem muzzleFlash2;
    public GameObject impactEffect;

    private bool canShoot = false;
    private float nextFireTime = 0f;

    void Awake()
    {
        canShoot = true; // This ensures shooting stays enabled after the initial delay
    }

    void Update()
    {
        if (canShoot && Time.time >= nextFireTime && Input.GetButtonDown("Fire1"))
        {
            nextFireTime = Time.time + fireRate; // Enforces cooldown
            Shoot();
        }
    }

    void Shoot()
    {
        Debug.Log("Shoot pressed");

        if (muzzleFlash1 != null) muzzleFlash1.Play();
        if (muzzleFlash2 != null) muzzleFlash2.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }
    }
}
