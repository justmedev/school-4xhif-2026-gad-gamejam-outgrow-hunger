using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI.Game
{
    [RequireComponent(typeof(UIDocument))]
    [RequireComponent(typeof(AudioSource))]
    public class GameUIController : MonoBehaviour
    {
        private static float _elapsedTimeNightScene;
        [SerializeField] private Texture2D nightImageGood;
        [SerializeField] private Texture2D nightImageMid;
        [SerializeField] private Texture2D nightImageBad;
        [SerializeField] private Texture2D nightImageBg;

        private AudioManager _audioMan;
        private AudioSource _audioSource;
        private GameControls _controls;
        private GameStateManager _gsm;
        private bool _isCurrentlyDay = true;
        private UIDocument _ui;

        private void Update()
        {
            if (_isCurrentlyDay) return;

            _elapsedTimeNightScene += Time.deltaTime;
            var t = Mathf.Clamp01(_elapsedTimeNightScene / GameStateManager.NightSceneDurationSeconds);

            _controls.RoomImage.style.backgroundPositionX =
                new StyleBackgroundPosition(
                    new BackgroundPosition(
                        BackgroundPositionKeyword.Left,
                        new Length(
                            Mathf.Lerp(0, 100, t),
                            LengthUnit.Percent
                        )
                    )
                );
        }

        private void OnEnable()
        {
            _gsm = FindFirstObjectByType<GameStateManager>();
            _ui = GetComponent<UIDocument>();
            _audioMan = FindFirstObjectByType<AudioManager>();
            _audioSource = GetComponent<AudioSource>();

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

            SwitchToDayUI();

            EventBus.Instance.OnNightStarted += () =>
            {
                _isCurrentlyDay = false;
                SwitchToNightUI();
                var diff = _gsm.CurrentSaturationLevel - _gsm.requiredSaturationLevel;
                _audioMan.DuckBackgroundMusic();
                switch (diff)
                {
                    case < 0:
                        SetNightImage(nightImageBad, nightImageBg);
                        _audioSource.PlayOneShot(_audioMan.BadNightClip, .7f);
                        break;
                    case 0:
                        SetNightImage(nightImageMid, nightImageBg);
                        _audioSource.PlayOneShot(_audioMan.MidNightClip, .7f);
                        break;
                    case > 0:
                        SetNightImage(nightImageGood, nightImageBg);
                        _audioSource.PlayOneShot(_audioMan.GoodNightClip, .7f);
                        break;
                }
            };
            EventBus.Instance.OnDayChanged += day =>
            {
                _audioMan.UnDuckBackgroundMusic();
                _controls.Day.text = $"{day}";
                SwitchToDayUI();
                _isCurrentlyDay = true;
            };

            InputSystem.actions.FindAction("Escape").performed += _ =>
            {
                if (!_isCurrentlyDay) return;
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

        private void SwitchToNightUI()
        {
            _controls.NightVe.style.display = DisplayStyle.Flex;
            _controls.PauseVe.style.display = DisplayStyle.None;
            _controls.DayVe.style.display = DisplayStyle.None;
            _elapsedTimeNightScene = 0.0f;
        }

        private void SwitchToPauseUI()
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

        private static void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }

        private void SetNightImage(Texture2D nightImage, Texture2D nightBackground)
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