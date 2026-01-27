using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private InputAction _moveAction;
    private Rigidbody2D _rb;
    private Vector2 _movePreviousFrame;
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
            var curMove = _moveAction.ReadValue<Vector2>();
            if (curMove.x != 0 && curMove.y != 0)
            {
                // here 2 keys are pressed at the same time
                if (_lastMovement.x == 0)
                {
                    curMove.y = 0;
                }
                else
                {
                    curMove.x = 0;
                }
                _rb.MovePosition(_rb.position + curMove.normalized * speed);
            }
            else
            {
                _rb.MovePosition(_rb.position + curMove.normalized * speed);
                _lastMovement = curMove;
            }
        }
    }
}