using UnityEngine;

/// <summary>
/// A concrete implementation of WeaponBase for a Submachine Gun (SMG).
/// This weapon is characterized by its high rate of fire and moderate spread.
/// </summary>
public class SMG : WeaponBase
{
    /// <summary>
    /// Sets default values for the SMG in the editor for easy setup.
    /// </summary>
    private void Awake()
    {
        // These are suggested values for an SMG and can be overridden in the Inspector.
        fireRate = 15f;    // High rate of fire
        damage = 18;       // Lower damage per shot
        clipSize = 40;     // Large clip size
        spread = 0.8f;     // More spread than a pistol
    }

    /// <summary>
    /// Implements the shooting logic for the SMG. It fires a single raycast with a slight random spread.
    /// </summary>
    public override void Shoot(PlayerShooting player)
    {
        Debug.Log("SMG fired!");

        // Calculate a random direction within the spread angle
        Quaternion fireRotation = Quaternion.LookRotation(player.playerCamera.transform.forward);
        Quaternion randomRotation = Random.rotation;
        fireRotation = Quaternion.RotateTowards(fireRotation, randomRotation, Random.Range(0.0f, spread));

        Ray ray = new Ray(player.playerCamera.transform.position, fireRotation * Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Let the player script process the hit result
            player.ProcessHit(hit);
        }
        else
        {
            // If we miss everything, reset the combo
            GameManager.Instance.ResetCombo();
        }
    }
}