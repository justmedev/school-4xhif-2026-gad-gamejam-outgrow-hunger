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
        private bool _isInputAllowed = true;

        private static float _nightSceneTimeProgress = 0.0f;

        private void Update()
        {
            //_controls.NightBackground.style.translate =
            //     new Vector2(Mathf.Lerp(-100, 100, _nightSceneTimeProgress), 0);
            // _controls.RoomImage.style.translate =
            //     new Vector2(0, (Mathf.Lerp(-75, 75, _nightSceneTimeProgress)));
            _nightSceneTimeProgress += 0.1f * Time.deltaTime;

            //_controls.NightBackground.style.backgroundPositionX =
            //    new StyleBackgroundPosition(
            //        new BackgroundPosition(
            //            BackgroundPositionKeyword.Left,
            //            new Length(
            //                Mathf.Lerp(-100, 100, _nightSceneTimeProgress),
            //                LengthUnit.Percent
            //            )
            //        )
            //    );
            
            _controls.RoomImage.style.backgroundPositionX =
                new StyleBackgroundPosition(
                    new BackgroundPosition(
                        BackgroundPositionKeyword.Left,
                        new Length(
                            Mathf.Lerp(0, 200, _nightSceneTimeProgress),
                            LengthUnit.Percent
                        )
                    )
                );
        }

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
                root.Q<Label>("Day"),
                root.Q<Image>("NightImage"),
                root.Q<Image>("NightBackground")
            );

            _controls.ReturnToGameButton.clicked += ResumeGame;
            _controls.ExitButton.clicked += ExitGame;

            EventBus.Instance.OnNightStarted += () => _isInputAllowed = false;
            EventBus.Instance.OnDayChanged += day =>
            {
                _controls.Day.text = $"{day}";
                SwitchToDayUI();
                _isInputAllowed = true;
            };

            SwitchToDayUI();
            InputSystem.actions.FindAction("Escape").performed += _ =>
            {
                if (!_isInputAllowed) return;
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
            _nightSceneTimeProgress = 0.0f;
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

        public void SetNightImage(Texture2D nightImage, Texture2D nightBackground)
        {
            _controls.NightBackground.style.backgroundImage = nightBackground;
            _controls.RoomImage.style.backgroundImage = nightImage;
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