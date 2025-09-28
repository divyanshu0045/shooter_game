using UnityEngine;

/// <summary>
/// A concrete implementation of WeaponBase for a standard pistol.
/// </summary>
public class Pistol : WeaponBase
{
    /// <summary>
    /// Implements the shooting logic for the pistol by firing a single, precise raycast.
    /// </summary>
    public override void Shoot(PlayerShooting player)
    {
        Debug.Log("Pistol fired!");

        Ray ray = player.playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
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