using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static string savePath = Application.persistentDataPath + "/SaveGame.json";
    public Vector3 PlayerData()
    {
<<<<<<< HEAD
        
<<<<<<< HEAD
=======
        LoadSystem loadSystem = FindObjectOfType<LoadSystem>();
        loadSystem.LoadGame();
>>>>>>> EnemySpawnerAndBuffs
=======
        return transform.position;
>>>>>>> Kopecky/MenuUtils
    }

    public static void Savegame()
    {
        CheckExistence();
        
        
        string json = JsonUtility.ToJson(data,true);
        File.WriteAllText(savePath, json);
        
    }

    public static void Loadgame()
    {
        CheckExistence();
        
        string json = File.ReadAllText(savePath);
        
    }

    public static void CheckExistence()
    {
        if (!File.Exists(savePath))
        {
            File.WriteAllLines(savePath, new string[] { });
        }
    }
    
    
    
    
}

