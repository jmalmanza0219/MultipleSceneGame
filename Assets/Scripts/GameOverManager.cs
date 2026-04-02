using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
  [SerializeField] private TMP_Text finalScoreText;

    void Start()
    {
       int finalScore = 0;
        float completionTime = 0f; // Time in seconds to complete the level/game

        if (GameManager.Instance != null)
        {
            finalScore = GameManager.Instance.Score;

            // Example: track completion time
            completionTime = Time.timeSinceLevelLoad;
        }

        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {finalScore}";

        // Save high score to database
        if (DatabaseManager.Instance != null)
        {
            string playerName = "Player1"; // Replace with playerNameInput.text if you have an input field
            DatabaseManager.Instance.AddHighScore(playerName, finalScore, completionTime);
        }
    }

    public void TryAgain()
    {
       if (GameManager.Instance != null)
        GameManager.Instance.ResetStats();

    if (CoinPoolManager.Instance != null)
        CoinPoolManager.Instance.ResetCoins();

    SceneManager.LoadScene("GameScene");
    }
}
