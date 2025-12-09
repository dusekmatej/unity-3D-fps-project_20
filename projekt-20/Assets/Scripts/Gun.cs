using System;
using Enemy;
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
    public LayerMask excludeLayers;

    void Awake()
    {
        canShoot = true;
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
        EnemyAI enemyAi = new EnemyAI();
        
        Debug.Log("Shoot pressed");

        if (muzzleFlash1 != null) muzzleFlash1.Play();
        if (muzzleFlash2 != null) muzzleFlash2.Play();

        RaycastHit hit;
        Vector3 origin = fpsCam.transform.position;
        Vector3 direction = fpsCam.transform.forward;
        
        int layerMask = ~excludeLayers;

        // Debug.DrawRay(origin, direction * range, Color.red, 10f);

        if (Physics.Raycast(origin, direction, out hit, range, layerMask))
        {
            // Debug.DrawLine(origin, hit.point, Color.yellow, 1f);

            Debug.Log($"Ray hit: {hit.transform.name} (layer: {LayerMask.LayerToName(hit.transform.gameObject.layer)}), collider: {hit.collider}");
            
            if (hit.transform.CompareTag("Enemy"))
            {
                var enemy = hit.transform.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log($"Dealt {damage} damage to {hit.transform.name}");
                }
            }

        }
    }
}
