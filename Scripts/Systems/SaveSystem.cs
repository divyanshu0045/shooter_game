using UnityEngine;

/// <summary>
/// A static class to handle saving and loading game data using PlayerPrefs.
/// This system saves high scores and level progression (star ratings).
/// </summary>
public static class SaveSystem
{
    private const string HighScoreKey = "HighScore";
    private const string LevelStarsKeyPrefix = "Level_"; // e.g., "Level_1_Stars"

    /// <summary>
    /// Saves the player's score, but only if it's a new high score.
    /// </summary>
    /// <param name="score">The final score to check and save.</param>
    public static void SaveHighScore(int score)
    {
        int currentHighScore = LoadHighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            Debug.Log("New high score saved: " + score);
            PlayerPrefs.Save(); // Immediately write to disk
        }
    }

    /// <summary>
    /// Loads the saved high score.
    /// </summary>
    /// <returns>The high score. Returns 0 if none is saved.</returns>
    public static int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    /// <summary>
    /// Saves the number of stars earned for a specific level.
    /// It only saves if the new star count is higher than the previously saved one.
    /// </summary>
    /// <param name="levelIndex">The build index of the level.</param>
    /// <param name="stars">The number of stars earned (1-3).</param>
    public static void SaveLevelProgress(int levelIndex, int stars)
    {
        string key = LevelStarsKeyPrefix + levelIndex;
        int currentStars = LoadLevelStars(levelIndex);
        if (stars > currentStars)
        {
            PlayerPrefs.SetInt(key, stars);
            Debug.Log($"Saved {stars} stars for level {levelIndex}.");
            PlayerPrefs.Save(); // Immediately write to disk
        }
    }

    /// <summary>
    /// Loads the number of stars earned for a specific level.
    /// </summary>
    /// <param name="levelIndex">The build index of the level.</param>
    /// <returns>The number of stars earned. Returns 0 if none are saved.</returns>
    public static int LoadLevelStars(int levelIndex)
    {
        string key = LevelStarsKeyPrefix + levelIndex;
        return PlayerPrefs.GetInt(key, 0);
    }
}