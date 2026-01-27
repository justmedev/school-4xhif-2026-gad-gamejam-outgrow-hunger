using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class GameUIController : MonoBehaviour
    {
        private UIDocument _uiDoc;
        private GameControls _controls;

        private void Awake()
        {
            _uiDoc = GetComponent<UIDocument>();
            var root = _uiDoc.rootVisualElement;
            _controls = new GameControls(
                root.Q<VisualElement>("DayVE"),
                root.Q<VisualElement>("NightVE")
            );
        }
        
        public void NightOver()
        {
            _controls.DayVe.style.display = DisplayStyle.None;
            _controls.NightVe.style.display  = DisplayStyle.Flex;
        }
        
        public void NightBegin()
        {
            _controls.NightVe.style.display  = DisplayStyle.None;
            _controls.DayVe.style.display = DisplayStyle.Flex;
        }
    }
}