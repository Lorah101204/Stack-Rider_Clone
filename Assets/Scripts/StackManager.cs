using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject ballPrefab;

    public LogicScript logic;

    float jumpHeight = 1f;
    float objHeight = 1f;
    List<Transform> stackList = new List<Transform>();

    public float rotateSpeed = 5f;
    private PlayerMovement playerMovement;

    void Start() {
        GameObject firstBall = Instantiate(ballPrefab, transform);
        firstBall.transform.localPosition = new Vector3(0, 0.5f, 0);
        stackList.Add(firstBall.transform);
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update() {
        foreach (Transform ball in stackList) {
            ball.Rotate(-rotateSpeed, 0, 0, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Ball")) {
            AudioManagement.Vibrate();
            Transform t = other.transform;
            t.tag = "Untagged"; 
            t.SetParent(this.transform);

            foreach (Transform ball in stackList) {
                ball.localPosition += Vector3.up * objHeight;
            }

            t.localPosition = new Vector3(0, 0.5f, 0);
            stackList.Insert(0, t);
            player.position += Vector3.up * jumpHeight;
        }
        else if (other.CompareTag("Obstacle")) {
            AudioManagement.Vibrate();
            RemoveBall();
        }
        else if (other.CompareTag("Finish")) {
            AudioManagement.Vibrate();
            FinishedRemoveBall();
            logic.Won();
        }
    }

    void RemoveBall() {
        if (stackList.Count > 0) {
            Transform bottomBall = stackList[0];
            stackList.RemoveAt(0);
            bottomBall.SetParent(null);

            if(stackList.Count > 0) {
                StartCoroutine(DelayedStackFall());
            } else {
                GameOverOnTrigger();
            }
        }
    }

    IEnumerator DelayedStackFall() {
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(SmoothStackFall());
    }

    IEnumerator SmoothStackFall() {
        Vector3 playerStartPos = player.localPosition;
        Vector3 targetPos = playerStartPos - new Vector3(0, objHeight, 0);

        List<Vector3> ballStartPos = new List<Vector3>();
        List<Vector3> ballTargetPos = new List<Vector3>();

        for (int i = 0; i < stackList.Count; i++) {
            ballStartPos.Add(stackList[i].localPosition);
            ballTargetPos.Add(stackList[i].localPosition - new Vector3(0, objHeight, 0));
        }

        float elapsed = 0;
        float duration = 0.2f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            player.localPosition = Vector3.Lerp(playerStartPos, targetPos, t);
            
        for (int i = 0; i < stackList.Count; i++) {
            stackList[i].localPosition = Vector3.Lerp(ballStartPos[i], ballTargetPos[i], t);
        }

        yield return null;
    }

    player.localPosition = targetPos;
    for (int i = 0; i < stackList.Count; i++) {
        stackList[i].localPosition = ballTargetPos[i];
    }
}


    public void FinishedRemoveBall() {
        StartCoroutine(DelayFallFinish());
    }

    IEnumerator DelayFallFinish() {
        while (stackList.Count > 0) {
            Transform bottomBall = stackList[0];
            stackList.RemoveAt(0);
            Destroy(bottomBall.gameObject);

            yield return new WaitForSeconds(0.1f);

            foreach (Transform ball in stackList) {
                ball.localPosition -= Vector3.up * objHeight;
            }

            yield return new WaitForSeconds(0.5f);
            player.position -= Vector3.up * jumpHeight;
        }
        yield return new WaitForSeconds(0.3f);
        player.position = new Vector3(player.position.x, 0f, player.position.z);
    }

    void GameOverOnTrigger() {
        logic.GameOver();
    }
}
