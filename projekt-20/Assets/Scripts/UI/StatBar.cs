using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class StatBar : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;

    public void Bind(Statistic statistic)
    {
        _image.fillAmount = statistic.Value / statistic.MaxValue;
        _text.text = $"{statistic.Value}%";

        statistic.OnValueChanged += (newValue) =>
        {
            _image.fillAmount = newValue / statistic.MaxValue;
            _text.text = $"{Math.Floor(statistic.Value)}%";
        };
    }
}
