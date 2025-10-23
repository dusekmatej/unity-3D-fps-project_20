using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiStatisticHandling : MonoBehaviour
{
    [SerializeField] private StatManager playerStats;
    
    [SerializeField] private StatBar healthBar;
    [SerializeField] private StatBar hungerBar;
    [SerializeField] private StatBar thirstBar;

    void Start()
    {
        healthBar.Bind(playerStats.Health);
        hungerBar.Bind(playerStats.Hunger);
        thirstBar.Bind(playerStats.Thirst);
    }
}
