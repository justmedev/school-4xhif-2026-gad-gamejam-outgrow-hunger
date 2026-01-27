using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.MainMenu
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
                root.Q<Button>("CreditsButton"),
                root.Q<Button>("ExitButton"),
                root.Q<Button>("ReturnButton"),
                root.Q<Label>("MainMenuText"),
                root.Q<VisualElement>("MainMenuVE"),
                root.Q<VisualElement>("CreditsVE")
            );

            _controls.StartButton.clicked += OnStartButtonClicked;
            _controls.CreditsButton.clicked += OnCreditsButtonClicked;
            _controls.ExitButton.clicked += OnExitButtonClicked;
            _controls.ReturnButton.clicked += OnReturnButtonClicked;
        }

        private void OnStartButtonClicked()
        {
            _uiDoc.rootVisualElement.SetEnabled(false);
            SceneManager.LoadScene("SampleScene");
        }

        private void OnCreditsButtonClicked()
        {
            _controls.MainMenuVe.style.display = DisplayStyle.None;
            _controls.CreditsVe.style.display  = DisplayStyle.Flex;
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
        
        private void OnReturnButtonClicked()
        {
            _controls.MainMenuVe.style.display = DisplayStyle.Flex;
            _controls.CreditsVe.style.display  = DisplayStyle.None;
        }
    }
}