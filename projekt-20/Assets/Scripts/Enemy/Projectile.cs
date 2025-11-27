using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

using UnityEngine;


namespace Enemy
{
    public class Projectile : MonoBehaviour
    {
        public float damage = -10f;

        void Awake()
        {
            if (TryGetComponent<Rigidbody>(out var rb))
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        void OnCollisionEnter(Collision collision)
        {
            HandleHit(collision.gameObject);
        }

        void HandleHit(GameObject hit)
        {
            
            if (hit.CompareTag("Player"))
            {
                StatManager statManager = FindObjectOfType<StatManager>();
                if (statManager != null)
                {
                    statManager.Health.Modify(damage);
                }
            }
            
            Destroy(gameObject , 5f);
        }
    }
}