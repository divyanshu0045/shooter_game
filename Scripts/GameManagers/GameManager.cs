using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the overall game state, including scoring, player health, and scene transitions.
/// Implemented as a Singleton to ensure there is only one instance.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [Header("Player Stats")]
    public int playerScore = 0;
    public int playerHealth = 100;
    public int comboMultiplier = 1;

    [Header("Game State")]
    public bool isGamePaused = false;
    public EnemySpawnManager enemySpawnManager;

    [Header("Special Abilities")]
    [Tooltip("How long the slow-motion 'Justice Mode' lasts in seconds.")]
    public float justiceModeDuration = 5.0f;
    [Tooltip("The time scale during Justice Mode. 1 is normal speed, 0.5 is half speed.")]
    public float justiceModeScale = 0.5f;
    private bool isJusticeModeActive = false;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Find the spawn manager in the scene
        enemySpawnManager = FindObjectOfType<EnemySpawnManager>();
    }

    #region Scoring
    /// <summary>
    /// Adds points to the player's score.
    /// </summary>
    /// <param name="points">The number of points to add.</param>
    public void AddScore(int points)
    {
        if (isGamePaused) return;
        playerScore += points * comboMultiplier;
        Debug.Log("Score: " + playerScore);
        UIManager.Instance.UpdateScore(playerScore);
    }

    /// <summary>
    /// Increases the combo multiplier.
    /// </summary>
    public void IncreaseCombo()
    {
        comboMultiplier++;
        UIManager.Instance.UpdateCombo(comboMultiplier);
    }

    /// <summary>
    /// Resets the combo multiplier to 1.
    /// </summary>
    public void ResetCombo()
    {
        comboMultiplier = 1;
        UIManager.Instance.UpdateCombo(comboMultiplier);
    }
    #endregion

    #region Player Health
    /// <summary>
    /// Damages the player by a specified amount.
    /// </summary>
    /// <param name="damage">The amount of damage to inflict.</param>
    public void DamagePlayer(int damage)
    {
        if (isGamePaused) return;
        playerHealth -= damage;
        ResetCombo(); // Player taking damage resets the combo
        Debug.Log("Player Health: " + playerHealth);
        UIManager.Instance.UpdateHealth(playerHealth, 100); // Assuming 100 is max health

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            GameOver();
        }
    }

    /// <summary>
    /// Applies a penalty for hitting a civilian.
    /// </summary>
    public void PenalizePlayerForCivilianHit()
    {
        // Example penalty: reduce score or health
        DamagePlayer(20); // Reduce health
        ResetCombo();
        Debug.LogWarning("Player penalized for hitting a civilian.");
    }
    #endregion

    #region Game State Management
    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // Freezes all time-based operations
        UIManager.Instance.ShowPauseMenu(true);
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        UIManager.Instance.ShowPauseMenu(false);
    }

    /// <summary>
    /// Handles the game over sequence.
    /// </summary>
    public void GameOver()
    {
        isGamePaused = true;
        Debug.Log("Game Over!");
        UIManager.Instance.ShowGameOverScreen();
        SaveSystem.SaveHighScore(playerScore);
    }

    /// <summary>
    /// Handles the level completion sequence.
    /// </summary>
    public void LevelComplete()
    {
        isGamePaused = true;
        Debug.Log("Level Complete!");

        // Calculate star rating based on final health
        int stars = 0;
        if (playerHealth > 80) stars = 3;
        else if (playerHealth > 40) stars = 2;
        else if (playerHealth > 0) stars = 1;

        UIManager.Instance.ShowLevelCompleteScreen(stars);

        // Save the progress for the current level
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SaveSystem.SaveLevelProgress(currentLevelIndex, stars);
    }

    /// <summary>
    /// Loads a new level by its scene name.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public void LoadLevel(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    #endregion

    #region Special Abilities
    /// <summary>
    /// Activates the Justice Mode slow-motion effect if it's not already active.
    /// </summary>
    public void ActivateJusticeMode()
    {
        if (isJusticeModeActive || isGamePaused)
        {
            Debug.Log("Cannot activate Justice Mode right now.");
            return;
        }
        StartCoroutine(JusticeModeCoroutine());
    }

    /// <summary>
    /// Coroutine that handles the slow-motion effect duration.
    /// </summary>
    private System.Collections.IEnumerator JusticeModeCoroutine()
    {
        isJusticeModeActive = true;
        Time.timeScale = justiceModeScale;
        Debug.Log("Justice Mode Activated!");

        // We must wait using unscaled time, otherwise the wait duration would be affected by the slow-motion.
        yield return new WaitForSecondsRealtime(justiceModeDuration);

        Time.timeScale = 1.0f;
        isJusticeModeActive = false;
        Debug.Log("Justice Mode Deactivated.");
    }

    /// <summary>
    /// Activates the Grenade Shot, destroying all enemies on screen.
    /// </summary>
    public void ActivateGrenadeShot()
    {
        if (isGamePaused) return;

        Debug.Log("Grenade Shot Activated!");

        // Find all active EnemyAI components in the scene
        EnemyAI[] allEnemies = FindObjectsOfType<EnemyAI>();

        foreach (EnemyAI enemy in allEnemies)
        {
            // We only want to affect enemies that are not already dead
            if (enemy.currentState != EnemyAI.EnemyState.Dead)
            {
                enemy.TakeDamage(9999); // Deal massive damage to ensure they are defeated
            }
        }
    }
    #endregion
}