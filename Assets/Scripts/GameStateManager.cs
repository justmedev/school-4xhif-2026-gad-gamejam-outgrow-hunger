using System;
using System.Collections;
using UI.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public const float NightSceneDurationSeconds = 1.5f;
    [SerializeField] private GameObject audioManagerPrefab;
    [SerializeField] private int maxHealthLevel = 8;
    [SerializeField] public int requiredSaturationLevel = 4;
    private GlobalGameState _ggs;
    private GameUIController _gui;
    public int CurrentSaturationLevel { get; private set; }
    private int CurrentHealthLevel { get; set; }
    private int CurrentDay { get; set; } = 1;

    private void Awake()
    {
        var audioMan = FindFirstObjectByType<AudioManager>();
        if (audioMan != null) return;
        Debug.Log("Adding audio manager ourselves, because it was not found. This may produce warnings.");
        var go = Instantiate(audioManagerPrefab, transform.root);
        go.GetComponent<AudioManager>().LoadAllAudioClips();
    }

    private void Start()
    {
        CurrentHealthLevel = maxHealthLevel;
        _ggs = FindFirstObjectByType<GlobalGameState>();
        _gui = FindFirstObjectByType<GameUIController>();
        _gui.UpdateHealthLevel(CurrentHealthLevel, maxHealthLevel);
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
        CurrentSaturationLevel = 0;
        CurrentHealthLevel = diff >= 0
            ? Math.Clamp(CurrentHealthLevel + 1, 0, maxHealthLevel)
            : Math.Clamp(CurrentHealthLevel - 1, 0, maxHealthLevel);
        _gui.UpdateSaturationLevel(0, requiredSaturationLevel);
        _gui.UpdateHealthLevel(CurrentHealthLevel, maxHealthLevel);

        if (day % 2 == 0) requiredSaturationLevel += 2;
    }

    public IEnumerator NextDay()
    {
        EventBus.Instance.OnNightStarted?.Invoke();
        yield return new WaitForSecondsRealtime(NightSceneDurationSeconds);
        CurrentDay++;
        EventBus.Instance.OnDayChanged?.Invoke(CurrentDay);
        if (CurrentHealthLevel <= 0)
        {
            _ggs.FinalDay = CurrentDay - 1;
            SceneManager.LoadScene("Lose");
        }
    }
}