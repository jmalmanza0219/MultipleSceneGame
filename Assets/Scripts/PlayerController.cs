using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private float horizontalInput;

    // Ground detection (collision-based)
    private int groundContacts = 0;
    private bool jumpQueued = false;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    public int Health { get; private set; }
    public int Score  { get; private set; }

    // UI reference (drag & drop in Inspector)
    [SerializeField] private PlayerUI ui;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Health = maxHealth;
        Score = 0;
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
            jumpQueued = true;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (jumpQueued && groundContacts > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        jumpQueued = false;
    }

    // ----- Ground detection -----
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Count only if we touched from above-ish
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.GetContact(i).normal.y > 0.5f)
                {
                    groundContacts++;
                    break;
                }
            }
        }

        // Enemy damage (collision)
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ChangeHealth(-10);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundContacts = Mathf.Max(0, groundContacts - 1);
        }
    }

    // ----- Coin pickup (trigger) -----
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            AddScore(10);
            Destroy(other.gameObject);
        }
    }

    // ----- Stat helpers -----
    private void ChangeHealth(int amount)
    {
        int old = Health;
    Health = Mathf.Clamp(Health + amount, 0, maxHealth);

    if (Health != old)
        UpdateUI();

    if (Health <= 0)
    {
        GameOver();
    }
    }

    private void AddScore(int amount)
    {
        int old = Score;
        Score = Mathf.Max(0, Score + amount);

        if (Score != old)
            UpdateUI();
    }

    private void UpdateUI()
    {
        if (ui != null)
            ui.SetUI(Health, Score);
    }

    private void GameOver()
{
    // Save final score
    PlayerPrefs.SetInt("FinalScore", Score);
    PlayerPrefs.Save();

    // Load GameOver scene
    SceneManager.LoadScene("GameOver");
}
}
