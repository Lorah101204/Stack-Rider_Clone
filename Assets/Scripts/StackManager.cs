using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StackManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject ballPrefab;

    public LogicScript logic;

    float jumpHeight = 1f;
    float stackCount = 0;
    float objHeight = 1f;
    List<Transform> stackList = new List<Transform>();

    public float rotateSpeed = 5f;


    void Start() {
        GameObject firstBall = Instantiate(ballPrefab, transform);
        firstBall.transform.localPosition = new Vector3(0, 0.5f, 0);
        stackList.Add(firstBall.transform);
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
            other.tag = "Untagged";
            Transform t = other.transform;
            t.SetParent(this.transform);

            foreach (Transform ball in stackList) {
                ball.localPosition += Vector3.up * objHeight;
            }

            t.localPosition = new Vector3(0, 0.5f, 0);
            stackList.Insert(0, t);
            player.position += Vector3.up * jumpHeight;
            stackCount++;
        }
        else if (other.CompareTag("Obstacle")) {
            other.tag = "Untagged";
            AudioManagement.Vibrate();
            RemoveBall();
            if (stackList.Count < 2) {
                GameOverOnTrigger();
            }
        }
        else if (other.CompareTag("Finish")) {
            other.tag = "Untagged";
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

            Rigidbody rb = bottomBall.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.back * 1f, ForceMode.Impulse);

            StartCoroutine(DelayFall());
        }
    }

    IEnumerator DelayFall() {
        yield return new WaitForSeconds(0.3f);

        foreach (Transform ball in stackList) {
            ball.localPosition -= Vector3.up * objHeight;
        }

        player.position -= Vector3.up * jumpHeight;
    }

    public void FinishedRemoveBall() {
        StartCoroutine(DelayFallFinish());
    }

    IEnumerator DelayFallFinish() {
        while(stackList.Count > 0) {
            Transform bottomBall = stackList[0];
            stackList.RemoveAt(0);
            Destroy(bottomBall.gameObject);

            foreach (Transform ball in stackList) {
                yield return new WaitForSeconds(0.1f);
                ball.localPosition -= Vector3.up * objHeight;
            }

            player.position -= Vector3.up * jumpHeight;
        }
        player.position = new Vector3(player.position.x, 0f, player.position.z);
    }

    void GameOverOnTrigger() {
        logic.GameOver();
    }

}
