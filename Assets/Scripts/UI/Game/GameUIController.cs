using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI.Game
{
    [RequireComponent(typeof(UIDocument))]
    public class GameUIController : MonoBehaviour
    {
        private UIDocument _ui;
        private GameControls _controls;

        private void Awake()
        {
            _ui = GetComponent<UIDocument>();
            var root = _ui.rootVisualElement;
            _controls = new GameControls(
                root.Q<VisualElement>("NightVE"),
                root.Q<VisualElement>("DayVE"),
                root.Q<VisualElement>("PauseVE"),
                root.Q<Button>("ReturnToGameButton"),
                root.Q<Button>("ExitGameButton"),
                root.Q<Label>("Saturation"),
                root.Q<Label>("Health"),
                root.Q<Label>("Day")
            );

            _controls.ReturnToGameButton.clicked += ResumeGame;
            _controls.ExitButton.clicked += ExitGame;
            
            EventBus.Instance.OnDayChanged += day =>
            {
                _controls.Day.text =  $"{day}";
                SwitchToDayUI();
            }; 

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

        private void SwitchToDayUI()
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

        private void ResumeGame()
        {
            Time.timeScale = 1;
            SwitchToDayUI();
        }

        private void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }

        public void UpdateSaturationLevel(int currentSaturationLevel, int maxSaturationLevel)
        {
            _controls.Saturation.text = $"{currentSaturationLevel}/{maxSaturationLevel}";
        }

        public void UpdateHealthLevel(int currentHealthLevel, int maxHealthLevel)
        {
            _controls.Health.text = $"{currentHealthLevel}/{maxHealthLevel}";
        }
    }
}