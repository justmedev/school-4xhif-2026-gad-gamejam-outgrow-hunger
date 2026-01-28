using UnityEngine.UIElements;

namespace UI.Lose
{
    public record LoseControls(
        Button ExitButton,
        Button CreditsButton,
        VisualElement LoseVe,
        VisualElement CreditsVe,
        Button ReturnButton,
        Label ScoreLabel);
}