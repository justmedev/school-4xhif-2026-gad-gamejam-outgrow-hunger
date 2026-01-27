using System.Collections;
using Game;
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
        _gameUIController.NightBegin();

        StartCoroutine(NightScene());
        
        CurrentDay++;
        EventBus.Instance.OnDayChanged?.Invoke(CurrentDay);
    }

    IEnumerator NightScene()
    {
        yield return new WaitForSeconds(1.5f);
        _gameUIController.NightOver();
    }
}