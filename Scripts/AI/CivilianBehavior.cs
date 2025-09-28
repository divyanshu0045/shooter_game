using UnityEngine;

/// <summary>
/// Manages the behavior of a civilian.
/// Its main purpose is to penalize the player if it gets shot.
/// </summary>
public class CivilianBehavior : MonoBehaviour
{
    [Tooltip("A flag to ensure the penalty is applied only once.")]
    private bool wasHit = false;

    [Header("SFX")]
    [Tooltip("Sound to play when the civilian is hit.")]
    public AudioClip scaredSound;

    private AudioSource audioSource;

    private void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { audioSource = gameObject.AddComponent<AudioSource>(); }
    }

    /// <summary>
    /// This method is called by the PlayerShooting script when the civilian is hit by a raycast.
    /// </summary>
    public void OnHit()
    {
        if (wasHit) return;
        wasHit = true;

        Debug.LogWarning("Civilian has been hit! Applying penalty.");

        // Notify the GameManager to penalize the player.
        GameManager.Instance.PenalizePlayerForCivilianHit();

        // Play a 'scared' sound effect
        if (audioSource != null && scaredSound != null)
        {
            audioSource.PlayOneShot(scaredSound);
        }

        // After being hit, the civilian should probably be removed from the scene.
        // We can deactivate it immediately.
        gameObject.SetActive(false);

        // Alternatively, destroy it after a short delay.
        // Destroy(gameObject, 1f);
    }

    /// <summary>
    /// Civilians might appear and disappear based on triggers or timers.
    /// This function can be called by a spawn manager.
    /// </summary>
    public void Spawn()
    {
        wasHit = false;
        gameObject.SetActive(true);
        // TODO: Play an animation of running into view.
    }

    /// <summary>
    /// Hides the civilian without penalizing the player.
    /// </summary>
    public void DespawnSafely()
    {
        // TODO: Play an animation of running to safety.
        gameObject.SetActive(false);
    }
}