using UnityEngine;

/// <summary>
/// Bullet fired by the player. Moves forward, damages ZombieAI on hit, then destroys itself.
/// Uses OnTriggerEnter2D for zombies (trigger collider) and OnCollisionEnter2D for walls (solid collider).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed    = 12f;
    public int   damage   = 1;
    public float lifetime = 3f;

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rb.linearVelocity = transform.up * speed;
        Destroy(gameObject, lifetime);
    }

    bool hasHit = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit) return;

        ZombieAI zombie = collision.gameObject.GetComponent<ZombieAI>();
        if (zombie != null)
        {
            hasHit = true;
            zombie.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}