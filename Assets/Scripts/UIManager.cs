using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Public variables to be set from the Inspector
    public GameObject gameOver;          // GameObject representing the game over screen
    public TextMeshProUGUI winLoseText;  // TextMeshProUGUI component for displaying win/lose messages

    public static UIManager instance;    // Singleton instance of UIManager

    private void Awake()
    {
        // Implement Singleton pattern to ensure only one instance of UIManager exists
        if (instance == null)
        {
            instance = this;
            Debug.Log("UIManager instance set.");  // Log when the instance is set
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            Debug.LogWarning("Duplicate UIManager instance destroyed.");  // Log warning for duplicate instance
        }
    }

    private void Start()
    {
        // Check if the gameOver GameObject is assigned and set its active state to false
        if (gameOver != null)
        {
            gameOver.SetActive(false);
        }
        else
        {
            Debug.LogError("GameOver GameObject is not assigned.");  // Log error if gameOver is not assigned
        }
    }

    // Method to restart the game by reloading the initial scene
    public void PlayAgain()
    {
        SceneManager.LoadScene(0); // Load the scene with index 0, typically the first scene in the build
    }

    // Method to show the game over screen with a win/lose message
    public void ShowGameOver(bool isWin)
    {
        // Check if winLoseText is assigned and set the appropriate message
        if (winLoseText != null)
        {
            winLoseText.text = isWin ? "YOU WON!" : "YOU LOSE!"; // Set message based on the outcome
        }
        else
        {
            Debug.LogError("WinLoseText is not assigned.");  // Log error if winLoseText is not assigned
        }

        // Check if gameOver is assigned and activate it
        if (gameOver != null)
        {
            gameOver.SetActive(true); // Show the game over screen
        }
        else
        {
            Debug.LogError("GameOver GameObject is not assigned.");  // Log error if gameOver is not assigned
        }
    }
}
