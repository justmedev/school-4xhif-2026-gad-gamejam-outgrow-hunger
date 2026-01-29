using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private static readonly int AnimPropMoveX = Animator.StringToHash("MoveX");
    private static readonly int AnimPropMoveY = Animator.StringToHash("MoveY");
    private static readonly int AnimPropIsMoving = Animator.StringToHash("IsMoving");

    [SerializeField] private float secondsBetweenStepSounds = .55f;
    [SerializeField] private float speed;
    private Animator _anim;
    private AudioManager _audioMan;
    private AudioSource _audioSource;
    private bool _isMovementEnabled = true;
    private Vector2 _lastMovement;
    private InputAction _moveAction;
    private Vector2 _movePreviousFrame;
    private Rigidbody2D _rb;
    private float _secondsElapsedSinceLastSound = 999;

    private void FixedUpdate()
    {
        _secondsElapsedSinceLastSound += Time.fixedDeltaTime;
        if (!_isMovementEnabled)
        {
            UpdateAnimator(Vector2.zero);
            return;
        }

        var curMove = _moveAction.ReadValue<Vector2>();
        if (curMove.magnitude > 0 && _secondsElapsedSinceLastSound > secondsBetweenStepSounds)
        {
            _audioSource.PlayOneShot(_audioMan.GetRandomStepClip(), .7f);
            _secondsElapsedSinceLastSound = 0;
        }

        _anim.SetBool(AnimPropIsMoving, curMove.magnitude > 0);
        if (curMove.x != 0 && curMove.y != 0)
        {
            // here 2 keys are pressed at the same time, we apply the last pressed key
            if (_lastMovement.x == 0) curMove.y = 0;
            else curMove.x = 0;

            _rb.MovePosition(_rb.position + curMove.normalized * (speed * Time.fixedDeltaTime));
            UpdateAnimator(curMove);
            return;
        }

        _rb.MovePosition(_rb.position + curMove.normalized * (speed * Time.fixedDeltaTime));
        UpdateAnimator(curMove);
        _lastMovement = curMove;
    }

    private void OnEnable()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _audioMan = FindFirstObjectByType<AudioManager>();
        _moveAction = InputSystem.actions.FindAction("Move");

        EventBus.Instance.OnNightStarted += () => _isMovementEnabled = false;
        EventBus.Instance.OnDayChanged += _ => _isMovementEnabled = true;
    }

    private void UpdateAnimator(Vector2 move)
    {
        var isMoving = move.sqrMagnitude > 0;
        _anim.SetBool(AnimPropIsMoving, isMoving);

        if (!isMoving) return;
        // Only update X and Y when moving to preserve "Facing" direction for Idle
        _anim.SetFloat(AnimPropMoveX, move.x);
        _anim.SetFloat(AnimPropMoveY, move.y);
    }
}