using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoostPickup : MonoBehaviour
{
    public int healAmount = 25;

    public void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            //player.Heal(HealAmount);
            Destroy(gameObject);
        }
        
    }
}
