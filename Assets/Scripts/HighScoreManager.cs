using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HighScoresManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text[] highScoreTexts; // 5 TextMeshPro elements
    [SerializeField] private TMP_Text titleText;

    private void Start()
    {
        if (titleText != null)
            titleText.text = "High Scores";

        DisplayTopHighScores();
    }

    private void DisplayTopHighScores()
    {
        if (DatabaseManager.Instance == null || highScoreTexts.Length == 0)
            return;

        List<HighScoreData> topScores = DatabaseManager.Instance.GetTopHighScores(5);

        for (int i = 0; i < highScoreTexts.Length; i++)
        {
            if (i < topScores.Count)
            {
                HighScoreData hs = topScores[i];
                highScoreTexts[i].text = $"{i + 1}. {hs.PlayerName} - {hs.Score} pts - {hs.CompletionTime:F2}s";
            }
            else
            {
                highScoreTexts[i].text = $"{i + 1}. ---";
            }
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}