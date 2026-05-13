using System.Collections;
using UnityEngine;

/// <summary>
/// Handles player damage. Notifies GameManager on each hit.
/// Provides brief invincibility frames after being hit.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Invincibility Frames")]
    public float invincibilityDuration = 1.0f;

    private bool _isInvincible = false;
    private SpriteRenderer _sr;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    // Called by ZombieAI when it overlaps the player
    public void TakeHit()
    {
        if (_isInvincible) return;
        GameManager.Instance?.OnPlayerHit();
        StartCoroutine(InvincibilityRoutine());
    }

    IEnumerator InvincibilityRoutine()
    {
        _isInvincible = true;

        // Flash the sprite to signal damage
        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            if (_sr != null) _sr.enabled = !_sr.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (_sr != null) _sr.enabled = true;
        _isInvincible = false;
    }
}
