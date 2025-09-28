using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the UI and functionality of the main menu screen.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Tooltip("The name of the scene to load when the 'Start' button is clicked. Usually the level select screen.")]
    public string levelSelectSceneName = "LevelSelect";

    [Tooltip("The name of the scene containing the game settings.")]
    public string settingsSceneName = "Settings";

    /// <summary>
    /// Loads the level select scene.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Loading Level Select scene...");
        SceneManager.LoadScene(levelSelectSceneName);
    }

    /// <summary>
    /// Loads the settings scene.
    /// </summary>
    public void OpenSettings()
    {
        Debug.Log("Loading Settings scene...");
        SceneManager.LoadScene(settingsSceneName);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

        // If running in the Unity Editor, stop playing.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}