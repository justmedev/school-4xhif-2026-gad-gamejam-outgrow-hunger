using UnityEngine.UIElements;

namespace MainMenu
{
    public record MainMenuControls(
        Button StartButton,
        Button SettingsButton,
        Button ExitButton,
        Label MainMenuLabel,
        VisualElement MainMenuVe,
        VisualElement SettingsVe);
}