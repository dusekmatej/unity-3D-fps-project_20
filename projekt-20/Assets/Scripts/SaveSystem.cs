using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    // Cesta k souboru, kde budou uložena data
    private string filePath;

    void Start()
    {
        // Urèujeme cestu pro uložení souboru (v adresáøi Application.persistentDataPath)
        filePath = Path.Combine(Application.persistentDataPath, "playerSave.json");
    }

    // Uložení pozice hráèe do JSON souboru
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

    // Naèítání pozice hráèe z JSON souboru
    public void LoadGame(Transform playerTransform)
    {
        if (File.Exists(filePath))
        {
            // Naèítání JSON dat ze souboru
            string json = File.ReadAllText(filePath);

            // Deserializace JSON do PlayerData
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            // Obnovení pozice hráèe
            playerTransform.position = new Vector3 (data.posX, data.posY, data.posZ);
            Debug.Log("Pozice hráèe byla naètena.");
        }
        else
        {
            Debug.Log("Uložená hra neexistuje.");
        }
    }
}