using UnityEngine;

/// <summary>
/// A sniper enemy that takes a long time to aim but deals high damage.
/// Inherits from the base EnemyAI.
/// </summary>
public class SniperEnemyAI : EnemyAI
{
    protected void Start()
    {
        // Snipers have unique properties set at the start.
        // These can still be overridden in the Inspector if desired.
        attackWindUpTime = 4.0f; // Long time to aim, giving the player a chance to react.
        damage = 40;             // High damage per shot.
        health = 80;             // Snipers are often less durable.
    }

    // This class doesn't need to override any methods.
    // It simply uses the base class's logic with its own unique stats.
    // You could add a laser sight effect here in the future by adding a LineRenderer
    // that activates during the Attacking state.
}