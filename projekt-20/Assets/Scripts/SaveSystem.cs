using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    // Cesta k souboru, kde budou ulo�ena data
    private string filePath;

    void Start()
    {
        // Ur�ujeme cestu pro ulo�en� souboru (v adres��i Application.persistentDataPath)
        filePath = Path.Combine(Application.persistentDataPath, "playerSave.json");
    }

    // Ulo�en� pozice hr��e do JSON souboru
    public void SaveGame(Transform playerTransform)
    {
        if (playerTransform != null)
        {
            PlayerData data = new PlayerData(playerTransform.position);
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
            Debug.Log("Hra byla ulozena");

        }
        else 
        {
            Debug.Log("Transform hrace neni definovan.");
        }
    }

    // Na��t�n� pozice hr��e z JSON souboru
    public void LoadGame(Transform playerTransform)
    {
        if (File.Exists(filePath))
        {
            // Na��t�n� JSON dat ze souboru
            string json = File.ReadAllText(filePath);

            // Deserializace JSON do PlayerData
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            // Obnoven� pozice hr��e
            playerTransform.position = new Vector3 (data.posX, data.posY, data.posZ);
            Debug.Log("Pozice hr��e byla na�tena.");
        }
        else
        {
            Debug.Log("Ulo�en� hra neexistuje.");
        }
    }
}