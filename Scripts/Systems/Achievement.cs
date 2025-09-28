using UnityEngine;

/// <summary>
/// A ScriptableObject to define the properties of a single achievement.
/// Using ScriptableObjects allows you to create and manage achievements as assets in the project.
/// </summary>
[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievements/Achievement")]
public class Achievement : ScriptableObject
{
    [Tooltip("The unique key for this achievement (e.g., 'DEFEAT_100_ENEMIES').")]
    public string achievementKey;

    [Tooltip("The display name of the achievement.")]
    public string displayName;

    [TextArea]
    [Tooltip("A description of what the player needs to do to unlock the achievement.")]
    public string description;

    [Tooltip("Icon to display when the achievement is unlocked.")]
    public Sprite unlockedIcon;

    [Tooltip("Icon to display when the achievement is locked.")]
    public Sprite lockedIcon;

    // This is not saved in the asset, but checked at runtime.
    [HideInInspector]
    public bool isUnlocked = false;
}