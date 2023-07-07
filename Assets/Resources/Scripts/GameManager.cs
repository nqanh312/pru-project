using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Just a simple class that connects game logic with UI
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Image[] healthImages;
    public Text coinsText;
    public Text announcementText;

    public Button restartButton;
    public Button nextButton;

    void Start()
    {
        instance = this;

        LevelStart();
    }

    /// <summary>
    /// Update health counter
    /// </summary>
    /// <param name="health"></param>
    public void UpdateHealth(float health)
    {
        for (int i = 0; i < healthImages.Length; i++)
        {
            if (health <= i)
                healthImages[i].sprite = Resources.Load<Sprite>("Sprites/UI/heart_empty");
            else if (health > i && health < i + 1)
                healthImages[i].sprite = Resources.Load<Sprite>("Sprites/UI/heart_half");
            else if (health > i)
                healthImages[i].sprite = Resources.Load<Sprite>("Sprites/UI/heart_full");
        }
    }

    /// <summary>
    /// Update coins counter
    /// </summary>
    /// <param name="coins"></param>
    public void UpdateCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }

    /// <summary>
    /// Show level start text
    /// </summary>
    public void LevelStart()
    {
        //announcementText.text = "LEVEL 1";
        announcementText.gameObject.SetActive(true);
        StartCoroutine(HideText());
    }

    /// <summary>
    /// Show death text
    /// </summary>
    public void GameOver()
    {
        announcementText.text = "GAME OVER";
        restartButton.gameObject.SetActive(true);
        announcementText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Show level completed text
    /// </summary>
    public void LevelEnd()
    {
        announcementText.text = "COMPLETED!";
        restartButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
        announcementText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide text with delay
    /// </summary>
    /// <returns></returns>
    IEnumerator HideText()
    {
        yield return new WaitForSeconds(3);
        announcementText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Restart current level
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
