using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  public void StartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetStats(); // resets Health + Score to starting values
        }

        SceneManager.LoadScene("GameScene");
    }
}
