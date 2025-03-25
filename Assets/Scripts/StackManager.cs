using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] CameraRotate cam;

    public LogicScript logic;

    float jumpHeight = 1f;
    float objHeight = 1f;
    List<Transform> stackList = new List<Transform>();

    private float rotateSpeed = 6f;
    private PlayerMovement playerMovement;


    void Start() {
        GameObject firstBall = Instantiate(ballPrefab, transform);
        firstBall.transform.localPosition = new Vector3(0, 0.5f, 0);
        stackList.Add(firstBall.transform);
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update() {
        for (int i = 0; i < stackList.Count; i++) {
            float direction = (i % 2 == 0) ? -1f : 1f;
            stackList[i].Rotate(direction * rotateSpeed, 0, 0, Space.World);
            
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

            StartCoroutine(AddBall(t));
            player.position += Vector3.up * jumpHeight;
        }

        else if (other.CompareTag("Obstacle")) {
            AudioManagement.Vibrate();
            RemoveBall();
        }

        else if (other.CompareTag("Finish")) {
            AudioManagement.Vibrate();
            FinishedRemoveBall();
            cam.speed = -50f;
        }
    }

    IEnumerator AddBall(Transform ball) {
        Vector3 startPos = ball.localPosition;
        Vector3 endPos = new Vector3 (0, 0.5f, 0f);

        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            ball.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        ball.localPosition = endPos;
        stackList.Insert(0, ball);
    }

    void RemoveBall() {
        Transform bottomBall = stackList[0];
        stackList.RemoveAt(0);
        bottomBall.SetParent(null);

        StartCoroutine(DelayedStackFall());

        if (stackList.Count < 2) {
            GameOverOnTrigger();
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

            Vector3 playerStartPos = player.position;
            Vector3 playerTargetPos = playerStartPos - new Vector3(0, jumpHeight, 0);

            List<Vector3> ballStartPos = new List<Vector3>();
            List<Vector3> ballTargetPos = new List<Vector3>();

            for (int i = 0; i < stackList.Count; i++) {
                ballStartPos.Add(stackList[i].position);
                ballTargetPos.Add(stackList[i].position - new Vector3(0, objHeight, 0));
            }

            float elapsed = 0;
            float duration = 0.2f; 

            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                player.position = Vector3.Lerp(playerStartPos, playerTargetPos, t);

                for (int i = 0; i < stackList.Count; i++) {
                    stackList[i].position = Vector3.Lerp(ballStartPos[i], ballTargetPos[i], t);
                }

                yield return null;
            }

            player.position = playerTargetPos;
            for (int i = 0; i < stackList.Count; i++) {
                stackList[i].position = ballTargetPos[i];
            }

            yield return new WaitForSeconds(0.25f);
        }
        player.position = new Vector3(player.position.x, 0f, player.position.z);
        logic.Won();
    }

    void GameOverOnTrigger() {
        StopAllCoroutines();
        logic.GameOver();
    }
}
