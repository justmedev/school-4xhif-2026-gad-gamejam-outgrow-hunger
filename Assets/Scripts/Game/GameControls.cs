using UnityEngine.UIElements;

namespace Game
{
    public record GameControls(
        VisualElement NightVe,
        VisualElement DayVe,
        VisualElement PauseVe,
        Button ReturnToGameButton,
        Button ExitButton);
}