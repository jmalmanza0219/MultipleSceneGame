using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;

    private void OnEnable()
    {
        Subscribe();
    }

    private void Start()
    {
        // Backup in case GameManager wasn't ready in OnEnable
        Subscribe();

        if (GameManager.Instance != null)
        {
            UpdateScore(GameManager.Instance.Score);
            UpdateHealth(GameManager.Instance.Health);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnScoreChanged -= UpdateScore;
        GameManager.Instance.OnHealthChanged -= UpdateHealth;
        GameManager.Instance.OnGameOver -= HandleGameOver;
    }

    private void Subscribe()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnScoreChanged -= UpdateScore;
        GameManager.Instance.OnHealthChanged -= UpdateHealth;
        GameManager.Instance.OnGameOver -= HandleGameOver;

        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnHealthChanged += UpdateHealth;
        GameManager.Instance.OnGameOver += HandleGameOver;
    }

    private void UpdateScore(int newScore)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + newScore;
    }

    private void UpdateHealth(int newHealth)
    {
        if (healthText != null)
            healthText.text = "Health: " + newHealth;
    }

    private void HandleGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
