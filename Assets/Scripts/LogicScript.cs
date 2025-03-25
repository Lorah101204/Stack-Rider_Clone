using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    [SerializeField] PlayerMovement playMove;
    [SerializeField] Animator anim;
    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject settingScreen;


    void Start() {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    public void GameOver() 
    {
        playMove.finished = true;
        playMove.speed = 0f;
        playMove.rb.velocity = Vector3.zero;
        playMove.isOver = true;
        gameOverScreen.SetActive(true);
        anim.SetBool("isLost", true);
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