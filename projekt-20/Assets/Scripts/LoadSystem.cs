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
            // Na�ten� pozice hr��e
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");

            if (playerTransform != null)
            {
                playerTransform.position = new Vector3(x, y, z);
                Debug.Log("Hra byla �sp�n� na�tena.");
            }
            else
            {
                Debug.LogError("Transform hr��e nen� nastaven! Nelze na��st pozici.");
            }
        }
        else
        {
            Debug.LogError("��dn� ulo�en� hra nebyla nalezena.");
        }
    }
}