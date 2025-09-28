using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the unlocking and saving of achievements.
/// </summary>
public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem Instance { get; private set; }

    [Tooltip("A list of all achievements available in the game.")]
    public List<Achievement> achievements;

    // A dictionary for quick lookups of achievements by their key.
    private Dictionary<string, Achievement> achievementDictionary = new Dictionary<string, Achievement>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Populates the dictionary and loads the saved unlocked status for each achievement.
    /// </summary>
    private void SetupAchievements()
    {
        foreach (Achievement achievement in achievements)
        {
            // Add to dictionary for easy access
            achievementDictionary[achievement.achievementKey] = achievement;

            // Check PlayerPrefs to see if it's already unlocked
            achievement.isUnlocked = PlayerPrefs.GetInt(achievement.achievementKey, 0) == 1;
        }
    }

    /// <summary>
    /// Unlocks a specific achievement by its key.
    /// </summary>
    /// <param name="key">The unique key of the achievement to unlock.</param>
    public void UnlockAchievement(string key)
    {
        if (achievementDictionary.TryGetValue(key, out Achievement achievement))
        {
            if (!achievement.isUnlocked)
            {
                achievement.isUnlocked = true;
                PlayerPrefs.SetInt(key, 1);
                PlayerPrefs.Save();
                Debug.Log("Achievement Unlocked: " + achievement.displayName);

                // TODO: Trigger a UI notification to show the player what they unlocked.
                // UIManager.Instance.ShowAchievementNotification(achievement);
            }
        }
        else
        {
            Debug.LogWarning("Tried to unlock an achievement with an invalid key: " + key);
        }
    }
}