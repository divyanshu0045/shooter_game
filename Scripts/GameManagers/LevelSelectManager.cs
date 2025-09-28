using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// A helper class to hold the UI elements for a single level button.
/// </summary>
[System.Serializable]
public class LevelSelectUI
{
    public string levelSceneName;
    public Button levelButton;
    public Image[] starImages;
}

/// <summary>
/// Manages the level select screen, displaying player progress and loading levels.
/// </summary>
public class LevelSelectManager : MonoBehaviour
{
    [Tooltip("The UI data for all level selection buttons.")]
    public LevelSelectUI[] levels;

    private void Start()
    {
        RefreshUI();
    }

    /// <summary>
    /// Updates all the level buttons with the saved star data.
    /// </summary>
    private void RefreshUI()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            int levelIndex = i + 1; // Assuming level build indices start at 1
            int stars = SaveSystem.LoadLevelStars(levelIndex);

            // Disable all stars first
            foreach (var star in levels[i].starImages)
            {
                star.enabled = false;
            }

            // Enable the correct number of stars
            for (int j = 0; j < stars; j++)
            {
                if (j < levels[i].starImages.Length)
                {
                    levels[i].starImages[j].enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// Loads the specified level scene. This method is called by the UI buttons.
    /// </summary>
    /// <param name="sceneName">The name of the level scene to load.</param>
    public void LoadLevel(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is null or empty!");
            return;
        }
        Debug.Log("Loading level: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}