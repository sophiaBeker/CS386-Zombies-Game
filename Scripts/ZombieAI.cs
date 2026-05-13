using UnityEngine;

/// <summary>
/// Zombie AI with dual collider setup:
/// - BoxCollider2D (Is Trigger = OFF): solid body, bullets detect this
/// - CircleCollider2D (Is Trigger = ON, Radius ~1.5): damage zone, detects player contact
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ZombieAI : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed     = 2f;
    public int   health        = 1;
    public float damageInterval = 1.0f;

    private Transform   _player;
    private Rigidbody2D _rb;
    private Animator    _animator;
    private float       _nextDamageTime;

    private static readonly int WalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int DeathHash   = Animator.StringToHash("Die");

    // ---------------------------------------------------------------
    void Awake()
    {
        _rb       = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null) _player = playerGO.transform;
        else Debug.LogWarning("ZombieAI: No GameObject found with tag 'Player'!");
    }

    // ---------------------------------------------------------------
    void FixedUpdate()
    {
        if (_player == null) return;

        Vector2 direction = ((Vector2)_player.position - _rb.position).normalized;
        _rb.MovePosition(_rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (_animator != null) _animator.SetBool(WalkingHash, true);
    }

    // ---------------------------------------------------------------
    // Triggered by the LARGE CircleCollider2D (Is Trigger = ON)
    // Handles player damage on contact
    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time < _nextDamageTime) return;

        Debug.Log("Zombie dealing damage to player!");
        _nextDamageTime = Time.time + damageInterval;
        other.GetComponent<PlayerHealth>()?.TakeHit();
    }

    // ---------------------------------------------------------------
    private bool _isDead = false;

    // Called by Bullet.cs when bullet hits the solid BoxCollider2D
    public void TakeDamage(int amount)
    {

        if (_isDead) return; //STOP if already dead

        health -= amount;
        AudioManager.Instance?.PlaySFX("ZombieHurt");

        if (health <= 0)
            Die();
    }

    void Die()
    {
        if (_isDead) return; //double safety
        _isDead = true;

        Debug.Log("Zombie Die() called!");

        if (_animator != null)
        {
            _animator.SetTrigger(DeathHash);
            _rb.linearVelocity = Vector2.zero;

            foreach (Collider2D col in GetComponents<Collider2D>())
                col.enabled = false;

            enabled = false;
            Destroy(gameObject, 0.8f);
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.Instance?.OnZombieKilled();
    }
}
