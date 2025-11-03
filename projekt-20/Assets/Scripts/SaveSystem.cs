using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    
    private string filePath;

    void Start()
    {
       
        filePath = Path.Combine(Application.persistentDataPath, "playerSave.json");
    }

    
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

    
    public void LoadGame(Transform playerTransform)
    {
        if (File.Exists(filePath))
        {
            
            string json = File.ReadAllText(filePath);

            
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            
            playerTransform.position = new Vector3 (data.posX, data.posY, data.posZ);
            Debug.Log("Pozice hr��e byla na�tena.");
        }
        else
        {
            Debug.Log("Ulo�en� hra neexistuje.");
        }
    }
}