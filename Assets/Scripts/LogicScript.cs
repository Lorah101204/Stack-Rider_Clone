using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject settingScreen;

    public void GameOver() {
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }

    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void Won()
    {
        winScreen.SetActive(true);
    }

    public void Setting(bool isPaused) {
        if (!isPaused) {
            settingScreen.SetActive(true);
            Time.timeScale = 0f;
        } else {
            settingScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}