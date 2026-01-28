using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class House : MonoBehaviour
{
    private GameStateManager _gsm;
    private InputAction _interactAction;
    private bool _isInHouseRange;
    private bool _isSleepEnabled = true;

    private void OnEnable()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _gsm = FindFirstObjectByType<GameStateManager>();
        _interactAction.performed += _ =>
        {
            if (!_isInHouseRange) return;
            if (!_isSleepEnabled) return;
            StartCoroutine(_gsm.NextDay());
        };

        EventBus.Instance.OnNightStarted += () => _isSleepEnabled = false;
        EventBus.Instance.OnDayChanged += _ => _isSleepEnabled = true;
    }

    private void OnCollisionEnter2D([NotNull] Collision2D col)
    {
        if (col.gameObject.CompareTag("House")) _isInHouseRange = true;
    }

    private void OnCollisionExit2D([NotNull] Collision2D col)
    {
        if (col.gameObject.CompareTag("House")) _isInHouseRange = false;
    }
}