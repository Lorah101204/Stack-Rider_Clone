using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicCollectable : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int scoreNum;

    void Start()
    {
        scoreNum = 0;   
        scoreText.text = " " + scoreNum;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Coin") {
            scoreNum++;
            Destroy(other.gameObject);
        }

        if (other.transform.tag == "Ball") {
            scoreNum++;
        }
        scoreText.text = " " + scoreNum;
    }
}
