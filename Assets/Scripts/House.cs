using UnityEngine;
using UnityEngine.InputSystem;

public class House : MonoBehaviour
{
    private InputAction _interactAction;
    private GameStateManager _gsm;
    private bool _isInHouseRange;

    private void OnEnable()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _gsm = FindFirstObjectByType<GameStateManager>();
        _interactAction.performed += _ =>
        {
            if (!_isInHouseRange) return;
            _gsm.NextDay();
        };
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("House"))
        {
            _isInHouseRange = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("House"))
        {
            _isInHouseRange = false;
        }
    }
}