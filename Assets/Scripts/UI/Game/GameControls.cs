using UnityEngine.UIElements;

namespace UI.Game
{
    public record GameControls(
        VisualElement NightVe,
        VisualElement DayVe,
        VisualElement PauseVe,
        Button ReturnToGameButton,
        Button ExitButton);
}