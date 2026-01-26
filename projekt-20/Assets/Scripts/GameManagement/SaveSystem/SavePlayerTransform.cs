using System.Numerics;
using UnityEngine;

namespace GameManagement.SaveSystem
{
    public class SavePlayerTransform : MonoBehaviour, ISaveable
    {
        public object CaptureState()
        {
            return new PlayerTransformData
            {
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