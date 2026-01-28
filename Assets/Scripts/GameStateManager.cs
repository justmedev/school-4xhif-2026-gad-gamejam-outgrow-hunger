using System.Collections;
using UI.Game;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int CurrentDay { get; private set; }
    private GameUIController _gameUIController;
    private PlayerMovement _playerMovement;
    private House _house;

    void Start()
    {
        _gameUIController = FindFirstObjectByType<GameUIController>();
        _playerMovement = FindFirstObjectByType<PlayerMovement>();
        _house = FindFirstObjectByType<House>();
    }
    
    public void NextDay()
    {
        _gameUIController.SwitchToNightUI();
        _playerMovement.canMove = false;
        _house.canSleep = false;
        
        StartCoroutine(NightScene());
        
        CurrentDay++;
        EventBus.Instance.OnDayChanged?.Invoke(CurrentDay);
    }

    IEnumerator NightScene()
    {
        yield return new WaitForSeconds(1.5f);
        _gameUIController.SwitchToDayUI();
        _playerMovement.canMove = true;
        _house.canSleep = true;
    }
}