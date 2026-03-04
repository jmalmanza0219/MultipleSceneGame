using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Starting Values")]
    [SerializeField] private int startingHealth = 100;

    public int Health { get; private set; }
    public int Score { get; private set; }

    // Delegates
    public delegate void ScoreChanged(int newScore);
    public delegate void HealthChanged(int newHealth);
    public delegate void GameOverEvent();

    // Events
    public event ScoreChanged OnScoreChanged;
    public event HealthChanged OnHealthChanged;
    public event GameOverEvent OnGameOver;

    private bool gameOverTriggered = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetStats();
    }

    public void ResetStats()
    {
        gameOverTriggered = false;
        Health = startingHealth;
        Score = 0;

        OnHealthChanged?.Invoke(Health);
        OnScoreChanged?.Invoke(Score);
    }

    public void AddScore(int amount)
    {
        if (amount <= 0 || gameOverTriggered) return;

        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || gameOverTriggered) return;

        Health = Mathf.Max(0, Health - amount);
        OnHealthChanged?.Invoke(Health);

        if (Health <= 0)
            TriggerGameOver();
    }

    public void TriggerGameOver()
    {
        if (gameOverTriggered) return;
    gameOverTriggered = true;

    // Fire event so UIManager can respond
    OnGameOver?.Invoke();
    }
}
