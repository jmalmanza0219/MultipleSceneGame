using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScoreText;      // Shows the player's score
    [SerializeField] private TMP_Text leaderboardTextUI;   // Shows top 5 high scores

    void Start()
    {
        // Display final score
        int finalScore = 0;
        float completionTime = 0f;

        if (GameManager.Instance != null)
        {
            finalScore = GameManager.Instance.Score;
            completionTime = Time.timeSinceLevelLoad; // example timer
        }

        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {finalScore}";

        // Save high score to database
        if (DatabaseManager.Instance != null)
        {
            string playerName = "Player1"; // Replace with input field if you have one
            DatabaseManager.Instance.AddHighScore(playerName, finalScore, completionTime);
        }

        // Display top 5 high scores
        DisplayTopHighScores();
    }

    private void DisplayTopHighScores()
    {
        if (DatabaseManager.Instance == null || leaderboardTextUI == null)
            return;

        List<HighScoreData> topScores = DatabaseManager.Instance.GetTopHighScores(5);

        string leaderboardText = "Top 5 High Scores:\n";
        for (int i = 0; i < topScores.Count; i++)
        {
            HighScoreData hs = topScores[i];
            leaderboardText += $"{i + 1}. {hs.PlayerName} - {hs.Score} pts - {hs.CompletionTime:F2}s\n";
        }

        leaderboardTextUI.text = leaderboardText;
    }

    public void TryAgain()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResetStats();

        if (CoinPoolManager.Instance != null)
            CoinPoolManager.Instance.ResetCoins();

        SceneManager.LoadScene("GameScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}