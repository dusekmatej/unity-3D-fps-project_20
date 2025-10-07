using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Pøi spuštìní scény naèti uloženou hru
        LoadSystem loadSystem = FindObjectOfType<LoadSystem>();
        loadSystem.LoadGame();
    }
}

