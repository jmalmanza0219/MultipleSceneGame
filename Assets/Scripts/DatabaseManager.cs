using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SQLite;

// Class to hold high score data
public class HighScoreData
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public int Score { get; set; }
    public float CompletionTime { get; set; }
}

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private SQLiteConnection connection;
    private string dbPath;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        dbPath = Path.Combine(Application.persistentDataPath, "GameDatabase.db");

        Debug.Log("DB Path: " + dbPath);

        connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        Debug.Log("Database initialized successfully.");

        CreateHighScoresTable();
    }

    private void CreateHighScoresTable()
    {
        string query = @"
            CREATE TABLE IF NOT EXISTS HighScores (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                playerName TEXT NOT NULL,
                score INTEGER NOT NULL,
                completionTime REAL NOT NULL
            );
        ";

        connection.Execute(query);
        Debug.Log("HighScores table ensured.");
    }

    public void AddHighScore(string playerName, int score, float completionTime)
    {
        if (connection == null)
        {
            Debug.LogError("Database connection not initialized!");
            return;
        }

        string query = "INSERT INTO HighScores (playerName, score, completionTime) VALUES (?, ?, ?)";

        try
        {
            connection.Execute(query, playerName, score, completionTime);
            Debug.Log($"High score added: {playerName} - {score} - {completionTime}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to insert high score: " + ex.Message);
        }
    }

    /// <summary>
    /// Retrieves the top N high scores ordered by score descending.
    /// Default topCount = 5.
    /// </summary>
    public List<HighScoreData> GetTopHighScores(int topCount = 5)
    {
        var highScores = new List<HighScoreData>();

        if (connection == null)
        {
            Debug.LogError("Database connection not initialized!");
            return highScores;
        }

        try
        {
            string query = $"SELECT id, playerName, score, completionTime FROM HighScores ORDER BY score DESC LIMIT {topCount}";
            var results = connection.Query<HighScoreData>(query);
            highScores.AddRange(results);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to retrieve high scores: " + ex.Message);
        }

        return highScores;
    }

    public SQLiteConnection GetConnection()
    {
        return connection;
    }

    private void OnApplicationQuit()
    {
        if (connection != null)
        {
            connection.Close();
            Debug.Log("Database connection closed.");
        }
    }
}