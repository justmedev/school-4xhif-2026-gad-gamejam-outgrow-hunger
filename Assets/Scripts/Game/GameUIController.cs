using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameUIController : MonoBehaviour
    {
        private UIDocument _uiDoc;
        private GameControls _controls;
        private InputAction _escapeAction;

        private void Awake()
        {
            _uiDoc = GetComponent<UIDocument>();
            var root = _uiDoc.rootVisualElement;
            _escapeAction = InputSystem.actions.FindAction("Escape");
            _controls = new GameControls(
                root.Q<VisualElement>("NightVE"),
                root.Q<VisualElement>("DayVE"),
                root.Q<VisualElement>("PauseVE"),
                root.Q<Button>("ReturnToGameButton"),
                root.Q<Button>("ExitGameButton")
            );

            _controls.ReturnToGameButton.clicked += ReturnToGame;
            _controls.ExitButton.clicked += ExitGame;
        }

        private void Update()
        {
            if (_escapeAction.WasPressedThisFrame())
            {
                if (_controls.PauseVe.style.display == DisplayStyle.None)
                {
                    Time.timeScale = 0;
                    _controls.DayVe.style.display = DisplayStyle.None;
                    _controls.NightVe.style.display = DisplayStyle.None;
                    _controls.PauseVe.style.display = DisplayStyle.Flex;
                }
                else
                {
                    Time.timeScale = 1;
                    _controls.NightVe.style.display  = DisplayStyle.None;
                    _controls.PauseVe.style.display = DisplayStyle.None;
                    _controls.DayVe.style.display = DisplayStyle.Flex;
                }
            }
        }

        public void NightOver()
        {
            _controls.DayVe.style.display = DisplayStyle.Flex;
            _controls.PauseVe.style.display = DisplayStyle.None;
            _controls.NightVe.style.display  = DisplayStyle.None;
        }
        
        public void NightBegin()
        {
            _controls.NightVe.style.display  = DisplayStyle.Flex;
            _controls.PauseVe.style.display = DisplayStyle.None;
            _controls.DayVe.style.display = DisplayStyle.None;
        }

        public void ReturnToGame()
        {
            Time.timeScale = 1;
            _controls.NightVe.style.display  = DisplayStyle.None;
            _controls.PauseVe.style.display = DisplayStyle.None;
            _controls.DayVe.style.display = DisplayStyle.Flex;
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