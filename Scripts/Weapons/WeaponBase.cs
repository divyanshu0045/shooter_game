using UnityEngine;

/// <summary>
/// Base class for all weapons. Defines core attributes and functionality.
/// This is an abstract class, so you must create derived classes for specific weapons.
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Attributes")]
    [Tooltip("The rate of fire in rounds per second.")]
    public float fireRate = 10f;

    [Tooltip("Damage dealt per bullet.")]
    public int damage = 25;

    [Tooltip("Time it takes to reload the weapon in seconds.")]
    public float reloadTime = 1.5f;

    [Tooltip("Maximum number of bullets in a clip.")]
    public int clipSize = 30;

    [Tooltip("The accuracy spread of the weapon. 0 is perfect accuracy.")]
    public float spread = 0.1f;

    [Header("Ammo")]
    [SerializeField]
    private int _currentAmmo;
    public int currentAmmo
    {
        get { return _currentAmmo; }
        protected set { _currentAmmo = value; }
    }

    // Cooldown to manage fire rate
    protected float nextFireTime = 0f;

    protected virtual void Awake()
    {
        currentAmmo = clipSize;
    }

    /// <summary>
    /// Abstract method for shooting. Each weapon will implement its own logic.
    /// The PlayerShooting script is passed in to give the weapon access to the camera and hit processing logic.
    /// </summary>
    public abstract void Shoot(PlayerShooting player);

    /// <summary>
    /// Starts the reload process for the weapon.
    /// </summary>
    public virtual void StartReload()
    {
        // In a full implementation, this would likely trigger a coroutine
        // and an animation. For now, we'll just reset the ammo count.
        Debug.Log("Reloading...");
        Invoke(nameof(FinishReload), reloadTime);
    }

    /// <summary>
    /// Finishes the reload process.
    /// </summary>
    protected virtual void FinishReload()
    {
        currentAmmo = clipSize;
        Debug.Log("Reload complete. Ammo: " + currentAmmo);
        // Here you would notify the UI to update the ammo count
        UIManager.Instance.UpdateAmmo(currentAmmo, clipSize);
    }

    /// <summary>
    /// Checks if the weapon can be fired.
    /// </summary>
    /// <returns>True if the weapon can fire, false otherwise.</returns>
    public bool CanShoot()
    {
        return Time.time >= nextFireTime && currentAmmo > 0;
    }
}