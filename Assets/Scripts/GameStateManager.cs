using System;
using System.Collections;
using UI.Game;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int RequiredSaturationLevel = 10;
    public const int MaxHealthLevel = 10;
    public int CurrentSaturationLevel { get; private set; }
    public int CurrentHealthLevel { get; private set; } = MaxHealthLevel;
    public int CurrentDay { get; private set; } = 1;
    private GameUIController _gui;

    private void Start()
    {
        _gui = FindFirstObjectByType<GameUIController>();
        _gui.UpdateHealthLevel(CurrentHealthLevel, MaxHealthLevel);
        _gui.UpdateSaturationLevel(CurrentSaturationLevel, RequiredSaturationLevel);

        EventBus.Instance.OnDayChanged += ConsumeAndIncreaseSaturation;
    }

    public void AddSaturationLevel(int level)
    {
        CurrentSaturationLevel += level;
        _gui.UpdateSaturationLevel(CurrentSaturationLevel, RequiredSaturationLevel);
    }

    private void ConsumeAndIncreaseSaturation(int day)
    {
        var diff = CurrentSaturationLevel - RequiredSaturationLevel;
        CurrentSaturationLevel = Math.Max(0, diff);
        CurrentHealthLevel = diff >= 0
            ? Math.Clamp(CurrentHealthLevel + 1, 0, MaxHealthLevel)
            : Math.Clamp(CurrentHealthLevel - 1, 0, MaxHealthLevel);
        _gui.UpdateSaturationLevel(CurrentSaturationLevel, RequiredSaturationLevel);
        _gui.UpdateHealthLevel(CurrentHealthLevel, MaxHealthLevel);

        if (CurrentHealthLevel <= 0)
        {
            // TODO: Loose Game
        }

        if (day % 2 == 0)
        {
            RequiredSaturationLevel += 2;
        }
    }

    public void NextDay()
    {
        _gui.SwitchToNightUI();

        StartCoroutine(WaitForNight());

        CurrentDay++;
        EventBus.Instance.OnDayChanged?.Invoke(CurrentDay);
    }

    private static IEnumerator WaitForNight()
    {
        yield return new WaitForSecondsRealtime(3);
    }
}