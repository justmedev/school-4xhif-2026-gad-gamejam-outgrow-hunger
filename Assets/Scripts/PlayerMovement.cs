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
        Debug.Log("Awake");
        _moveAction = InputSystem.actions.FindAction("Move");
        Debug.Log(_moveAction);
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            var move = _moveAction.ReadValue<Vector2>();
            Debug.Log(move);
            if (move.x != 0 && move.y != 0)
            {
                Debug.Log("Move1Works");
                _rb.MovePosition(_rb.position + _lastMovement * speed);
                Debug.Log(_rb.position + _lastMovement * speed);
            }
            else
            {
                Debug.Log("Move2Works");
                _rb.MovePosition(_rb.position + move * speed);
                _lastMovement = move;
                Debug.Log(_rb.position + _lastMovement * speed);
            }
        }
    }
}