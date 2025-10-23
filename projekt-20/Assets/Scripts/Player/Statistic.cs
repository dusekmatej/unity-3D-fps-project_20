using System;
using UnityEngine;


[Serializable]
public class Statistic
{
    [SerializeField] private string statisticName;
    [SerializeField] private float currentValue;
    [SerializeField] private float maxValue;

    public event Action<float> OnValueChanged;

    public string Name => statisticName;
    public float Value => currentValue;
    public float MaxValue => maxValue;

    public Statistic(string name, float maxValue)
    {
        this.statisticName = name;
        this.maxValue = maxValue;
        this.currentValue = maxValue;
    }

    public void Modify(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);
        OnValueChanged?.Invoke(currentValue);
    }

    public void Set(float newValue)
    {
        currentValue = Mathf.Clamp(newValue, 0, maxValue);
        OnValueChanged?.Invoke(currentValue);
    }
}
