using UnityEngine;
using UnityEngine.InputSystem; // Using the new Input System for modern controls

/// <summary>
/// Manages the player's shooting mechanics, input, and weapon handling.
/// It delegates the actual shooting logic to the equipped weapon.
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Handling")]
    [Tooltip("The currently equipped weapon.")]
    public WeaponBase currentWeapon;

    [Tooltip("The camera used for raycasting. Should be the main player camera.")]
    public Camera playerCamera;

    [Header("VFX & SFX")]
    [Tooltip("The particle system for the muzzle flash effect.")]
    public ParticleSystem muzzleFlash;

    [Tooltip("The particle system to instantiate at the hit point.")]
    public ParticleSystem hitEffect;

    [Tooltip("The audio source for gunshot sounds.")]
    public AudioSource gunshotAudio;

    [Header("Input")]
    [Tooltip("Deadzone for swipe detection to avoid accidental reloads.")]
    private float swipeThreshold = 50f;
    private Vector2 touchStartPosition;

    void Update()
    {
        // Simple input handling for PC testing
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            HandleTapToShoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            HandleReload();
        }
        #endif

        // Touch input handling for mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = touch.position;
                    HandleTapToShoot(); // Register a shot on the initial tap
                    break;

                case TouchPhase.Ended:
                    Vector2 swipeDelta = touch.position - touchStartPosition;
                    // Detect a downward swipe for reloading
                    if (swipeDelta.y < -swipeThreshold)
                    {
                        HandleReload();
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Handles the core shooting logic.
    /// </summary>
    private void HandleTapToShoot()
    {
        if (currentWeapon == null)
        {
            Debug.LogError("No weapon equipped!");
            return;
        }

        if (currentWeapon.CanShoot())
        {
            // Set the next fire time based on the weapon's fire rate
            currentWeapon.nextFireTime = Time.time + 1f / currentWeapon.fireRate;

            // Call the Shoot method on the current weapon, passing a reference to this script.
            // The weapon itself will handle the raycasting and hit processing.
            currentWeapon.Shoot(this);

            // Decrement ammo after firing
            currentWeapon.currentAmmo--;

            // Trigger VFX and SFX
            if (muzzleFlash != null) muzzleFlash.Play();
            if (gunshotAudio != null) gunshotAudio.Play();

            // Notify UIManager to update ammo count
            UIManager.Instance.UpdateAmmo(currentWeapon.currentAmmo, currentWeapon.clipSize);
        }
        else if (currentWeapon.currentAmmo <= 0)
        {
            // Optional: Auto-reload when trying to shoot with an empty clip
            HandleReload();
        }
    }

    /// <summary>
    /// Processes the result of a raycast hit. This is called by the individual weapon scripts.
    /// </summary>
    /// <returns>True if a primary target (enemy or civilian) was hit.</returns>
    public bool ProcessHit(RaycastHit hit)
    {
        Debug.Log("Hit: " + hit.transform.name);
        bool hitPrimaryTarget = false;

        // Instantiate hit effect at the point of impact
        if (hitEffect != null)
        {
            Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }

        // Check if we hit a boss weak point
        WeakPoint weakPoint = hit.transform.GetComponent<WeakPoint>();
        if (weakPoint != null)
        {
            weakPoint.OnHit(currentWeapon.damage);
            GameManager.Instance.IncreaseCombo(); // Hitting a weak point continues the combo
            hitPrimaryTarget = true;
        }

        // Check if we hit an enemy
        EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(currentWeapon.damage);
            GameManager.Instance.AddScore(100); // Base score for a hit
            GameManager.Instance.IncreaseCombo();
            hitPrimaryTarget = true;
        }

        // Check if we hit a civilian
        CivilianBehavior civilian = hit.transform.GetComponent<CivilianBehavior>();
        if (civilian != null)
        {
            civilian.OnHit();
            hitPrimaryTarget = true; // Hitting a civilian is still hitting a "target", so combo doesn't reset
        }

        // If we didn't hit an enemy or civilian, reset the combo
        if (!hitPrimaryTarget)
        {
            GameManager.Instance.ResetCombo();
        }

        return hitPrimaryTarget;
    }

    /// <summary>
    /// Initiates the weapon reload sequence.
    /// </summary>
    public void HandleReload()
    {
        if (currentWeapon != null)
        {
            currentWeapon.StartReload();
        }
    }
}