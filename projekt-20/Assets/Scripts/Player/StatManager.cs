using System;
using GameManagement.SaveSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class StatManager : MonoBehaviour, ISaveable
{
    [Header("Stats")]
    public Statistic Health = new Statistic("Health", 100f);
    public Statistic Thirst = new Statistic("Thirst", 100f);
    public Statistic Hunger = new Statistic("Hunger", 100f);

    public IEnumerable<Statistic> AllStats => new[] { Health, Thirst, Hunger };

    [Header("UI Effects")]
    public Image damageOverlay;

    [Header("Post Processing")]
    public PostProcessVolume volume;

    [Header("Settings")]
    [Range(0f, 1f)] public float lowStatThreshold = 0.3f;
    public float maxRedAlpha = 0.4f;
    public float maxVignetteIntensity = 0.45f;

    private Vignette vignette;

    // ==============================
    // UNITY
    // ==============================

    void Start()
    {
        foreach (var stat in AllStats)
            stat.Initialize();
        
        if (volume != null && volume.profile.TryGetSettings(out vignette))
        {
            vignette.intensity.Override(0f);
        }
        else
        {
            Debug.LogWarning("Vignette not found in Volume!");
        }
    }

    void Update()
    {
        UpdateStats();
        UpdateLowHealthEffect();
        UpdateVignetteEffect();
    }

    // ==============================
    // STAT LOGIKA (p�vodn�)
    // ==============================

    void UpdateStats()
    {
        Hunger.Modify(-Time.deltaTime * 1f);
        Thirst.Modify(-Time.deltaTime * 0.5f);

        if (Hunger.Value <= 0)
        {
            Health.Modify(-Time.deltaTime * 0.1f);
            Debug.Log("Health modify for hunger" + Health.Value);
        }

        if (Thirst.Value <= 0)
        {
            Health.Modify(-Time.deltaTime * 0.2f);
            Debug.Log("Health modify for hunger" + Health.Value);
        }
    }

    // ==============================
    // LOW HP EFFECT
    // ==============================

    void UpdateLowHealthEffect()
    {
        if (damageOverlay == null) return;

        float healthPercent = Health.Value / Health.MaxValue;
        Color c = damageOverlay.color;

        if (healthPercent <= lowStatThreshold)
        {
            float t = 1f - (healthPercent / lowStatThreshold);
            float targetAlpha = Mathf.Lerp(0f, maxRedAlpha, t);
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * 5f);
        }
        else
        {
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * 5f);
        }

        damageOverlay.color = c;
    }

    // ==============================
    // VIGNETTE EFFECT (Hlad / ��ze�)
    // ==============================

    void UpdateVignetteEffect()
    {
        if (vignette == null) return;

        float hungerPercent = Hunger.Value / Hunger.MaxValue;
        float thirstPercent = Thirst.Value / Thirst.MaxValue;

        float lowest = Mathf.Min(hungerPercent, thirstPercent);

        float targetIntensity = 0f;

        if (lowest <= lowStatThreshold)
        {
            float t = 1f - (lowest / lowStatThreshold);
            targetIntensity = Mathf.Lerp(0f, maxVignetteIntensity, t);
        }

        vignette.intensity.Override(
            Mathf.Lerp(vignette.intensity.value, targetIntensity, Time.deltaTime * 2f)
        );
    }

    // ==============================
    // SAVE SYSTEM
    // ==============================

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
                return stat;
        }

        return null;
    }
}
