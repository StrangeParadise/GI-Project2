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

    public int lifeCount;

    public GameProcessController gameProcess;

    private Rigidbody rb;
    private CameraController script;

    private Vector3 destination;
    private float offset;

    // Use this for initialization
    void Start() {
        script = view.GetComponent<CameraController>();
        rb = GetComponent<Rigidbody>();
        lifeCount = 3;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.Escape)) {
            gameProcess.quit();
        }
        if (transform.position.y < -10 && lifeCount > 0)
        {
            respawn();
            lifeCount--;
        }
        else if (lifeCount == 0 && transform.position.y < -10) {
            gameProcess.gameOver();
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
        checkWin(destination, offset);
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


        ballClone.GetComponent<BallController>().script = script;
        ballClone.GetComponent<BallController>().woodBall = woodBall;
        ballClone.GetComponent<BallController>().stoneBall = stoneBall;
        ballClone.GetComponent<BallController>().metalBall = metalBall;
        ballClone.GetComponent<BallController>().view = view;
        ballClone.GetComponent<BallController>().acceleration = acceleration;
        ballClone.GetComponent<BallController>().accConstant = accConstant;
        ballClone.GetComponent<BallController>().gameProcess = gameProcess;
        ballClone.GetComponent<BallController>().lifeCount = lifeCount;
        print("lifeCount == " + lifeCount);

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

    private void checkWin(Vector3 destination, float offset) {
        if (transform.position.x < destination.x + offset && transform.position.x > destination.x - offset 
            && transform.position.z < destination.z + offset && transform.position.z > destination.z - offset && transform.position.y == destination.y + 0.5) {
            gameProcess.gameWin();
        }
    }

    public void setDestination(Vector3 destination)
    {
        this.destination = destination;
    }
    public void setOffset(float offset)
    {
        this.offset = offset;
    }
}
