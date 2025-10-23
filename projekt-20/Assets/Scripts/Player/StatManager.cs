using UnityEngine;

public class StatManager : MonoBehaviour
{
    public Statistic Health = new Statistic("Health", 100f);
    public Statistic Thirst = new Statistic("Thirst", 100f);
    public Statistic Hunger = new Statistic("Hunger", 100f);
    
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
