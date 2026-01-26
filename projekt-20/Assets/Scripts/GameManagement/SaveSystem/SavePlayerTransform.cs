using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement.SaveSystem
{
    public class SavePlayerTransform : MonoBehaviour, ISaveable
    {
        public object CaptureState()
        {
            return new PlayerTransformData
            {
                levelSceneName = SceneManager.GetActiveScene().name,
                position = transform.position,
                rotation = transform.rotation,
            };
        }

        public void RestoreState(object state)
        {
            var savedState = (PlayerTransformData)state;
            
            transform.position = savedState.position;
            transform.rotation = savedState.rotation;
        }
    }
}