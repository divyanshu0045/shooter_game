using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Text and Slider
using TMPro; // Often used for better text rendering, so I'll include it

/// <summary>
/// Manages all user interface elements in the game.
/// Implemented as a Singleton for easy access from other scripts.
/// </summary>
public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance { get; private set; }

    [Header("In-Game HUD")]
    [Tooltip("Text element to display the player's score.")]
    public TextMeshProUGUI scoreText;

    [Tooltip("Text element to display current ammo and clip size.")]
    public TextMeshProUGUI ammoText;

    [Tooltip("Slider or Image Fill to represent player health.")]
    public Slider healthBar;

    [Tooltip("Text element for the combo multiplier.")]
    public TextMeshProUGUI comboText;

    [Tooltip("Animator for the combo text, to play an effect on increase.")]
    public Animator comboAnimator;

    [Header("UI Panels")]
    [Tooltip("The parent object for the pause menu UI.")]
    public GameObject pauseMenuPanel;

    [Tooltip("The parent object for the game over screen UI.")]
    public GameObject gameOverPanel;

    [Tooltip("The parent object for the level complete screen UI.")]
    public GameObject levelCompletePanel;

    [Tooltip("The slider used for the boss's health bar.")]
    public Slider bossHealthBar;

    [Tooltip("The images for the star rating display on the level complete screen.")]
    public Image[] starRatingImages;


    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ensure panels are hidden at the start of the level
        if(pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if(gameOverPanel != null) gameOverPanel.SetActive(false);
        if(levelCompletePanel != null) levelCompletePanel.SetActive(false);
        if(bossHealthBar != null) bossHealthBar.gameObject.SetActive(false);
    }


    #region HUD Updates
    /// <summary>
    /// Updates the score display.
    /// </summary>
    /// <param name="score">The new score to display.</param>
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("D8"); // "D8" pads with leading zeros
        }
    }

    /// <summary>
    /// Updates the ammo counter display.
    /// </summary>
    /// <param name="currentAmmo">The current ammo in the clip.</param>
    /// <param name="clipSize">The maximum size of the clip.</param>
    public void UpdateAmmo(int currentAmmo, int clipSize)
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {clipSize}";
        }
    }

    /// <summary>
    /// Updates the health bar.
    /// </summary>
    /// <param name="currentHealth">The player's current health.</param>
    /// <param name="maxHealth">The player's maximum health.</param>
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    /// <summary>
    /// Updates the combo multiplier display.
    /// </summary>
    /// <param name="combo">The new combo multiplier.</param>
    public void UpdateCombo(int combo)
    {
        if (comboText != null)
        {
            comboText.text = (combo > 1) ? $"x{combo}" : "";

            // Play a small animation on the combo text if the combo is increasing
            if (combo > 1 && comboAnimator != null)
            {
                comboAnimator.SetTrigger("Pulse");
            }
        }
    }
    #endregion

    #region Boss UI
    /// <summary>
    /// Shows or hides the boss health bar.
    /// </summary>
    /// <param name="show">True to show, false to hide.</param>
    public void ShowBossHealthBar(bool show)
    {
        if (bossHealthBar != null)
        {
            bossHealthBar.gameObject.SetActive(show);
        }
    }

    /// <summary>
    /// Updates the boss health bar's value.
    /// </summary>
    /// <param name="currentHealth">The boss's current health.</param>
    /// <param name="maxHealth">The boss's maximum health.</param>
    public void UpdateBossHealth(int currentHealth, int maxHealth)
    {
        if (bossHealthBar != null)
        {
            bossHealthBar.maxValue = maxHealth;
            bossHealthBar.value = currentHealth;
        }
    }
    #endregion

    #region Panel Management
    /// <summary>
    /// Shows or hides the pause menu.
    /// </summary>
    public void ShowPauseMenu(bool show)
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(show);
        }
    }

    /// <summary>
    /// Shows the game over screen.
    /// </summary>
    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Shows the level complete screen and displays the star rating.
    /// </summary>
    /// <param name="stars">The number of stars earned (1-3).</param>
    public void ShowLevelCompleteScreen(int stars)
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);

            // Ensure we have star images to work with
            if (starRatingImages == null || starRatingImages.Length == 0)
            {
                Debug.LogWarning("Star rating images are not assigned in the UIManager.");
                return;
            }

            // First, disable all stars
            for (int i = 0; i < starRatingImages.Length; i++)
            {
                starRatingImages[i].enabled = false;
            }

            // Then, enable the earned stars
            for (int i = 0; i < stars; i++)
            {
                if (i < starRatingImages.Length)
                {
                    starRatingImages[i].enabled = true;
                }
            }
        }
    }
    #endregion
}