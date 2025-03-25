using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float moveSpeed = 5f;
    public float laneWidth = 2f;
    public Transform finisher;
    public Animator anim;

    public Rigidbody rb;
    private float targetX;
    public bool finished;
    public bool gameStarted = false;

    public bool isOver = false;

    

    void Start() {
        rb = GetComponent<Rigidbody>(); 
        targetX = transform.position.x;
        anim.SetBool("isRunning", false);
    }

    void Update() {
        if (!gameStarted) {
            CheckForStart();
            return;
        }

        if (isOver) return;

        rb.velocity = new Vector3(0, rb.velocity.y, speed);

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Move(-1);   
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Move(1);
        }

        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved) {
                float moveAmount = touch.deltaPosition.x * 0.01f;
                targetX += moveAmount;
                targetX = Mathf.Clamp(targetX, -laneWidth, laneWidth);
            }
        }

        float newX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        
        if(transform.position.z >= finisher.position.z) {
            Finished();
        }
    }

    void CheckForStart() {
        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)) {
            gameStarted = true;
            anim.SetBool("isRunning", true);
        }
    }

    public void Finished() {
        finished = true;
        speed = 0f;
        rb.velocity = Vector3.zero;
        anim.SetBool("isFinished", true);
        isOver = true;
    }

    void Move(int direction) {
        targetX += direction * laneWidth;
        targetX = Mathf.Clamp(targetX, -laneWidth, laneWidth);
    }
}
