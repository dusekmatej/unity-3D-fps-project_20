using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSystem : MonoBehaviour
{
    public Transform playerTransform;

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("HasSave") && PlayerPrefs.GetInt("HasSave") == 1)
        {
            // Naètení pozice hráèe
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");

            if (playerTransform != null)
            {
                playerTransform.position = new Vector3(x, y, z);
                Debug.Log("Hra byla úspìšnì naètena.");
            }
            else
            {
                Debug.LogError("Transform hráèe není nastaven! Nelze naèíst pozici.");
            }
        }
        else
        {
            Debug.LogError("Žádná uložená hra nebyla nalezena.");
        }
    }
}