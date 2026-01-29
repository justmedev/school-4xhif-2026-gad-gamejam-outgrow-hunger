using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class House : MonoBehaviour
{
    private GameStateManager _gsm;
    private InputAction _interactAction;
    private PlayerInteractionNotifier _interactionNotifier;
    private bool _isInHouseRange;
    private bool _isSleepEnabled = true;

    private void OnEnable()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactionNotifier = FindFirstObjectByType<PlayerInteractionNotifier>();
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
        if (!col.gameObject.CompareTag("House")) return;
        _isInHouseRange = true;
        _interactionNotifier.ShowPressEMessage();
    }

    private void OnCollisionExit2D([NotNull] Collision2D col)
    {
        if (!col.gameObject.CompareTag("House")) return;
        _isInHouseRange = false;
        _interactionNotifier.HidePressEMessage();
    }
}