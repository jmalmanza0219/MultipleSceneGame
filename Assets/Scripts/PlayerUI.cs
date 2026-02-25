using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text scoreText;

    public void SetUI(int health, int score)
    {
        if (healthText != null) healthText.text = $"Health: {health}";
        if (scoreText != null)  scoreText.text  = $"Score: {score}";
    }
}
