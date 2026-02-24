using System;
using GameManagement.SaveSystem;

namespace GameManagement.SaveSystem
{
    [Serializable]
    public class ItemsToSave
    {
        public PlayerTransformData playerTransform;
        public PlayerStatsData playerStats;
    }
}