using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float speed;
    private InputAction _moveAction;
    private Rigidbody2D _rb;
    private Vector2 _lastMovement;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        Debug.Log("OnEnable");
        _moveAction = InputSystem.actions.FindAction("Move");
        Debug.Log(_moveAction);
    }

    private void FixedUpdate()
    {
        var move = _moveAction.ReadValue<Vector2>();
        if (move.x != 0 && move.y != 0)
        {
            _rb.MovePosition(_rb.position + _lastMovement * speed);
        }
        else
        {
            _rb.MovePosition(_rb.position + move * speed);
            _lastMovement = move;
        }
    }
}