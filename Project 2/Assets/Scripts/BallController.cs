using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    // Variables
    public float acceleration;
    public float accConstant;

    public Camera view;

    public GameObject woodBall;
    public GameObject stoneBall;
    public GameObject metalBall;

    private Rigidbody rb;
    private CameraController script;

    // Use this for initialization
    void Start() {
        script = view.GetComponent<CameraController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
		if (Input.GetKey (KeyCode.Space)) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
		if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        } 
        if (transform.position.y < -10) {
            respawn();
        }
        if (Input.GetKeyDown(KeyCode.C) && isStable()) {
            int ballType = Random.Range(0, 3);
            changeBall(ballType);
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            accelerate();
        }
        else {
            move(script.orin);
        }
    }

    private void move(int orin) {
        switch (orin) {
            case 0:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    moveFwd();
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    moveBack();
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    moveLeft();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    moveRight();
                }
                break;
            case 1:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    moveLeft();
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    moveRight();
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    moveBack();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    moveFwd();
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    moveBack();
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    moveFwd();
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    moveRight();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    moveLeft();
                }
                break;
            case 3:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    moveRight();
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    moveLeft();
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    moveFwd();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    moveBack();
                }
                break;
            default:
                break;
        }
    }

    private void moveFwd() {
        rb.AddForce(Vector3.forward * acceleration);
    }
    private void moveBack() {
        rb.AddForce(Vector3.back * acceleration);
    }
    private void moveLeft() {
        rb.AddForce(Vector3.left * acceleration);
    }
    private void moveRight() {
        rb.AddForce(Vector3.right * acceleration);
    }
    private void respawn() {
        transform.position = new Vector3(0, 2, 0);
        stablize();
    }
    private void accelerate() {
        acceleration *= accConstant;
        move(script.orin);
        acceleration /= accConstant;
    }
    private void changeBall(int ballType) {
        GameObject ball;
        if (ballType == 0)
        {
            ball = woodBall;
        }
        else if (ballType == 1)
        {
            ball = stoneBall;
        }
        else {
            ball = metalBall;
        }
        
        GameObject ballClone = (GameObject)Instantiate(ball, transform.position, transform.rotation);

        
        ballClone.GetComponent<BallController>().script = GetComponent<BallController>().script;
        ballClone.GetComponent<BallController>().woodBall = GetComponent<BallController>().woodBall;
        ballClone.GetComponent<BallController>().stoneBall = GetComponent<BallController>().stoneBall;
        ballClone.GetComponent<BallController>().metalBall = GetComponent<BallController>().metalBall;
        ballClone.GetComponent<BallController>().view = GetComponent<BallController>().view;
        ballClone.GetComponent<BallController>().acceleration = GetComponent<BallController>().acceleration;
        ballClone.GetComponent<BallController>().accConstant = GetComponent<BallController>().accConstant;

        Destroy(gameObject);
        script.player = ballClone;
    }

    private void stablize() {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    private bool isStable() {
        return rb.angularVelocity.magnitude <= 2 && rb.velocity.magnitude <= 2;
    }
}
