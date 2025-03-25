using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicCollectable : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreAfterDone;
    public TextMeshProUGUI scoreFinished;
    public int scoreNum;

    void Start()
    {
        scoreNum = 0;   
        scoreText.text = scoreFinished.text = scoreAfterDone.text = "" + scoreNum;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Coin") {
            scoreNum++;
            StartCoroutine(CoinCollect(other.gameObject));
        }

        if (other.transform.tag == "Ball") {
            scoreNum++;
        }
        scoreText.text = scoreFinished.text = scoreAfterDone.text = "" + scoreNum;
    }

    IEnumerator CoinCollect(GameObject coin) {
        Vector3 targetPos = coin.transform.position + new Vector3(0, 2, -5);
        float coinDuration = 0.7f;
        float elapse = 0;
        Vector3 startPos = coin.transform.position;

        while(elapse < coinDuration) {
            elapse += Time.deltaTime;
            float t = elapse / coinDuration;
            coin.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        Destroy(coin);
    }
}
