using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace UI.Lose
{
    public class LoseUIController : MonoBehaviour
    {
        private LoseControls _controls;
        private UIDocument _uiDoc;
        private GlobalGameState _ggs;

        private void Awake()
        {
            _ggs = FindFirstObjectByType<GlobalGameState>();
            _uiDoc = GetComponent<UIDocument>();
            var root = _uiDoc.rootVisualElement;
            _controls = new LoseControls(
                root.Q<Button>("ExitButton"),
                root.Q<Button>("CreditsButton"),
                root.Q<VisualElement>("LoseVE"),
                root.Q<VisualElement>("CreditsVE"),
                root.Q<Button>("ReturnButton"),
                root.Q<Label>("ScoreLabel")
            );

            _controls.ScoreLabel.text =
                $"Tragically, your children starved after {_ggs.FinalDay} days. As a result, you hung yourself.";
            _controls.ExitButton.clicked += OnExitButtonClicked;
            _controls.CreditsButton.clicked += OnCreditsButtonClicked;
            _controls.ReturnButton.clicked += OnReturnButtonClicked;
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }

        private void OnCreditsButtonClicked()
        {
            _controls.LoseVe.style.display = DisplayStyle.None;
            _controls.CreditsVe.style.display = DisplayStyle.Flex;
        }

        private void OnReturnButtonClicked()
        {
            _controls.LoseVe.style.display = DisplayStyle.Flex;
            _controls.CreditsVe.style.display = DisplayStyle.None;
        }
    }
}