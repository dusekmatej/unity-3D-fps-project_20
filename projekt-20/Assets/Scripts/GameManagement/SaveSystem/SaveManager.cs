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

        [SerializeField] private SavePlayerTransform _playerTransform;
        [SerializeField] private StatManager _statManager;
        [SerializeField] private bool _loadOnStart = false;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // copy inspector flag into property
            loadOnStart = _loadOnStart;

            // try to resolve missing references at runtime so player transform is captured
            if (_playerTransform == null)
            {
                _playerTransform = FindObjectOfType<SavePlayerTransform>();
                if (_playerTransform != null)
                    Debug.Log("SaveManager: Found SavePlayerTransform via FindObjectOfType.");
                else
                    Debug.LogWarning("SaveManager: _playerTransform is null. Save will not include player transform unless assigned.");

                // as a fallback try to find object tagged "Player"
                if (_playerTransform == null)
                {
                    var playerGO = GameObject.FindWithTag("Player");
                    if (playerGO != null)
                    {
                        _playerTransform = playerGO.GetComponent<SavePlayerTransform>();
                        if (_playerTransform != null)
                            Debug.Log("SaveManager: Found SavePlayerTransform on GameObject tagged Player.");
                    }
                }
            }

            if (_statManager == null)
            {
                _statManager = FindObjectOfType<StatManager>();
                if (_statManager != null)
                    Debug.Log("SaveManager: Found StatManager via FindObjectOfType.");
                else
                    Debug.LogWarning("SaveManager: _statManager is null. Player stats will not be saved unless assigned.");
            }
        }
        
        void Start()
        {
            if (loadOnStart)
            {
                LoadGame();
            }
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveGame();
            }

            if (Input.GetKeyDown(KeyCode.F6))
                LoadGame();
        }
        
       public void SaveGame()
       {
           if (_playerTransform == null && _statManager == null)
           {
               Debug.LogError("SaveManager.SaveGame: No components found to save. Assign SavePlayerTransform or StatManager.");
               return;
           }

           try
           {
               ItemsToSave saveItems = new ItemsToSave();

               if (_playerTransform != null)
               {
                   var pt = _playerTransform.CaptureState();
                   if (pt == null)
                       Debug.LogWarning("SaveManager.SaveGame: _playerTransform.CaptureState() returned null.");
                   else
                       saveItems.playerTransform = (PlayerTransformData)pt;
               }
               else
               {
                   Debug.LogWarning("SaveManager.SaveGame: _playerTransform is null; player transform will not be saved.");
               }

               if (_statManager != null)
               {
                   var ps = _statManager.CaptureState();
                   if (ps == null)
                       Debug.LogWarning("SaveManager.SaveGame: _statManager.CaptureState() returned null.");
                   else
                       saveItems.playerStats = (PlayerStatsData)ps;
               }
               else
               {
                   Debug.LogWarning("SaveManager.SaveGame: _statManager is null; player stats will not be saved.");
               }

               var jsonSave = JsonUtility.ToJson(saveItems, true);
               File.WriteAllText(_savePath, jsonSave);
               Debug.Log($"SaveManager: Saved game to {_savePath}\nSaved content:\n{jsonSave}");
           }
           catch (System.Exception ex)
           {
               Debug.LogError($"SaveManager.SaveGame: Exception while saving: {ex}");
           }
       }
       
       public void LoadGame()
       {
           if (File.Exists(_savePath))
           {
               var jsonSave = File.ReadAllText(_savePath);
               ItemsToSave saveItems = JsonUtility.FromJson<ItemsToSave>(jsonSave);

               if (saveItems == null)
               {
                   Debug.LogError("SaveManager.LoadGame: Deserialized save is null");
                   return;
               }

               if (saveItems.playerTransform != null)
               {
                   if (_playerTransform != null)
                       _playerTransform.RestoreState(saveItems.playerTransform);
                   else
                       Debug.LogWarning("SaveManager.LoadGame: playerTransform data present but _playerTransform reference is null.");
               }

               if (saveItems.playerStats != null)
               {
                   if (_statManager != null)
                       _statManager.RestoreState(saveItems.playerStats);
                   else
                       Debug.LogWarning("SaveManager.LoadGame: playerStats data present but _statManager reference is null.");
               }

               Debug.Log("SaveManager: Load completed.");
           }
           else
           {
               Debug.LogWarning($"SaveManager.LoadGame: No save file at {_savePath}");
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