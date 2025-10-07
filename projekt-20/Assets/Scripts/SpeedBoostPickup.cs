using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    public float boostAmount = 5.0f;
    public float boostDuration = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.BoostSpeed(boostAmount, boostDuration);
            Destroy(gameObject);
        }
    }
}
