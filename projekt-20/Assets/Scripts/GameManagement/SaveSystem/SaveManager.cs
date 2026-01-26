using System.IO;
using UnityEngine;

namespace GameManagement.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        private string _savePath = Path.Combine(Application.persistentDataPath, "save");

        private SavePlayerTransform _playerTransform;
        private StatManager _statManager;
        
       public void SaveGame()
       {
           ItemsToSave saveItems = new ItemsToSave
           {
               playerTransform = (PlayerTransformData)_playerTransform.CaptureState(),
               playerStats = (PlayerStatsData)_statManager.CaptureState(),
           };

           var jsonSave = JsonUtility.ToJson(saveItems, true);
            File.WriteAllText(_savePath, jsonSave);
       }
       
       public void LoadGame()
       {
           if (File.Exists(_savePath))
           {
               var jsonSave = File.ReadAllText(_savePath);
               ItemsToSave saveItems = JsonUtility.FromJson<ItemsToSave>(jsonSave);

               _playerTransform.RestoreState(saveItems.playerTransform);
               _statManager.RestoreState(saveItems.playerStats);
           }
       }
    }
}