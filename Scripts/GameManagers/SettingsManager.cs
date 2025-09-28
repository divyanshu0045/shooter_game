using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Manages the settings screen, handling graphics quality, audio volume,
/// and saving these preferences.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("The dropdown for selecting graphics quality.")]
    public TMP_Dropdown qualityDropdown;
    [Tooltip("The slider for controlling master volume.")]
    public Slider volumeSlider;

    [Header("Navigation")]
    [Tooltip("The name of the scene to return to (e.g., MainMenu).")]
    public string mainMenuSceneName = "MainMenu";

    // PlayerPrefs keys
    private const string QualityKey = "GraphicsQuality";
    private const string VolumeKey = "MasterVolume";

    private void Start()
    {
        // Setup Quality Dropdown
        if (qualityDropdown != null)
        {
            qualityDropdown.ClearOptions();
            List<string> options = new List<string>(QualitySettings.names);
            qualityDropdown.AddOptions(options);
        }

        LoadSettings();
    }

    /// <summary>
    /// Sets the graphics quality level. Called by the quality dropdown's OnValueChanged event.
    /// </summary>
    /// <param name="qualityIndex">The index of the selected quality level.</param>
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt(QualityKey, qualityIndex);
        Debug.Log("Graphics quality set to: " + QualitySettings.names[qualityIndex]);
    }

    /// <summary>
    /// Sets the master audio volume. Called by the volume slider's OnValueChanged event.
    /// </summary>
    /// <param name="volume">The new volume level (0.0 to 1.0).</param>
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);
        Debug.Log("Master volume set to: " + volume);
    }

    /// <summary>
    /// Loads saved settings from PlayerPrefs and applies them.
    /// </summary>
    public void LoadSettings()
    {
        // Load and apply quality settings
        int qualityIndex = PlayerPrefs.GetInt(QualityKey, QualitySettings.GetQualityLevel());
        if (qualityDropdown != null)
        {
            qualityDropdown.value = qualityIndex;
            qualityDropdown.RefreshShownValue();
        }
        SetQuality(qualityIndex);

        // Load and apply volume settings
        float volume = PlayerPrefs.GetFloat(VolumeKey, 1.0f);
        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
        }
        SetVolume(volume);
    }

    /// <summary>
    /// Returns to the main menu scene.
    /// </summary>
    public void BackToMainMenu()
    {
        // Save settings one last time just in case
        PlayerPrefs.Save();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}