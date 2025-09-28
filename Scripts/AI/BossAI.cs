using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the behavior and state transitions for a boss enemy.
/// </summary>
public class BossAI : MonoBehaviour
{
    public enum BossState
    {
        Spawning,   // Initial appearance state
        Attacking,  // Main attack phase
        Vulnerable, // Cooldown phase where it can be damaged
        Dead        // Defeated state
    }

    [Header("Boss Stats")]
    public int maxHealth = 2000;
    public int currentHealth;
    public BossState currentState = BossState.Spawning;

    [Header("State Timings")]
    public float spawnDuration = 3.0f;
    public float attackDuration = 8.0f;
    public float vulnerableDuration = 5.0f;

    private float stateTimer;

    private void Start()
    {
        currentHealth = maxHealth;
        EnterState(BossState.Spawning);
    }

    private void Update()
    {
        if (currentState == BossState.Dead) return;

        stateTimer -= Time.deltaTime;

        // State machine logic
        switch (currentState)
        {
            case BossState.Spawning:
                if (stateTimer <= 0) EnterState(BossState.Attacking);
                break;
            case BossState.Attacking:
                // TODO: Implement actual attack patterns here (e.g., firing projectiles)
                if (stateTimer <= 0) EnterState(BossState.Vulnerable);
                break;
            case BossState.Vulnerable:
                if (stateTimer <= 0) EnterState(BossState.Attacking);
                break;
        }
    }

    /// <summary>
    /// Enters a new state and sets up its initial conditions.
    /// </summary>
    private void EnterState(BossState newState)
    {
        currentState = newState;
        Debug.Log("Boss entering state: " + newState);

        switch (newState)
        {
            case BossState.Spawning:
                stateTimer = spawnDuration;
                UIManager.Instance.ShowBossHealthBar(true);
                UIManager.Instance.UpdateBossHealth(currentHealth, maxHealth);
                // TODO: Play spawning animation/VFX
                break;
            case BossState.Attacking:
                stateTimer = attackDuration;
                // TODO: Make weak points inactive/invulnerable
                break;
            case BossState.Vulnerable:
                stateTimer = vulnerableDuration;
                // TODO: Make weak points active/vulnerable
                break;
            case BossState.Dead:
                Die();
                break;
        }
    }

    /// <summary>
    /// Inflicts damage on the boss. This is called by the WeakPoint scripts.
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        if (currentState != BossState.Vulnerable)
        {
            Debug.Log("Boss is not vulnerable, damage ignored!");
            // TODO: Play an "invulnerable" hit sound/effect
            return;
        }

        currentHealth -= damageAmount;
        Debug.Log("Boss took " + damageAmount + " damage. Health: " + currentHealth);
        // Update the boss health bar on the UI
        UIManager.Instance.UpdateBossHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EnterState(BossState.Dead);
        }
    }

    private void Die()
    {
        Debug.Log("Boss has been defeated!");
        UIManager.Instance.ShowBossHealthBar(false);
        // TODO: Play a large death explosion/animation
        // TODO: Notify GameManager to end the level or trigger a victory sequence
        GameManager.Instance.LevelComplete(); // A boss death should complete the level
        Destroy(gameObject, 5f); // Destroy after a delay
    }
}