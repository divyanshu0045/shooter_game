using UnityEngine;

/// <summary>
/// A concrete implementation of WeaponBase for a shotgun.
/// This weapon fires multiple pellets in a spread pattern.
/// </summary>
public class Shotgun : WeaponBase
{
    [Header("Shotgun Specifics")]
    [Tooltip("The number of pellets fired in a single shot.")]
    public int pelletCount = 8;

    [Tooltip("The maximum spread angle for the pellets.")]
    public float maxSpreadAngle = 15.0f;

    /// <summary>
    /// Implements the shooting logic for the shotgun by firing multiple, randomized raycasts.
    /// </summary>
    public override void Shoot(PlayerShooting player)
    {
        Debug.Log("Shotgun fired!");
        bool didHitTarget = false;

        for (int i = 0; i < pelletCount; i++)
        {
            // Calculate a random direction within the spread angle
            Quaternion fireRotation = Quaternion.LookRotation(player.playerCamera.transform.forward);
            Quaternion randomRotation = Random.rotation;
            fireRotation = Quaternion.RotateTowards(fireRotation, randomRotation, Random.Range(0.0f, maxSpreadAngle));

            Ray ray = new Ray(player.playerCamera.transform.position, fireRotation * Vector3.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                // Let the player script process the hit result
                bool wasPrimaryTarget = player.ProcessHit(hit);
                if (wasPrimaryTarget)
                {
                    didHitTarget = true;
                }
            }
        }

        // If not a single pellet hit a primary target (enemy/civilian), reset the combo
        if (!didHitTarget)
        {
            GameManager.Instance.ResetCombo();
        }
    }
}