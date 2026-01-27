using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI.Game
{
    [RequireComponent(typeof(UIDocument))]
    public class GameUIController : MonoBehaviour
    {
        private UIDocument _uiDoc;
        private GameControls _controls;

        private void Awake()
        {
            _uiDoc = GetComponent<UIDocument>();
            var root = _uiDoc.rootVisualElement;
            _controls = new GameControls(
                root.Q<VisualElement>("NightVE"),
                root.Q<VisualElement>("DayVE"),
                root.Q<VisualElement>("PauseVE"),
                root.Q<Button>("ReturnToGameButton"),
                root.Q<Button>("ExitGameButton")
            );

            _controls.ReturnToGameButton.clicked += ResumeGame;
            _controls.ExitButton.clicked += ExitGame;

            SwitchToDayUI();
            InputSystem.actions.FindAction("Escape").performed += _ =>
            {
                if (_controls.PauseVe.style.display == DisplayStyle.None)
                {
                    Time.timeScale = 0;
                    SwitchToPauseUI();
                    return;
                }

                ResumeGame();
            };
        }

        public void SwitchToDayUI()
        {
            _controls.DayVe.style.display = DisplayStyle.Flex;
            _controls.PauseVe.style.display = DisplayStyle.None;
            _controls.NightVe.style.display = DisplayStyle.None;
        }

        public void SwitchToNightUI()
        {
            _controls.NightVe.style.display = DisplayStyle.Flex;
            _controls.PauseVe.style.display = DisplayStyle.None;
            _controls.DayVe.style.display = DisplayStyle.None;
        }

        public void SwitchToPauseUI()
        {
            _controls.PauseVe.style.display = DisplayStyle.Flex;
            _controls.DayVe.style.display = DisplayStyle.None;
            _controls.NightVe.style.display = DisplayStyle.None;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            SwitchToDayUI();
        }

        public void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}