using UnityEngine;

/// <summary>
/// An enemy type with a shield that can block incoming damage from the front.
/// Inherits from the base EnemyAI.
/// </summary>
public class ShieldEnemyAI : EnemyAI
{
    [Header("Shield Specifics")]
    [Tooltip("Whether the shield is currently active and blocking damage.")]
    public bool isShieldUp = true;

    // You could add logic here to make the enemy raise and lower its shield based on a timer.
    // For example, it could lower its shield right before attacking.

    protected void Start()
    {
        // Shield enemies are tougher by default
        health = 200;
    }

    /// <summary>
    /// Overrides the base TakeDamage method to account for the shield.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to deal.</param>
    public override void TakeDamage(int damageAmount)
    {
        if (currentState == EnemyState.Dead) return;

        // Determine the direction from the player to this enemy
        Vector3 directionFromPlayer = (transform.position - Camera.main.transform.position).normalized;

        // Calculate the dot product to see if the player is in front of the enemy
        float dotProduct = Vector3.Dot(transform.forward, directionFromPlayer);

        // A negative dot product means the player is generally in front of the enemy.
        if (isShieldUp && dotProduct < 0)
        {
            Debug.Log("Damage blocked by shield!");
            // TODO: Play a "shield hit" sound or particle effect
            return; // Block the damage
        }

        // If the shield is down or the hit is from the side/back, take damage normally.
        base.TakeDamage(damageAmount);
    }
}