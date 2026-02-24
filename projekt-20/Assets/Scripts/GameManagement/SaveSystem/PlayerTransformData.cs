using System;
using UnityEngine;

namespace GameManagement.SaveSystem
{
    [Serializable]
    public class PlayerTransformData
    {
        public string levelSceneName;
        public Vector3 position;
        public Quaternion rotation;
        
        public PlayerTransformData() { }

        public PlayerTransformData(string level, Vector3 pos, Quaternion rot)
        {
            levelSceneName = level;
            position = pos;
            rotation = rot;
        }
    }
}