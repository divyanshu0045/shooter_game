using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A delegate for a console command function.
/// </summary>
/// <param name="args">The arguments passed to the command.</param>
public delegate void ConsoleCommand(string[] args);

/// <summary>
/// A simple in-game developer console for debugging and cheats.
/// </summary>
public class DeveloperConsole : MonoBehaviour
{
    [Header("Console Settings")]
    [Tooltip("The key to toggle the console's visibility.")]
    public KeyCode toggleKey = KeyCode.BackQuote;

    [Header("UI")]
    [Tooltip("The parent GameObject of the console UI.")]
    public GameObject consoleUI;
    [Tooltip("The input field for typing commands.")]
    public TMPro.TMP_InputField inputField;

    // A dictionary to store all registered commands.
    private Dictionary<string, ConsoleCommand> commands = new Dictionary<string, ConsoleCommand>();
    private bool isGodMode = false;

    private void Awake()
    {
        RegisterCommands();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleConsole();
        }
    }

    /// <summary>
    /// Toggles the visibility of the console UI.
    /// </summary>
    public void ToggleConsole()
    {
        bool isActive = !consoleUI.activeSelf;
        consoleUI.SetActive(isActive);

        if (isActive)
        {
            inputField.ActivateInputField();
        }
    }

    /// <summary>
    /// Registers all the built-in console commands.
    /// </summary>
    private void RegisterCommands()
    {
        commands.Add("help", Help);
        commands.Add("god_mode", GodMode);
        commands.Add("add_score", AddScore);
        // Add more commands here
    }

    /// <summary>
    /// Called when the user presses Enter in the input field.
    /// </summary>
    public void ProcessCommand(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return;
        }

        // Split the input into the command and its arguments
        string[] parts = input.Split(' ');
        string commandKey = parts[0].ToLower();
        string[] args = parts.Skip(1).ToArray();

        if (commands.TryGetValue(commandKey, out ConsoleCommand command))
        {
            command.Invoke(args);
            Debug.Log($"Executed command: {commandKey}");
        }
        else
        {
            Debug.LogWarning($"Unknown command: {commandKey}");
        }

        // Clear the input field for the next command
        inputField.text = "";
        inputField.ActivateInputField();
    }

    #region Command Implementations

    private void Help(string[] args)
    {
        Debug.Log("Available commands:");
        foreach (var command in commands.Keys)
        {
            Debug.Log("- " + command);
        }
    }

    private void GodMode(string[] args)
    {
        isGodMode = !isGodMode;
        // This requires a modification to GameManager to respect the god mode flag.
        // For now, we'll just log the state.
        Debug.Log("God mode is now " + (isGodMode ? "ENABLED" : "DISABLED"));

        // A full implementation would be:
        // if(GameManager.Instance != null) { GameManager.Instance.isGodMode = isGodMode; }
    }

    private void AddScore(string[] args)
    {
        if (args.Length == 0)
        {
            Debug.LogWarning("Usage: add_score <amount>");
            return;
        }

        if (int.TryParse(args[0], out int amount))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(amount);
                Debug.Log($"Added {amount} to score.");
            }
        }
        else
        {
            Debug.LogWarning($"Invalid amount: {args[0]}");
        }
    }

    #endregion
}