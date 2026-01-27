using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuMouseScript : MonoBehaviour
{
    private Vector3 _startPosition;

    [SerializeField] private float strength;
    [SerializeField] private float smoothness;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Vector2 screenCenter = new Vector2(
            Screen.width / 2f,
            Screen.height / 2f
        );

        Vector2 offset = mousePos - screenCenter;

        Vector3 targetPosition = _startPosition + new Vector3(
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