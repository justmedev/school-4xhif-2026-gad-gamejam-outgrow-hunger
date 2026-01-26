using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement; // <- needed for changing scenes

public class MainMenuController : MonoBehaviour
{
    private UIDocument _uiDoc;
    private Button _StartButton, _SettingsButton, _ExitButton;
    private Label _MainMenuText;
    private VisualElement _MainMenuVE, _SettingsVE;

    void Awake()
    {
        _uiDoc = GetComponent<UIDocument>();
        _StartButton = _uiDoc.rootVisualElement.Q<Button>("StartButton");
        _SettingsButton = _uiDoc.rootVisualElement.Q<Button>("SettingsButton");
        _ExitButton = _uiDoc.rootVisualElement.Q<Button>("ExitButton");
        _MainMenuText = _uiDoc.rootVisualElement.Q<Label>("MainMenuText");
        _MainMenuVE = _uiDoc.rootVisualElement.Q<VisualElement>("MainMenuVE");
        _SettingsVE = _uiDoc.rootVisualElement.Q<VisualElement>("SettingsVE");

        _StartButton.clicked += OnStartButtonClicked;
        _SettingsButton.clicked += OnSettingsButtonClicked;
        _ExitButton.clicked += OnExitButtonClicked;
    }

    private void OnStartButtonClicked()
    {
        _uiDoc.rootVisualElement.SetEnabled(false);
        SceneManager.LoadScene("SampleScene");
    }

    private void OnSettingsButtonClicked()
    {
        Debug.Log("Settings Button Pressed!");
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("Quit Game");
    }
}