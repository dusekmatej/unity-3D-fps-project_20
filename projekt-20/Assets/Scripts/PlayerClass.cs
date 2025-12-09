using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float posX;
    public float posY;
    public float posZ;

    
    public PlayerData(Vector3 playerPosition)
    {
        posX = playerPosition.x;
        posY = playerPosition.y;
        posZ = playerPosition.z;
    }
}
