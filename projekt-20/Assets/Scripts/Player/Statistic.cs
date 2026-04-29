using System;
using UnityEngine;


[Serializable]
public class Statistic
{
    [SerializeField] private string statisticName;
    [SerializeField] private float maxValue;

    private float currentValue;
    public event Action<float> OnValueChanged;

    public string Name => statisticName;
    public float Value => currentValue;
    public float MaxValue => maxValue;

    public Statistic(string name, float maxValue)
    {
        this.statisticName = name;
        Debug.Log($"Statistic name: {statisticName}");
        this.maxValue = maxValue;
        this.currentValue = maxValue;
    }

    public void Initialize()
    {
        currentValue = maxValue;
    }
    
    public void Modify(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);
        Debug.Log($"[Statistic] {statisticName} modified by {amount} → {currentValue}/{maxValue} | Listeners: {OnValueChanged?.GetInvocationList().Length ?? 0}");
        OnValueChanged?.Invoke(currentValue);
    }

    public void Set(float newValue)
    {
        currentValue = Mathf.Clamp(newValue, 0, maxValue);
        OnValueChanged?.Invoke(currentValue);
    }
}
