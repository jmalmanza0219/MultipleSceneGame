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

        if (GameManager.Instance != null)
            finalScore = GameManager.Instance.Score;

        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {finalScore}";
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
