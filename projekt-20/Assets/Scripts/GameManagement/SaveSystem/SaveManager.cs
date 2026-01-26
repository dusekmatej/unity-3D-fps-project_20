using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;
        
        public bool loadOnStart { get; private set; }
        
        private string _savePath => System.IO.Path.Combine(
            Application.persistentDataPath, "save.json");

        private SavePlayerTransform _playerTransform;
        private StatManager _statManager;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveGame();
            }
        }
        
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

       public bool HasSave()
       {
           if (File.Exists(_savePath))
           {
               return true;
           }

           return false;
       }
    }
}