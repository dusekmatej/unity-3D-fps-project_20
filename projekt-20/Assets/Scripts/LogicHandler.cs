using System;

namespace DefaultNamespace
{
    public class LogicHandler
    {
        void SaveGame()
        {
            GameManager.Savegame();
        }

        void LoadGame()
        {
            GameManager.Loadgame();
        }
    }
}