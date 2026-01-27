using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace MainMenu
{
    public class MainMenuUIController : MonoBehaviour
    {
        private UIDocument _uiDoc;
        private MainMenuControls _controls;

        private void Awake()
        {
            _uiDoc = GetComponent<UIDocument>();
            var root = _uiDoc.rootVisualElement;
            _controls = new MainMenuControls(
                root.Q<Button>("StartButton"),
                root.Q<Button>("SettingsButton"),
                root.Q<Button>("ExitButton"),
                root.Q<Label>("MainMenuText"),
                root.Q<VisualElement>("MainMenuVE"),
                root.Q<VisualElement>("SettingsVE")
            );

            _controls.StartButton.clicked += OnStartButtonClicked;
            _controls.SettingsButton.clicked += OnSettingsButtonClicked;
            _controls.ExitButton.clicked += OnExitButtonClicked;
        }

        private void OnStartButtonClicked()
        {
            _uiDoc.rootVisualElement.SetEnabled(false);
            SceneManager.LoadScene("SampleScene");
        }

        private void OnSettingsButtonClicked()
        {
            Debug.Log("Settings Button Pressed!");
            // TODO: Settings
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}