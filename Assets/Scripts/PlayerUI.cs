using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text scoreText;

    void Update()
    {
        if (GameManager.Instance == null) return;

        if (healthText != null)
            healthText.text = $"Health: {GameManager.Instance.Health}";

        if (scoreText != null)
            scoreText.text = $"Score: {GameManager.Instance.Score}";
    }
}
