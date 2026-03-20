using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPoolManager : MonoBehaviour
{
    public static CoinPoolManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private ObjectPool coinPool;

    private List<Vector3> coinSpawnPositions = new List<Vector3>();
    private List<GameObject> activeCoins = new List<GameObject>();

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InitializeCoins();
    }

    // ---------------------------
    // Create coins at start
    // ---------------------------
    private void InitializeCoins()
    {
        // Find all coins placed in scene (your original coins)
        GameObject[] coinsInScene = GameObject.FindGameObjectsWithTag("Coin");

        foreach (GameObject coin in coinsInScene)
        {
            coinSpawnPositions.Add(coin.transform.position);

            // Disable original coin
            coin.SetActive(false);
        }

        // Spawn pooled coins at those positions
        foreach (Vector3 pos in coinSpawnPositions)
        {
            GameObject coin = coinPool.GetObject();
            coin.transform.position = pos;

            activeCoins.Add(coin);
        }
    }

    // ---------------------------
    // When player collects coin
    // ---------------------------
    public void CollectCoin(GameObject coin)
    {
        // Play coin sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.coinSound);

        if (coinPool != null)
        {
            coinPool.ReturnObject(coin);
        }

        activeCoins.Remove(coin);
    }

    // ---------------------------
    // Reset coins on retry
    // ---------------------------
    public void ResetCoins()
    {
        // Return all active coins to pool
        foreach (GameObject coin in activeCoins)
        {
            coinPool.ReturnObject(coin);
        }

        activeCoins.Clear();

        // Respawn coins at original positions
        foreach (Vector3 pos in coinSpawnPositions)
        {
            GameObject coin = coinPool.GetObject();
            coin.transform.position = pos;

            activeCoins.Add(coin);
        }
    }
}
