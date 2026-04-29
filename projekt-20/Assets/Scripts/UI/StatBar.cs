using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class StatBar : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;
    
// StatBar.cs
    public void Bind(Statistic statistic)
    {
        Debug.Log($"[StatBar] Binding to {statistic.Name}, value={statistic.Value}, max={statistic.MaxValue}");
    
        _image.fillAmount = statistic.Value / statistic.MaxValue;
        _text.text = $"{statistic.Value}%";

        statistic.OnValueChanged += (newValue) =>
        {
            Debug.Log($"[StatBar] OnValueChanged fired for {statistic.Name}, newValue={newValue}");
            _image.fillAmount = newValue / statistic.MaxValue;
            _text.text = $"{Math.Floor(newValue)}%";
        };
    
        Debug.Log($"[StatBar] Subscribed to {statistic.Name} OnValueChanged");
    }
}
