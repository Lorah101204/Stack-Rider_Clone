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
    public TextMeshProUGUI popupScore;
    public CanvasGroup popupCanva; 

    public int scoreNum;

    void Start()
    {
        scoreNum = 0;   
        scoreText.text = scoreFinished.text = scoreAfterDone.text = "" + scoreNum;

        if (popupScore != null) 
        {
            popupScore.text = "+1";
            popupCanva.alpha = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Coin") {
            scoreNum++;
            StartCoroutine(CoinCollect(other.gameObject));
        }

        if (other.transform.tag == "Ball") {
            scoreNum++;
            StartCoroutine(PopupScore());
        }
        scoreText.text = scoreFinished.text = scoreAfterDone.text = "" + scoreNum;
    }

    public void UpdateScoreUI() {
        string scoreStr = scoreNum.ToString();
        scoreText.text = scoreAfterDone.text = scoreFinished.text = scoreStr;
    }


    IEnumerator CoinCollect(GameObject coin) {
        Vector3 targetPos = coin.transform.position + new Vector3(0, 2, -5);
        float coinDuration = 0.7f;
        float elapse = 0;
        Vector3 startPos = coin.transform.position;

        while (elapse < coinDuration) {
            elapse += Time.deltaTime;
            float t = elapse / coinDuration;
            coin.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        Destroy(coin);
    }
    public IEnumerator PopupScore()
    {
        float fadeDuration = 0.1f;
        float moveDistance = 50f;

        Vector3 startPos = popupScore.rectTransform.anchoredPosition;
        Vector3 targetPos = startPos + new Vector3(0, moveDistance, 0);

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeDuration;
            popupCanva.alpha = Mathf.Lerp(0, 1, progress);
            popupScore.rectTransform.anchoredPosition = Vector3.Lerp(startPos, targetPos, progress);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeDuration;
            popupCanva.alpha = Mathf.Lerp(1, 0, progress);
            popupScore.rectTransform.anchoredPosition = Vector3.Lerp(startPos, targetPos + new Vector3(0, moveDistance / 2, 0), progress);
            yield return null;
        }

        popupScore.rectTransform.anchoredPosition = startPos;   
    }
}
