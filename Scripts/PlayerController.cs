using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    public GameObject Bullet;
    public Transform  FirePoint;
    public float      fireRate = 0.3f;

    private Rigidbody2D _rb;
    private Vector2     _moveInput;
    private float       _nextFireTime;
    private Camera      _cam;
    private Animator    _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _lastMoveDir = Vector2.left;
    [SerializeField] private Transform _visual;

    // New Input System
    private InputAction _moveAction;
    private InputAction _shootAction;

    private static readonly int SpeedHash    = Animator.StringToHash("Speed");
    private static readonly int ShootingHash = Animator.StringToHash("IsShooting");

    void Awake()
    {
        _rb       = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _cam      = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Define actions in code
        _moveAction  = new InputAction("Move", binding: "<Gamepad>/leftStick");
        _moveAction.AddCompositeBinding("2DVector")
            .With("Up",    "<Keyboard>/w")
            .With("Down",  "<Keyboard>/s")
            .With("Left",  "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        _shootAction = new InputAction("Shoot", binding: "<Mouse>/leftButton");
        _shootAction.AddBinding("<Keyboard>/space");
    }

    void OnEnable()
    {
        _moveAction.Enable();
        _shootAction.Enable();
    }

    void OnDisable()
    {
        _moveAction.Disable();
        _shootAction.Disable();
    }

    void Update()
    {
        // Movement
        _moveInput = _moveAction.ReadValue<Vector2>().normalized;

        //Track last movement direction
        if (_moveInput != Vector2.zero)
        {
            _lastMoveDir = _moveInput;
        }

        //Flip sprite based on horizontal direction
        if (_lastMoveDir != Vector2.zero)
        {
            float angle = Mathf.Atan2(_lastMoveDir.y, _lastMoveDir.x) * Mathf.Rad2Deg - 90f;
            _visual.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Shooting
        bool wantsShoot = _shootAction.IsPressed();
        if (wantsShoot && Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + fireRate;
        }

        // Animator
        if (_animator != null)
        {
            _animator.SetFloat(SpeedHash, _moveInput.magnitude);
            _animator.SetBool(ShootingHash, wantsShoot);
        }
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        if (Bullet == null || FirePoint == null) return;

        Vector2 shootDir = _lastMoveDir.normalized;

        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject bulletInstance = Instantiate(Bullet, FirePoint.position, rotation);

        Collider2D playerCol = GetComponent<Collider2D>();
        Collider2D bulletCol = bulletInstance.GetComponent<Collider2D>();

        if (playerCol != null && bulletCol != null)
        {
            Physics2D.IgnoreCollision(playerCol, bulletCol);
        }

        AudioManager.Instance?.PlaySFX("PlayerShoot");
    }
}