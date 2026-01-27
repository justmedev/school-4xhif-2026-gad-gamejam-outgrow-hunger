using UnityEngine.UIElements;

namespace UI.MainMenu
{
    public record MainMenuControls(
        Button StartButton,
        Button CreditsButton,
        Button ExitButton,
        Button ReturnButton,
        Label MainMenuLabel,
        VisualElement MainMenuVe,
        VisualElement CreditsVe);
}