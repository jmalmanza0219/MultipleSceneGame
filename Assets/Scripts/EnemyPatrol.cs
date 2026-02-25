using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float patrolDistance = 3f;

    private Rigidbody2D rb;
    private float leftX;
    private float rightX;
    private int direction = 1; // 1 = right, -1 = left

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        float startX = transform.position.x;
        leftX = startX - patrolDistance;
        rightX = startX + patrolDistance;
    }

    void FixedUpdate()
    {
        float nextX = rb.position.x + (direction * moveSpeed * Time.fixedDeltaTime);

        // If we reached/passed an endpoint, clamp and flip ONCE
        if (direction == 1 && nextX >= rightX)
        {
            nextX = rightX;
            direction = -1;
        }
        else if (direction == -1 && nextX <= leftX)
        {
            nextX = leftX;
            direction = 1;
        }

        rb.MovePosition(new Vector2(nextX, rb.position.y));
    }
}
