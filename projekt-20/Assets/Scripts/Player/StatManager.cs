using System.Collections.Generic;
using GameManagement.SaveSystem;
using UnityEngine;

public class StatManager : MonoBehaviour, ISaveable
{
    public Statistic Health = new Statistic("Health", 100f);
    public Statistic Thirst = new Statistic("Thirst", 100f);
    public Statistic Hunger = new Statistic("Hunger", 100f);

    public IEnumerable<Statistic> AllStats => new
        [] { Health, Thirst, Hunger };
    
    public object CaptureState()
    {
        var statisticState = new PlayerStatsData
        {
            statList = new List<StatisticData>()
        };
        
        foreach (var statistic in AllStats)
        {
            statisticState.statList.Add(new StatisticData
            {
                name = statistic.Name,
                value = statistic.Value,
                maxValue = statistic.MaxValue,
            });
        }
        
        return statisticState;
    }
    
    public void RestoreState(object state)
    {
        // This is called casting, what it does it takes the object 'state' and converts it to type PlayerStatsData
        var savedState = (PlayerStatsData)state;

        foreach (var savedStatistic in savedState.statList)
        {
            var statToLoadInto = FindStatByName(savedStatistic.name);
            if (statToLoadInto == null) continue;
            
            statToLoadInto.Set(savedStatistic.value);
        }
    }

    private Statistic FindStatByName(string statisticName)
    {
        foreach (var stat in AllStats)
        {
            if (stat.Name == statisticName)
            {
                return stat;
            }
        }

        return null;
    }
    
    void Update()
    {
        
        // Decrease hunger over time
        Hunger.Modify(-Time.deltaTime * 1f);
        
        // Decrease thirst over time
        Thirst.Modify(-Time.deltaTime * 0.5f);
        
        // Damage if hunger
        if (Hunger.Value <= 0)
            Health.Modify(-Time.deltaTime * 0.1f);
        
        // Damage if thirst is zero
        if (Thirst.Value <= 0)
            Health.Modify(-Time.deltaTime * 0.2f);
    }
}
