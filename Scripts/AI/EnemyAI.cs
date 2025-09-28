using UnityEngine;

/// <summary>
/// Manages the behavior of an individual enemy, including its state, health, and actions.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    // Enum to define the possible states of the enemy
    public enum EnemyState
    {
        Idle,       // Not aware of the player yet
        Attacking,  // Actively trying to shoot the player
        TakingCover, // (Optional) Hiding from the player
        Dead        // Defeated
    }

    [Header("AI Settings")]
    [Tooltip("The current state of the enemy.")]
    public EnemyState currentState = EnemyState.Idle;

    [Tooltip("The total health of the enemy.")]
    public int health = 100;

    [Tooltip("The damage this enemy deals to the player per attack.")]
    public int damage = 10;

    [Tooltip("Time the enemy will aim at the player before firing.")]
    public float attackWindUpTime = 1.5f;

    [Tooltip("Time before the enemy is removed or hides after attacking.")]
    public float attackCooldown = 2.0f;

    [Header("VFX & SFX")]
    [Tooltip("Particle effect to play when the enemy takes damage.")]
    public ParticleSystem hitEffect;

    [Tooltip("Particle effect to play when the enemy is defeated.")]
    public ParticleSystem deathEffect;

    [Tooltip("Sound to play when taking damage.")]
    public AudioClip hitSound;

    [Tooltip("Sound to play upon death.")]
    public AudioClip deathSound;

    private AudioSource audioSource;
    private float stateTimer;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { audioSource = gameObject.AddComponent<AudioSource>(); }

        // Enemies might start in Idle and be activated by a trigger
        // Or they can be set to Attacking immediately upon spawning.
        if (currentState == EnemyState.Attacking)
        {
            EnterAttackingState();
        }
    }

    void Update()
    {
        // The state machine logic is handled in Update
        switch (currentState)
        {
            case EnemyState.Idle:
                // Do nothing, wait for a trigger to change state
                break;
            case EnemyState.Attacking:
                UpdateAttackingState();
                break;
            case EnemyState.Dead:
                // The Dead state logic is handled once in the Die() method
                break;
        }
    }

    /// <summary>
    /// Public function to inflict damage on the enemy. Can be overridden by child classes.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to deal.</param>
    public virtual void TakeDamage(int damageAmount)
    {
        if (currentState == EnemyState.Dead) return;

        health -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Health: " + health);

        // Trigger VFX and SFX
        if (hitEffect != null) hitEffect.Play();
        if (audioSource != null && hitSound != null) audioSource.PlayOneShot(hitSound);

        if (health <= 0)
        {
            Die();
        }
    }

    private void EnterAttackingState()
    {
        currentState = EnemyState.Attacking;
        stateTimer = attackWindUpTime;
        // TODO: Play an animation for appearing or aiming
        // TODO: Aim at the player's camera
        Debug.Log(gameObject.name + " is attacking!");
    }

    private void UpdateAttackingState()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            // The enemy's "attack" happens here. In a rail shooter, this is when they damage the player.
            Debug.Log(gameObject.name + " fires at the player!");
            GameManager.Instance.DamagePlayer(this.damage); // Use the enemy's specific damage value

            // After attacking, go into a cooldown, then potentially hide or despawn
            // For simplicity, we'll just 'deactivate' the enemy.
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Handles the enemy's death.
    /// </summary>
    private void Die()
    {
        currentState = EnemyState.Dead;
        Debug.Log(gameObject.name + " has been defeated.");

        // Notify managers
        GameManager.Instance.AddScore(250); // Award more points for a kill
        if (GameManager.Instance.enemySpawnManager != null)
        {
            GameManager.Instance.enemySpawnManager.OnEnemyDefeated();
        }

        // Trigger VFX and SFX
        if (deathEffect != null)
        {
            // Instantiate at the enemy's position, but don't parent it
            // so it doesn't get destroyed with the enemy object immediately.
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        if (audioSource != null && deathSound != null) audioSource.PlayOneShot(deathSound);

        // Disable the collider so it can't be shot anymore
        GetComponent<Collider>().enabled = false;

        // Destroy the game object after a short delay to allow animation to play
        Destroy(gameObject, 2f);
    }

    /// <summary>
    /// Activates the enemy from an idle state, usually called by a spawn manager or trigger.
    /// </summary>
    public void Activate()
    {
        if(currentState == EnemyState.Idle)
        {
            EnterAttackingState();
        }
    }
}