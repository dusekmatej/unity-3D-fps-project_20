using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // P�i spu�t�n� sc�ny na�ti ulo�enou hru
        LoadSystem loadSystem = FindObjectOfType<LoadSystem>();
        loadSystem.LoadGame();
    }
}

