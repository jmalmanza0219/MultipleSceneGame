using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private float horizontalInput;

    private int groundContacts = 0;
    private bool jumpQueued = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

            // Play jump sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.jumpSound);
        }

        jumpQueued = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.GetContact(i).normal.y > 0.5f)
                {
                    groundContacts++;
                    break;
                }
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");

            // Play damage sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.damageSound);

            if (GameManager.Instance != null)
            {
                Debug.Log("Taking damage");
                GameManager.Instance.TakeDamage(10);
            }
            else
            {
                Debug.Log("GameManager is NULL");
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            groundContacts = Mathf.Max(0, groundContacts - 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Coin collected");

            // Play coin sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.coinSound);

            if (GameManager.Instance != null)
            {
                Debug.Log("Adding score");
                GameManager.Instance.AddScore(10);
            }
            else
            {
                Debug.Log("GameManager is NULL");
            }

            if (CoinPoolManager.Instance != null)
            {
                Debug.Log("Returning coin to pool");
                CoinPoolManager.Instance.CollectCoin(other.gameObject);
            }
        }
    }
}
