using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.MainMenu
{
    public class MainMenuCameraMover : MonoBehaviour
    {
        private Vector3 _startPosition;

        [SerializeField] private float strength;
        [SerializeField] private float smoothness;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            var mousePos = Mouse.current.position.ReadValue();

            var screenCenter = new Vector2(
                Screen.width / 2f,
                Screen.height / 2f
            );

            var offset = mousePos - screenCenter;

            var targetPosition = _startPosition + new Vector3(
                offset.x * strength,
                offset.y * strength,
                0
            );

            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                Time.deltaTime * smoothness
            );
        }
    }
}