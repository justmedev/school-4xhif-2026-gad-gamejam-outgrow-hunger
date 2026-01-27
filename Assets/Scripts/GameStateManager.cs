using System.Collections;
using UI.Game;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int CurrentDay { get; private set; }
    private GameUIController _gameUIController;

    void Start()
    {
        _gameUIController = FindFirstObjectByType<GameUIController>();
    }
    
    public void NextDay()
    {
        _gameUIController.SwitchToNightUI();

        StartCoroutine(NightScene());
        
        CurrentDay++;
        EventBus.Instance.OnDayChanged?.Invoke(CurrentDay);
    }

    IEnumerator NightScene()
    {
        yield return new WaitForSeconds(1.5f);
        _gameUIController.SwitchToDayUI();
    }
}