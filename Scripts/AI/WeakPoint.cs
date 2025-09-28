using UnityEngine;

/// <summary>
/// A component to be placed on a child object of a boss.
/// It detects when it's been hit and reports damage back to the main BossAI script.
/// </summary>
public class WeakPoint : MonoBehaviour
{
    [Tooltip("The main BossAI script to which this weak point belongs.")]
    public BossAI boss;

    [Tooltip("How much to multiply the incoming damage by. >1 for critical spots.")]
    public float damageMultiplier = 2.0f;

    private void Start()
    {
        // Automatically find the boss script in the parent if it's not assigned.
        if (boss == null)
        {
            boss = GetComponentInParent<BossAI>();
        }

        if (boss == null)
        {
            Debug.LogError("A WeakPoint could not find a BossAI script in its parents!", this);
        }
    }

    /// <summary>
    /// Called by the PlayerShooting script when this weak point is shot.
    /// </summary>
    /// <param name="damage">The base damage from the weapon.</param>
    public void OnHit(int damage)
    {
        if (boss != null && boss.currentState != BossAI.BossState.Dead)
        {
            int finalDamage = (int)(damage * damageMultiplier);
            boss.TakeDamage(finalDamage);

            // Optionally, we can destroy the weak point after it's been hit once
            // or after it has taken a certain amount of damage.
            // For now, we'll let it be a persistent critical spot.
        }
    }
}