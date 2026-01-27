using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private InputAction _moveAction;
    private Rigidbody2D _rb;
    private Vector2 _lastMovement;
    public bool canMove = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        if (canMove)
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
}