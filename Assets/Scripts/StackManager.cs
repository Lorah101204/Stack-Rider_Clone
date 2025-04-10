using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] CameraRotate cam;

    public EndLogic endlogic;
    public LogicScript logic;
    public LogicCollectable logicCol;
    private PlayerMovement playerMovement;

    float jumpHeight = 1f;
    float objHeight = 1f;
    private float rotateSpeed = 10f;

    private bool isFall = false;
    private bool ballRotate = true;
    List<Transform> stackList = new List<Transform>();

    void Start() {
        GameObject firstBall = Instantiate(ballPrefab, transform);
        firstBall.transform.localPosition = new Vector3(0, 0.5f, 0);
        stackList.Add(firstBall.transform);
        playerMovement = player.GetComponent<PlayerMovement>();

        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls) {
            ParticleSystem ballParticle = ball.GetComponent<ParticleSystem>();
            if (ballParticle != null) {
                ballParticle.Stop();
            }
        }
    }

    void Update() {
        if (ballRotate) {
            for (int i = 0; i < stackList.Count; i++) {
                float direction = (i % 2 == 0) ? 1f : -1f;
                stackList[i].Rotate(direction * rotateSpeed, 0, 0, Space.World);
            }
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

        else if (other.CompareTag("Fallout")) 
        {
            if(!isFall) {
                isFall = true;
                StartCoroutine(Fallout());
            }
        }
        
        else if (other.CompareTag("Lava"))
        {
            if (!isFall) {
                isFall = true;
                StartCoroutine(LavaFall());
            }
        }

        else if (other.CompareTag("Finish")) {
            AudioManagement.Vibrate();
            FinishedRemoveBall();
            ballRotate = false;
            cam.speed = -50f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fallout") || other.CompareTag("Lava") && stackList.Count > 00) {
            isFall = false;
        }
    }

    IEnumerator AddBall(Transform ball) 
    {
        Vector3 startPos = ball.localPosition;
        Vector3 endPos = new Vector3 (0, 0.5f, 0f);

        float elapsed = 0f;
        float duration = 0.075f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            ball.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        ball.localPosition = endPos;
        stackList.Insert(0, ball);
    }

    IEnumerator LavaFall() {
        while (isFall && stackList.Count > 0) 
        {
            Transform bottomBall = stackList[0];
            stackList.RemoveAt(0);
            bottomBall.SetParent(null);

            Rigidbody rb = bottomBall.GetComponent<Rigidbody>();
            if (rb == null) 
            {
                rb = bottomBall.AddComponent<Rigidbody>();
            }
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            Destroy(bottomBall.gameObject, 0.1f);

            Vector3 playerStartPos = player.localPosition;
            Vector3 playerTargetPos = playerStartPos - new Vector3(0, objHeight, 0);
            playerTargetPos.y = Mathf.Max(playerTargetPos.y, 0);

            List<Vector3> ballStartPos = new List<Vector3>();
            List<Vector3> ballTargetPos = new List<Vector3>();

            for (int i = 0; i < stackList.Count; i++) 
            {
                ballStartPos.Add(stackList[i].localPosition);
                ballTargetPos.Add(stackList[i].localPosition - new Vector3(0, objHeight, 0));
            }

            float elapsed = 0f;
            float duration = 0.2f;

            while (elapsed < duration) 
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                Vector3 newPos = Vector3.Lerp(playerStartPos, playerTargetPos, t);
                newPos.y = Mathf.Max(newPos.y, 0);
                player.localPosition = newPos;

                for (int i = 0; i < stackList.Count; i++) {
                    stackList[i].localPosition = Vector3.Lerp(ballStartPos[i], ballTargetPos[i], t);
                }

                yield return null;
            }

            player.localPosition = playerTargetPos;
            for (int i = 0; i < stackList.Count; i++) 
            {
                stackList[i].localPosition = ballTargetPos[i];
            }

            yield return new WaitForSeconds(0.03f);
        }
        
        if (stackList.Count < 2) 
        {
            GameOverOnTrigger();
        }
    }

    IEnumerator Fallout() {
        while (isFall && stackList.Count > 0) 
        {
            Transform bottomBall = stackList[0];
            stackList.RemoveAt(0);
            bottomBall.SetParent(null);

            Rigidbody rb = bottomBall.GetComponent<Rigidbody>();
            if (rb == null) 
            {
                rb = bottomBall.AddComponent<Rigidbody>();
            }

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            Destroy(bottomBall.gameObject, 2f);

            Vector3 playerStartPos = player.localPosition;
            Vector3 playerTargetPos = playerStartPos - new Vector3(0, objHeight, 0);
            playerTargetPos.y = Mathf.Max(playerTargetPos.y, 0);

            List<Vector3> ballStartPos = new List<Vector3>();
            List<Vector3> ballTargetPos = new List<Vector3>();

            for (int i = 0; i < stackList.Count; i++) 
            {
                ballStartPos.Add(stackList[i].localPosition);
                ballTargetPos.Add(stackList[i].localPosition - new Vector3(0, objHeight, 0));
            }

            float elapsed = 0f;
            float duration = 0.2f;

            while (elapsed < duration) 
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                Vector3 newPos = Vector3.Lerp(playerStartPos, playerTargetPos, t);
                newPos.y = Mathf.Max(newPos.y, 0);
                player.localPosition = newPos;

                for (int i = 0; i < stackList.Count; i++) {
                    stackList[i].localPosition = Vector3.Lerp(ballStartPos[i], ballTargetPos[i], t);
                }

                yield return null;
            }

            player.localPosition = playerTargetPos;
            for (int i = 0; i < stackList.Count; i++) 
            {
                stackList[i].localPosition = ballTargetPos[i];
            }

            yield return new WaitForSeconds(0.03f);
        }
        
        if (stackList.Count < 2) 
        {
            GameOverOnTrigger();
        }
    }

    void RemoveBall() 
    {
        Transform bottomBall = stackList[0];
        stackList.RemoveAt(0);
        bottomBall.SetParent(null);

        StartCoroutine(DelayedStackFall());

        if (stackList.Count < 2) {
            GameOverOnTrigger();
        }
    }

    IEnumerator DelayedStackFall() 
    {
        yield return new WaitForSeconds(0.35f);
        StartCoroutine(SmoothStackFall());
    }

    IEnumerator SmoothStackFall() 
    {
        Vector3 playerStartPos = player.localPosition;
        Vector3 targetPos = playerStartPos - new Vector3(0, objHeight, 0);

        List<Vector3> ballStartPos = new List<Vector3>();
        List<Vector3> ballTargetPos = new List<Vector3>();

        for (int i = 0; i < stackList.Count; i++) 
        {
            ballStartPos.Add(stackList[i].localPosition);
            ballTargetPos.Add(stackList[i].localPosition - new Vector3(0, objHeight, 0));
        }

        float elapsed = 0;
        float duration = 0.2f;

        while (elapsed < duration) 
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            player.localPosition = Vector3.Lerp(playerStartPos, targetPos, t);
            
            for (int i = 0; i < stackList.Count; i++) 
            {
                stackList[i].localPosition = Vector3.Lerp(ballStartPos[i], ballTargetPos[i], t);
            }

            yield return null;
        }

        player.localPosition = targetPos;
        for (int i = 0; i < stackList.Count; i++) 
        {
            stackList[i].localPosition = ballTargetPos[i];
        }
    }


    public void FinishedRemoveBall() 
    {
        StartCoroutine(DelayFallFinish());
    }

    IEnumerator DelayFallFinish() 
    {
        while (stackList.Count > 0) 
        {
            Transform bottomBall = stackList[0];
            
            ParticleSystem ballPar = bottomBall.GetComponent<ParticleSystem>();

            if (ballPar != null) 
            {
                ballPar.Play();
                yield return new WaitForSeconds(ballPar.main.duration);
            }

            stackList.RemoveAt(0);
            Destroy(bottomBall.gameObject);

            logicCol.scoreNum++;
            StartCoroutine(logicCol.PopupScore());
            logicCol.UpdateScoreUI();

            if (stackList.Count > 0) 
            {
            
                Vector3 playerStartPos = player.position;
                Vector3 playerTargetPos = playerStartPos - new Vector3(0, jumpHeight, 0);

                List<Vector3> ballStartPos = new List<Vector3>();
                List<Vector3> ballTargetPos = new List<Vector3>();

                for (int i = 0; i < stackList.Count; i++) 
                {
                    ballStartPos.Add(stackList[i].position);
                    ballTargetPos.Add(stackList[i].position - new Vector3(0, objHeight, 0));
                }

                float elapsed = 0;
                float duration = 0.2f; 

                while (elapsed < duration) 
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / duration;

                    player.position = Vector3.Lerp(playerStartPos, playerTargetPos, t);

                    for (int i = 0; i < stackList.Count; i++) 
                    {
                        stackList[i].position = Vector3.Lerp(ballStartPos[i], ballTargetPos[i], t);
                    }

                    yield return null;
                }

                player.position = playerTargetPos;
                for (int i = 0; i < stackList.Count; i++) 
                {
                    stackList[i].position = ballTargetPos[i];
                }

                yield return new WaitForSeconds(0.25f);
            }
        }

        logic.Won();

        player.position = new Vector3(player.position.x, endlogic.targetPosition.position.y, player.position.z);
    }

    void GameOverOnTrigger() {
        StopAllCoroutines();
        logic.GameOver();
    }
}
