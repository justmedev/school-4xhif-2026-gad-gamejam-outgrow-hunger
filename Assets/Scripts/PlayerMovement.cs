using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool _isMovementEnabled = true;
    private Vector2 _lastMovement;
    private InputAction _moveAction;
    private Vector2 _movePreviousFrame;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _moveAction = InputSystem.actions.FindAction("Move");

        EventBus.Instance.OnNightStarted += () => _isMovementEnabled = false;
        EventBus.Instance.OnDayChanged += _ => _isMovementEnabled = true;
    }

    private void FixedUpdate()
    {
        if (!_isMovementEnabled) return;

        var curMove = _moveAction.ReadValue<Vector2>();
        if (curMove.x != 0 && curMove.y != 0)
        {
            // here 2 keys are pressed at the same time
            if (_lastMovement.x == 0)
                curMove.y = 0;
            else
                curMove.x = 0;

            _rb.MovePosition(_rb.position + curMove.normalized * speed);
            return;
        }

        _rb.MovePosition(_rb.position + curMove.normalized * speed);
        _lastMovement = curMove;
    }
}