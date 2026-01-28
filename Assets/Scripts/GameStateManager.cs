using System;
using System.Collections;
using UI.Game;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public const float NightSceneDurationSeconds = 1.5f;
    public int requiredSaturationLevel = 10;
    public const int MaxHealthLevel = 10;
    public int CurrentSaturationLevel { get; private set; }
    public int CurrentHealthLevel { get; private set; } = MaxHealthLevel;
    public int CurrentDay { get; private set; } = 1;
    private GameUIController _gui;

    private void Start()
    {
        _gui = FindFirstObjectByType<GameUIController>();
        _gui.UpdateHealthLevel(CurrentHealthLevel, MaxHealthLevel);
        _gui.UpdateSaturationLevel(CurrentSaturationLevel, requiredSaturationLevel);

        EventBus.Instance.OnDayChanged += ConsumeAndIncreaseSaturation;
    }

    public void AddSaturationLevel(int level)
    {
        CurrentSaturationLevel += level;
        _gui.UpdateSaturationLevel(CurrentSaturationLevel, requiredSaturationLevel);
    }

    private void ConsumeAndIncreaseSaturation(int day)
    {
        var diff = CurrentSaturationLevel - requiredSaturationLevel;
        CurrentSaturationLevel = Math.Max(0, diff);
        CurrentHealthLevel = diff >= 0
            ? Math.Clamp(CurrentHealthLevel + 1, 0, MaxHealthLevel)
            : Math.Clamp(CurrentHealthLevel - 1, 0, MaxHealthLevel);
        _gui.UpdateSaturationLevel(CurrentSaturationLevel, requiredSaturationLevel);
        _gui.UpdateHealthLevel(CurrentHealthLevel, MaxHealthLevel);

        if (CurrentHealthLevel <= 0)
        {
            // TODO: Loose Game
        }

        if (day % 2 == 0)
        {
            requiredSaturationLevel += 2;
        }
    }

    public IEnumerator NextDay()
    {
        EventBus.Instance.OnNightStarted?.Invoke();
        yield return new WaitForSecondsRealtime(NightSceneDurationSeconds);
        CurrentDay++;
        EventBus.Instance.OnDayChanged?.Invoke(CurrentDay);
    }
}